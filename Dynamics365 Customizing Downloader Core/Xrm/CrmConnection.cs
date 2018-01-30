//-----------------------------------------------------------------------
// <copyright file="CrmConnection.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Xrm
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CRM Connection
    /// </summary>
    public class CrmConnection
    {
        /// <summary>
        /// Gets or sets the ID of the CRM Connection
        /// </summary>
        public Guid ConnectionID { get; set; }

        /// <summary>
        /// Gets or sets the name of the CRM Connection = Organization Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Connection String
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the local path
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the CRM Solutions for this connection
        /// </summary>
        public List<CrmSolution> CRMSolutions { get; set; }
    }
}
