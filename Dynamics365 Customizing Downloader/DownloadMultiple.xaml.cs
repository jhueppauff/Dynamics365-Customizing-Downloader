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
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DownloadMultiple
    /// </summary>
    public partial class DownloadMultiple : Window
    {
        /// <summary>
        /// Static Status Text Box, used for multi threading UI Update
        /// </summary>
        private static TextBox statusTextBox;

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
        /// Indicates if an error occured
        /// </summary>
        private bool errorOccured;

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
            DownloadMultiple.statusTextBox = this.tbx_status;
            DownloadMultiple.LogToUI("Started Form");
            this.Btn_close.IsEnabled = false;
            this.CRMConnection = crmConnection;
            DownloadMultiple.LogToUI($"CRM Connection: {this.CRMConnection.Name}", true);
            this.CRMSolutions = crmSolutions;

            foreach (Xrm.CrmSolution crmSolution in this.CRMSolutions)
            {
                DownloadMultiple.LogToUI($"Added Solution: { crmSolution.UniqueName} to Download List", true);
                this.downloadIndex++;
            }

            this.tbx_download.Text = crmConnection.LocalPath;
            DownloadMultiple.LogToUI($"Pulled Path form config: {crmConnection.LocalPath}", true);
        }

        /// <summary>
        /// Delegated UI Update 
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="isDebugging">Identifies if the message is a Debug Message.</param>
        public delegate void UpdateTextCallback(string message, bool isDebugging);

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
        /// Updated the UI Status Text Box
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="isDebugging">Identifies if the message is a Debugging Message, Default = false</param>
        public static void LogToUI(string message, bool isDebugging = false)
        {
            if (isDebugging && (bool)App.Current.Properties["Debugging.Enabled"])
            {
                // Debug Messages
                statusTextBox.Text += $"{DateTime.Now} : Debug: {message}\n";
                statusTextBox.CaretIndex = statusTextBox.Text.Length;
                statusTextBox.ScrollToEnd();
            }
            else if (!isDebugging)
            {
                statusTextBox.Text += $"{DateTime.Now} : {message}\n";
                statusTextBox.CaretIndex = statusTextBox.Text.Length;
                statusTextBox.ScrollToEnd();
            }
        }

        /// <summary>
        /// Updates the UI delegated
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="isDebugging">Identifies if the message is a Debugging Message</param>
        public static void UpdateUI(string message, bool isDebugging)
        {
            statusTextBox.Dispatcher.Invoke(
                new UpdateTextCallback(LogToUI),
                message,
                isDebugging);
        }

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
            this.DownloadWindow_Closing(null, null);
        }

        /// <summary>
        /// Button Action, Download
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            errorOccured = false;
            this.loadingPanel.IsLoading = true;
            this.selectedPath = tbx_download.Text;

            Xrm.CrmConnection crmConnection = new Xrm.CrmConnection
            {
                ConnectionString = this.CRMConnection.ConnectionString,
                LocalPath = this.selectedPath,
                Name = this.CRMConnection.Name
            };
            this.CRMConnection = crmConnection;

            // Update Connection
            StorageExtensions.Update(crmConnection);

            // Background Worker
            this.worker.DoWork += this.Worker_DoWork;
            this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
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
                // Create Folder if it does not exists
                if (!Directory.Exists(this.CRMConnection.LocalPath))
                {
                    Directory.CreateDirectory(this.CRMConnection.LocalPath);
                }

                downloadIndex = CRMSolutions.Count;
                foreach (Xrm.CrmSolution solution in this.CRMSolutions)
                {
                    if (!this.worker.CancellationPending)
                    {
                        using (Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector())
                        {
                            // Delete Solution File if it exists
                            if (File.Exists(Path.Combine(this.selectedPath, solution.UniqueName + ".zip")))
                            {
                                File.Delete(Path.Combine(this.selectedPath, solution.UniqueName + ".zip"));
                            }

                            toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(connectionString: this.CRMConnection.ConnectionString), solution.UniqueName, this.selectedPath);

                            Xrm.CrmSolutionPackager crmSolutionPackager = new Xrm.CrmSolutionPackager();

                            if (Directory.Exists(Path.Combine(this.selectedPath, solution.UniqueName)))
                            {
                                Directory.Delete(Path.Combine(this.selectedPath, solution.UniqueName), true);
                                LogToUI($"Delete {Path.Combine(this.selectedPath, solution.UniqueName).ToString()}", true);
                            }

                            crmSolutionPackager.ExtractCustomizing(Path.Combine(this.selectedPath, solution.UniqueName + ".zip"), Path.Combine(this.selectedPath, solution.UniqueName));

                            File.Delete(Path.Combine(this.selectedPath, solution.UniqueName + ".zip"));
                            LogToUI($"Delete {Path.Combine(this.selectedPath, solution.UniqueName + ".zip").ToString()}", true);

                            this.downloadIndex--;
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorOccured = true;
                UpdateUI($"An Error occured: {ex.Message}", false);
            }
        }

        /// <summary>
        /// Background Worker Event Worker_RunWorkerCompleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.loadingPanel.IsLoading = false;
            this.Btn_close.IsEnabled = true;
            DownloadMultiple.LogToUI("---------------");
            DownloadMultiple.LogToUI("Finished download/extraction");
            DownloadMultiple.LogToUI("---------------");
        }

        /// <summary>
        /// Form Closing Event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void DownloadWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.downloadIndex <= 0)
            {
                this.Close();
            }
            else
            {
                MessageBoxResult dialogResult = MessageBox.Show("Download is still running, are you sure to abort the process?", "Background thread is still active!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    this.worker.CancelAsync();
                }
            }
        }
    }
}
