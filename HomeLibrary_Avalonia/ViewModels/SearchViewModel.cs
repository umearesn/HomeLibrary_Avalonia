using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Avalonia.ReactiveUI;
using System.Runtime.Serialization;
using Splat;
using System.Reactive;
using System.IO;
using DynamicData;
using Network;
using HomeLibrary_Avalonia.Repositories;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using DynamicData.Binding;
using System.Collections.ObjectModel;
using HomeLibrary_Avalonia.Services;

namespace HomeLibrary_Avalonia.ViewModels
{

    public class AuthorsAsString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> authorsList = value as List<string>;
            if (authorsList == null || authorsList.Count == 0)
                return "No data about authors.";
            
            string res = authorsList[0];
            int i = 1;
            while (i < authorsList.Count)
            {
                res += $", {authorsList[i]}";
                i++;
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
          => throw new NotImplementedException();
    }

    [DataContract]
    public class SearchViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }

        public string UrlPathSegment => "/search";



        public SearchViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            var cancellation = authorsQuerySource
                .Connect()
                .Bind(out authorsQuery)
                .DisposeMany()
                .Subscribe();

            var searchPartBinding = searchResultSource
                .Connect()
                .Bind(out searchResult)
                .DisposeMany()
                .Subscribe();

            StartSearching = ReactiveCommand.Create(async () =>
            {
                var responseMessage 
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page, TitleField, authorsQuerySource.Items);

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    searchResultSource.Clear();
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(item);
                    }
                }
            });

            AddAuthor = ReactiveCommand.Create(() =>
            {
             
                if (AuthorField.Length > 0) { 
                    authorsQuerySource.Add(AuthorField);
                    AuthorField = "";
                }
            });

            GoNext = ReactiveCommand.Create(async () =>
            {
                Page++;

                var responseMessage 
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page, TitleField, authorsQuerySource.Items);

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    searchResultSource.Clear();
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(item);
                    }
                }
            });
        }

        public int Page { get; private set; } = 1;

        public CoreRepository repo = new CoreRepository();

        // Results

        public ReadOnlyObservableCollection<ArticleObject> searchResult;

        private SourceList<ArticleObject> searchResultSource
            = new SourceList<ArticleObject>();

        // Navigation
        public ReactiveCommand<Unit, Task> GoNext { get; }

        //

        public string TitleField { get; set; }

        // 

        public ReadOnlyObservableCollection<string> authorsQuery;

        private SourceList<string> authorsQuerySource
            = new SourceList<string>();

        public string AuthorField { get; set; }

        //

        public ReactiveCommand<Unit, Unit> AddAuthor { get; }

        public ReactiveCommand<Unit, Task> StartSearching { get; }

    }
}