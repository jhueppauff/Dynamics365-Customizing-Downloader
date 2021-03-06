﻿//-----------------------------------------------------------------------
// <copyright file="ConnectionOverview.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ConnectionOverview
    /// </summary>
    public partial class ConnectionOverview : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// List of all CRM Connections <see cref="List{CRM Connections}"/>
        /// </summary>
        private List<Core.Xrm.CrmConnection> crmConnections;

        /// <summary>
        /// CRM Connection
        /// </summary>
        private Core.Xrm.CrmConnection crmConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOverview"/> class.
        /// </summary>
        public ConnectionOverview()
        {
            this.InitializeComponent();
            this.LoadCRMConnections();

            this.Owner = App.Current.MainWindow;
        }

        /// <summary>
        /// Loads every CRM Connection from Config
        /// </summary>
        private void LoadCRMConnections()
        {
            this.crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);
            this.Cbx_CRMConnections.Items.Clear();
            this.Tbx_ConnectionName.Text = string.Empty;
            this.Tbx_ConnectionString.Text = string.Empty;

            foreach (Core.Xrm.CrmConnection connection in this.crmConnections)
            {
                if (connection.ConnectionID == Guid.Empty)
                {
                    connection.ConnectionID = Core.Data.StorageExtensions.UpdateConnectionWithID(connection.Name);
                }

                this.Cbx_CRMConnections.Items.Add(connection.Name);
            }
        }

        /// <summary>
        /// Loads the selected Connection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Cbx_CRMConnections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.Cbx_CRMConnections.SelectedItem != null)
            {
                string connectionName = this.Cbx_CRMConnections.SelectedItem.ToString();

                this.crmConnection = this.crmConnections.Find(x => x.Name == connectionName);

                Tbx_ConnectionName.Text = this.crmConnection.Name;
                Tbx_ConnectionString.Text = this.crmConnection.ConnectionString;
                Btn_SaveConnection.IsEnabled = false;
            }
        }

        /// <summary>
        /// Tests the Connection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_TestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Core.Xrm.ToolingConnector toolingConnector = new Core.Xrm.ToolingConnector())
                {
                    if (toolingConnector.TestCRMConnection(Tbx_ConnectionString.Text))
                    {
                        Btn_SaveConnection.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Unable to connect to CRM", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception occured while trying to test the CRM Connection : " + ex.Message, "An Exception occured", MessageBoxButton.OK, MessageBoxImage.Error);
                ConnectionOverview.Log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Button Action, Save Connection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_SaveConnection_Click(object sender, RoutedEventArgs e)
        {
            if (this.Tbx_ConnectionString.Text != string.Empty && this.Tbx_ConnectionName.Text != string.Empty)
            {
                if (Core.Data.StorageExtensions.GetCountOfConnectionNamesByName(this.Tbx_ConnectionName.Text) <= 1)
                {
                    this.crmConnection.ConnectionString = this.Tbx_ConnectionString.Text;
                    this.crmConnection.Name = this.Tbx_ConnectionName.Text;

                    Core.Data.StorageExtensions.Update(this.crmConnection, MainWindow.EncryptionKey);

                    MessageBox.Show("Updated CRM Connection successfully", "Updated CRM Connection", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.LoadCRMConnections();
                }
                else
                {
                    MessageBox.Show("CRM Connection name is not unique", "Name is not unique", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Crm Connection or Name is emtpy", "Value can not be null", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Text Change Event, will disable the button
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void Tbx_ConnectionString_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Btn_SaveConnection.IsEnabled = false;
        }

        /// <summary>
        /// Will delete the connection and the local repository if selected in the dialog
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_DeleteRepository_Click(object sender, RoutedEventArgs e)
        {
            if (Cbx_CRMConnections.SelectedItem == null)
            {
                MessageBox.Show("Please select a CRM Connection first", "Missing Repository", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Are you sure to delete the Repository?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Stop, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                if (Directory.Exists(this.crmConnection.LocalPath))
                {
                    if (MessageBox.Show("Do you also want to delete the local Folder? All Data within this Folder will be deleted", "Do you want to wipe the data?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            Core.Data.StorageExtensions.Delete(Tbx_ConnectionName.Text, true);
                            MessageBox.Show("Deleted Connection.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "An error occured!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        this.DeleteConnectionWithoutFolder();
                    }
                }
                else
                {
                    this.DeleteConnectionWithoutFolder();
                }
            }
        }

        /// <summary>
        /// Deletes the Connection from Configuration
        /// </summary>
        private void DeleteConnectionWithoutFolder()
        {
            try
            {
                Core.Data.StorageExtensions.Delete(Tbx_ConnectionName.Text);
                MessageBox.Show("Deleted Connection.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                errorReport.Show();
            }
        }
    }
}