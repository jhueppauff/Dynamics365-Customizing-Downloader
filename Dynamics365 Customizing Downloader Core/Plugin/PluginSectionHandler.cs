//-----------------------------------------------------------------------
// <copyright file="PluginSectionHandler.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Plugin
{
    using System.Xml;


    class PluginSectionHandler
    {
        public PluginSectionHandler()
        {
        }
        // Iterate through all the child nodes
        //   of the XMLNode that was passed in and create instances
        //   of the specified Types by reading the attribite values of the nodes
        //   we use a try/Catch here because some of the nodes
        //   might contain an invalid reference to a plugin type
        public object Create(object parent, object configContext, XmlNode section)
        {
            // PluginCollection plugins = new PluginCollection();

            foreach (XmlNode node in section.ChildNodes)
            {
                //Code goes here to instantiate
                //and invoke the plugins

            }
            return plugins;
        }
    }
}
