//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
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
    using System.IO;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dispose bool
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.InitializeComponent();
            Application.Current.Properties["Debugging.Enabled"] = false;

            if (!File.Exists(Core.Data.StorageExtensions.StoragePath))
            {
                Btn_OpenConnectionOverview.IsEnabled = false;
            }

            if (MainWindow.EncryptionKey == null || MainWindow.EncryptionKey == string.Empty)
            {
                EncryptionKey encryptionKey = new EncryptionKey();
                encryptionKey.ShowDialog();

                if (MainWindow.EncryptionKey != string.Empty && MainWindow.EncryptionKey != null)
                {
                    Pages.SolutionSelector solutionSelector = new Pages.SolutionSelector();
                    solutionSelector.ReloadConnections();
                }
                else
                {
                    this.Close();
                }
            }

            this.CheckForUpdate();
        }



        /// <summary>
        /// Gets or sets the Encryption Key
        /// </summary>
        public static string EncryptionKey { get; set; }



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
            this.CheckForUpdate();
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
            if ((bool)Cbx_DebuggingEnabled.IsChecked)
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

            //Pages.SolutionSelector.ReloadConnections();
        }

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        private void CheckForUpdate()
        {
            if (Properties.Settings.Default.CheckForUpdates)
            {
                Core.Update.UpdateChecker updateChecker = new Core.Update.UpdateChecker();
                if (updateChecker.IsUpdateAvailable($"{Properties.Settings.Default.GitHubAPIURL}/releases/latest"))
                {
                    PatchNotes patchNotes = new PatchNotes();
                    patchNotes.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Button Click, Opens the Connection Overview Dialog
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemPatchNotes_Click(object sender, RoutedEventArgs e)
        {
            PatchNotes patchNotes = new PatchNotes();
            patchNotes.Show();
        }
    }
}