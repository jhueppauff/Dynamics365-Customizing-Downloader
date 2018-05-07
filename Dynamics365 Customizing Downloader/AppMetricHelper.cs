//-----------------------------------------------------------------------
// <copyright file="AppMetricHelper.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Dynamics365CustomizingDownloader.Core.Data;

    /// <summary>
    /// AppMetric Client
    /// </summary>
    public class AppMetricHelper
    {
        /// <summary>
        /// The query string
        /// </summary>
        private const string QueryString = "SELECT SerialNumber FROM Win32_OperatingSystem";

        /// <summary>
        /// Identifier of the Application
        /// </summary>
        private readonly string applicationId;

        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The API key
        /// </summary>
        private readonly string apiKey;

        /// <summary>
        /// The API identifier
        /// </summary>
        private readonly string apiId;

        /// <summary>
        /// The session identifier
        /// </summary>
        private readonly Guid sessionId;

        /// <summary>
        /// The client identifier
        /// </summary>
        private readonly string clientId;

        /// <summary>
        /// The product identifier
        /// </summary>
        private readonly string productId = (from ManagementObject managementObject in new ManagementObjectSearcher(QueryString).Get()
                            from PropertyData propertyData in managementObject.Properties
                            where propertyData.Name == "SerialNumber"
                            select (string)propertyData.Value).FirstOrDefault();

        /// <summary>
        /// OS Version Name
        /// </summary>
        private readonly string osName;

        /// <summary>
        /// Application Version
        /// </summary>
        private readonly string version;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppMetricHelper"/> class.
        /// </summary>
        public AppMetricHelper()
        {
            this.applicationId = Properties.Settings.Default.AppMetricAppId;
            this.apiId = Properties.Settings.Default.AppMetricApiId;
            this.apiKey = Properties.Settings.Default.AppMetricApiKey;
            this.sessionId = Guid.NewGuid();
            this.version = $"v.{ Assembly.GetEntryAssembly().GetName().Version}";
            this.osName = this.GetWindowsFriendlyName();

            this.clientId = this.CalculateMD5Hash(this.productId);
        }

        /// <summary>
        /// Tracks the fatal exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>Returns <see cref="Task"/></returns>
        public async Task TrackFatalException(Exception exception)
        {
            RestSharp.IRestResponse response = null;

            try
            {
                RestClient restClient = new RestClient(Properties.Settings.Default.RestTimeout);
                RestHeader[] restHeaders = new RestHeader[6];

                restHeaders[0] = new RestHeader() { KeyName = "apiKey", KeyValue = this.apiKey };
                restHeaders[1] = new RestHeader() { KeyName = "apiId", KeyValue = this.apiId };
                restHeaders[2] = new RestHeader() { KeyName = "clientId", KeyValue = this.clientId };
                restHeaders[3] = new RestHeader() { KeyName = "applicationId", KeyValue = this.applicationId };
                restHeaders[4] = new RestHeader() { KeyName = "Content-Type", KeyValue = "application/json" };
                restHeaders[5] = new RestHeader() { KeyName = "versionId", KeyValue = this.version };

                string body = Newtonsoft.Json.JsonConvert.SerializeObject(exception);

                response = await restClient.ExecuteRestRequest(Properties.Settings.Default.AppMetricEndpoint + "/ReportError", restHeaders, body, RestSharp.Method.POST);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                if (response != null && !response.IsSuccessful)
                {
                    log.Error(response.ErrorException.Message, response.ErrorException);
                }
            }
        }

        public async Task ReportUsage(string componentId = null)
        {
            if (componentId == null)
            {
                componentId = Properties.Settings.Default.AppMetricBaseCompId;
            }
            RestSharp.IRestResponse response = null;
            try
            {
                 RestClient restClient = new RestClient(Properties.Settings.Default.RestTimeout);
                RestHeader[] restHeaders = new RestHeader[3];

                restHeaders[0] = new RestHeader() { KeyName = "Content-Type", KeyValue = "application/json" };
                restHeaders[1] = new RestHeader() { KeyName = "apiKey", KeyValue = apiKey };
                restHeaders[2] = new RestHeader() { KeyName = "apiId", KeyValue = apiId };
                

                MetricData metricData = new MetricData()
                {
                    ApplicationId = this.applicationId,
                    ClientId = this.clientId,
                    ComponentId = componentId,
                    SessionId = this.sessionId.ToString(),
                    VersionId = this.version
                };

                string body = Newtonsoft.Json.JsonConvert.SerializeObject(metricData).ToString();

                response = await restClient.ExecuteRestRequest(Properties.Settings.Default.AppMetricEndpoint + "/ReportUsage", restHeaders, body, RestSharp.Method.POST);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                if (response != null && !response.IsSuccessful)
                {
                    log.Error(response.ErrorException.Message, response.ErrorException);
                }
            }
        }

        public void FlushData()
        {

        }

        /// <summary>
        /// Query the WMI for the Windows OS Name
        /// </summary>
        /// <returns>Returns Name of the OS</returns>
        private string GetWindowsFriendlyName()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        public string CalculateMD5Hash(string input)
        {
            string salt = System.Environment.MachineName;

            var provider = MD5.Create();

            byte[] bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + input));
            string computedHash = BitConverter.ToString(bytes);

            return computedHash;
        }
    }
}
