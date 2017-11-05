//-----------------------------------------------------------------------
// <copyright file="DownloadDialog.xaml.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DownloadDialog
    /// </summary>
    public partial class DownloadDialog : Window
    {
        #region Fields
        /// <summary>
        /// BackGround Worker
        /// </summary>
        private readonly BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// selected File Path
        /// </summary>
        private string selectedPath;

        /// <summary>
        /// Indicates if the panel is loading
        /// </summary>
        private bool panelLoading;

        /// <summary>
        /// Panel Message
        /// </summary>
        private string panelMainMessage = "Please wait, downloading and extracting Solution";

        /// <summary>
        /// Indicates if a Solution should be extracted
        /// </summary>
        private bool extractSolution = false;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadDialog"/> class
        /// </summary>
        public DownloadDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        /// <summary>
        /// Gets or sets the CRM Solution Name
        /// </summary>
        public string CrmSolutionName { get; set; }

        /// <summary>
        /// Gets or sets the CRM Connection String
        /// </summary>
        public string CrmConnectionString { get; set; }

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
        #endregion

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Button Action, Download
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_download_Click(object sender, RoutedEventArgs e)
        {
            this.loadingPanel.IsLoading = true;
            this.selectedPath = this.tbx_filepath.Text;

            extractSolution = (bool)this.cbx_extract.IsChecked;

            // Background Worker
            this.worker.DoWork += this.Worker_DoWork;
            this.worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// Background Worker Event DoWork
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector();
            toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(this.CrmConnectionString), this.CrmSolutionName, this.selectedPath);

            Xrm.CrmSolutionPackager crmSolutionPackager = new Xrm.CrmSolutionPackager();
            if (extractSolution)
            {
                crmSolutionPackager.ExtractCustomizing(Path.Combine(this.selectedPath, this.CrmSolutionName + ".zip"), Path.Combine(this.selectedPath, this.CrmSolutionName));
            }
            
            File.Delete(Path.Combine(this.selectedPath, this.CrmSolutionName + ".zip"));
        }

        /// <summary>
        /// Background Worker Event Worker_RunWorkerCompleted
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.loadingPanel.IsLoading = false;
            this.Close();
        }

        /// <summary>
        /// Button Action, Select Path
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_selectPath_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result.ToString() == "OK")
                {
                    this.tbx_filepath.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
