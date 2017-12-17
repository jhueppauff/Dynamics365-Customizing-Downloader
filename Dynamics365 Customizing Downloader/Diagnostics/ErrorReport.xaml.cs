//-----------------------------------------------------------------------
// <copyright file="ErrorReport.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Diagnostics
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ErrorReport
    /// </summary>
    public partial class ErrorReport : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorReport"/> class.
        /// </summary>
        /// <param name="ex">The <see cref="System.Exception"/> for the Error Report</param>
        /// <param name="customMessage">If this is set, the Custom Message will be displayed before the <see cref="Exception.Message"/> in the Error Dialog.</param>
        public ErrorReport(Exception ex, string customMessage = "")
        {
            this.InitializeComponent();

            if (customMessage != string.Empty)
            {
                this.Tbx_ErrorMessage.Text = $"{customMessage} : {ex.Message}";
            }
            else
            {
                this.Tbx_ErrorMessage.Text = ex.Message;
            }
            
            this.Tbx_StackTrace.Text = ex.StackTrace;
        }

        /// <summary>
        /// Button Click Close Window
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
