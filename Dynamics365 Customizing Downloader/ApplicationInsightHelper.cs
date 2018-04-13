//-----------------------------------------------------------------------
// <copyright file="ApplicationInsightHelper.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
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
    using Microsoft.ApplicationInsights;

    /// <summary>
    /// Helper Class for AI
    /// </summary>
    public class ApplicationInsightHelper
    {
        /// <summary>
        /// AI Telemetry Client
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Unique Session Key
        /// </summary>
        private string sessionKey;

        /// <summary>
        /// OS Version Name
        /// </summary>
        private string osName;

        /// <summary>
        /// Application Version
        /// </summary>
        private string version;

        /// <summary>
        /// Name of the Application
        /// </summary>
        private string application;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightHelper"/> class.
        /// </summary>
        public ApplicationInsightHelper()
        {
            this.telemetryClient = new TelemetryClient() { InstrumentationKey = Properties.Settings.Default.AppInsights };
            this.GatherDetails();
            this.SetupTelemetry();
        }

        /// <summary>
        /// Tracks the Page Views
        /// </summary>
        /// <param name="pageName">Name of the Page</param>
        public void TrackPageView(string pageName)
        {
            this.telemetryClient.TrackPageView(pageName);
        }

        /// <summary>
        /// Tracks NonFatal Exception
        /// </summary>
        /// <param name="ex">Exception to Track</param>
        public void TrackNonFatalExceptions(Exception ex)
        {
            IDictionary<string, double> metric = new Dictionary<string, double>
            {
                { "Non-fatal Exception", 1 }
            };

            this.telemetryClient.TrackException(ex, null, metric);
        }

        /// <summary>
        /// Tracks Fatal Exception
        /// </summary>
        /// <param name="ex">Exception to Track</param>
        public void TrackFatalException(Exception ex)
        {
            var exceptionTelemetry = new Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry(new Exception())
            {
                HandledAt = Microsoft.ApplicationInsights.DataContracts.ExceptionHandledAt.Unhandled
            };

            this.telemetryClient.TrackException(exceptionTelemetry);
        }

        /// <summary>
        /// Flushes the Telemetry Client
        /// </summary>
        public void FlushData()
        {
            this.telemetryClient.Flush();
        }

        /// <summary>
        /// Populates the variables
        /// </summary>
        private void GatherDetails()
        {
            this.sessionKey = Guid.NewGuid().ToString();
            this.osName = this.GetWindowsFriendlyName();
            this.version = $"v.{ Assembly.GetEntryAssembly().GetName().Version}";
            this.application = $"{ Assembly.GetEntryAssembly().GetName().Name} {this.version}";
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

        /// <summary>
        /// Populates the telemetry Client
        /// </summary>
        private void SetupTelemetry()
        {
            this.telemetryClient.Context.Properties.Add("Application Version", this.version);
            this.telemetryClient.Context.User.UserAgent = this.application;
            this.telemetryClient.Context.Component.Version = this.version;
            this.telemetryClient.Context.Session.Id = this.sessionKey;
            this.telemetryClient.Context.Device.OperatingSystem = this.osName;
        }
    }
}
