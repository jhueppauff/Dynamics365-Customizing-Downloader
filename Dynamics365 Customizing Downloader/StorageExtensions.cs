//-----------------------------------------------------------------------
// <copyright file="StorageExtensions.cs" company="None">
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
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// JSON.Net Storage Adapter for saving CRM Connection to file
    /// </summary>
    public static class StorageExtensions
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Local Path of the JSON Storage File
        /// </summary>
        private static string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dyn365_Configuration.json");

        /// <summary>
        /// Gets the local path of the JSON Storage File
        /// </summary>
        public static string StoragePath
        {
            get
            {
                return storagePath;
            }
        }

        /// <summary>
        /// Saves the <see cref="Xrm.CrmConnection"/> to the local Configuration
        /// </summary>
        /// <param name="crmConnection">CRM Connection <see cref="Xrm.CrmConnection"/></param>
        public static void Save(Xrm.CrmConnection crmConnection)
        {
            List<Xrm.CrmConnection> crmConnections = new List<Xrm.CrmConnection>();

            // Create if File does not exist
            if (!File.Exists(StoragePath))
            {
                File.Create(StoragePath).Close();

                // Encrypt Connection String
                crmConnection.ConnectionString = Cryptography.EncryptStringAES(crmConnection.ConnectionString);
                crmConnection.LocalPath = string.Empty;
                crmConnections.Add(crmConnection);
                string json = JsonConvert.SerializeObject(crmConnections);

                File.WriteAllText(StoragePath, json);
            }
            else
            {
                using (FileStream fs = File.Open(StoragePath, FileMode.Open))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        stringBuilder.AppendLine(temp.GetString(b));
                    }

                    // Convert Json to List
                    crmConnections = JsonConvert.DeserializeObject<List<Xrm.CrmConnection>>(stringBuilder.ToString());

                    // Encrypt Connection String
                    crmConnection.ConnectionString = Cryptography.EncryptStringAES(crmConnection.ConnectionString);
                    crmConnection.LocalPath = string.Empty;
                    crmConnections.Add(crmConnection);

                    // Close File Stream
                    fs.Flush();
                    fs.Close();
                }

                // Write to Configuration File
                string json = JsonConvert.SerializeObject(crmConnections);
                File.WriteAllText(StoragePath, json);
            }
        }

        /// <summary>
        /// Loads the current configuration
        /// </summary>
        /// <returns>Returns <see cref="List{Xrm.CrmConnection}"/></returns>
        public static List<Xrm.CrmConnection> Load()
        {
            List<Xrm.CrmConnection> crmConnections;

            if (File.Exists(StoragePath))
            {
                using (FileStream fs = File.OpenRead(StoragePath))
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

                    foreach (Xrm.CrmConnection crmTempConnection in crmConnections)
                    {
                        crmTempConnection.ConnectionString = Cryptography.DecryptStringAES(crmTempConnection.ConnectionString);
                    }

                    // Close File Stream
                    fs.Flush();
                    fs.Close();
                }
            }
            else
            {
                StorageExtensions.Log.Error("File was not found", new FileNotFoundException("File was not found", StoragePath));
                throw new FileNotFoundException("File was not found", StoragePath);
            }

            return crmConnections;
        }

        /// <summary>
        /// Updates an existing CRM Connection
        /// </summary>
        /// <param name="crmConnection">CRM Connection <see cref="Xrm.CrmConnection"/></param>
        public static void Update(Xrm.CrmConnection crmConnection)
        {
            string json = File.ReadAllText(StoragePath);
            List<Xrm.CrmConnection> crmConnections = JsonConvert.DeserializeObject<List<Xrm.CrmConnection>>(json);

            crmConnections.Find(x => x.ConnectionID == crmConnection.ConnectionID).ConnectionString = Cryptography.EncryptStringAES(crmConnection.ConnectionString);
            crmConnections.Find(x => x.ConnectionID == crmConnection.ConnectionID).LocalPath = crmConnection.LocalPath;
            crmConnections.Find(x => x.ConnectionID == crmConnection.ConnectionID).Name  = crmConnection.Name;

            string jsonNew = JsonConvert.SerializeObject(crmConnections);
            File.WriteAllText(StoragePath, jsonNew);
        }

        /// <summary>
        /// Gets the Connection ID by the Connection Name
        /// </summary>
        /// <param name="connectionName"><see cref="string"/> Connection Name</param>
        /// <returns>Returns the <see cref="Guid"/> of the connection.</returns>
        public static Guid FindConnectionIDByName(string connectionName)
        {
            string json = File.ReadAllText(StoragePath);
            List<Xrm.CrmConnection> crmConnections = JsonConvert.DeserializeObject<List<Xrm.CrmConnection>>(json);

            // Add GUID if it does not exists
            if (crmConnections.Find(x => x.Name == connectionName).ConnectionID == Guid.Empty)
            {
                Guid connectionID = Guid.NewGuid();
                
                crmConnections.Find(x => x.Name == connectionName).ConnectionID = connectionID;

                string jsonNew = JsonConvert.SerializeObject(crmConnections);
                File.WriteAllText(StoragePath, jsonNew);
            }

            return crmConnections.Find(x => x.Name == connectionName).ConnectionID;
        }

        /// <summary>
        /// Adds a Guid to a Connection if no GUID exists
        /// </summary>
        /// <param name="connectionName">Name Connection</param>
        /// <returns>Returns the <see cref="Guid"/>of the Updated Connection</returns>
        public static Guid UpdateConnectionWithID(string connectionName)
        {
            Guid connectionID = Guid.NewGuid();
            string json = File.ReadAllText(StoragePath);
            List<Xrm.CrmConnection> crmConnections = JsonConvert.DeserializeObject<List<Xrm.CrmConnection>>(json);

            if (crmConnections.Find(x => x.Name == connectionName).ConnectionID == Guid.Empty)
            {
                crmConnections.Find(x => x.Name == connectionName).ConnectionID = connectionID;

                string jsonNew = JsonConvert.SerializeObject(crmConnections);
                File.WriteAllText(StoragePath, jsonNew);

                return connectionID;
            }
            else
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Gets the count of all Connections with the specified connectionName
        /// </summary>
        /// <param name="connectionName">Name of the Connection</param>
        /// <returns>Returns a count <see cref="int"/> of all found connections</returns>
        public static int GetCountOfConnectionNamesByName(string connectionName)
        {
            string json = File.ReadAllText(StoragePath);
            List<Xrm.CrmConnection> crmConnections = JsonConvert.DeserializeObject<List<Xrm.CrmConnection>>(json);

            int count = 0;

            count = crmConnections.Count(x => x.Name == connectionName);

            return count;
        }
    }
}