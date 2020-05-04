using HomeLibrary_Avalonia.Models.Response;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Repositories
{
    public static class ElasticRepository
    {
        private static ElasticClient elasticClient;

        /*public static void InitElasticRepository()
        {
            ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"))
                .DefaultMappingFor<ArticleObject>(i => i
                    .IndexName("articles")
                    .IdProperty(p => p.Id)
                )
                .EnableDebugMode()
                .PrettyJson()
                .DefaultIndex("articles")
                .RequestTimeout(TimeSpan.FromMinutes(2));
            elasticClient = new ElasticClient(connectionSettings);
            using (StreamWriter sw = new StreamWriter("ElasticRepo.log", true))
            {
                sw.WriteLine($"Connected to http://127.0.0.1:9200 [HighLevel] at {DateTime.Now}.");
            }
        }*/

        static ElasticRepository()
        {
            ConnectionSettings connectionSettings = new ConnectionSettings(new Uri("http://127.0.0.1:9200"))
                .DefaultMappingFor<ArticleObject>(i => i
                    .IndexName("articles")
                    .IdProperty(p => p.Id)
                )
                .EnableDebugMode()
                .PrettyJson()
                .DefaultIndex("articles")
                .RequestTimeout(TimeSpan.FromMinutes(2));
            elasticClient = new ElasticClient(connectionSettings);
            using (StreamWriter sw = new StreamWriter("ElasticRepo.log", true))
            {
                sw.WriteLine($"Connected to http://127.0.0.1:9200 [HighLevel] at {DateTime.Now}.");
            }
        }

        public static IndexResponse PutArticle(ArticleObject article)
        {
             return elasticClient.Index(article, i => i.Index("articles"));
        }

        public static async Task<IndexResponse> PutArticleAsync(ArticleObject article)
        {
            return await Task.Run(() => PutArticle(article));
        }

        public static bool IsAdded(ArticleObject article)
        {
            var result =  elasticClient.Search<ArticleObject>(
                s => s.Index("articles")
                .Query(q => q.
                    Match(m => m
                    .Field(f => f.Id)
                    .Query(article.Id))));
            return result.Documents.Count > 0;
        }

        public static async Task<List<ArticleObject>> GetArticlesAsync(int from = 0, int amount = 10)
        {
            return await Task.Run(() => GetArticles(from, amount));
        }

        public static List<ArticleObject> GetArticles(int from = 0, int amount = 10)
        {
            var searchResults = elasticClient.Search<ArticleObject>(s => s
                .Index("articles")
                .From(from)
                .Size(amount)
                .Query(q => q.MatchAll()));
            Thread.Sleep(5000);

            using (StreamWriter wr = new StreamWriter("GetArticlesDebug.txt", true))
            {
                wr.WriteLine(DateTime.Now);
                foreach (var item in searchResults.Documents)
                {
                    wr.WriteLine(item);
                }
            }
            return new List<ArticleObject>(searchResults.Documents);
        }

    }
}
