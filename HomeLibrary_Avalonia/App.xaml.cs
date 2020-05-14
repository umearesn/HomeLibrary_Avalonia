using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.Services;
using HomeLibrary_Avalonia.ViewModels;
using HomeLibrary_Avalonia.Views;
using ReactiveUI;
using Splat;
using System;
using System.IO;
using static HomeLibrary_Avalonia.Views.SearchView;

namespace HomeLibrary_Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {

            AvaloniaXamlLoader.Load(this);


        }

        public override void OnFrameworkInitializationCompleted()
        {

            Locator.CurrentMutable.RegisterConstant<IScreen>(new HostingViewModel());
           
            Locator.CurrentMutable.Register<IViewFor<SearchViewModel>>(() => new SearchView());
            Locator.CurrentMutable.Register<IViewFor<LocalLibraryViewModel>>(() => new LocalLibraryView());
            Locator.CurrentMutable.Register<IViewFor<SettingsViewModel>>(() => new SettingsView());

            new HostingView { DataContext = Locator.Current.GetService<IScreen>() }.Show();

            SettingsService.UpdateServiceConfig();

            base.OnFrameworkInitializationCompleted();

        }
    }
}