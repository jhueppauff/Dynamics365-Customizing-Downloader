//-----------------------------------------------------------------------
// <copyright file="Asset.cs" company="None">
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
