using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System.IO;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class LocalLibraryView : ReactiveUserControl<LocalLibraryViewModel>
    {

        private ListBox localArticlesList;

        private CheckBox isFulltextAvailable;
        // temporary
        // private Button startSearch;

        public LocalLibraryView()
        {
            AvaloniaXamlLoader.Load(this);

            // For fonts debug - doesn't work by the moment
            string[] a = typeof(LocalLibraryView).Assembly.GetManifestResourceNames();


            localArticlesList = this.Find<ListBox>("LocalArticlesList");

            isFulltextAvailable = this.Find<CheckBox>("IsFulltextAvailavble");

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(
                    ViewModel,
                    vm => vm.localArticles,
                    v => v.localArticlesList.Items)
                .DisposeWith(disposables);
            });
        }
    }
}
