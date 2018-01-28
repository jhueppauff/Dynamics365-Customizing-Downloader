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
        private readonly TelemetryClient telemetryClient;

        private string sessionKey;

        private string osName;

        private string version;

        private string application;

        private void GatherDetails()

        {

            sessionKey = Guid.NewGuid().ToString();

            osName = GetWindowsFriendlyName();

            version = $"v.{ Assembly.GetEntryAssembly().GetName().Version}";

            application = $"{ Assembly.GetEntryAssembly().GetName().Name} {version}";
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

        /// <summary>
        /// Populates the Selemetry Client
        /// </summary>
        private void SetupTelemetry()
        {
            telemetryClient.Context.Properties.Add("Application Version", version);
            telemetryClient.Context.User.UserAgent = application;
            telemetryClient.Context.Component.Version = version;
            telemetryClient.Context.Session.Id = sessionKey;
            telemetryClient.Context.Device.OperatingSystem = osName;
        }

        /// <summary>
        /// Initializes a new Instance <see cref="ApplicationInsightHelper"/> class.
        /// </summary>
        public ApplicationInsightHelper()
        {
            telemetryClient = new TelemetryClient() { InstrumentationKey = Properties.Settings.Default.AppInsights };
            GatherDetails();
            SetupTelemetry();
        }

        /// <summary>
        /// Tracks the Page Views
        /// </summary>
        /// <param name="pageName">Name of the Page</param>
        public void TrackPageView(string pageName)
        {
            telemetryClient.TrackPageView(pageName);
        }

        /// <summary>
        /// Tracks NonFatal Exception
        /// </summary>
        /// <param name="ex">Excetion to Track</param>
        public void TrackNonFatalExceptions(Exception ex)
        {
            IDictionary<string, double> metric = new Dictionary<string, double>
            {
                { "Non-fatal Exception", 1 }
            };

            telemetryClient.TrackException(ex, null, metric);
        }

        /// <summary>
        /// Tracks Fatal Exception
        /// </summary>
        /// <param name="ex">Excetion to Track</param>
        public void TrackFatalException(Exception ex)
        {

            var exceptionTelemetry = new Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry(new Exception())
            {
                HandledAt = Microsoft.ApplicationInsights.DataContracts.ExceptionHandledAt.Unhandled
            };

            telemetryClient.TrackException(exceptionTelemetry);
        }

        /// <summary>
        /// Flushes the tememetry Client
        /// </summary>
        public void FlushData()
        {
            telemetryClient.Flush();
        }
    }
}
