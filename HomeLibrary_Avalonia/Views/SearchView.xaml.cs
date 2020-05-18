using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class SearchView : ReactiveUserControl<SearchViewModel>
    {
        private TextBlock searchStatus;

        private ListBox searchResultList;

        private Button startSearch;

        private StackPanel navigationPanel;
        private Button prevPage;
        private TextBlock curPage;
        private Button nextPage;

        private TextBox titleQuery;
        private TextBox authorsQuery;

        public SearchView()
        {
            AvaloniaXamlLoader.Load(this);

            searchStatus = this.Find<TextBlock>("SearchStatus");

            titleQuery = this.Find<TextBox>("TitleQuery");

            authorsQuery = this.Find<TextBox>("AuthorsQuery");

            // Navigation
            navigationPanel = this.Find<StackPanel>("NavigationPanel");
            prevPage = this.Find<Button>("PrevPage");
            curPage = this.Find<TextBlock>("CurPage");
            nextPage = this.Find<Button>("NextPage");

            startSearch = this.Find<Button>("StartSearch");

            searchResultList = this.Find<ListBox>("SearchResultList");

            this.WhenActivated(disposables =>
            {
                //Status

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

                this.Bind(
                    ViewModel,
                    vm => vm.TitleField,
                    v => v.titleQuery.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.AuthorsField,
                    v => v.authorsQuery.Text)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.searchResult,
                    v => v.searchResultList.Items)
                .DisposeWith(disposables);

                this.BindCommand(
                    ViewModel,
                    vm => vm.StartSearching,
                    v => v.startSearch)
                .DisposeWith(disposables);

                // Navigation view
                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationEnabled,
                    v => v.navigationPanel.IsVisible)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationForwardEnabled,
                    v => v.nextPage.IsEnabled)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.Page,
                    v => v.curPage.Text,
                    value => value.ToString())
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationEnabled,
                    v => v.navigationPanel.IsEnabled)
                .DisposeWith(disposables);

                // Navigation back view
                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationEnabled,
                    v => v.prevPage.IsVisible)
                .DisposeWith(disposables);

                this.OneWayBind(
                    ViewModel,
                    vm => vm.IsNavigationBackEnabled,
                    v => v.prevPage.IsEnabled)
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

            });

        }
    }
}
