//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 julian
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Properties
{
    /// <summary>
    /// This class allows you to handle specific events on the settings class:
    /// The SettingChanging event is raised before a setting's value is changed.
    /// The PropertyChanged event is raised after a setting's value is changed.
    /// The SettingsLoaded event is raised after the setting values are loaded.
    /// The SettingsSaving event is raised before the setting values are saved.
    /// </summary>
    internal sealed partial class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            // Add Code to handle events
        }

        /// <summary>
        /// Setting Changing Event handler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SettingChangingEventArgs"/> instance containing the event data.</param>
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        /// <summary>
        /// Setting saving Event handler
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }
    }
}