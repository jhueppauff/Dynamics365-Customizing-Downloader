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
            string encryptedString = Dynamics365CustomizingDownloader.Cryptography.EncryptStringAES("Something", "secret");
            if (Dynamics365CustomizingDownloader.Cryptography.DecryptStringAES(encryptedString, "secret") != "Something")
            {
                throw new Exception("String does not match");
            }
        }
    }
}
