//-----------------------------------------------------------------------
// <copyright file="ConnectionManger.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
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
    using System.Windows;

    /// <summary>
    /// Interaction logic for ConnectionManger
    /// </summary>
    public partial class ConnectionManger : Window, IDisposable
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// CRM Connection
        /// </summary>
        private Core.Xrm.CrmConnection crmConnection;

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionManger"/> class.
        /// </summary>
        public ConnectionManger()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// implement the disposable pattern
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
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
                using (Core.Xrm.ToolingConnector toolingConnector = new Core.Xrm.ToolingConnector())
                {
                    using (Microsoft.Xrm.Tooling.Connector.CrmServiceClient crmServiceClient = toolingConnector.GetCrmServiceClient(tbx_connectionString.Text))
                    {

                        if (crmServiceClient != null)
                        {
                            this.crmConnection = new Core.Xrm.CrmConnection
                            {
                                ConnectionID = Guid.NewGuid(),
                                ConnectionString = this.tbx_connectionString.Text,
                                Name = crmServiceClient.ConnectedOrgFriendlyName
                            };

                            tbx_connectionName.IsReadOnly = false;
                            tbx_connectionName.Text = this.crmConnection.Name;
                            btn_save.IsEnabled = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An Error occured: {ex.Message}", "An Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                ConnectionManger.Log.Error(ex.Message, ex);
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
                    List<Core.Xrm.CrmConnection> crmConnections = Core.Data.StorageExtensions.Load(MainWindow.EncryptionKey);

                    bool connectionExists = false;
                    foreach (Core.Xrm.CrmConnection crmTempConnection in crmConnections)
                    {
                        if (crmTempConnection.Name == this.tbx_connectionName.Text)
                        {
                            connectionExists = true;
                            MessageBox.Show($"Connection {this.tbx_connectionName.Text} does already exist!", "Connection already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    if (!connectionExists)
                    {
                        Core.Data.StorageExtensions.Save(this.crmConnection, MainWindow.EncryptionKey);
                        MessageBox.Show("Added Connection successfully", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Ignore Error, in a fresh installation there is no config File
                    Core.Data.StorageExtensions.Save(this.crmConnection, MainWindow.EncryptionKey);
                    MessageBox.Show("Added Connection successfully", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An Error occured: {ex.Message}", "An Error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                ConnectionManger.Log.Error(ex.Message, ex);
            }
        }

        #region IDisposable Support

        /// <summary>
        /// implement the disposable pattern
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Nothing to dispose
                }

                disposedValue = true;
            }
        }
        #endregion
    }
}
