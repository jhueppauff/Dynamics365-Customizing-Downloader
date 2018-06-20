//-----------------------------------------------------------------------
// <copyright file="Connector.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Initializes a new instance of the <see cref="Connector"/> class.
    /// </summary>
    public class Connector
    {
        /// <summary>
        /// Solution XML Relative Path
        /// </summary>
        private const string SolutionXMLPath = @"Other\Solution.xml";

        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the local CRM Solutions
        /// </summary>
        /// <param name="repositoryPath">Path to the directory</param>
        /// <returns>Returns a <see cref="List{Xrm.CrmSolution}"/> with all local Solutions</returns>
        public List<Xrm.CrmSolution> GetLocalCRMSolutions(string repositoryPath)
        {
            List<Xrm.CrmSolution> solutions = new List<Xrm.CrmSolution>();

            if (Directory.Exists(repositoryPath))
            {
                foreach (string solutionFolder in Directory.GetDirectories(repositoryPath))
                {
                    string path = Path.Combine(solutionFolder, SolutionXMLPath);
                    
                    if (File.Exists(path))
                    {
                        XDocument xdoc = XDocument.Load(path);
                        string version = xdoc.Descendants("Version").First().Value;
                        string uniqueName = xdoc.Descendants("UniqueName").First().Value;

                        Xrm.CrmSolution solution = new Xrm.CrmSolution
                        {
                            UniqueName = uniqueName,
                            LocalVersion = version
                        };

                        solutions.Add(solution);
                    }
                }
            }
            else
            {
                FileNotFoundException fileNotFoundException = new FileNotFoundException($"Path does not exist: {Path.Combine(repositoryPath, SolutionXMLPath).ToString() }");
                Log.Error(fileNotFoundException.Message, fileNotFoundException);
            }

            return solutions;
        }
    }
}
