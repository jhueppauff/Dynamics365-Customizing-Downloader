//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Overwrite for the Application Startup
        /// </summary>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}