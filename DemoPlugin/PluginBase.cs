//-----------------------------------------------------------------------
// <copyright file="PluginBase.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

using Dynamics365CustomizingDownloader.Core.Plugin;

namespace DemoPlugin
{
    public class PluginBase : IPlugin
    {
        public string Name => "DemoPlugin";

        public PluginBase()
        {
        }

        public void PerformAction(IPluginContext context)
        {
           var repository = context.CurrentRepository;
        }
    }
}
