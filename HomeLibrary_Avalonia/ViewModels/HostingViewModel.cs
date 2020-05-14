using Avalonia.Controls;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.Views;
using ReactiveUI;
using System.Reactive;
using System.Runtime.Serialization;

namespace HomeLibrary_Avalonia.ViewModels
{
    [DataContract]
    public class HostingViewModel : ReactiveObject, IScreen
    {
        public enum ButtonNames { GoSearching, GoLocal, GoSettings };

        private RoutingState _router = new RoutingState();
        
        [DataMember]
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

            IsLocalActivated =
                ElasticRepository.ConnectRepository() == "OK";

            GoSearching = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new SearchViewModel(this)));

            GoLocal = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new LocalLibraryViewModel(this)));

            GoSettings = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new SettingsViewModel(this)));
        }

        private bool isLocalActivated;
        public bool IsLocalActivated
        {
            get => isLocalActivated;
            set => this.RaiseAndSetIfChanged(ref isLocalActivated, value);
        }
    }
}