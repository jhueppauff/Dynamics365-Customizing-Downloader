//-----------------------------------------------------------------------
// <copyright file="SolutionUploader.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
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
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Forms;

    /// <summary>
    /// Interaction logic for SolutionUploader
    /// </summary>
    public partial class SolutionUploader : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Selected CRM Connection
        /// </summary>
        private Core.Xrm.CrmConnection selectedCrmConnection;

        /// <summary>
        /// List of all CRM Solutions/>
        /// </summary>
        private List<Core.Xrm.CrmSolution> crmSolutions = new List<Core.Xrm.CrmSolution>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionUploader"/> class.
        /// </summary>
        public SolutionUploader()
        {
            this.InitializeComponent();
            this.LoadConnections();
        }

        /// <summary>
        /// Loads the CRM Connections into the Connection ComboBox
        /// </summary>
        private void LoadConnections()
        {
            try
            {
                List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
                Cbx_Connection.Items.Clear();
                this.Dtg_Solutions.ItemsSource = null;

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
                Log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Event Method if the selection of the Connection ComboBox changed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Cbx_Connection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedCrmConnection = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey).SingleOrDefault(x => x.Name == Cbx_Connection.SelectedItem.ToString());
        }

        /// <summary>
        /// Add a new Item to the List
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".zip",
                Filter = "Zip Archives (.zip) | *.zip"
            };

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Core.Xrm.CrmSolution crmSolution = new Core.Xrm.CrmSolution
                {
                    LocalPath = openFileDialog.FileName,
                    ImportOrder = this.crmSolutions.Count
                };

                this.crmSolutions.Add(crmSolution);

                Dtg_Solutions.ItemsSource = null;
                Dtg_Solutions.ItemsSource = this.crmSolutions;
            }
        }

        /// <summary>
        /// Remove all Items from List
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_DeleteAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dtg_Solutions.ItemsSource = null;
            this.crmSolutions.Clear();
        }

        /// <summary>
        /// Starts Upload to CRM
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_Upload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SolutionUploadDialog solutionUploadDialog = new SolutionUploadDialog(this.selectedCrmConnection, this.crmSolutions, (bool)cbx_overwrite.IsChecked);
            solutionUploadDialog.ShowDialog();
        }
    }
}