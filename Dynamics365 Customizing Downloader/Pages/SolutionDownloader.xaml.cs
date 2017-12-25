namespace Dynamics365CustomizingDownloader.Pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        private string selectedCrmConnection;

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
                    Cbx_Connection.IsEnabled = false;
                    Core.Xrm.CrmConnection crmConnection = (Core.Xrm.CrmConnection)Cbx_Connection.SelectedItem;

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
                    List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
                    Core.Xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == this.selectedCrmConnection);

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
        }

        private void btn_download_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Reload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cbx_Connection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadSolutions(this.Cbx_Connection.SelectedItem.ToString());
        }
    }
}
