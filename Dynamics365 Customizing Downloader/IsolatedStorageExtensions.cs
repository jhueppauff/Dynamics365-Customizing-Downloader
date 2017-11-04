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

    public static class IsolatedStorageExtensions
    {
        private static string storagePath = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, "Dyn365_Configuration.dat");

        public static void SaveObject(this IsolatedStorage isoStorage, object obj)
        {
            IsolatedStorageFileStream writeStream = new IsolatedStorageFileStream(storagePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(writeStream, obj);
            writeStream.Flush();
            writeStream.Close();
        }

        public static T LoadObject<T>(this IsolatedStorage isoStorage)
        {
            IsolatedStorageFileStream readStream = new IsolatedStorageFileStream(storagePath, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            T readData = (T)formatter.Deserialize(readStream);
            readStream.Flush();
            readStream.Close();

            return readData;
        }
    }



    [Serializable]
    internal class DataStoreContainer
    {
        public DataStoreContainer()
        {
            List<xrm.CrmConnection> crmConnection = new List<xrm.CrmConnection>();
        }

        public List<xrm.CrmConnection> CrmConnections { get; set; }
    }


}
