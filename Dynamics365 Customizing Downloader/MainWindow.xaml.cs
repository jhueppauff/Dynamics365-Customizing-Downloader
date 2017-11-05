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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        private string selectedCrmConnection;

        private List<xrm.CrmSolution> crmSolutions = new List<xrm.CrmSolution>();

        private bool panelLoading;
        private string panelMainMessage = "Please wait, connecting to Crm and retriving the Solutions";

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
                return panelLoading;
            }
            set
            {
                panelLoading = value;
                RaisePropertyChanged("PanelLoading");
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
                return panelMainMessage;
            }
            set
            {
                panelMainMessage = value;
                RaisePropertyChanged("PanelMainMessage");
            }
        }

        /// <summary>
        /// Gets the hide panel command.
        /// </summary>
        public ICommand HidePanelCommand
        {
            get
            {
                return new HelperClasses.DelegateCommand(() =>
                {
                    PanelLoading = false;
                });
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            cbx_connection.Items.Add("New");
            try
            {
                List<xrm.CrmConnection> crmConnections = StorageExtensions.Load();

                foreach (xrm.CrmConnection crmConnection in crmConnections)
                {
                    cbx_connection.Items.Add(crmConnection.Name);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                // Ignor File Not found
            }
        }

        private void cbx_connection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Will cause an error when connections was flushed
                if (cbx_connection.SelectedItem.ToString() == "New")
                {
                    ConnectionManger connectionManger = new ConnectionManger();
                    connectionManger.ShowDialog();
                    ReloadConnections();
                }
                else
                {
                    loadingPanel.IsLoading = true;

                    worker.DoWork += worker_DoWork;
                    worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    worker.RunWorkerAsync();

                    selectedCrmConnection = cbx_connection.SelectedItem.ToString();


                }
            }
            catch (NullReferenceException)
            {
                // Ignor, reload will change the index and will trigger this without items
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured: " + ex.Message);
                loadingPanel.IsLoading = false;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            xrm.ToolingConnector toolingConnector = new xrm.ToolingConnector();

            List<xrm.CrmConnection> crmConnections = StorageExtensions.Load();
            xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == selectedCrmConnection);

            // Get Crm Solutions
            crmSolutions.Clear();
            crmSolutions = toolingConnector.GetCrmSolutions(toolingConnector.GetCrmServiceClient(crmConnection.ConnectionString));

            
        }

        private void worker_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            foreach (xrm.CrmSolution crmSolution in crmSolutions)
            {
                cbx_crmsolution.Items.Add(crmSolution.UniqueName);
            }

            loadingPanel.IsLoading = false;
        }

        /// <summary>
        /// Reloads the Connection Drop down after a connection was created
        /// </summary>
        private void ReloadConnections()
        {
            try
            {
                List<xrm.CrmConnection> crmConnections = StorageExtensions.Load();
                cbx_connection.Items.Clear();

                cbx_connection.Items.Add("New");

                foreach (xrm.CrmConnection crmConnection in crmConnections)
                {
                    cbx_connection.Items.Add(crmConnection.Name);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                // Ignor File Not found
            }
        }

        /// <summary>
        /// Download Button Click Event
        /// </summary>
        /// <param name="sender">Button sender object</param>
        /// <param name="e">Button event args</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<xrm.CrmConnection> crmConnections = StorageExtensions.Load();
            xrm.CrmConnection crmConnection = crmConnections.Find(x => x.Name == cbx_connection.SelectedItem.ToString());

            DownloadDialog downloadDialog = new DownloadDialog
            {
                CrmSolutionName = cbx_crmsolution.SelectedItem.ToString(),
                CrmConnectionString = crmConnection.ConnectionString
            };
            downloadDialog.ShowDialog();
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
