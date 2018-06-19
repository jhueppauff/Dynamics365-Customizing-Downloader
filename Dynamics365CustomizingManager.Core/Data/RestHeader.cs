//-----------------------------------------------------------------------
// <copyright file="RestHeader.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Data
{
    /// <summary>
    /// Rest Header
    /// </summary>
    public class RestHeader
    {
        /// <summary>
        /// Gets or sets the name of the key.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets the key value.
        /// </summary>
        public string KeyValue { get; set; }
    }
}
