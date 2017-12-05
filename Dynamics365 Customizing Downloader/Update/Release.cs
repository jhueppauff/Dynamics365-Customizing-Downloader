//-----------------------------------------------------------------------
// <copyright file="Release.cs" company="None">
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
    using Newtonsoft.Json;

    /// <summary>
    /// Release JSON Class for GitHub API
    /// </summary>
    public class Release
    {
        /// <summary>
        /// Gets or sets the URL of the Release
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Url of the Assets
        /// </summary>
        [JsonProperty("assets_url")]
        public string Assets_url { get; set; }

        /// <summary>
        /// Gets or sets the Upload URL
        /// </summary>
        [JsonProperty("Upload_url")]
        public string Upload_url { get; set; }

        /// <summary>
        /// Gets or sets the HTML URL of the Release
        /// </summary>
        [JsonProperty("html_url")]
        public string Html_url { get; set; }

        /// <summary>
        ///  Gets or sets the Release ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Release Tag
        /// </summary>
        [JsonProperty("tag_name")]
        public string Tag_name { get; set; }

        /// <summary>
        /// Gets or sets the Target commit
        /// </summary>
        [JsonProperty("target_commitish")]
        public string Target_commitish { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Release
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Release is draft?
        /// </summary>
        [JsonProperty("draft")]
        public bool Draft { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Author"/> Author of the Release
        /// </summary>
        [JsonProperty("author")]
        public Author Author { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Release is a PreRelease?
        /// </summary>
        [JsonProperty("prerelease")]
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the when the release was created
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime Created_at { get; set; }

        /// <summary>
        /// Gets or sets the when the Release was published
        /// </summary>
        [JsonProperty("published_at")]
        public DateTime Published_at { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="List{Asset}"/> of all Assets of the Release
        /// </summary>
        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }

        /// <summary>
        /// Gets or sets the Tar ball URL
        /// </summary>
        [JsonProperty("tarball_url")]
        public string Tarball_url { get; set; }

        /// <summary>
        /// Gets or sets the Zip ball URL
        /// </summary>
        [JsonProperty("zipball_url")]
        public string Zipball_url { get; set; }

        /// <summary>
        /// Gets or sets the Release Body
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}