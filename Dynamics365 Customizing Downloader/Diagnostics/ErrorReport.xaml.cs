//-----------------------------------------------------------------------
// <copyright file="ErrorReport.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Diagnostics
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ErrorReport
    /// </summary>
    public partial class ErrorReport : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> for the Error Report</param>
        /// <param name="customMessage">If this is set, the Custom Message will be displayed before the <see cref="Exception.Message"/> in the Error Dialog.</param>
        public ErrorReport(Exception ex, string customMessage = "")
        {
            try
            {
                this.InitializeComponent();

                if (customMessage != string.Empty)
                {
                    this.Tbx_ErrorMessage.Text = $"{customMessage} : {ex.Message}";
                }
                else
                {
                    this.Tbx_ErrorMessage.Text = ex.Message;
                }

                this.Tbx_StackTrace.Text = ex.StackTrace;
                this.Owner = App.Current.MainWindow;
                MainWindow.ApplicationInsightHelper.TrackFatalException(ex);
            }
            catch (Exception exc)
            {
                Log.Error(exc.Message, exc);
                MessageBox.Show("There was an error showing the error Dialog", "Ups, you broke it", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Button Click Close Window
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}