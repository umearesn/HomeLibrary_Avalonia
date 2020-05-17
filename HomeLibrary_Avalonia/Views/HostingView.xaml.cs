using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.ViewModels;

namespace HomeLibrary_Avalonia.Views
{
    public class HostingView : ReactiveWindow<HostingViewModel>
    {
        public HostingView()
        {
            ElasticRepository.ConnectRepository();

            AvaloniaXamlLoader.Load(this);
        }
    }
}
