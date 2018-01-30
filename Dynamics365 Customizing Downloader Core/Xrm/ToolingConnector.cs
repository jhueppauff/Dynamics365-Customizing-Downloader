//-----------------------------------------------------------------------
// <copyright file="ToolingConnector.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Xrm
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Crm.Sdk.Messages;
    using Microsoft.Win32.SafeHandles;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Tooling.Connector;

    /// <summary>
    /// XRM/CRM Tooling Connector
    /// </summary>
    public class ToolingConnector : IDisposable
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Instantiate a SafeHandle instance.
        /// </summary>
        private readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Tests the CRM Connection
        /// </summary>
        /// <param name="connectionString">CRM Connection String</param>
        /// <returns>Returns <see cref="bool"/> if the connection succeeded.</returns>
        public bool TestCRMConnection(string connectionString)
        {
            CrmServiceClient crmServiceClient = null;

            try
            {
                crmServiceClient = new CrmServiceClient(connectionString);

                if (crmServiceClient.IsReady)
                {
                    return true;
                }
                else
                {
                    CrmConnectionException connectionException = new CrmConnectionException(crmServiceClient.LastCrmError, crmServiceClient.LastCrmException.InnerException);
                    throw connectionException;
                }
            }
            catch (Exception ex)
            {
                ToolingConnector.Log.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                if (crmServiceClient != null)
                {
                    crmServiceClient.Dispose();
                }
            }
        }

        /// <summary>
        /// Uploads the <see cref="List{CrmSolution}"/> to CRM
        /// </summary>
        /// <param name="crmSolution">CRM Solutions to Upload</param>
        /// <param name="crmServiceClient">CRM Service Client to connect to CRM. <see cref="CrmServiceClient"/></param>
        /// <param name="overwriteCustomizing">Defines if the customizing should be overwritten</param>
        /// <param name="convertToManaged">Defines if components should be converted to manage</param>
        /// <param name="publishWorkflow">Defines if workflows should be published after import</param>
        public void UploadCrmSolution(CrmSolution crmSolution, CrmServiceClient crmServiceClient, bool overwriteCustomizing = false, bool convertToManaged = false, bool publishWorkflow = true)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(crmSolution.LocalPath);

                ImportSolutionRequest importSolutionRequest = new ImportSolutionRequest()
                {
                    CustomizationFile = fileBytes,
                    ImportJobId = Guid.NewGuid(),
                    OverwriteUnmanagedCustomizations = overwriteCustomizing,
                    ConvertToManaged = convertToManaged,
                    PublishWorkflows = publishWorkflow
                };

                crmServiceClient.Execute(importSolutionRequest);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Connect to CRM and get the CRM service client
        /// </summary>
        /// <param name="connectionString">XRM Connection String</param>
        /// <returns>Returns <see cref="CrmServiceClient"/>Returns the <see cref="Microsoft.Xrm.Tooling.Connector.CrmServiceClient"/></returns>
        public Microsoft.Xrm.Tooling.Connector.CrmServiceClient GetCrmServiceClient(string connectionString)
        {
            try
            {
                CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
                if (crmServiceClient.ConnectedOrgFriendlyName != string.Empty && crmServiceClient.ConnectedOrgFriendlyName != null)
                {
                    return crmServiceClient;
                }
                else
                {
                    // CRM Client is empty
                    if (crmServiceClient.LastCrmException != null)
                    {
                        throw new CrmConnectionException(crmServiceClient.LastCrmError, crmServiceClient.LastCrmException);
                    }
                    else
                    {
                        throw new CrmConnectionException(crmServiceClient.LastCrmError);
                    }
                }
            }
            catch (Exception ex)
            {
                ToolingConnector.Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Get all installed CRM Solutions
        /// </summary>
        /// <param name="crmServiceClient">CRM Service Client <see cref="CrmServiceClient"/></param>
        /// <param name="onlyUnmanaged">Indicates if only Unmanaged Solutions should be returned</param>
        /// <returns>Returns <see cref="List{CrmSolution}"/></returns>
        public List<CrmSolution> GetCrmSolutions(CrmServiceClient crmServiceClient, bool onlyUnmanaged = true)
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
                if (solution["uniquename"].ToString() != string.Empty && !(bool)solution["ismanaged"] && solution["friendlyname"].ToString() != string.Empty && solution["uniquename"].ToString() != "System" && solution["uniquename"].ToString() != "Active" && solution["uniquename"].ToString() != "Basic" && solution["uniquename"].ToString() != "ActivityFeedsCore")
                {
                    solutionList.Add(
                    new CrmSolution()
                    {
                        Id = (Guid)solution["solutionid"],
                        Name = solution["friendlyname"].ToString(),
                        PublisherId = ((EntityReference)solution["publisherid"]).Id,
                        UniqueName = solution["uniquename"].ToString(),
                        Version = solution["version"].ToString()
                    });
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
        /// <param name="isManaged">Defines if the Solution should be downloaded Managed</param>
        public void DownloadSolution(CrmServiceClient crmServiceClient, string crmSolutionName, string filePath, bool isManaged = false)
        {
            ExportSolutionRequest exportSolutionRequest = new ExportSolutionRequest
            {
                Managed = isManaged,
                SolutionName = crmSolutionName
            };

            ExportSolutionResponse exportSolutionResponse = (ExportSolutionResponse)crmServiceClient.Execute(exportSolutionRequest);

            byte[] exportXml = exportSolutionResponse.ExportSolutionFile;

            string filename;
            if (isManaged)
            {
                filename = Path.Combine(filePath, crmSolutionName + "_managed.zip");
            }
            else
            {
                filename = Path.Combine(filePath, crmSolutionName + ".zip");
            }

            File.WriteAllBytes(filename, exportXml);
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing">Identifies if the class is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.handle.Dispose();

                // Free any other managed objects here.
            }

            // Free any unmanaged objects here.
            this.disposed = true;
        }
    }
}