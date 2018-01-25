﻿//-----------------------------------------------------------------------
// <copyright file="CrmSolution.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Xrm
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
