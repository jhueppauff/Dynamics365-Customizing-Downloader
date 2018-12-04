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
    using Hueppauff.RestClient;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Update Check with the GitHub Release API
    /// </summary>
    public class UpdateChecker
    {

        /// <summary>
        /// Gets the Release Information from GitHub API
        /// </summary>
        /// <param name="apiURL">The GitHub API Url from config</param>
        /// <returns>Returns a single Release <see cref="Update.Release"/></returns>
        public async Task<Release> GetReleaseInfo(string apiURL)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                string json = await this.PerformAPICall(apiURL).ConfigureAwait(false);
                var release = JsonConvert.DeserializeObject<Release>(json, serializerSettings);

                return release;
            }
            catch (Exception ex)
            {
                // UpdateChecker.Log.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        /// <param name="apiURL">GitHub API Url from config</param>
        /// <returns>Returns if an update is available</returns>
        public async Task<bool> IsUpdateAvailable(string apiURL)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var release = JsonConvert.DeserializeObject<Release>(await this.PerformAPICall(apiURL).ConfigureAwait(false), serializerSettings);

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
                //UpdateChecker.Log.Error(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the Update URL for the Release
        /// </summary>
        /// <param name="apiURL">GitHub API url form config</param>
        /// <returns>Returns the Update URL<see cref="Uri"/> of the latest Update</returns>
        public async Task<Uri> GetUpdateURL(string apiURL)
        {
            try
            {
                var release = JsonConvert.DeserializeObject<Release>(await this.PerformAPICall(apiURL).ConfigureAwait(false));

                return new Uri(release.Assets.SingleOrDefault(x => x.Name == "Dynamics365CustomizingDownloader.zip").Browser_download_url);
            }
            catch (Exception ex)
            {
                //UpdateChecker.Log.Error(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Performs the Call to the API
        /// </summary>
        /// <param name="apiURL">Get API to invoke</param>
        /// <returns>Returns <see cref="string"/> Answer of the API</returns>
        private async Task<string> PerformAPICall(string apiURL)
        {
            try
            {
                ServiceClient serviceClient = new ServiceClient();
                WebRequest request = WebRequest.Create(apiURL);

                return await serviceClient.GetAsync<string>("GET", request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                //UpdateChecker.Log.Error(ex.Message, ex);
                return null;
            }
        }
    }
}