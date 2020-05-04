using Avalonia.Controls;
using HomeLibrary_Avalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Runtime.Serialization;
using System.Text;

namespace HomeLibrary_Avalonia.ViewModels
{
    [DataContract]
    public class HostingViewModel : ReactiveObject, IScreen
    {
        private RoutingState _router = new RoutingState();

        [DataMember]
        public RoutingState Router
        {
            get => _router;
            set => this.RaiseAndSetIfChanged(ref _router, value);
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoSearching { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> GoLocal { get; }

        public HostingViewModel()
        {
            Router.Navigate.Execute(new SearchViewModel());

            GoSearching = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new SearchViewModel(this)));

            GoLocal = ReactiveCommand.CreateFromObservable(
                () => Router.Navigate.Execute(new LocalLibraryViewModel(this)));
        }
    }
}