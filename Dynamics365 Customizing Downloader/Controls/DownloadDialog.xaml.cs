//-----------------------------------------------------------------------
// <copyright file="DownloadDialog.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DownloadDialog
    /// </summary>
    public partial class DownloadDialog : UserControl, IDisposable
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
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
