using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.ViewModels;
using HomeLibrary_Avalonia.Views;
using ReactiveUI;
using Splat;
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
            //Locator.CurrentMutable.Register<IViewFor<NavigationViewModel>> (() => new NavigationView());
            Locator.CurrentMutable.Register<IViewFor<SearchViewModel>>(() => new SearchView());

            //Locator.CurrentMutable.RegisterConstant(new QueryAuthorsBindingTypeConverter(), typeof(IBindingTypeConverter)
//);

            new HostingView { DataContext = Locator.Current.GetService<IScreen>() }.Show();
            //new HostingView { DataContext = Locator.Current.GetService<IViewFor<NavigationViewModel>>() }.Show();

            base.OnFrameworkInitializationCompleted();
        }
    }
}