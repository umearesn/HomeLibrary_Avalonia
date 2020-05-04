using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class HostingView : ReactiveWindow<HostingViewModel>
    {

        public HostingView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated((CompositeDisposable disposable) => { });
        }
    }
}
