//-----------------------------------------------------------------------
// <copyright file="DownloadSolution.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 julian
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Interaction logic for DownloadSolution
    /// </summary>
    public partial class DownloadSolution : Window, IDisposable
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// CRM Connection
        /// </summary>
        private Core.Xrm.CrmConnection crmConnection;

        /// <summary>
        /// Local Path
        /// </summary>
        private string localPath;

        /// <summary>
        /// Sum Progress
        /// </summary>
        private decimal progress;

        /// <summary>
        /// Progress Step
        /// </summary>
        private decimal progressStep;

        /// <summary>
        /// List of all CRM Solutions to Download
        /// </summary>
        private List<Core.Xrm.CrmSolution> crmSolutions;

        /// <summary>
        /// Detects redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Determines if Solutions should be Downloaded managed
        /// </summary>
        private bool managed;

        /// <summary>
        /// Determines if Solutions should be Downloaded unmanaged
        /// </summary>
        private bool unmanaged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadSolution"/> class.
        /// </summary>
        public DownloadSolution()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadSolution"/> class.
        /// </summary>
        /// <param name="crmConnection">CRM Connection</param>
        /// <param name="crmSolutions">CRM Solutions to Download</param>
        /// <param name="managed">Determines if the Solution should be Downloaded managed</param>
        /// <param name="unmanaged">Determines if the Solution should be Downloaded unmanaged</param>
        public DownloadSolution(Core.Xrm.CrmConnection crmConnection, List<Core.Xrm.CrmSolution> crmSolutions, bool managed, bool unmanaged)
        {
            this.InitializeComponent();
            this.crmConnection = crmConnection;
            this.crmSolutions = crmSolutions;
            this.managed = managed;
            this.unmanaged = unmanaged;
            this.progress = 0;
            this.progressStep = 100 / (decimal)crmSolutions.Count;
            this.Pgb_downloadProgress.Value = 0;

            this.DownloadSolutions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadSolution"/> class.
        /// </summary>
        public void DownloadSolutions()
        {
            try
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        this.localPath = dialog.SelectedPath.ToString();
                    }
                }

                // Background Worker
                this.worker.DoWork += this.Worker_DoWork;
                this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
                this.worker.WorkerReportsProgress = true;
                this.worker.ProgressChanged += this.Worker_ReportProgress;
                this.worker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose objects
        /// </summary>
        /// <param name="disposing">Determines if object should be disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.worker.Dispose();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Background Worker Event DoWork
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.localPath == null)
            {
                return;
            }

            if (this.crmSolutions == null)
            {
                return;
            }

            if (this.crmConnection == null)
            {
                return;
            }

            foreach (Core.Xrm.CrmSolution crmSolution in this.crmSolutions)
            {
                if (this.managed)
                {
                    Core.Xrm.ToolingConnector toolingConnector = new Core.Xrm.ToolingConnector();
                    toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(this.crmConnection.ConnectionString), crmSolution.UniqueName, this.localPath, true);
                }

                if (this.unmanaged)
                {
                    Core.Xrm.ToolingConnector toolingConnector = new Core.Xrm.ToolingConnector();
                    toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(this.crmConnection.ConnectionString), crmSolution.UniqueName, this.localPath);
                }

                this.progress = this.progress + this.progressStep;
                this.worker.ReportProgress(Convert.ToInt16(this.progress));
            }
        }

        /// <summary>
        /// Background Worker Event Report Progress
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void Worker_ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            Pgb_downloadProgress.Value = e.ProgressPercentage;
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

            this.Close();
        }
    }
}
