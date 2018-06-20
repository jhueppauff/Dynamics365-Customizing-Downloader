using System;

using CommonServiceLocator;

using Dynamics365CustomizingManager.Services;
using Dynamics365CustomizingManager.Views;

using GalaSoft.MvvmLight.Ioc;

namespace Dynamics365CustomizingManager.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<DownloaderViewModel, DownloaderPage>();
            Register<ExtractorViewModel, ExtractorPage>();
            Register<PackagerViewModel, PackagerPage>();
            Register<UploaderViewModel, UploaderPage>();
            Register<SettingsViewModel, SettingsPage>();
        }

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public UploaderViewModel UploaderViewModel => ServiceLocator.Current.GetInstance<UploaderViewModel>();

        public PackagerViewModel PackagerViewModel => ServiceLocator.Current.GetInstance<PackagerViewModel>();

        public ExtractorViewModel ExtractorViewModel => ServiceLocator.Current.GetInstance<ExtractorViewModel>();

        public DownloaderViewModel DownloaderViewModel => ServiceLocator.Current.GetInstance<DownloaderViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
