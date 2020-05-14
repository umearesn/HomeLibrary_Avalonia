using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class SettingsView : ReactiveUserControl<SettingsViewModel>
    {
        private TextBox coreURL;

        private TextBox coreApiKey;

        private TextBox elasticHost;

        private TextBox elasticPort;

        private TextBox directory;

        public SettingsView()
        {
            AvaloniaXamlLoader.Load(this);

            coreURL = this.Find<TextBox>("CoreUrl");
            coreApiKey = this.Find<TextBox>("CoreApiKey");
            elasticHost = this.Find<TextBox>("ElasticHost");
            elasticPort = this.Find<TextBox>("ElasticPort");
            directory = this.Find<TextBox>("Directory");

            this.WhenActivated((CompositeDisposable disposables) => {
                this.Bind(
                    ViewModel,
                    vm => vm.CoreUrl,
                    v => v.coreURL.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.CoreApiKey,
                    v => v.coreApiKey.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.ElasticHost,
                    v => v.elasticHost.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.ElasticPort,
                    v => v.elasticPort.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.Directory,
                    v => v.directory.Text)
                .DisposeWith(disposables);
            });
        }

       
    }
}
