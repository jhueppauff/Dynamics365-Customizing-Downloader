//-----------------------------------------------------------------------
// <copyright file="CrmSolutionPackager.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Xrm
{
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Extract CRM Solution see also: <seealso href="https://msdn.microsoft.com/en-us/library/jj602987.aspx"/> 
    /// </summary>
    public class CrmSolutionPackager
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Extract the CRM Solution
        /// </summary>
        /// <param name="path">Path of the Solution</param>
        /// <param name="extractFolder">Path to the extraction Folder</param>
        /// <param name="logPath">Path to save the Packager Logs to</param>
        /// <param name="localize">Defines if the Packager should extract the labels into resource Files</param>
        /// <returns>Returns the log Messages</returns>
        public string ExtractCustomizing(string path, string extractFolder, string logPath = null, bool localize = false)
        {
            string log = string.Empty;

            if (!File.Exists(path))
            {
                CrmSolutionPackager.Log.Error("Assumed solution file not found: " + path, new FileNotFoundException("Assumed solution file not found: " + path));
                throw new FileNotFoundException("Assumed solution file not found: " + path);
            }

            // Start Solution Packager
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "SolutionPackager.exe",
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $"/action:Extract /zipfile:{path} /folder:{extractFolder} /packagetype:Unmanaged /allowWrite:yes /allowDelete:yes /clobber /nologo"
            };

            // Optional Parameters
            if (!string.IsNullOrEmpty(logPath))
            {
                startInfo.Arguments = startInfo.Arguments + " /log: " + logPath;
            }

            if (localize)
            {
                startInfo.Arguments = startInfo.Arguments + " /sourceLoc:auto /localize";
            }

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    log += exeProcess.StandardOutput.ReadToEnd();
                    exeProcess.WaitForExit();
                }

                return log;
            }
            catch (System.Exception ex)
            {
                CrmSolutionPackager.Log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
