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
