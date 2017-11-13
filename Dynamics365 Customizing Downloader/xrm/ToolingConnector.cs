//-----------------------------------------------------------------------
// <copyright file="ToolingConnector.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Xrm
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Tooling.Connector;
    using Microsoft.Win32.SafeHandles;
    using System.Runtime.InteropServices;

    /// <summary>
    /// XRM/CRM Tooling Connector
    /// </summary>
    public class ToolingConnector : IDisposable
    {
        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Instantiate a SafeHandle instance.
        /// </summary>
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Connect to CRM and get the CRM service client
        /// </summary>
        /// <param name="connectionString">XRM Connection String</param>
        /// <returns>Returns <see cref="CrmServiceClient"/></returns>
        public CrmServiceClient GetCrmServiceClient(string connectionString)
        {
            CrmServiceClient crmServiceClient;
            try
            {
                crmServiceClient = new CrmServiceClient(connectionString);
                if (crmServiceClient.ConnectedOrgFriendlyName != string.Empty && crmServiceClient.ConnectedOrgFriendlyName != null)
                {
                    return crmServiceClient;
                }
                else
                {
                    // CRM Client is empty
                    throw new NullReferenceException("CRM Service Client is empty");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all installed CRM Solutions
        /// </summary>
        /// <param name="crmServiceClient">CRM Service Client <see cref="CrmServiceClient"/></param>
        /// <returns>Returns <see cref="List{CrmSolution}"/></returns>
        public List<CrmSolution> GetCrmSolutions(CrmServiceClient crmServiceClient)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = "solution",
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };

            EntityCollection result = crmServiceClient.RetrieveMultiple(query);
            List<CrmSolution> solutionList = new List<CrmSolution>();

            foreach (var solution in result.Entities)
            {
                if (solution["uniquename"].ToString() != "System" && solution["uniquename"].ToString() != "Active" && solution["uniquename"].ToString() != "Basic" && solution["uniquename"].ToString() != "ActivityFeedsCore")
                {
                    if (solution["uniquename"].ToString() != "" && solution["friendlyname"].ToString() != "")
                    {
                        solutionList.Add(
                        new CrmSolution()
                        {
                            Id = (Guid)solution["solutionid"],
                            Name = solution["friendlyname"].ToString(),
                            PublisherId = ((EntityReference)solution["publisherid"]).Id,
                            UniqueName = solution["uniquename"].ToString()
                        });
                    }
                }
            }

            return solutionList;
        }

        /// <summary>
        /// Downloads the CRM Solution
        /// </summary>
        /// <param name="crmServiceClient">CRM Service Client</param>
        /// <param name="crmSolutionName">CRM Solution Name</param>
        /// <param name="filePath">File Path</param>
        public void DownloadSolution(CrmServiceClient crmServiceClient, string crmSolutionName, string filePath)
        {
            ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest
            {
                Managed = false,
                SolutionName = crmSolutionName
            };

            ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)crmServiceClient.Execute(exportSolutionRequest);

            byte[] exportXml = exportSolutionResponse.ExportSolutionFile;
            string filename = Path.Combine(filePath, crmSolutionName + ".zip");
            File.WriteAllBytes(filename, exportXml);
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}