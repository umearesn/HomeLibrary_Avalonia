using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class LocalLibraryView : ReactiveUserControl<LocalLibraryViewModel>
    {
        private TextBlock searchStatus;

        private ListBox localArticlesList;

        private Button startSearch;

        private StackPanel navigationPanel;
        private Button prevPage;
        private TextBlock curPage;
        private Button nextPage;

        private TextBox titleField;
        private TextBox authorsField;
        private TextBox fulltextField;

        public LocalLibraryView()
        {
            AvaloniaXamlLoader.Load(this);

            startSearch = this.Find<Button>("StartSearch");

            titleField = this.Find<TextBox>("TitleField");
            authorsField = this.Find<TextBox>("AuthorsField");
            fulltextField = this.Find<TextBox>("FulltextField");

            navigationPanel = this.Find<StackPanel>("NavigationPanel");
            prevPage = this.Find<Button>("PrevPage");
            curPage = this.Find<TextBlock>("CurPage");
            nextPage = this.Find<Button>("NextPage");

            searchStatus = this.Find<TextBlock>("SearchStatus");

            localArticlesList = this.Find<ListBox>("LocalArticlesList");

            this.WhenActivated(disposables =>
            {

                this.Bind(
                    ViewModel,
                    vm => vm.TitleField,
                    v => v.titleField.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.AuthorsField,
                    v => v.authorsField.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.FulltextField,
                    v => v.fulltextField.Text)
                .DisposeWith(disposables);

                this.BindCommand(
                    ViewModel,
                    vm => vm.Search,
                    v => v.startSearch)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.localArticles,
                    v => v.localArticlesList.Items)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsStatusEnabled,
                    v => v.searchStatus.IsVisible)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.Status,
                    v => v.searchStatus.Text,
                    value => value)
                .DisposeWith(disposables);

                // Navigation commands
                this.BindCommand(
                    ViewModel,
                    vm => vm.GoNext,
                    v => v.nextPage)
                .DisposeWith(disposables);

                this.BindCommand(
                    ViewModel,
                    vm => vm.GoPrev,
                    v => v.prevPage)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.CurrentPage,
                    v => v.curPage.Text,
                    value => value.ToString())
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationVisible,
                    v => v.navigationPanel.IsVisible)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationVisible,
                    v => v.navigationPanel.IsEnabled)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationVisible,
                    v => v.prevPage.IsVisible)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationForwardEnabled,
                    v => v.nextPage.IsEnabled)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationBackEnabled,
                    v => v.prevPage.IsEnabled)
                .DisposeWith(disposables);
            });
        }
    }
}
