//-----------------------------------------------------------------------
// <copyright file="DownloadMultiple.xaml.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DownloadMultiple
    /// </summary>
    public partial class DownloadMultiple : Window
    {
        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// Dispose bool
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Indicates if the panel is loading
        /// </summary>
        private bool panelLoading;

        /// <summary>
        /// Panel Message
        /// </summary>
        private string panelMainMessage = "Please wait, downloading and extracting Solution";

        /// <summary>
        /// Identifies if all Downloads are done
        /// </summary>
        private int downloadIndex = 0;

        /// <summary>
        /// Selected local Path
        /// </summary>
        private string selectedPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadMultiple"/> class.
        /// </summary>
        /// <param name="crmConnection"><see cref="Xrm.CrmConnection"/> for the XRM Connector</param>
        /// <param name="crmSolutions"><see cref="List{Xrm.CrmSolution}"/> of all Solutions to Download</param>
        public DownloadMultiple(Xrm.CrmConnection crmConnection, List<Xrm.CrmSolution> crmSolutions)
        {
            this.InitializeComponent();
            this.Btn_close.IsEnabled = false;
            this.CRMConnection = crmConnection;
            this.CRMSolutions = crmSolutions;
            this.tbx_download.Text = crmConnection.LocalPath;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a <see cref="List{Xrm.CrmSolution}"/> of all Solutions to Download
        /// </summary>
        public List<Xrm.CrmSolution> CRMSolutions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Xrm.CrmConnection"/> for the XRM Connector
        /// </summary>
        public Xrm.CrmConnection CRMConnection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [panel loading].
        /// </summary>
        /// <value>
        /// <c>true</c> if [panel loading]; otherwise, <c>false</c>.
        /// </value>
        public bool PanelLoading
        {
            get
            {
                return this.panelLoading;
            }

            set
            {
                this.panelLoading = value;
                this.RaisePropertyChanged("PanelLoading");
            }
        }

        /// <summary>
        /// Gets or sets the panel main message.
        /// </summary>
        /// <value>The panel main message.</value>
        public string PanelMainMessage
        {
            get
            {
                return this.panelMainMessage;
            }

            set
            {
                this.panelMainMessage = value;
                this.RaisePropertyChanged("PanelMainMessage");
            }
        }

        /// <summary>
        /// Gets the hide panel command.
        /// </summary>
        public ICommand HidePanelCommand => new HelperClasses.DelegateCommand(() =>
        {
            PanelLoading = false;
        });

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose class
        /// </summary>
        /// <param name="disposing">Bool disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    this.worker.Dispose();
                }

                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.disposed = true;
            }
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Closes the Form on Button Action
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            if (this.downloadIndex == 0)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Button Action, Download
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.loadingPanel.IsLoading = true;
            this.selectedPath = tbx_download.Text;

            Xrm.CrmConnection crmConnection = new Xrm.CrmConnection
            {
                ConnectionString = this.CRMConnection.ConnectionString,
                LocalPath = this.selectedPath,
                Name = this.CRMConnection.Name
            };

            // Update Connection
            StorageExtensions.Update(crmConnection);

            // Background Worker
            this.worker.DoWork += this.Worker_DoWork;
            this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
            this.worker.ProgressChanged += new ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.worker.WorkerReportsProgress = true;
            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// Background Worker Event DoWork
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                foreach (Xrm.CrmSolution solution in this.CRMSolutions)
                {
                    using (Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector())
                    {
                        toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(connectionString: this.CRMConnection.ConnectionString), solution.Name, this.selectedPath);

                        Xrm.CrmSolutionPackager crmSolutionPackager = new Xrm.CrmSolutionPackager();

                        if (Directory.Exists(Path.Combine(this.selectedPath, solution.Name)))
                        {
                            Directory.Delete(Path.Combine(this.selectedPath, solution.Name), true);
                            this.worker.ReportProgress(0, $"Delete {Path.Combine(this.selectedPath, solution.Name).ToString()}");
                        }

                        string log = crmSolutionPackager.ExtractCustomizing(Path.Combine(this.selectedPath, solution.Name + ".zip"), Path.Combine(this.selectedPath, solution.Name));
                        this.worker.ReportProgress(0, log);

                        File.Delete(Path.Combine(this.selectedPath, solution.Name + ".zip"));
                        this.worker.ReportProgress(0, $"Delete {Path.Combine(this.selectedPath, solution.Name + ".zip").ToString()}");
                    }
                }
            }
            catch (Exception)
            {
                // ToDo:
                throw;
            }
        }

        /// <summary>
        /// This event handler updates the UI
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tbx_status.Text = sender.ToString();
        }

        /// <summary>
        /// Background Worker Event Worker_RunWorkerCompleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.loadingPanel.IsLoading = false;
            MessageBox.Show("Finished download/extraction", "Process completed", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
