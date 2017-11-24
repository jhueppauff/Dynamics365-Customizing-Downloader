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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class UpdateChecker
    {
        /// <summary>
        /// Github API, Release URL
        /// </summary>
        private const string githubReleasePath = "/repos/jhueppauff/Dynamics365-Customizing-Downloader/releases";

        private const string githubAPIRUL = "https://api.github.com";

        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Checks if an Update is availible
        /// </summary>
        /// <returns>Returns if an update is availible</returns>
        public bool IsUpdateAvailible ()
        {
            

            return false;
        }

        private static async Task<List<Release>> RetriveReleasesAsync()
        {
            List<Release> releases = new List<Release>();
            Release release = new Release();

            httpClient.BaseAddress = new Uri(githubAPIRUL);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync(githubReleasePath);

            if (response.IsSuccessStatusCode)
            {
                release = await response.Content.ReadAsAsync<Release>();
            }
        }
    }
}
