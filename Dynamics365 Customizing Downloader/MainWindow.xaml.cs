//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="None">
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
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// Dispose bool
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Selected CRM Connection
        /// </summary>
        private string selectedCrmConnection;

        /// <summary>
        /// List of all CRM Solutions/>
        /// </summary>
        private List<Xrm.CrmSolution> crmSolutions = new List<Xrm.CrmSolution>();

        /// <summary>
        /// Indicates if the panel is loading
        /// </summary>
        private bool panelLoading;

        /// <summary>
        /// Panel Message
        /// </summary>
        private string panelMainMessage = "Please wait, connecting to Crm and retriving the Solutions";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.cbx_connection.Items.Add("New");
            Application.Current.Properties["Debugging.Enabled"] = false;

            if (!File.Exists(StorageExtensions.StoragePath))
            {
                Btn_OpenConnectionOverview.IsEnabled = false;
            }

            if (MainWindow.EncryptionKey != null && MainWindow.EncryptionKey == string.Empty)
            {
                try
                {
                    List<Xrm.CrmConnection> crmConnections = StorageExtensions.Load();

                    foreach (Xrm.CrmConnection crmConnection in crmConnections)
                    {
                        this.cbx_connection.Items.Add(crmConnection.Name);
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Ignore File Not found
                }
            }
            else
            {
                EncryptionKey encryptionKey = new EncryptionKey();
                encryptionKey.ShowDialog();

                if (MainWindow.EncryptionKey != string.Empty && MainWindow.EncryptionKey != null)
                {
                    this.ReloadConnections();
                }
                else
                {
                    this.Close();
                }
            }

            this.CheckForUpdate();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the Encryption Key
        /// </summary>
        public static string EncryptionKey { get; set; }

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

        /// <summary>
        /// Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose class
        /// </summary>
        /// <param name="disposing">bool disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }

                // Free your own state (unmanaged objects).
                // Set large fields to null.
                this.disposed = true;
                this.worker.Dispose();
                this.crmSolutions = null;
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
        /// Event Selection Changed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Cbx_connection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Dtg_Solutions.ItemsSource = null;

                // Will cause an error when connections was flushed
                if (this.cbx_connection.SelectedItem.ToString() == "New")
                {
                    ConnectionManger connectionManger = new ConnectionManger();
                    connectionManger.ShowDialog();
                    this.ReloadConnections();
                }
                else
                {
                    this.loadingPanel.IsLoading = true;

                    this.worker.DoWork += this.Worker_DoWork;
                    this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
                    this.worker.RunWorkerAsync();

                    this.selectedCrmConnection = this.cbx_connection.SelectedItem.ToString();
                }
            }
            catch (NullReferenceException)
            {
                // Ignore, reload will change the index and will trigger this without items
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                loadingPanel.IsLoading = false;
            }
        }

        /// <summary>
        /// Background Worker Event DoWork
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector())
            {
                List<Xrm.CrmConnection> crmConnections = StorageExtensions.Load();
                Xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == this.selectedCrmConnection);

                // Get Crm Solutions
                this.crmSolutions.Clear();
                this.crmSolutions = toolingConnector.GetCrmSolutions(toolingConnector.GetCrmServiceClient(crmConnection.ConnectionString));
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

            this.loadingPanel.IsLoading = false;
        }

        /// <summary>
        /// Reloads the Connection Drop down after a connection was created
        /// </summary>
        private void ReloadConnections()
        {
            try
            {
                List<Xrm.CrmConnection> crmConnections = StorageExtensions.Load();
                this.cbx_connection.Items.Clear();
                this.Dtg_Solutions.ItemsSource = null;

                this.cbx_connection.Items.Add("New");

                foreach (Xrm.CrmConnection crmConnection in crmConnections)
                {
                    this.cbx_connection.Items.Add(crmConnection.Name);
                }

                Btn_OpenConnectionOverview.IsEnabled = true;
            }
            catch (System.IO.FileNotFoundException)
            {
                // Ignore File Not found
            }
        }

        /// <summary>
        /// Download Button Click Event
        /// </summary>
        /// <param name="sender">Button sender object</param>
        /// <param name="e">Button event args</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_connection.SelectedItem != null)
            {
                int downloadCounter = 0;
                List<Xrm.CrmConnection> crmConnections = StorageExtensions.Load();
                Xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == cbx_connection.SelectedItem.ToString());

                if (Dtg_Solutions.ItemsSource != null)
                {
                    List<Xrm.CrmSolution> crmSolutions = new List<Xrm.CrmSolution>();
                    foreach (Xrm.CrmSolution crmSolution in Dtg_Solutions.ItemsSource)
                    {
                        if (crmSolution.DownloadIsChecked)
                        {
                            downloadCounter++;
                            crmSolutions.Add(crmSolution);
                        }
                    }

                    if (downloadCounter != 0)
                    {
                        DownloadMultiple downloadMultiple = new DownloadMultiple(crmConnection, crmSolutions);
                        downloadMultiple.ShowDialog();
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
        /// Menu Item Click Event, Info
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
            about.Dispose();
        }

        /// <summary>
        /// Menu Item Click Event, Update
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            // ToDo
            Update.UpdateChecker updateChecker = new Update.UpdateChecker();

            if (updateChecker.IsUpdateAvailable())
            {
                Uri uri = updateChecker.GetUpdateURL();

                if (uri != null)
                {
                    System.Diagnostics.Process.Start(uri.ToString());
                }
                else
                {
                    MessageBox.Show("Failed to retrive Update URL", "Failed to get URL", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("There is no Update available. Please visit Github if you are looking for a PreRelease", "No Update available", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Menu Item Click Event, Exit
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Enables Debugging
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Cbx_DebuggingEnabled_Click(object sender, RoutedEventArgs e)
        {
            if (Cbx_DebuggingEnabled.IsChecked)
            {
                Application.Current.Properties["Debugging.Enabled"] = true;
            }
            else
            {
                Application.Current.Properties["Debugging.Enabled"] = false;
            }
        }

        /// <summary>
        /// Button Click, Opens the Connection Overview Dialog
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_OpenConnectionOverview_Click(object sender, RoutedEventArgs e)
        {
            ConnectionOverview connectionOverview = new ConnectionOverview();
            connectionOverview.ShowDialog();

            this.ReloadConnections();
        }

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        private void CheckForUpdate()
        {
            Update.UpdateChecker updateChecker = new Update.UpdateChecker();
            if (updateChecker.IsUpdateAvailable())
            {
                Uri uri = updateChecker.GetUpdateURL();

                if (uri != null)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("There is an new Update available, would you like to Download it?", "Update available!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(uri.ToString());
                    }
                }
            }
        }
    }
}