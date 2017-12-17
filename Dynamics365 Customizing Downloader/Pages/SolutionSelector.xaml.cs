//-----------------------------------------------------------------------
// <copyright file="SolutionSelector.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Pages
{
    using Dynamics365CustomizingDownloader.Controls;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SolutionSelector.xaml
    /// </summary>
    public partial class SolutionSelector : UserControl
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// BackGround Worker
        /// </summary>
        private BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// Selected CRM Connection
        /// </summary>
        private string selectedCrmConnection;

        /// <summary>
        /// Local CRM Solution
        /// </summary>
        private List<Core.Xrm.CrmSolution> localSolutions;

        /// <summary>
        /// List of all CRM Solutions/>
        /// </summary>
        private List<Core.Xrm.CrmSolution> crmSolutions = new List<Core.Xrm.CrmSolution>();

        /// <summary>
        /// Indicates if the panel is loading
        /// </summary>
        private bool panelLoading;

        /// <summary>
        /// Panel Message
        /// </summary>
        private string panelMainMessage = "Please wait, connecting to Crm and retriving the Solutions";

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
            this.PanelLoading = false;
        });

        public SolutionSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reloads the Connection Drop down after a connection was created
        /// </summary>
        public void ReloadConnections()
        {
            try
            {
                List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
                this.Lbx_Repos.Items.Clear();
                this.Dtg_Solutions.ItemsSource = null;

                foreach (Core.Xrm.CrmConnection crmConnection in crmConnections)
                {
                    this.Lbx_Repos.Items.Add(crmConnection);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                // Ignore File Not found
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
        /// Background Worker Event DoWork
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Microsoft.Xrm.Tooling.Connector.CrmServiceClient crmServiceClient = null;

            try
            {
                using (Core.Xrm.ToolingConnector toolingConnector = new Core.Xrm.ToolingConnector())
                {
                    List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
                    Core.Xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == this.selectedCrmConnection);

                    crmServiceClient = toolingConnector.GetCrmServiceClient(crmConnection.ConnectionString);

                    // Get Crm Solutions
                    this.crmSolutions.Clear();
                    this.crmSolutions = toolingConnector.GetCrmSolutions(crmServiceClient);

                    foreach (Core.Xrm.CrmSolution solution in this.localSolutions)
                    {
                        this.crmSolutions.Find(x => x.UniqueName == solution.UniqueName).LocalVersion = solution.LocalVersion;
                    }

                    crmServiceClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (crmServiceClient != null)
                {
                    crmServiceClient.Dispose();
                }

                Log.Error(ex.Message, ex);
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex, "An error occured in the Backgroud Thread");
                errorReport.Show();
            }
        }



        /// <summary>
        /// Background Worker Event RunWorkerCompleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dtg_Solutions.ItemsSource = this.crmSolutions;
            this.Lbx_Repos.IsEnabled = true;
            this.loadingPanel.IsLoading = false;
        }

        /// <summary>
        /// Opens the Connection Create Dialog
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_OpenCreateNewConnection_Click(object sender, RoutedEventArgs e)
        {
            using (ConnectionManger connectionManger = new ConnectionManger())
            {
                connectionManger.ShowDialog();
                this.ReloadConnections();
            }
        }

        /// <summary>
        /// Download Button Click Event
        /// </summary>
        /// <param name="sender">Button sender object</param>
        /// <param name="e">Button event args</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Lbx_Repos.SelectedItem != null)
            {
                int downloadCounter = 0;
                List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
                Core.Xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == ((Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem).Name);

                if (Dtg_Solutions.ItemsSource != null)
                {
                    List<Core.Xrm.CrmSolution> crmSolutionList = new List<Core.Xrm.CrmSolution>();
                    foreach (Core.Xrm.CrmSolution crmSolution in Dtg_Solutions.ItemsSource)
                    {
                        if (crmSolution.DownloadIsChecked)
                        {
                            downloadCounter++;
                            crmSolutionList.Add(crmSolution);
                        }
                    }

                    if (downloadCounter != 0)
                    {
                        DownloadMultiple downloadMultiple = new DownloadMultiple(crmConnection, crmSolutionList);
                        downloadMultiple.ShowDialog();

                        this.LoadVersionIntoGrid();
                    }
                    else
                    {
                        MessageBox.Show("Please select at least one Solution", "No Solution Selected", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please connect to CRM and wait for the Background Job fetching the CRM Solutions", "Solutions are empty", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please connect to CRM first", "No CRM Connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Loads the Local Solution Version into the Grid
        /// </summary>
        private void LoadVersionIntoGrid()
        {
            this.Dtg_Solutions.ItemsSource = null;
            Core.Repository.Connector connector = new Core.Repository.Connector();
            List<Core.Xrm.CrmSolution> localRepoSolutions = connector.GetLocalCRMSolutions(((Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem).LocalPath);

            foreach (Core.Xrm.CrmSolution solution in localRepoSolutions)
            {
                this.crmSolutions.Find(x => x.UniqueName == solution.UniqueName).LocalVersion = solution.LocalVersion;
            }

            this.Dtg_Solutions.ItemsSource = this.crmSolutions;
        }

        /// <summary>
        /// Connects to the selected CRM
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Lbx_Repos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Btn_Reload.IsEnabled = false;
            this.LoadSolutions();
            this.Btn_Reload.IsEnabled = true;
        }

        /// <summary>
        /// Reloads the CRM Solutions
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            this.Btn_Reload.IsEnabled = false;
            this.LoadSolutions();
            this.Btn_Reload.IsEnabled = true;
        }

        /// <summary>
        /// Loads the CRM Solution for the selected Repo into the grid
        /// </summary>
        private void LoadSolutions()
        {
            try
            {
                if (Lbx_Repos.SelectedItem != null)
                {
                    Lbx_Repos.IsEnabled = false;
                    Core.Xrm.CrmConnection crmConnection = (Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem;

                    Core.Repository.Connector repositoryConnector = new Core.Repository.Connector();
                    this.localSolutions = repositoryConnector.GetLocalCRMSolutions(((Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem).LocalPath);

                    this.Dtg_Solutions.ItemsSource = null;
                    this.loadingPanel.IsLoading = true;

                    this.worker.DoWork += this.Worker_DoWork;
                    this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
                    this.worker.RunWorkerAsync();

                    this.selectedCrmConnection = crmConnection.Name;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                if (!Properties.Settings.Default.DisableErrorReports)
                {
                    Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                    errorReport.Show();
                }

                loadingPanel.IsLoading = false;
                SolutionSelector.Log.Error(ex.Message, ex);
                Lbx_Repos.IsEnabled = true;
                Btn_Reload.IsEnabled = true;
            }
        }
    }
}
