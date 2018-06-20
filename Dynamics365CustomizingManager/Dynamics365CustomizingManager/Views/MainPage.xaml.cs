using System;

using Dynamics365CustomizingManager.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Dynamics365CustomizingManager.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
