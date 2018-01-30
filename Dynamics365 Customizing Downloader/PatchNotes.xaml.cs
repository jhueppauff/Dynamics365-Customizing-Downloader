//-----------------------------------------------------------------------
// <copyright file="PatchNotes.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 julian
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using Core.Update;

    /// <summary>
    /// Interaction logic for PatchNotes
    /// </summary>
    public partial class PatchNotes : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchNotes"/> class.
        /// </summary>
        public PatchNotes()
        {
            try
            {
                UpdateChecker updateChecker = new UpdateChecker();
                Release release = updateChecker.GetReleaseInfo($"{Properties.Settings.Default.GitHubAPIURL}/releases/latest");

                this.InitializeComponent();
                this.Tbx_PatchNotes.Text = release.Body;
                this.Lbl_IsPreRelease.Content = release.Prerelease.ToString();
                this.Lbl_VersionNumber.Content = release.Name;
                this.Lbl_ReleaseDate.Content = release.Published_at.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                if (!Properties.Settings.Default.DisableErrorReports)
                {
                    Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                    errorReport.Show();
                }
            }
        }

        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_Skip_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Download the Patch
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_Download_Click(object sender, RoutedEventArgs e)
        {
            Core.Update.UpdateChecker updateChecker = new Core.Update.UpdateChecker();
            Uri uri = updateChecker.GetUpdateURL($"{Properties.Settings.Default.GitHubAPIURL}/releases/latest");

            using (var process = Process.Start(uri.ToString()))
            {
                // Use using to dispose object
            }
        }
    }
}