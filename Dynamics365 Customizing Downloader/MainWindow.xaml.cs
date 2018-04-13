//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// App Insight Helper
        /// </summary>
        private ApplicationInsightHelper applicationInsightHelper = null;

        /// <summary>
        /// Busy Indicator
        /// </summary>
        private static Xceed.Wpf.Toolkit.BusyIndicator busyIndicator = new Xceed.Wpf.Toolkit.BusyIndicator();

        /// <summary>
        /// Dispose bool
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Menu Items
        /// </summary>
        private List<Pages.MenuItem> menuItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="applicationInsightHelper">AI Helper</param>
        public MainWindow(ApplicationInsightHelper applicationInsightHelper)
        {
            log4net.Config.XmlConfigurator.Configure();
            this.InitializeComponent();
            this.applicationInsightHelper = applicationInsightHelper;

            // Load Language
            this.LoadLanguage(Properties.Settings.Default.Culture);

            busyIndicator = this.prg_progress;
            MainWindow.SetProgress(false);

            if (Properties.Settings.Default.DebuggingEnabled)
            {
                Application.Current.Properties["Debugging.Enabled"] = true;
            }
            else
            {
                Application.Current.Properties["Debugging.Enabled"] = false;
            }

            if (!File.Exists(Core.Data.StorageExtensions.StoragePath))
            {
                Btn_OpenConnectionOverview.IsEnabled = false;
            }

            if (string.IsNullOrEmpty(MainWindow.EncryptionKey))
            {
                EncryptionKey encryptionKey = new EncryptionKey();
                encryptionKey.ShowDialog();

                if (!string.IsNullOrEmpty(MainWindow.EncryptionKey))
                {
                    Pages.SolutionSelector solutionSelector = new Pages.SolutionSelector();
                    solutionSelector.ReloadConnections();
                    this.menuItems = this.GetMenu();
                    this.Lbx_Menu.ItemsSource = this.menuItems;
                    this.CheckForUpdate();
                }
                else
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Gets or sets the AI Helper
        /// </summary>
        public static ApplicationInsightHelper ApplicationInsightHelper { get; set; }

        /// <summary>
        /// Gets or sets the Encryption Key
        /// </summary>
        public static string EncryptionKey { get; set; }

        /// <summary>
        /// Sets the Progress Indicator to the specific state
        /// </summary>
        /// <param name="state">IsBusy State <see cref="bool"/></param>
        public static void SetProgress(bool state)
        {
            MainWindow.busyIndicator.IsBusy = state;
        }

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
        /// Load the specified language
        /// </summary>
        /// <param name="locale">Language Code</param>
        private void LoadLanguage(string locale)
        {
            var resources = new ResourceDictionary
            {
                Source = new Uri($"Lang/localizedMessage-{locale}.xaml", UriKind.Relative)
            };

            var current = Application.Current.Resources.MergedDictionaries.FirstOrDefault(m => m.Source.OriginalString.EndsWith($"{locale}.xaml"));

            if (current != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(current);
            }

            Application.Current.Resources.MergedDictionaries.Add(resources);
        }

        /// <summary>
        /// Generates the Side Menu Bar
        /// </summary>
        /// <returns>Returns a <see cref="List{Pages.MenuItem}"/></returns>
        private List<Pages.MenuItem> GetMenu()
        {
            List<Pages.MenuItem> listMenuItems = new List<Pages.MenuItem>();

            // Customizing Extractor
            Pages.SolutionSelector solutionSelector = new Pages.SolutionSelector();
            solutionSelector.ReloadConnections();

            Pages.MenuItem menuItem = new Pages.MenuItem
            {
                Name = (string)Application.Current.FindResource("MainWindow_Code_Customizing_Downloader"),
                Description = (string)Application.Current.FindResource("MainWindow_Code_Customizing_Downloader_Description"),
                Content = solutionSelector
            };

            listMenuItems.Add(menuItem);

            // Solution Downloader
            Pages.SolutionDownloader solutionDownloader = new Pages.SolutionDownloader();

            menuItem = new Pages.MenuItem()
            {
                Name = (string)Application.Current.FindResource("MainWindow_Code_Solution_Downloader"),
                Description = (string)Application.Current.FindResource("MainWindow_Code_Solution_Downloader_Description"),
                Content = solutionDownloader
            };

            listMenuItems.Add(menuItem);

            // Solution Uploader
            Pages.SolutionUploader solutionUploader = new Pages.SolutionUploader();

            menuItem = new Pages.MenuItem()
            {
                Name = (string)Application.Current.FindResource("MainWindow_Code_Solution_Uploader"),
                Description = (string)Application.Current.FindResource("MainWindow_Code_Solution_Uploader_Description"),
                Content = solutionUploader
            };

            listMenuItems.Add(menuItem);
            return listMenuItems;
        }

        /// <summary>
        /// Menu Item Click Event, Update
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.CheckForUpdate(true);
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
        /// Button Click, Opens the Connection Overview Dialog
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_OpenConnectionOverview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ConnectionOverview connectionOverview = new ConnectionOverview();
                connectionOverview.ShowDialog();

                ((Pages.SolutionSelector)this.menuItems[0].Content).ReloadConnections();
            }
            catch (Exception ex)
            {
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                errorReport.Show();
            }
        }

        /// <summary>
        /// Checks if an Update is available
        /// </summary>
        /// <param name="interactive">Determines if the Check was initialized by a User</param>
        private void CheckForUpdate(bool interactive = false)
        {
            if (Properties.Settings.Default.CheckForUpdates)
            {
                Core.Update.UpdateChecker updateChecker = new Core.Update.UpdateChecker();

                if (updateChecker.IsUpdateAvailable($"{Properties.Settings.Default.GitHubAPIURL}/releases/latest"))
                {
                    PatchNotes patchNotes = new PatchNotes();
                    patchNotes.ShowDialog();
                }
                else if (interactive)
                {
                    MessageBox.Show((string)Application.Current.FindResource("MainWindow_Code_RunningLatestVersion"), (string)Application.Current.FindResource("MainWindow_Code_RunningLatestVersion_Caption"), MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Button Click, Change Language => German
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_ChangeLanguageGerman_Click(object sender, RoutedEventArgs e)
        {
            this.LoadLanguage("de-de");
            Btn_ChangeLanguageGerman.IsEnabled = false;
            Btn_ChangeLanguageEnglish.IsEnabled = true;
        }

        /// <summary>
        /// Button Click, Change Language => English
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_ChangeLanguageEnglish_Click(object sender, RoutedEventArgs e)
        {
            this.LoadLanguage("en-us");
            Btn_ChangeLanguageGerman.IsEnabled = true;
            Btn_ChangeLanguageEnglish.IsEnabled = false;
        }

        /// <summary>
        /// Closes the whole Application
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Win_MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}