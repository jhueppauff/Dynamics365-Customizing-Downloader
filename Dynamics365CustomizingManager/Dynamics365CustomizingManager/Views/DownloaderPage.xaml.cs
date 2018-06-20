using System;

using Dynamics365CustomizingManager.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Dynamics365CustomizingManager.Views
{
    public sealed partial class DownloaderPage : Page
    {
        private DownloaderViewModel ViewModel
        {
            get { return DataContext as DownloaderViewModel; }
        }

        // TODO WTS: Change the grid as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
        public DownloaderPage()
        {
            InitializeComponent();
        }
    }
}
