using HomeLibrary_Avalonia.Repositories;
using ReactiveUI;
using System.Reactive;
using System.Runtime.Serialization;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class HostingViewModel : ReactiveObject, IScreen
    {
        private RoutingState _router = new RoutingState();

        public RoutingState Router
        {
            get => _router;
            set => this.RaiseAndSetIfChanged(ref _router, value);
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoSearching { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoLocal { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoSettings { get; }

        public HostingViewModel()
        {
            Router.Navigate.Execute(new SearchViewModel());

            GoSearching = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new SearchViewModel(this)));

            GoLocal = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new LocalLibraryViewModel(this)));

            GoSettings = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new SettingsViewModel(this)));
        }

    }
}