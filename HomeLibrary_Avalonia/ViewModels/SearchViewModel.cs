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
using System.Linq;

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
            //byte[] bytes = Encoding.Default.GetBytes(res);
            //res = Encoding.UTF32.GetString(bytes);
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

            var searchPartBinding = searchResultSource
                .Connect()
                .Bind(out searchResult)
                .DisposeMany()
                .Subscribe();

            // Проблемы, если мало ответов
            StartSearching = ReactiveCommand.Create(async () =>
            {
                IEnumerable<string> authorsList = AuthorsField == null ? null : AuthorsField.Split(' ').Select(x => x.Trim());

                var responseMessage
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page, TitleField, authorsList);
                
                using (StreamWriter sw = new StreamWriter("noInternet.txt"))
                {
                    sw.WriteLine(responseMessage.Item1);
                }

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    TotalHits = responseMessage.Item2.TotalHits;
                    IsStatusEnabled = true;

                    using(StreamWriter sw = new StreamWriter("encodingDebug.txt"))
                    {
                        foreach (var item in responseMessage.Item2.Data)
                        {
                            sw.WriteLine(item.Title);
                        }
                    }

                    searchResultSource.Clear();
                    //NULL REFERENCE!!!!!!
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(item);
                    }
                    IsNavigationEnabled = true;
                    if (Page == 1)
                    {
                        IsNavigationBackEnabled = false;
                    }
                    else
                    {
                        IsNavigationBackEnabled = true;
                    }
                }
            });


            GoNext = ReactiveCommand.Create(async () =>
            {
                IEnumerable<string> authorsList = AuthorsField == null ? null : AuthorsField.Split(' ').Select(x => x.Trim());

                var responseMessage
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page + 1, TitleField, authorsList);

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    TotalHits = responseMessage.Item2.TotalHits;
                    IsStatusEnabled = true;
                    searchResultSource.Clear();
                    // NULL REFERENCE
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(item);
                    }
                    Page++;
                    if (Page == 1)
                    {
                        IsNavigationBackEnabled = false;
                    }
                    else
                    {
                        IsNavigationBackEnabled = true;
                    }
                }
            });

            GoPrev = ReactiveCommand.Create(async () =>
            {
                IEnumerable<string> authorsList = AuthorsField == null ? null : AuthorsField.Split(' ').Select(x => x.Trim());

                /*using (StreamWriter sw = new StreamWriter("testing.txt", true))
                {
                    sw.WriteLine($"entered {AuthorsField}");
                    foreach (var item in authorsList)
                    {
                        sw.WriteLine(item);
                    }
                }*/


                var responseMessage
                    = await SearchService.GetArticlesAsync(SearchMode.articles, Page - 1, TitleField, authorsList);

                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    searchResultSource.Clear();
                    // NULL REFERENCE
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(item);
                    }
                    Page--;
                    if (Page == 1)
                    {
                        IsNavigationBackEnabled = false;
                    }
                    else
                    {
                        IsNavigationBackEnabled = true;
                    }
                }
            });

            /*ToTheLibrary = ReactiveCommand.Create(async () =>
            {
                searchResultSource
            });*/
        }

        // Status info
        private int? totalHits = null;
        public int? TotalHits
        {
            get => totalHits;
            set => this.RaiseAndSetIfChanged(ref totalHits, value);
        }

        // Should status be active
        private bool isStatusEnabled = false;
        public bool IsStatusEnabled
        {
            get => isStatusEnabled;
            set => this.RaiseAndSetIfChanged(ref isStatusEnabled, value);
        }

        private int page = 1;
        public int Page
        {
            get => page;
            set => this.RaiseAndSetIfChanged(ref page, value);
        }

        private bool isNavigationEnabled = false;

        public bool IsNavigationEnabled
        {
            get => isNavigationEnabled;
            set => this.RaiseAndSetIfChanged(ref isNavigationEnabled, value);
        }

        private bool isNavigationBackEnabled = false;

        public bool IsNavigationBackEnabled
        {
            get => isNavigationBackEnabled;
            set => this.RaiseAndSetIfChanged(ref isNavigationBackEnabled, value);
        }

        public CoreRepository repo = new CoreRepository();

        // Results

        public ReadOnlyObservableCollection<ArticleObject> searchResult;

        private SourceList<ArticleObject> searchResultSource
            = new SourceList<ArticleObject>();

        // Navigation
        public ReactiveCommand<Unit, Task> GoNext { get; }
        public ReactiveCommand<Unit, Task> GoPrev { get; }
        //

        public string TitleField { get; set; }

        // 

        public string AuthorsField { get; set; }

        public ReactiveCommand<Unit, Task> StartSearching { get; }

        public ReactiveCommand<Unit, Task> ToTheLibrary { get; }
    }
}