//-----------------------------------------------------------------------
// <copyright file="SolutionSelector.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SolutionSelector
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
        /// Initializes a new instance of the <see cref="SolutionSelector"/> class.
        /// </summary>
        public SolutionSelector()
        {
            this.InitializeComponent();
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
                        if (this.crmSolutions.Exists(x => x.UniqueName == solution.UniqueName))
                        {
                            this.crmSolutions.Find(x => x.UniqueName == solution.UniqueName).LocalVersion = solution.LocalVersion;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);

                if (!Properties.Settings.Default.DisableErrorReports)
                {
                    throw;
                }
            }
            finally
            {
                if (crmServiceClient != null)
                {
                    crmServiceClient.Dispose();
                }
            }
        }

        /// <summary>
        /// Background Worker Event RunWorkerCompleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(e.Error, "An error occured while retriving the CRM Solutions");
                errorReport.Show();
            }

            this.Dtg_Solutions.ItemsSource = this.crmSolutions;
            this.Lbx_Repos.IsEnabled = true;
            MainWindow.SetProgress(false);
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
            if (Lbx_Repos.SelectedItem == null)
            {
                MessageBox.Show("Please connect to CRM first", "No CRM Connection", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int downloadCounter = 0;
            List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
            Core.Xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == ((Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem).Name);

            if (Dtg_Solutions.ItemsSource == null)
            {
                MessageBox.Show("Please connect to CRM and wait for the Background Job fetching the CRM Solutions", "Solutions are empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

        /// <summary>
        /// Loads the Local Solution Version into the Grid
        /// </summary>
        private void LoadVersionIntoGrid()
        {
            try
            {
            this.Dtg_Solutions.ItemsSource = null;
            Core.Repository.Connector connector = new Core.Repository.Connector();
            List<Core.Xrm.CrmSolution> localRepoSolutions = connector.GetLocalCRMSolutions(((Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem).LocalPath);

            foreach (Core.Xrm.CrmSolution solution in localRepoSolutions)
            {
                if (this.crmSolutions.Exists(x => x.UniqueName == solution.UniqueName))
                {
                    this.crmSolutions.Find(x => x.UniqueName == solution.UniqueName).LocalVersion = solution.LocalVersion;
                }
            }

            this.Dtg_Solutions.ItemsSource = this.crmSolutions;
            }
            catch (Exception)
            {
                // Ignore if the solution does not exists in CRM
            }
        }

        /// <summary>
        /// Connects to the selected CRM
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Lbx_Repos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadSolutions();
        }

        /// <summary>
        /// Reloads the CRM Solutions
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            this.LoadSolutions();
        }

        /// <summary>
        /// Loads the CRM Solution for the selected Repo into the grid
        /// </summary>
        private void LoadSolutions()
        {
            this.Btn_Reload.IsEnabled = false;
            try
            {
                if (Lbx_Repos.SelectedItem != null)
                {
                    Lbx_Repos.IsEnabled = false;
                    Core.Xrm.CrmConnection crmConnection = (Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem;

                    Core.Repository.Connector repositoryConnector = new Core.Repository.Connector();
                    this.localSolutions = repositoryConnector.GetLocalCRMSolutions(((Core.Xrm.CrmConnection)Lbx_Repos.SelectedItem).LocalPath);

                    this.Dtg_Solutions.ItemsSource = null;
                    MainWindow.SetProgress(true);

                    this.worker.DoWork += this.Worker_DoWork;
                    this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
                    this.worker.RunWorkerAsync();

                    this.selectedCrmConnection = crmConnection.Name;
                    this.Btn_Reload.IsEnabled = true;
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

                MainWindow.SetProgress(false);
                SolutionSelector.Log.Error(ex.Message, ex);
                Lbx_Repos.IsEnabled = true;
                Btn_Reload.IsEnabled = true;
            }
        }
    }
}