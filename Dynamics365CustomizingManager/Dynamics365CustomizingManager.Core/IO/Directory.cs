//-----------------------------------------------------------------------
// <copyright file="Directory.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingManager.Core.IO
{
    using System.IO;

    /// <summary>
    /// Directory implementation
    /// </summary>
    public static class Directory
    {
        /// <summary>
        /// Deletes a Folder and everything below
        /// </summary>
        /// <param name="targetDirectory">Path of the Directory to delete.</param>
        public static void DeleteDirectory(string targetDirectory)
        {
            string[] files = System.IO.Directory.GetFiles(targetDirectory);
            string[] dirs = System.IO.Directory.GetDirectories(targetDirectory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            System.IO.Directory.Delete(targetDirectory, false);
        }
    }
}