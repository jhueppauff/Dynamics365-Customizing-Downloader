//-----------------------------------------------------------------------
// <copyright file="Connector.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Repository
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
            else
            {
                FileNotFoundException fileNotFoundException = new FileNotFoundException($"Path does not exist: {Path.Combine(repositoryPath, SolutionXMLPath).ToString() }");
                Log.Error(fileNotFoundException.Message, fileNotFoundException);
            }

            return solutions;
        }
    }
}
