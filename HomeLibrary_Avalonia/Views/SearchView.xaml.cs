using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;

namespace HomeLibrary_Avalonia.Views
{
    public class SearchView : ReactiveUserControl<SearchViewModel>
    {
        private TextBlock searchStatus;

        private StackPanel navigationPanel;
        private Button prevPage;

        private TextBox titleQuery;

        // Authors adding block
        private TextBox authorsQuery;

        private TextBlock curPage;
        private Button nextPage;

        private Button addToLibrary;

        private ComboBox modeSelect;
        
        private Button startSearch;
        private ListBox searchResultList;
        private Button selectedItem;

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

            modeSelect = this.Find<ComboBox>("ModeSelect");

            addToLibrary = this.Find<Button>("AddToLibrary");

            startSearch = this.Find<Button>("StartSearch");

            searchResultList = this.Find<ListBox>("SearchResultList");
            //selectedItem = this.Find<Button>("AddToLibrary");

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
                    vm => vm.TotalHits,
                    v => v.searchStatus.Text,
                    value => StatusForming(value))
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

        private string StatusForming(int? value)
        {
            if(value == null)
            {
                return "Request failed - unable to access Core Repository.";
            }
            else
            {
                return $"TotalHints: {value}";
            }
        }

        public string ToAuthorsQuery(IEnumerable<string> authorsQuery)
        {
            string res = "Entered authors: ";
            if (authorsQuery != null)
            {
                foreach (var item in authorsQuery)
                {
                    res += $"{item},";
                }
            }
            using (StreamWriter sw = new StreamWriter("log1.txt", true))
            {
                sw.WriteLine(res.Substring(0, res.Length - 1));
            }
            return res.Substring(0, res.Length - 1);
        }

        public List<string> FromAuthorsQuery(string stringedAuthors)
        {
            var res = new List<string>(stringedAuthors.Split(':')[1].Trim().Split(','));
            res.ForEach(x => x.Trim());
            return res;
        }
    }
}
