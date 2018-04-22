//-----------------------------------------------------------------------
// <copyright file="MetricClient.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Metric
{
    using Microsoft.Win32;
    using System.Globalization;
    using System.Management;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Metric Client
    /// </summary>
    public class MetricClient
    {
        /// <summary>
        /// The API key
        /// </summary>
        private readonly string apiKey;

        /// <summary>
        /// The API identifier
        /// </summary>
        private readonly string apiId;

        /// <summary>
        /// The endpoint base URL
        /// </summary>
        private readonly string endpointBaseUrl;

        private readonly string clientId;

        private const string applicationId = "26AA1555-A88A-49B1-9A3E-499C275ECD50";

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        /// <param name="apiId">The API identifier.</param>
        /// <param name="endpointBaseUrl">The endpoint base URL.</param>
        public MetricClient(string apiKey, string apiId, string endpointBaseUrl)
        {
            this.apiId = apiId;
            this.apiKey = apiKey;
            this.endpointBaseUrl = endpointBaseUrl;
            this.clientId = GetClientId();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricClient"/> class.
        /// </summary>
        public MetricClient()
        {
        }

        /// <summary>
        /// Reports the usage.
        /// </summary>
        /// <param name="componentId">Id of the module/component.</param>
        /// <returns></returns>
        public async Task ReportUsage(string componentId)
        {
            Data.RestClient restClient = new Data.RestClient();

            Data.RestHeader[] restHeaders = new Data.RestHeader[5];

            restHeaders[0] = new Data.RestHeader() { KeyName = "apiKey", KeyValue = apiKey };
            restHeaders[1] = new Data.RestHeader() { KeyName = "apiId", KeyValue = apiId };
            restHeaders[2] = new Data.RestHeader() { KeyName = "applicationId", KeyValue = applicationId };
            restHeaders[3] = new Data.RestHeader() { KeyName = "componentId", KeyValue = componentId };
            restHeaders[4] = new Data.RestHeader() { KeyName = "clientId", KeyValue = clientId };

            RestSharp.IRestResponse restResponse = await restClient.ExecuteRestRequest(endpointBaseUrl + "/metric/ReportUsage", restHeaders, null, RestSharp.Method.POST);

            if (!restResponse.IsSuccessful)
            {
                // ToDo: Replace with custom exception
                throw new System.Exception(restResponse.ErrorMessage, restResponse.ErrorException);
            }
        }

        /// <summary>
        /// Gets the Microsft Windows Product Id
        /// </summary>
        /// <returns></returns>
        public string GetClientId()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            
            return key.GetValue("ProductId").ToString();
        }
    }
}
