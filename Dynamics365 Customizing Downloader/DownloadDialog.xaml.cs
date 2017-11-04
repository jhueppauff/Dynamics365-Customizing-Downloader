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
    using System.Windows;

    /// <summary>
    /// Interaction logic for DownloadDialog.xaml
    /// </summary>
    public partial class DownloadDialog : Window
    {
        public string CrmSolutionName, CrmConnectionString;

        public DownloadDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button Action, Download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_download_Click(object sender, RoutedEventArgs e)
        {
            xrm.ToolingConnector toolingConnector = new xrm.ToolingConnector();
            toolingConnector.DownloadSolution(toolingConnector.GetCrmServiceClient(this.CrmConnectionString), this.CrmSolutionName, tbx_filepath.Text);
            this.Close();
        }

        /// <summary>
        /// Button Action, Select Path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_selectPath_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result.ToString() == "OK")
                {
                    tbx_filepath.Text = dialog.SelectedPath;
                }
            }
        }
    }
}
