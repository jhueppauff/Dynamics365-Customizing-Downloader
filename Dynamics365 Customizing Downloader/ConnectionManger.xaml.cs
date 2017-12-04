//-----------------------------------------------------------------------
// <copyright file="ConnectionManger.xaml.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ConnectionManger
    /// </summary>
    public partial class ConnectionManger : Window
    {
        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// CRM Service Client
        /// </summary>
        private Microsoft.Xrm.Tooling.Connector.CrmServiceClient crmServiceClient;

        /// <summary>
        /// CRM Connection
        /// </summary>
        private Xrm.CrmConnection crmConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManger"/> class.
        /// </summary>
        public ConnectionManger()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Button Click Action, Connect and add a CRM Organization
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector();
                this.crmServiceClient = toolingConnector.GetCrmServiceClient(tbx_connectionString.Text);

                if (this.crmServiceClient != null)
                {
                    this.crmConnection = new Xrm.CrmConnection
                    {
                        ConnectionID = new System.Guid(),
                        ConnectionString = this.tbx_connectionString.Text,
                        Name = this.crmServiceClient.ConnectedOrgFriendlyName
                    };

                    tbx_connectionName.IsReadOnly = false;
                    tbx_connectionName.Text = this.crmConnection.Name;
                    btn_save.IsEnabled = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An Error occured: {ex.Message}", "An Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Button Save click Event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    this.crmConnection.Name = this.tbx_connectionName.Text;
                    List<Xrm.CrmConnection> crmConnections = StorageExtensions.Load();

                    bool connectionExists = false;
                    foreach (Xrm.CrmConnection crmTempConnection in crmConnections)
                    {
                        if (crmTempConnection.Name == this.tbx_connectionName.Text)
                        {
                            connectionExists = true;
                            MessageBox.Show($"Connection {this.tbx_connectionName.Text} does already exist!", "Connection already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    if (!connectionExists)
                    {
                        StorageExtensions.Save(this.crmConnection);
                        MessageBox.Show("Added Connection successfully", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Ignore Error, in a fresh installation there is no config File
                    StorageExtensions.Save(this.crmConnection);
                    MessageBox.Show("Added Connection successfully", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An Error occured: {ex.Message}", "An Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
