namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DownloadMultiple.xaml
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

        public List<Xrm.CrmSolution> CRMSolutions;
        public Xrm.CrmConnection CRMConnection;
        private int downloadIndex = 0;
        private string selectedPath;

        public DownloadMultiple(Xrm.CrmConnection crmConnection, List<Xrm.CrmSolution> crmSolutions)
        {
            InitializeComponent();
            this.Btn_close.IsEnabled = false;
            this.CRMConnection = crmConnection;
            this.CRMSolutions = crmSolutions;
            this.tbx_download.Text = crmConnection.LocalPath;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        // <summary>
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
            if (downloadIndex == 0)
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
            this.worker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
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
                foreach (Xrm.CrmSolution Solution in CRMSolutions)
                { 
                    using (Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector())
                    {
                        toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(connectionString: this.CRMConnection.ConnectionString), Solution.Name, this.selectedPath);

                        Xrm.CrmSolutionPackager crmSolutionPackager = new Xrm.CrmSolutionPackager();


                        if (Directory.Exists(System.IO.Path.Combine(this.selectedPath, Solution.Name)))
                        {
                            Directory.Delete(System.IO.Path.Combine(this.selectedPath, Solution.Name), true);
                            worker.ReportProgress(0, $"Delete {System.IO.Path.Combine(this.selectedPath, Solution.Name).ToString()}");
                        }
                        
                        string log = crmSolutionPackager.ExtractCustomizing(System.IO.Path.Combine(this.selectedPath, Solution.Name + ".zip"), Path.Combine(this.selectedPath, Solution.Name));
                        worker.ReportProgress(0, log);

                        File.Delete(System.IO.Path.Combine(this.selectedPath, Solution.Name + ".zip"));
                        worker.ReportProgress(0, $"Delete {System.IO.Path.Combine(this.selectedPath, Solution.Name + ".zip").ToString()}");
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This event handler updates the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
