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
    using System.Windows;
    using Microsoft.Xrm.Tooling.Connector;

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
            this.tbx_connectionName.Visibility = Visibility.Hidden;
            this.Lbl_ConnectionName.Visibility = Visibility.Hidden;

            this.Owner = App.Current.MainWindow;
        }

        /// <summary>
        /// implement the disposable pattern
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// implement the disposable pattern
        /// </summary>
        /// <param name="disposing">If dispose is already triggered</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // Nothing to dispose
                }

                this.disposedValue = true;
            }
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
                    CrmServiceClient crmServiceClient = toolingConnector.GetCrmServiceClient(tbx_connectionString.Text);
                    if (crmServiceClient != null && crmServiceClient.IsReady)
                    {
                        this.crmConnection = new Core.Xrm.CrmConnection
                        {
                            ConnectionID = Guid.NewGuid(),
                            ConnectionString = this.tbx_connectionString.Text,
                            Name = crmServiceClient.ConnectedOrgFriendlyName
                        };

                        this.tbx_connectionName.IsReadOnly = false;
                        this.tbx_connectionName.Text = this.crmConnection.Name;
                        this.tbx_connectionName.Text = this.crmConnection.Name;
                        this.Lbl_ConnectionName.Visibility = Visibility.Visible;
                        this.tbx_connectionName.Visibility = Visibility.Visible;
                        btn_save.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show((string)Application.Current.FindResource("ConnectionManager_Code_UnableToOpenCRM"), (string)Application.Current.FindResource("ConnectionManager_Code_UnableToOpenCRM_Caption"), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (!Properties.Settings.Default.DisableErrorReports)
                {
                    Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                    errorReport.Show();
                }

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
                        this.Close();
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Ignore Error, in a fresh installation there is no config File
                    Core.Data.StorageExtensions.Save(this.crmConnection, MainWindow.EncryptionKey);
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                if (!Properties.Settings.Default.DisableErrorReports)
                {
                    Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                    errorReport.Show();
                }

                ConnectionManger.Log.Error(ex.Message, ex);
            }
        }
    }
}
