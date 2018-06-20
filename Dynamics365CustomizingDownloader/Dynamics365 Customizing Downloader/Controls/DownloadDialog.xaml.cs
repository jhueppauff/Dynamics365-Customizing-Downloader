//-----------------------------------------------------------------------
// <copyright file="DownloadDialog.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DownloadDialog
    /// </summary>
    public partial class DownloadDialog : UserControl, IDisposable
    {
        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// CRM Connection
        /// </summary>
        private Core.Xrm.CrmConnection crmConnection;

        /// <summary>
        /// List of all CRM Solutions to Download
        /// </summary>
        private List<Core.Xrm.CrmSolution> crmSolutions;

        /// <summary>
        /// detects redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadDialog"/> class.
        /// </summary>
        /// <param name="crmConnection">CRM Connection for the Download</param>
        /// <param name="crmSolutions">CRM Solutions to Download</param>
        public DownloadDialog(Core.Xrm.CrmConnection crmConnection, List<Core.Xrm.CrmSolution> crmSolutions)
        {
            this.InitializeComponent();
            this.crmConnection = crmConnection;
            this.crmSolutions = crmSolutions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadDialog"/> class.
        /// </summary>
        public DownloadDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This code added to correctly implement the disposable pattern.
        /// </summary>
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) below.
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose objects
        /// </summary>
        /// <param name="disposing">Determines if objects should be disposed</param>
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
    }
}
