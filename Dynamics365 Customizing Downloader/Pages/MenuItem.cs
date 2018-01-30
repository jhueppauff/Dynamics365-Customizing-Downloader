//-----------------------------------------------------------------------
// <copyright file="MenuItem.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Pages
{
    /// <summary>
    /// Menu Item Class
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the Display Name of the Menu Item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description of the Menu Item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the window which should be opened
        /// </summary>
        public object Content { get; set; }
    }
}
