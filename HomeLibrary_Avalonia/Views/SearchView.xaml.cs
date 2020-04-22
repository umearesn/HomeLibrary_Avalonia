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

        private TextBox titleQuery;

        // Authors adding block
        private ListBox authorsQuery;
        private TextBox newAuthor;
        private Button addAuthorButton;


        private ComboBox modeSelect;
        
        private Button startSearch;
        private ListBox searchResultList;

        public SearchView()
        {
            AvaloniaXamlLoader.Load(this);

            titleQuery = this.Find<TextBox>("TitleQuery");

            authorsQuery = this.Find<ListBox>("AuthorsQuery");
            addAuthorButton = this.Find<Button>("AddAuthor");
            newAuthor = this.Find<TextBox>("NewAuthor");
            


            modeSelect = this.Find<ComboBox>("ModeSelect");

            startSearch = this.Find<Button>("StartSearch");

            searchResultList = this.Find<ListBox>("SearchResultList");


            this.WhenActivated(disposables =>
            {
                /*this.Bind(
                    ViewModel,
                    vm => vm.Authors,
                    v => v.authorsQuery,
                    this.ToAuthorsQuery,
                    this.FromAuthorsQuery)
                .DisposeWith(disposables);*/

                /*this.Bind(ViewModel,
                    vm => vm.QueryAuthors,
                    v => v.authorsQuery.Text,
                    vmToViewConverterOverride: new QueryAuthorsBindingTypeConverter());*/

                //this.WhenAnyValue(v => v.addAuthorButton.Click)

                /// These has to do the same thing

                /*this.WhenAnyValue(x => x.ViewModel.ListQueryAuthors)
                    .BindTo(this, view => view.authorsQuery.Text);*/

                this.Bind(
                    ViewModel,
                    vm => vm.TitleField,
                    v => v.titleQuery.Text)
                .DisposeWith(disposables);

                this.Bind(
                    ViewModel,
                    vm => vm.AuthorField,
                    v => v.newAuthor.Text)
                .DisposeWith(disposables);



                this.WhenAnyValue(x => x.newAuthor.Text)
                    .BindTo(this, x => x.ViewModel.AuthorField);

                this.WhenAnyValue(x => x.ViewModel.AuthorField)
                    .BindTo(this, x => x.newAuthor.Text);
                    

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

                this.OneWayBind(
                    ViewModel,
                    vm => vm.authorsQuery,
                    v => v.authorsQuery.Items/*,
                    value => ToAuthorsQuery(value)*/)
                .DisposeWith(disposables);

                /*using(StreamWriter sw = new StreamWriter("ViewModelDebug.txt", true))
                {
                    sw.WriteLine("Opened file!");
                    foreach (var item in ViewModel.authorsQuery)
                    {
                        sw.WriteLine(item);
                    }
                    sw.WriteLine();
                }*/
                

                ///

                this.BindCommand(
                    ViewModel,
                    vm => vm.AddAuthor,
                    v => v.addAuthorButton)
                .DisposeWith(disposables);
            });

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
