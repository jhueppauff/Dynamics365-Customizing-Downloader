//-----------------------------------------------------------------------
// <copyright file="AppMetricHelper.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    public class AppMetricHelper
    {
        /// <summary>
        /// Identifier of the Application
        /// </summary>
        private readonly string applicationId;

        public AppMetricHelper(string applicationId)
        {
            this.applicationId = applicationId;
        }
    }
}
