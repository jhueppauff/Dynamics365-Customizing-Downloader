//-----------------------------------------------------------------------
// <copyright file="Asset.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Update
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Asset JSON Class for GitHub API
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Gets or sets the URL of the Asset
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ID of the Asset
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Asset
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Label of the Asset
        /// </summary>
        [JsonProperty("lable")]
        public object Label { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Uploader"/> Uploader of the Asset
        /// </summary>
        [JsonProperty("uploader")]
        public Uploader Uploader { get; set; }

        /// <summary>
        /// Gets or sets the Content Type of the Asset
        /// </summary>
        [JsonProperty("Content_type")]
        public string Content_type { get; set; }

        /// <summary>
        /// Gets or sets the State of the Asset
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the Size of the Asset
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the Download Count of the Asset
        /// </summary>
        [JsonProperty("download_count")]
        public int Download_count { get; set; }

        /// <summary>
        /// Gets or sets the Asset created at
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime Created_at { get; set; }

        /// <summary>
        /// Gets or sets the Asset uploaded at
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime Updated_at { get; set; }

        /// <summary>
        /// Gets or sets the Browser Download URL of the Asset
        /// </summary>
        [JsonProperty("browser_download_url")]
        public string Browser_download_url { get; set; }
    }
}
