//-----------------------------------------------------------------------
// <copyright file="IConfigurationSectionHandler.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Plugin
{
    interface IConfigurationSectionHandler
    {
        object Create(object parent, object configContext, System.Xml.XmlNode section);
    }
}
