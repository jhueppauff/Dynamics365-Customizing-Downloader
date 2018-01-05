namespace Dynamics365CustomizingDownloader.Pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SolutionDownloader
    /// </summary>
    public partial class SolutionDownloader : UserControl
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
        private Core.Xrm.CrmConnection selectedCrmConnection;

        /// <summary>
        /// List of all CRM Solutions/>
        /// </summary>
        private List<Core.Xrm.CrmSolution> crmSolutions = new List<Core.Xrm.CrmSolution>();

        public SolutionDownloader()
        {
            try
            {
                InitializeComponent();
                LoadConnections();
            }
            catch (System.Exception ex)
            {
                Log.Error(ex.Message, ex);
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                errorReport.Show();
            }
        }

        /// <summary>
        /// Loads every Solution into Grid
        /// </summary>
        private void LoadSolutions(string connectionNane)
        {
            try
            {
                if (Cbx_Connection.SelectedItem != null)
                {
                    Core.Xrm.CrmConnection crmConnection = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey).SingleOrDefault(x => x.Name == connectionNane);

                    this.Dtg_Solutions.ItemsSource = null;
                    MainWindow.SetProgress(true);

                    this.worker.DoWork += this.Worker_DoWork;
                    this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
                    this.worker.RunWorkerAsync();

                    this.selectedCrmConnection = crmConnection;
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
                SolutionDownloader.Log.Error(ex.Message, ex);
                Cbx_Connection.IsEnabled = true;
                Btn_Reload.IsEnabled = true;
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
                    Core.Xrm.CrmConnection crmConnection = selectedCrmConnection;

                    crmServiceClient = toolingConnector.GetCrmServiceClient(crmConnection.ConnectionString);

                    // Get Crm Solutions
                    this.crmSolutions.Clear();
                    this.crmSolutions = toolingConnector.GetCrmSolutions(crmServiceClient);
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
            this.Cbx_Connection.IsEnabled = true;
            MainWindow.SetProgress(false);
        }

        /// <summary>
        /// Loads Connection into Combo Box
        /// </summary>
        private void LoadConnections()
        {
            try
            {
                List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
                this.Cbx_Connection.Items.Clear();
                this.Cbx_Connection.ItemsSource = null;

                foreach (Core.Xrm.CrmConnection crmConnection in crmConnections)
                {
                    this.Cbx_Connection.Items.Add(crmConnection.Name);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                // Ignore File Not found
            }
            catch (Exception ex)
            {
                SolutionDownloader.Log.Error(ex.Message, ex);
            }
        }

        private void Btn_download_Click(object sender, RoutedEventArgs e)
        {
            if (Cbx_IsManaged.IsChecked == false && Cbx_IsUnmanaged.IsChecked == false)
            {
                MessageBox.Show("Please select at least one Solution Type", "Missing Solution Type", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (Dtg_Solutions.ItemsSource == null)
            {
                MessageBox.Show("Please connect to CRM and wait for the Background Job fetching the CRM Solutions", "Solutions are empty", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<Core.Xrm.CrmSolution> crmSolutionList = new List<Core.Xrm.CrmSolution>();
            int downloadCounter = 0;

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
                DownloadSolution downloadSolution = new DownloadSolution(selectedCrmConnection, crmSolutionList, (bool)Cbx_IsManaged.IsChecked, (bool)Cbx_IsUnmanaged.IsChecked);
                downloadSolution.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select at least one Solution", "No Solution Selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Btn_Reload_Click(object sender, RoutedEventArgs e)
        {
            this.LoadSolutions(this.Cbx_Connection.SelectedItem.ToString());
        }

        private void Cbx_Connection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadSolutions(this.Cbx_Connection.SelectedItem.ToString());
        }
    }
}
