//-----------------------------------------------------------------------
// <copyright file="UpdateChecker.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Update
{
    using System;
    using System.Diagnostics;
    using System.Linq;
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
        /// <param name="apiURL">The GitHub API Url from config</param>
        /// <returns>Returns a single Release <see cref="Update.Release"/></returns>
        public Release GetReleaseInfo(string apiURL)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                string json = this.PerformAPICall(apiURL);
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
        /// <param name="apiURL">GitHub API Url from config</param>
        /// <returns>Returns if an update is available</returns>
        public bool IsUpdateAvailable(string apiURL)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var release = JsonConvert.DeserializeObject<Release>(this.PerformAPICall(apiURL), serializerSettings);

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
        /// <param name="apiURL">GitHub API url form config</param>
        /// <returns>Returns the Update URL<see cref="Uri"/> of the latest Update</returns>
        public Uri GetUpdateURL(string apiURL)
        {
            try
            {
                var release = JsonConvert.DeserializeObject<Release>(this.PerformAPICall(apiURL));

                return new Uri(release.Assets.SingleOrDefault(x => x.Name == "Dynamics365CustomizingDownloader.zip").Browser_download_url);
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