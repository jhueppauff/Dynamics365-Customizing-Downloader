using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestCryptographyModule()
        {
            string encryptedString = Dynamics365CustomizingDownloader.Core.Data.Cryptography.EncryptStringAES("Something", "secret");
            if (Dynamics365CustomizingDownloader.Core.Data.Cryptography.DecryptStringAES(encryptedString, "secret") != "Something")
            {
                throw new Exception("String does not match");
            }
        }
    }
}
