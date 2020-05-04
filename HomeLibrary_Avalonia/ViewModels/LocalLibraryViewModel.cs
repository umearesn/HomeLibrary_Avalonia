using DynamicData;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class LocalLibraryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "/local_library";

        public IScreen HostScreen { get; }

        public LocalLibraryViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            var articlesPartsBinding = localArticlesSource
                .Connect()
                .Bind(out localArticles)
                .DisposeMany()
                .Subscribe();

            LoadArticlesAsync();

            Search = ReactiveCommand.Create(async () =>
            {

            });
        }

        private async void LoadArticlesAsync()
        {
            using (StreamWriter sw = new StreamWriter("AsyncLoadingDebug.txt", true))
            {
                sw.WriteLine($"{DateTime.Now} loading started!");
            }

            var response = await ElasticRepository.GetArticlesAsync(0, 10);


            foreach (var item in response)
            {
                localArticlesSource.Add(item);
            }

            using (StreamWriter sw = new StreamWriter("AsyncLoadingDebug.txt", true))
            {
                foreach (var item in localArticles)
                {
                    sw.WriteLine(item);
                    foreach (var str in item.Authors)
                    {
                        byte[] bytes = Encoding.Default.GetBytes(str);
                        string res = Encoding.ASCII
                            .GetString(bytes);
                        sw.WriteLine(res);
                    }
                    sw.WriteLine(item.Fulltext == null);
                }
                sw.WriteLine($"{DateTime.Now} loading ended!");
            }
        }

        // Results
        public ReadOnlyObservableCollection<ArticleObject> localArticles;

        public SourceList<ArticleObject> localArticlesSource
            = new SourceList<ArticleObject>();

        public ReactiveCommand<Unit, Task> Search { get; }
    }
}
