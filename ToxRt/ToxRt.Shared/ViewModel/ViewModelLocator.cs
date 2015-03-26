using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using ToxRt.NavigationService;
using ToxRt.View;

namespace ToxRt.ViewModel
{
    public class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MessagesViewModel>();
            SetupUpNavigationServices();

        }        
        private static void SetupUpNavigationServices()
        {
#if WINDOWS_APP
            var innerNavigationService = CreateInnerNavigationService();
            var navigationservice = CreateGlobalWindowsNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationservice);
            SimpleIoc.Default.Register<IMessagesNavigationService>(() => innerNavigationService);
#endif
#if WINDOWS_PHONE_APP            
            var navigationservice = CreateGlobalWindowsPhoneNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationservice);            
#endif

        }
        
#if WINDOWS_APP
        private static INavigationService CreateGlobalWindowsNavigationService()
        {
            //for page navigation 
            var navigationService = new GalaSoft.MvvmLight.Views.NavigationService();
            navigationService.Configure("CreditView", typeof(CreditView));
            navigationService.Configure("GroupeChatView", typeof(GroupeChatView));
            navigationService.Configure("SettingsView", typeof(SettingsView));
            navigationService.Configure("MainPage", typeof(MainPage));

            return navigationService;
        }
        private static IMessagesNavigationService CreateInnerNavigationService()
        {
            //for messages frame navigation 
            var navigationService = new MessagesNavigationService();
            navigationService.Configure("MessagesView", typeof(MessagesView));
            return navigationService;

        }
#endif
#if WINDOWS_PHONE_APP
        private static INavigationService CreateGlobalWindowsPhoneNavigationService()
        {
        //for page navigation 
            var navigationService = new GalaSoft.MvvmLight.Views.NavigationService();
            navigationService.Configure("CreditView", typeof(CreditView));
            navigationService.Configure("GroupeChatView", typeof(GroupeChatView));
            navigationService.Configure("SettingsView", typeof(SettingsView));
            navigationService.Configure("MainPage", typeof(MainPage));
            navigationService.Configure("AllFriendsView", typeof(AllFriendsView));
            return navigationService;
        }
#endif
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MessagesViewModel MessagesViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MessagesViewModel>();
            }
        }        

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}