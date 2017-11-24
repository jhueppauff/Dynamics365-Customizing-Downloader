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
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /// <summary>
    /// Update Check with the GitHub Release API
    /// </summary>
    public class UpdateChecker
    {
        /// <summary>
        /// GitHub API, Release URL
        /// </summary>
        private const string GitHubReleasePath = "/repos/jhueppauff/Dynamics365-Customizing-Downloader/releases";

        /// <summary>
        /// GitHub Base URL
        /// </summary>
        private const string GitHubAPIRUL = "https://api.github.com";

        /// <summary>
        /// HTTP Client
        /// </summary>
        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        /// <returns>Returns if an update is available</returns>
        public bool IsUpdateAvailible()
        {
            // ToDo:
            return false;
        }

        /// <summary>
        /// Retrieves the Releases Async
        /// </summary>
        /// <returns>Returns a <see cref="List{Release}"/> with all Releases</returns>
        private static async Task<List<Release>> RetriveReleasesAsync()
        {
            List<Release> releases = new List<Release>();
            Release release = new Release();

            httpClient.BaseAddress = new Uri(GitHubAPIRUL);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync(GitHubReleasePath);

            if (response.IsSuccessStatusCode)
            {
                release = await response.Content.ReadAsAsync<Release>();
            }

            return releases;
        }
    }
}
