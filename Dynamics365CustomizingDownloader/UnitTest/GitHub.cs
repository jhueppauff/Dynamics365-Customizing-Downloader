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
        public void GithubRetrieveReleaseInfo()
        {
            Dynamics365CustomizingDownloader.Core.Update.UpdateChecker updateChecker = new Dynamics365CustomizingDownloader.Core.Update.UpdateChecker();
            var release = updateChecker.GetReleaseInfo(GitHubApiUrl);

            if (release == null || release.Name == null)
            {
                throw new Exception("Release retrieve failed.");
            }
        }

        /// <summary>
        /// Validates if the Update Check works
        /// </summary>
        [TestMethod]
        public void GitHubUpdateCheck()
        {
            Dynamics365CustomizingDownloader.Core.Update.UpdateChecker updateChecker = new Dynamics365CustomizingDownloader.Core.Update.UpdateChecker();
            bool updateAvailable = updateChecker.IsUpdateAvailable(GitHubApiUrl);
        }

        /// <summary>
        /// Validates the Update Url retrieve
        /// </summary>
        [TestMethod]
        public void GetUpdateUrl()
        {
            Dynamics365CustomizingDownloader.Core.Update.UpdateChecker updateChecker = new Dynamics365CustomizingDownloader.Core.Update.UpdateChecker();
            Uri updateUrl = updateChecker.GetUpdateURL(GitHubApiUrl);

            // ToDo validate Url/Output
        }
    }
}