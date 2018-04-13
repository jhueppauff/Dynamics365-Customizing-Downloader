//-----------------------------------------------------------------------
// <copyright file="EncryptionKey.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
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
    using System.Text;
    using System.Windows;
    using Newtonsoft.Json;

    /// <summary>
    /// Interaction logic for EncryptionKey
    /// </summary>
    public partial class EncryptionKey : Window
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionKey"/> class.
        /// </summary>
        public EncryptionKey()
        {
            this.InitializeComponent();
            Tbx_EncryptionKey.Focus();
        }

        /// <summary>
        /// Button Action, save Encryption Key
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_SaveEncryptionKey_Click(object sender, RoutedEventArgs e)
        {
            if (Tbx_EncryptionKey.Password != string.Empty)
            {
                if (!File.Exists(Core.Data.StorageExtensions.StoragePath))
                {
                    MainWindow.EncryptionKey = Tbx_EncryptionKey.Password;
                    this.Close();
                }
                else
                {
                    if (this.ValidateKey())
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a Encryption Key", "No Key entered", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Key Down Event, will invoke save on key down enter within the password box
        /// </summary>
        /// <param name="sender">Button sender object</param>
        /// <param name="e">Button event args</param>
        private void Tbx_EncryptionKey_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Btn_SaveEncryptionKey_Click(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Validates the Encryption Key against the config store
        /// </summary>
        /// <returns>Returns <see cref="bool"/>true/false if the key is correct</returns>
        private bool ValidateKey()
        {
            MainWindow.EncryptionKey = Tbx_EncryptionKey.Password;

            // Connections are already created, need to check Encryption Key
            List<Core.Xrm.CrmConnection> crmConnections;

            using (StreamReader streamReader = new StreamReader(Core.Data.StorageExtensions.StoragePath))
            {
                string json = streamReader.ReadToEnd();

                // Converts Json to List
                crmConnections = JsonConvert.DeserializeObject<List<Core.Xrm.CrmConnection>>(json);
                try
                {
                    foreach (Core.Xrm.CrmConnection crmTempConnection in crmConnections)
                    {
                        crmTempConnection.ConnectionString = Core.Data.Cryptography.DecryptStringAES(crmTempConnection.ConnectionString, MainWindow.EncryptionKey);
                    }

                    streamReader.Close();
                    return true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Encryption Key does not match!", "Wrong Encryption Key", MessageBoxButton.OK, MessageBoxImage.Error);
                    MainWindow.EncryptionKey = null;
                    EncryptionKey.Log.Error("Encryption Key does not match!");
                    return false;
                }
            }
        }
    }
}