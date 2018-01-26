namespace Dynamics365CustomizingDownloader
{
    using Microsoft.ApplicationInsights;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class ApplicationInsightHelper
    {
        private readonly TelemetryClient _telemetryClient;

        private string _sessionKey;

        private string _userName;

        private string _osName;

        private string _version;

        private string _application;

        private string _manufacturer;

        private string _model;


        private void GatherDetails()

        {

            _sessionKey = Guid.NewGuid().ToString();

            _userName = Environment.UserName;

            _osName = GetWindowsFriendlyName();

            _version = $"v.{ Assembly.GetEntryAssembly().GetName().Version}";

            _application = $"{ Assembly.GetEntryAssembly().GetName().Name} {_version}";
        }

        /// <summary>
        /// Querys the Windows OS Name from the WMI
        /// </summary>
        /// <returns>Returns Name of the OS</returns>
        private string GetWindowsFriendlyName()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        private void SetupTelemetry()
        {

            _telemetryClient.Context.Properties.Add("Application Version",

                 _version);

            _telemetryClient.Context.User.Id = _userName;

            _telemetryClient.Context.User.UserAgent = _application;

            _telemetryClient.Context.Component.Version = _version;

            _telemetryClient.Context.Session.Id = _sessionKey;

            _telemetryClient.Context.Device.OperatingSystem = _osName;

        }

        /// <summary>
        /// Initializes a new Instance <see cref="ApplicationInsightHelper"/> class.
        /// </summary>
        public ApplicationInsightHelper()
        {
            _telemetryClient = new TelemetryClient() { InstrumentationKey = Properties.Settings.Default.AppInsights };
            GatherDetails();
            SetupTelemetry();
        }

        public void TrackPageView(string pageName)
        {
            _telemetryClient.TrackPageView(pageName);
        }

        public void TrackNonFatalExceptions(Exception ex)
        {
            IDictionary<string, double> metric = new Dictionary<string, double>
            {
                { "Non-fatal Exception", 1 }
            };

            _telemetryClient.TrackException(ex, null, metric);
        }

        public void TrackFatalException(Exception ex)
        {

            var exceptionTelemetry = new Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry(new

                Exception());

            exceptionTelemetry.HandledAt =

               Microsoft.ApplicationInsights.DataContracts.ExceptionHandledAt.

               Unhandled;

            _telemetryClient.TrackException(exceptionTelemetry);

        }

        public void FlushData()
        {
            DoDataFlush();
        }
    }
}
