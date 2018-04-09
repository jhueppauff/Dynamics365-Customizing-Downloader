//-----------------------------------------------------------------------
// <copyright file="UnitTest.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace UnitTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit Test
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// Tests the Cryptography Module if the Data could be Encrypted and Decrypted without losing any Data
        /// </summary>
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
