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
        /// Initializes a new instance of the <see cref="ConnectionManger"/> class.
        /// </summary>
        public ConnectionManger()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Button Click Action, Connect and add a CRM Organisation
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector();
            var crmServiceClient = toolingConnector.GetCrmServiceClient(tbx_connectionString.Text);

            if (crmServiceClient != null)
            {
                try
                {
                    List<Xrm.CrmConnection> crmConnections = StorageExtensions.Load();

                    foreach (Xrm.CrmConnection crmTempConnection in crmConnections)
                    {
                        if (crmTempConnection.Name != crmServiceClient.ConnectedOrgFriendlyName)
                        {
                            Xrm.CrmConnection crmConnection = new Xrm.CrmConnection
                            {
                                ConnectionString = tbx_connectionString.Text,
                                Name = crmServiceClient.ConnectedOrgFriendlyName
                            };
                            StorageExtensions.Save(crmConnection);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Connection {crmTempConnection.Name} does already exist!", "Connection already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Ignor
                }
                finally
                {
                    Xrm.CrmConnection crmConnection = new Xrm.CrmConnection
                    {
                        ConnectionString = tbx_connectionString.Text,
                        Name = crmServiceClient.ConnectedOrgFriendlyName
                    };
                    StorageExtensions.Save(crmConnection);
                    this.Close();
                }
            }
        }
    }
}
