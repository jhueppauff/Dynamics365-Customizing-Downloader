//-----------------------------------------------------------------------
// <copyright file="CrmSolution.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingManager.Core.Xrm
{
    using System;
    using System.IO;

    /// <summary>
    /// CRM Solution
    /// </summary>
    public class CrmSolution
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of solution.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id of solution.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the unique.
        /// </summary>
        /// <value>
        /// The name of the unique.
        /// </value>
        public string UniqueName { get; set; }

        /// <summary>
        /// Gets or sets the publisher id.
        /// </summary>
        /// <value>
        /// The publisher id.
        /// </value>
        public Guid PublisherId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Solution should be Downloaded
        /// </summary>
        public bool DownloadIsChecked { get; set; }

        /// <summary>
        /// Gets or sets the Path of the Solution.XML
        /// </summary>
        public string SolutionXMLPath { get; set; }

        /// <summary>
        /// Gets or sets the Solution Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the Local Solution Version
        /// </summary>
        public string LocalVersion { get; set; }

        /// <summary>
        /// Gets or sets the path of the Solution
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the import order
        /// </summary>
        public int ImportOrder { get; set; }
    }
}
