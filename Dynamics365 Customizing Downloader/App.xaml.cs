//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// AI Helper
        /// </summary>
        private AppMetricHelper appMetricHelper;

        /// <summary>
        /// Overwrite for the Application Startup
        /// </summary>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                this.appMetricHelper = new AppMetricHelper();

                // We use this to get access to unhandled exceptions so we can 
                // report app crashes to the Telemetry client
                var currentDomain = AppDomain.CurrentDomain;

                currentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
                currentDomain.ProcessExit += this.CurrentDomain_ProcessExit;

                // Open Application
                var mainWindow = new MainWindow(this.appMetricHelper);
                mainWindow.Show();

                this.appMetricHelper.ReportUsage(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Handles the close of the Application
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // ToDo
        }

        /// <summary>
        /// Handles Unhandled Exceptions and send them to AI
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.appMetricHelper.TrackFatalException(e.ExceptionObject as Exception).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}