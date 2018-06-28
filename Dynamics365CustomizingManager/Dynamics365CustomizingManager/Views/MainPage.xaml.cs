using System;

using Dynamics365CustomizingManager.ViewModels;
using Dynamics365CustomizingManager.Core.Data;
using Dynamics365CustomizingManager.Core.Xrm;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Dynamics365CustomizingManager.Views
{
    public sealed partial class MainPage : Page
    {
        private List<CrmConnection> crmConnections;

        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();

            this.LoadConnections();
        }

        private void LoadConnections()
        {
            this.crmConnections = StorageExtensions.Load(null);
            this.CbxRepositories.Items.Clear();

            foreach (var item in this.crmConnections)
            {
                this.CbxRepositories.Items.Add(item.Name);
            }
        }
    }
}
