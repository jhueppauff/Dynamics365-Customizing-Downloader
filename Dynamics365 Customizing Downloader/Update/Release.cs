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

    public class Person
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar_url { get; set; }

        [JsonProperty("gravatar_id")]
        public string Gravatar_id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string Html_url { get; set; }

        [JsonProperty("followers_url")]
        public string Followers_url { get; set; }

        [JsonProperty("following_url")]
        public string Following_url { get; set; }

        [JsonProperty("gists_url")]
        public string Gists_url { get; set; }

        [JsonProperty("starred_url")]
        public string Starred_url { get; set; }

        [JsonProperty("subscriptions_url")]
        public string Subscriptions_url { get; set; }

        [JsonProperty("organizations_url")]
        public string Organizations_url { get; set; }

        [JsonProperty("repos_url")]
        public string Repos_url { get; set; }

        [JsonProperty("events_url")]
        public string Events_url { get; set; }

        [JsonProperty("received_events_url")]
        public string Received_events_url { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("site_admin")]
        public bool Site_admin { get; set; }
    }

    public class Author : Person
    {
    }

    public class Uploader : Person
    {
    }

    public class Asset
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lable")]
        public object Label { get; set; }

        [JsonProperty("uploader")]
        public Uploader Uploader { get; set; }

        [JsonProperty("Content_type")]
        public string Content_type { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("download_count")]
        public int Download_count { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created_at { get; set; }

        [JsonProperty("updated_at")]
        public DateTime Updated_at { get; set; }

        [JsonProperty("browser_download_url")]
        public string Browser_download_url { get; set; }
    }

    public class Release
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("assets_url")]
        public string Assets_url { get; set; }

        [JsonProperty("Upload_url")]
        public string Upload_url { get; set; }

        [JsonProperty("html_url")]
        public string Html_url { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("tag_name")]
        public string Tag_name { get; set; }

        [JsonProperty("target_commitish")]
        public string Target_commitish { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("draft")]
        public bool Draft { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("prerelease")]
        public bool Prerelease { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created_at { get; set; }

        [JsonProperty("published_at")]
        public DateTime Published_at { get; set; }

        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }

        [JsonProperty("tarball_url")]
        public string Tarball_url { get; set; }

        [JsonProperty("zipball_url")]
        public string Zipball_url { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}