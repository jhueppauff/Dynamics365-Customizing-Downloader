//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 julian
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.CIClient
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.Xrm.Tooling.Connector;

    /// <summary>
    /// Command Line Main Class
    /// </summary>
    public class Program : IDisposable
    {
        /// <summary>
        /// Runtime Parameters
        /// </summary>
        private static List<ConfigurationParameter> parameters = new List<ConfigurationParameter>();

        /// <summary>
        /// To detect redundant dispose calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Main logic for the command line
        /// </summary>
        /// <param name="args">Command Line Args</param>
        public static void Main(string[] args)
        {
            GetParameter(args);
            Core.Xrm.ToolingConnector toolingConnector = null;
            try
            {
                toolingConnector = new Core.Xrm.ToolingConnector();
                using (CrmServiceClient client = toolingConnector.GetCrmServiceClient(parameters.SingleOrDefault(x => x.Name == "connection-string").Value))
                {
                    if (client.IsReady)
                    {
                        string solutionName = parameters.SingleOrDefault(x => x.Name == "solution-name").Value;
                        string localPath = parameters.SingleOrDefault(x => x.Name == "local-path").Value;

                        toolingConnector.DownloadSolution(client, solutionName, localPath);
                        Console.WriteLine("Download completed: " + Path.Combine(localPath, solutionName + ".zip").ToString());

                        if (Convert.ToBoolean(parameters.SingleOrDefault(x => x.Name == "action").Value))
                        {
                            // Extract Solution
                            Core.Xrm.CrmSolutionPackager packager = new Core.Xrm.CrmSolutionPackager();
                            string log = packager.ExtractCustomizing(Path.Combine(localPath, solutionName + ".zip").ToString(), localPath);

                            Console.Write(log);
                        }
                    }
                }

                toolingConnector.Dispose();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.Write(ex.StackTrace);
                Console.ResetColor();
                Console.ReadLine();
            }
            finally
            {
                if (toolingConnector != null)
                {
                    ((IDisposable)toolingConnector).Dispose();
                }
            }
        }

        /// <summary>
        ///  This code added to correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose logic
        /// </summary>
        /// <param name="disposing">if dispose is already triggered</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // Noting to dispose yet
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Extracts the Parameter from the command line
        /// </summary>
        /// <param name="args">Command Line Parameters</param>
        private static void GetParameter(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Missing Parameters");
                    Console.ResetColor();
                }

                int counter = 0;
                ConfigurationParameter parameter;
                foreach (string arg in args)
                {
                    if (counter == 0 || (counter % 2) == 0)
                    {
                        parameter = new ConfigurationParameter();

                        switch (arg.ToLowerInvariant())
                        {
                            case "--connection-string":
                                parameter.Name = arg.Remove(0, 2).ToLowerInvariant();
                                if (args[counter + 1].ToString().StartsWith("--") || args[counter + 1] == null)
                                {
                                    LogError("Missing Parameter Value : " + arg);
                                }
                                else
                                {
                                    parameter.Value = args[counter + 1];
                                }

                                break;
                            case "--solution-name":
                                parameter.Name = arg.Remove(0, 2).ToLowerInvariant();
                                if (args[counter + 1].ToString().StartsWith("--") || args[counter + 1] == null)
                                {
                                    LogError("Missing Parameter Value : " + arg);
                                }
                                else
                                {
                                    parameter.Value = args[counter + 1];
                                }

                                break;
                            case "--local-path":
                                parameter.Name = arg.Remove(0, 2).ToLowerInvariant();
                                if (args[counter + 1].ToString().StartsWith("--") || args[counter + 1] == null)
                                {
                                    LogError("Missing Parameter Value : " + arg);
                                }
                                else
                                {
                                    parameter.Value = args[counter + 1];
                                }

                                break;
                            case "--action":
                                parameter.Name = arg.Remove(0, 2).ToLowerInvariant();
                                if (args[counter + 1].ToString().StartsWith("--") || args[counter + 1] == null)
                                {
                                    parameter.Value = "false";
                                }
                                else
                                {
                                    parameter.Value = args[counter + 1];
                                }

                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Ignoring :" + arg);
                                break;
                        }

                        if (parameter.Value != null && parameter.Value != string.Empty)
                        {
                            parameters.Add(parameter);
                        }
                    }

                    counter++;
                }
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message);
                LogError(ex.StackTrace);
            }
        }

        /// <summary>
        /// Logs an error to console
        /// </summary>
        /// <param name="message">Error Message to show</param>
        private static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ResetColor();
        }
    }
}
