

namespace Dynamics365CustomizingDownloader.xrm
{
    using System.Diagnostics;
    using System.IO;

    // Extract CRM Solution see also: https://msdn.microsoft.com/en-us/library/jj602987.aspx
    public class CrmSolutionPackager
    {

        public void ExtractCustomizing(string path, string extractFolder)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Assumed solution file not found: " + path);
            }

            
            // Start Solution Packager
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "SolutionPackager.exe",
                WindowStyle = ProcessWindowStyle.Normal,
                Arguments = $"/action:Extract /zipfile:{path} /folder:{extractFolder} /packagetype:Unmanaged /allowWrite:yes /allowDelete:yes /clobber /nologo"
            };

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (System.Exception)
            {

                throw;
            }

        }

    }
}
