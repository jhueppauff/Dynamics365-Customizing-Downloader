//-----------------------------------------------------------------------
// <copyright file="EncryptionKey.xaml.cs" company="None">
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
                bool keyCorrect = false;
                if (!File.Exists(StorageExtensions.StoragePath))
                {
                    MainWindow.EncryptionKey = Tbx_EncryptionKey.Password;
                    this.Close();
                }
                else
                {
                    MainWindow.EncryptionKey = Tbx_EncryptionKey.Password;

                    // Connections are already created, need to check Encryption Key
                    List<Xrm.CrmConnection> crmConnections = new List<Xrm.CrmConnection>();
                    using (FileStream fs = File.OpenRead(StorageExtensions.StoragePath))
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        byte[] b = new byte[1024];
                        UTF8Encoding temp = new UTF8Encoding(true);

                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            stringBuilder.AppendLine(temp.GetString(b));
                        }

                        // Converts Json to List
                        crmConnections = JsonConvert.DeserializeObject<List<Xrm.CrmConnection>>(stringBuilder.ToString());
                        try
                        {
                            foreach (Xrm.CrmConnection crmTempConnection in crmConnections)
                            {
                                crmTempConnection.ConnectionString = Cryptography.DecryptStringAES(crmTempConnection.ConnectionString);
                            }

                            keyCorrect = true;
                        }
                        catch (Exception)
                        {
                            keyCorrect = false;
                            MessageBox.Show("Encryption Key does not match!", "Wrong Encryption Key", MessageBoxButton.OK, MessageBoxImage.Error);
                            MainWindow.EncryptionKey = null;
                        }

                        // Close File Stream
                        fs.Flush();
                        fs.Close();

                        if (keyCorrect)
                        {
                            this.Close();
                        }
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
    }
}