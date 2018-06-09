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

        [Option('s', "Solution Name", Required = true, HelpText = "Name of the Solution to Download")]
        public string SolutionName { get; set; }

        [Option('e', "Extract String", Required = false, HelpText = "Specify if you want to extract strings into ressource Files" )]
        public bool ExtractStrings { get; set; }

        [Option('a', "Action", Required = true, HelpText = "Specify the action to run, Download, Upload, DownloadAndExtract")]
        public string Action { get; set; }
    }
}
