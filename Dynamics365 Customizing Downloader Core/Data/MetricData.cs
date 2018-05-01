//-----------------------------------------------------------------------
// <copyright file="MetricData.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Data
{
    using System;

    public class MetricData
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        public Guid ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the component identifier.
        /// </summary>
        public Guid ComponentId { get; set; }
    }
}
