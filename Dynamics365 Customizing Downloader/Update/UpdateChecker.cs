//-----------------------------------------------------------------------
// <copyright file="UpdateChecker.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Update
{
    using System;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using RestSharp;

    /// <summary>
    /// Update Check with the GitHub Release API
    /// </summary>
    public class UpdateChecker
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the Release Information from GitHub API
        /// </summary>
        /// <returns>Returns a single Release <see cref="Update.Release"/></returns>
        public Release GetReleaseInfo()
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                string json = this.PerformAPICall($"{Properties.Settings.Default.GitHubAPIURL}/releases/latest");
                var release = JsonConvert.DeserializeObject<Release>(json, serializerSettings);

                return release;
            }
            catch (Exception ex)
            {
                UpdateChecker.Log.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        /// <returns>Returns if an update is available</returns>
        public bool IsUpdateAvailable()
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var release = JsonConvert.DeserializeObject<Release>(this.PerformAPICall(Properties.Settings.Default.GitHubAPIURL + "/releases/latest"), serializerSettings);

                // Get Verison
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                var version = new Version(fvi.FileVersion);

                var versionGithub = new Version(release.Name);

                var result = version.CompareTo(versionGithub);

                // Check if Version is newer than local version
                if (result < 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                UpdateChecker.Log.Error(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the Update URL for the Release
        /// </summary>
        /// <returns>Returns the Update URL<see cref="Uri"/> of the latest Update</returns>
        public Uri GetUpdateURL()
        {
            try
            {
                var release = JsonConvert.DeserializeObject<Release>(this.PerformAPICall(Properties.Settings.Default.GitHubAPIURL + "/releases/latest"));

                return new Uri(release.Assets[0].Browser_download_url);
            }
            catch (Exception ex)
            {
                UpdateChecker.Log.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Performs the Call to the API
        /// </summary>
        /// <param name="apiURL">Get API to invoke</param>
        /// <returns>Returns <see cref="string"/> Answer of the API</returns>
        private string PerformAPICall(string apiURL)
        {
            try
            {
                var client = new RestClient(apiURL);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);

                return response.Content;
            }
            catch (Exception ex)
            {
                UpdateChecker.Log.Error(ex.Message, ex);
                return null;
            }
        }
    }
}