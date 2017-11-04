//-----------------------------------------------------------------------
// <copyright file="IsolatedStorageExtensions.cs" company="None">
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
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public static class StorageExtensions
    {
        private static string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dyn365_Configuration.json");

        public static void Save(xrm.CrmConnection crmConnection)
        {
            List<xrm.CrmConnection> crmConnections = new List<xrm.CrmConnection>();

            // Create if File does not exist
            if (!File.Exists(storagePath))
            {
                File.Create(storagePath).Close();
                crmConnections.Add(crmConnection);
                string json = JsonConvert.SerializeObject(crmConnections);

                File.WriteAllText(storagePath, json);
            }
            else
            {


                using (FileStream fs = File.Open(storagePath, FileMode.Open))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        stringBuilder.AppendLine(temp.GetString(b));
                    }

                    crmConnections = JsonConvert.DeserializeObject<List<xrm.CrmConnection>>(stringBuilder.ToString());

                    crmConnections.Add(crmConnection);

                    fs.Flush();
                    fs.Close();
                }
                string json = JsonConvert.SerializeObject(crmConnections);
                File.WriteAllText(storagePath, json);
            }
        }

        public static List<xrm.CrmConnection> Load()
        {
            List<xrm.CrmConnection> crmConnections = new List<xrm.CrmConnection>();
            using (FileStream fs = File.OpenRead(storagePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    stringBuilder.AppendLine(temp.GetString(b));
                }

                crmConnections = JsonConvert.DeserializeObject<List<xrm.CrmConnection>>(stringBuilder.ToString());

                fs.Flush();
                fs.Close();
            }

            return crmConnections;
        }
    }
}
