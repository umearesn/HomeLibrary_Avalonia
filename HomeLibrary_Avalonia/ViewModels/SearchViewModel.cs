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

                var responseMessage = await repo.GetArticlesAsync(
                    BuildRequest(SearchMode.articles, TitleField, authorsQuerySource.Items));
                
                //ResponseBody<ArticleObject> resp = JsonSerializer.Deserialize<ResponseBody<ArticleObject>>(responseMessage);
                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    searchResultSource.Clear();
                    //SearchResultList = responseMessage.Item2.Data;
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        searchResultSource.Add(item);
                    }
                    //searchResultSource = new SourceList<ArticleObject>(responseMessage.Item2.Data);
                }

                using (StreamWriter sw = new StreamWriter("response.txt", true))
                {
                    sw.WriteLine(responseMessage.Item1);
                    sw.WriteLine(responseMessage.Item1 == HttpStatusCode.OK.ToString());
                    sw.WriteLine("what is here?");
                    foreach (var item in responseMessage.Item2.Data)
                    {
                        sw.WriteLine(item.Title);
                    }
                }

                /*using (StreamWriter sw = new StreamWriter("response.txt", true))
                {
                    sw.WriteLine("are we reaching that?");
                    foreach (var item in SearchResultList)
                    {
                        sw.WriteLine(item.Title);
                    }
                }*/
            });

            AddAuthor = ReactiveCommand.Create(() =>
            {
             
                if (AuthorField.Length > 0) { 
                    authorsQuerySource.Add(AuthorField);
                    AuthorField = "";
                }



                //ListQueryAuthors = new List<string>() { "text"};
                using (StreamWriter sw = new StreamWriter("add_author_1.txt", true))
                {
                    sw.WriteLine("Checking authorsQuery!");
                    foreach(var item in authorsQuery)
                    {
                        sw.WriteLine(item);
                    }
                    /*foreach (var item in authorsQuerySource)
                    {
                        sw.WriteLine(item);
                    }*/
                    sw.WriteLine();
                }
            });
        }

        public List<ArticleObject> SearchResultList { get; set; } = new List<ArticleObject> {
            new ArticleObject()
            {
                Title = "some title",
                Authors = new List<string> { "some authors" }
            },

            new ArticleObject()
            {
                Title = "another title",
                Authors = new List<string> { "second author", "third author" }
            },

            new ArticleObject()
            {
                Title = "third title",
                Authors = new List<string> { "forth author", "fifth author" }
            }
        };

        public CoreRepository repo = new CoreRepository();

        // Results

        public ReadOnlyObservableCollection<ArticleObject> searchResult;

        private SourceList<ArticleObject> searchResultSource
            = new SourceList<ArticleObject>();


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

        public string BuildRequest(SearchMode mode, string title = null, IEnumerable<string> authors = null)
        {
            GetRequest request = new GetRequest(mode);
            if (title != null) request.SetTitle(new List<string>() { title });
            if (authors != null) request.SetAuthors(new List<string>(authors));
            return request.ToString();
        }
    }
}