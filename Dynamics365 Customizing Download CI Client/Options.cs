//-----------------------------------------------------------------------
// <copyright file="Options.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------



namespace Dynamics365CustomizingDownload.CIClient
{
    using CommandLine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Options
    {
        [Option('c', "Connection Name", Required = true, HelpText = "Name of the Connection")]
        public string ConnectionName { get; set; }

    }
}
