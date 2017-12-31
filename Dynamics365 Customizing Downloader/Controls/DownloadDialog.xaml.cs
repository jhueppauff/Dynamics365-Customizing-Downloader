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
        /// Crm Connection
        /// </summary>
        private Core.Xrm.CrmConnection crmConnection;

        /// <summary>
        /// List of all CRM Soltuions to Download
        /// </summary>
        private List<Core.Xrm.CrmSolution> crmSolutions;

        public DownloadDialog(Core.Xrm.CrmConnection crmConnection, List<Core.Xrm.CrmSolution> crmSolutions)
        {
            InitializeComponent();
            this.crmConnection = crmConnection;
            this.crmSolutions = crmSolutions;
        }

        public DownloadDialog(string solutionName, Core.Xrm.CrmConnection connection)
        {
            InitializeComponent();
            this.crmConnection = connection;
            DownloadSolution(solutionName);
        }

        private void DownloadSolution(string solutionName)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    worker.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
