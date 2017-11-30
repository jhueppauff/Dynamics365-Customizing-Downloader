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
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using RestSharp;

    /// <summary>
    /// Update Check with the GitHub Release API
    /// </summary>
    public class UpdateChecker
    {
        /// <summary>
        /// HTTP Client
        /// </summary>
        HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        /// <returns>Returns if an update is available</returns>
        public bool IsUpdateAvailable()
        {
            try
            {
                var client = new RestClient(Properties.Settings.Default.GitHubAPIURL);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);

                var release = JsonConvert.DeserializeObject<Release>(response.Content);

                DateTime published = (DateTime)release.published_at;

                if (published > (DateTime)Properties.Settings.Default.Version)
                {

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}