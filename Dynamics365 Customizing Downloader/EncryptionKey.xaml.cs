//-----------------------------------------------------------------------
// <copyright file="EncryptionKey.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
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