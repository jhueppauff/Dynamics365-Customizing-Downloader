//-----------------------------------------------------------------------
// <copyright file="GitHub.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace UnitTest
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit Test for GitHub related functions
    /// </summary>
    [TestClass]
    public class GitHub
    {
        /// <summary>
        /// GitHub API URL
        /// </summary>
        private const string GitHubApiUrl = "https://api.github.com/repos/jhueppauff/Dynamics365-Customizing-Downloader";

        /// <summary>
        /// Validates if the Release Information can be retrieved
        /// </summary>
        [TestMethod]
        public async Task GithubRetrieveReleaseInfo()
        {
            Dynamics365CustomizingDownloader.Core.Update.UpdateChecker updateChecker = new Dynamics365CustomizingDownloader.Core.Update.UpdateChecker();
            var release = await updateChecker.GetReleaseInfo(GitHubApiUrl).ConfigureAwait(false);

            release.Should().NotBeNull();

            release.Name.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// Validates if the Update Check works
        /// </summary>
        [TestMethod]
        public async Task GitHubUpdateCheck()
        {
            Dynamics365CustomizingDownloader.Core.Update.UpdateChecker updateChecker = new Dynamics365CustomizingDownloader.Core.Update.UpdateChecker();
            (await updateChecker.IsUpdateAvailable(GitHubApiUrl).ConfigureAwait(false)).Should();
        }

        /// <summary>
        /// Validates the Update Url retrieve
        /// </summary>
        [TestMethod]
        public async Task GetUpdateUrl()
        {
            Dynamics365CustomizingDownloader.Core.Update.UpdateChecker updateChecker = new Dynamics365CustomizingDownloader.Core.Update.UpdateChecker();
            Uri updateUrl = await updateChecker.GetUpdateURL(GitHubApiUrl).ConfigureAwait(false);

            // ToDo validate Url/Output
        }
    }
}