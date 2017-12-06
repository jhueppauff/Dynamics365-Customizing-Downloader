using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dynamics365CustomizingDownloader;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestCryptographyModule()
        {
            string encryptedString = Dynamics365CustomizingDownloader.Data.Cryptography.EncryptStringAES("Something", "secret");
            if (Dynamics365CustomizingDownloader.Data.Cryptography.DecryptStringAES(encryptedString, "secret") != "Something")
            {
                throw new Exception("String does not match");
            }
        }
    }
}
