//-----------------------------------------------------------------------
// <copyright file="ConfigurationParameter.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.CIClient
{
    /// <summary>
    /// Class to hold the Parameters needed in <see cref="Program"/>
    /// </summary>
    public class ConfigurationParameter
    {
        /// <summary>
        /// Gets or sets the Parameter Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter
        /// </summary>
        public string Value { get; set; }
    }
}
