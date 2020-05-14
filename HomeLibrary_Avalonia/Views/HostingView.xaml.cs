using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class HostingView : ReactiveWindow<HostingViewModel>
    {

        private Button goSearch;

        public Button goLocal;

        public HostingView()
        {

            goLocal = this.Find<Button>("GoLocal");

            ElasticRepository.ConnectRepository();

            AvaloniaXamlLoader.Load(this);
            this.WhenActivated((CompositeDisposable disposable) => {
                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsLocalActivated,
                    v => v.goLocal.IsEnabled)
                .DisposeWith(disposable);
            });
        }

        

    }
}
