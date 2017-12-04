using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Dynamics365CustomizingDownloader.Patcher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Retrieving Updates from Github");
            UpdateChecker updateChecker = new UpdateChecker();

            if (updateChecker.IsUpdateAvailable())
            {
                LogToUI($"Found Update : {updateChecker.GetVersion()}");

                LogToUI($"Get Download URL : {updateChecker.GetUpdateURL()}");

                if (LogToUI("Are you sure to download this patch", true, "Type 'y' to download this patch") == "y")
                {
                    string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory());
                    foreach (string item in fileEntries)
                    {
                        if (!item.Contains("Dyn365_Configuration.json"))
                        {
                            File.Move(item, item + ".prepatch");
                        }
                    }

                    using (var client = new WebClient())
                    {
                        client.DownloadFile(updateChecker.GetUpdateURL(), "Dynamics365-Customizing-Downloader.zip");
                    }

                    try
                    {
                        using (ZipFile zip = ZipFile.Read("Dynamics365-Customizing-Downloader.zip"))
                        {
                            foreach (ZipEntry item in zip)
                            {
                                item.Extract(Directory.GetCurrentDirectory(), ExtractExistingFileAction.OverwriteSilently);
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        // ToDo: rename existing file back to old name
                    }

                        
                
                }
            }
            else
            {
                LogToUI("No Update Found!", true);
            }
        }

        private static string LogToUI(string message, bool waitForUserImput = false, string customUserInputMessage = "Press any key to continue.")
        {
            Console.WriteLine(message);

            if (waitForUserImput)
            {
                Console.WriteLine(customUserInputMessage);
                string input = Console.ReadLine();
                return input;
            }
            else
            {
                return null;
            }
        }
    }
}
