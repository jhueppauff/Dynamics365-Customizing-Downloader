using System;
using System.Collections.ObjectModel;

using Dynamics365CustomizingManager.Models;
using Dynamics365CustomizingManager.Services;

using GalaSoft.MvvmLight;

namespace Dynamics365CustomizingManager.ViewModels
{
    public class ExtractorViewModel : ViewModelBase
    {
        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
