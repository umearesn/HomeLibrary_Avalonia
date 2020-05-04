using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using HomeLibrary_Avalonia.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.ViewModels
{
    public class ArticleViewModel : ReactiveObject
    {
        private ArticleObject article;

        public ArticleObject Article { get; }

        public ReactiveCommand<Unit, Task> AddToTheLibrary;

        public ArticleViewModel(ArticleObject article)
        {
            this.article = article;

            AddToTheLibrary = ReactiveCommand.Create(async () => 
            {
                var responseMessage
                    = await SearchService.GetFulltextArticle(article.Id);
                if (responseMessage.Item1 == HttpStatusCode.OK.ToString())
                {
                    ElasticRepository.PutArticle(responseMessage.Item2.Data[0]);

                    using (StreamWriter sw = new StreamWriter("ArticleViewModel.txt"))
                    {
                        foreach (var item in responseMessage.Item2.Data)
                        {
                            sw.WriteLine(item.Title);
                        }
                    }
                }
                else
                {
                    ElasticRepository.PutArticle(this.Article);
                }
            });
        }
    }
}
