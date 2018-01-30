//-----------------------------------------------------------------------
// <copyright file="SolutionUploadDialog.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 julian
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Pages
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using Core.Xrm;

    /// <summary>
    /// Interaction logic for SolutionUpload
    /// </summary>
    public partial class SolutionUploadDialog : Window
    {
        /// <summary>
        /// Defines if the Solution Import should overwrite the Customizings
        /// </summary>
        private readonly bool overwriteCustomizings = false;

        /// <summary>
        /// BackGround Worker for the Import
        /// </summary>
        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();

        /// <summary>
        /// CRM Connection
        /// </summary>
        private readonly CrmConnection crmConnection;

        /// <summary>
        /// <see cref="List{CrmSolution}"/> to Upload to CRM
        /// </summary>
        private readonly List<CrmSolution> crmSolutions;

        /// <summary>
        /// Helper to iterate the Solution list
        /// </summary>
        private int uploadCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionUploadDialog"/> class.
        /// </summary>
        /// <param name="crmConnection">CRM Connection</param>
        /// <param name="crmSolutions">Solutions to Upload</param>
        public SolutionUploadDialog(CrmConnection crmConnection, List<CrmSolution> crmSolutions, bool overwriteCustomizings)
        {
            this.InitializeComponent();

            this.crmConnection = crmConnection;
            this.crmSolutions = crmSolutions;
            this.overwriteCustomizings = overwriteCustomizings;
        }

        /// <summary>
        /// Button Event, start Solution Import
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_start_Click(object sender, RoutedEventArgs e)
        { 
            // Background Worker
            this.backgroundWorker.DoWork += this.Worker_DoWork;
            this.backgroundWorker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
            this.backgroundWorker.WorkerReportsProgress = true;

            foreach (CrmSolution crmSolution in this.crmSolutions)
            {
                this.backgroundWorker.RunWorkerAsync(crmSolution);
            }
        }

        /// <summary>
        /// Background Worker Event DoWork
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CrmSolution crmSolution = (CrmSolution)e.Argument;

            ToolingConnector toolingConnector = new ToolingConnector();

            try
            {
                toolingConnector.UploadCrmSolution(crmSolution: crmSolution, crmServiceClient: toolingConnector.GetCrmServiceClient(this.crmConnection.ConnectionString), overwriteCustomizing: overwriteCustomizings);
            }
            catch (System.Exception ex)
            {
                tbx_status.Text += $"\n {ex.Message} \nStackTrace: {ex.StackTrace}";
            }
        }

        /// <summary>
        /// Background Worker Event Worker_RunWorkerCompleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(e.Error, "An error occured while downloading or extracting the solution");
                errorReport.Show();
            }
            else if (this.uploadCount == this.crmSolutions.Count)
            {
                tbx_status.Text += "\n Upload of {0} completed" + this.crmSolutions[this.uploadCount].LocalPath;
                tbx_status.Text += "\n Upload completed";
                pgr_upload.Value = 100;
            }
            else
            {
                pgr_upload.Value += (100 / uploadCount);
                tbx_status.Text += "\n Upload of {0} completed" + this.crmSolutions[this.uploadCount].LocalPath;
            }
        }
    }
}
