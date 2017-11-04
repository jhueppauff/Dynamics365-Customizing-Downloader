//-----------------------------------------------------------------------
// <copyright file="ToolingConnector.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.xrm
{
    using System;
    using Microsoft.Xrm.Tooling.Connector;
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// XRM/CRM Tooling Connector
    /// </summary>
    public class ToolingConnector
    {
        /// <summary>
        /// Connect to crm and get the crm service client
        /// </summary>
        /// <param name="connectionString">XRM Connection String</param>
        /// <returns>Returns <see cref="CrmServiceClient"/></returns>
        public CrmServiceClient GetCrmServiceClient (string connectionString)
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

        public List<CrmSolution> GetCrmSolutions (CrmServiceClient crmServiceClient)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = "solution",
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression()
            };

            EntityCollection result = crmServiceClient.RetrieveMultiple(query);
            List<CrmSolution> SolutionList = new List<CrmSolution>();

            foreach (var solution in result.Entities)
            {
                if (solution["uniquename"].ToString() != "System" && solution["uniquename"].ToString() != "Active" && solution["uniquename"].ToString() != "Basic" && solution["uniquename"].ToString() != "ActivityFeedsCore")
                {
                    SolutionList.Add(
                        new CrmSolution()
                        {
                            Id = (Guid)solution["solutionid"],
                            Name = solution["friendlyname"].ToString(),
                            PublisherId = ((EntityReference)solution["publisherid"]).Id,
                            UniqueName = solution["uniquename"].ToString()
                        });
                }
            }

            return SolutionList;
        }
    }
}
