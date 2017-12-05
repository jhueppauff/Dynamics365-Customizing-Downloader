//-----------------------------------------------------------------------
// <copyright file="CrmSolutionPackager.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Xrm
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
        /// <returns>Returns the log Messages</returns>
        public string ExtractCustomizing(string path, string extractFolder)
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

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    DownloadMultiple.UpdateUI(exeProcess.StandardOutput.ReadToEnd(), false);
                    exeProcess.WaitForExit();
                    return log;
                }
            }
            catch (System.Exception ex)
            {
                CrmSolutionPackager.Log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
