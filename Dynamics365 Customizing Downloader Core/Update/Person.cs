//-----------------------------------------------------------------------
// <copyright file="Person.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 julian
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Update
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
