//-----------------------------------------------------------------------
// <copyright file="Person.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Update
{
    using Newtonsoft.Json;

    /// <summary>
    /// Person JSON Class for GitHub API
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the Login of the Person
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Person
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Avatar URL of the Person
        /// </summary>
        [JsonProperty("avatar_url")]
        public string Avatar_url { get; set; }

        /// <summary>
        /// Gets or sets the Gravatar ID of the Person
        /// </summary>
        [JsonProperty("gravatar_id")]
        public string Gravatar_id { get; set; }

        /// <summary>
        /// Gets or sets the URL of the Person
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the HTML Url of the Person
        /// </summary>
        [JsonProperty("html_url")]
        public string Html_url { get; set; }

        /// <summary>
        /// Gets or sets the Followers of the Person
        /// </summary>
        [JsonProperty("followers_url")]
        public string Followers_url { get; set; }

        /// <summary>
        /// Gets or sets the Following URL of the Person
        /// </summary>
        [JsonProperty("following_url")]
        public string Following_url { get; set; }

        /// <summary>
        /// Gets or sets the Gist URL of the Person
        /// </summary>
        [JsonProperty("gists_url")]
        public string Gists_url { get; set; }

        /// <summary>
        /// Gets or sets the Star URL of the Person
        /// </summary>
        [JsonProperty("starred_url")]
        public string Starred_url { get; set; }

        /// <summary>
        /// Gets or sets the Subscription URL of the Person
        /// </summary>
        [JsonProperty("subscriptions_url")]
        public string Subscriptions_url { get; set; }

        /// <summary>
        /// Gets or sets the Organization URL of the Person
        /// </summary>
        [JsonProperty("organizations_url")]
        public string Organizations_url { get; set; }

        /// <summary>
        /// Gets or sets the Repository URL of the Person
        /// </summary>
        [JsonProperty("repos_url")]
        public string Repos_url { get; set; }

        /// <summary>
        /// Gets or sets the Event URL of the Person
        /// </summary>
        [JsonProperty("events_url")]
        public string Events_url { get; set; }

        /// <summary>
        /// Gets or sets the Received Events URLs of the Person
        /// </summary>
        [JsonProperty("received_events_url")]
        public string Received_events_url { get; set; }

        /// <summary>
        /// Gets or sets the Type of the Person
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the Site is a AdminSide
        /// </summary>
        [JsonProperty("site_admin")]
        public bool Site_admin { get; set; }
    }
}
