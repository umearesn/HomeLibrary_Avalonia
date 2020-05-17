using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Services;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Repositories
{
    public static class ElasticRepository
    {
        private static ElasticClient elasticClient;

        private static string elasticHost;

        private static string elasticPort;

        public static string ConnectRepository()
        {
            string result;
            (string host, string port) = SettingsService.GetElasticInfo();

            if (host == null || port == null)
            {
                result = "Configuration error: Elaticsearch host or port is not given";
            }
            else
            {
                try
                {
                    result = "OK";
                    elasticHost = host;
                    elasticPort = port;
                    ConnectionSettings connectionSettings = new ConnectionSettings(new Uri($"{elasticHost}:{elasticPort}"))
                        .DefaultMappingFor<ArticleObject>(i => i
                            .IndexName("articles")
                            .IdProperty(p => p.Id)
                        )
                        .EnableDebugMode()
                        .PrettyJson()
                        .DefaultIndex("articles")
                        .RequestTimeout(TimeSpan.FromSeconds(10));
                    elasticClient = new ElasticClient(connectionSettings);
                }
                catch (Exception ex)
                {
                    result = $"ConfigurationError: {ex.Message}.";
                }

            }
            return result;
        }

        public static IndexResponse PutArticle(ArticleObject article)
        {
            var res = elasticClient.Index(article, i => i.Index("articles"));
            return res;
        }

        public static async Task<IndexResponse> PutArticleAsync(ArticleObject article)
        {
            return await Task.Run(() => PutArticle(article));
        }

        public static bool IsAdded(ArticleObject article)
        {
            var result = elasticClient.Search<ArticleObject>(
                s => s.Index("articles")
                .Query(q => q.
                    Match(m => m
                    .Field(f => f.Id)
                    .Query(article.Id))));
            return result.Documents.Count > 0;
        }

        public static Task<bool> IsAddedAsync(ArticleObject article)
        {
            return Task.Run(() => IsAdded(article));
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
            return new List<ArticleObject>(searchResults.Documents);
        }


        public static async Task<List<ArticleObject>> SearchArticlesAsync(string title, List<string> authors, string fulltext, int from = 0, int amount = 10)
        {
            return await Task.Run(() => SearchArticles(title, authors, fulltext, from, amount));
        }

        public static List<ArticleObject> SearchArticles(string title, List<string> authors, string fulltext, int from = 0, int amount = 10)
        {
            QueryContainer combination = null;

            QueryContainer titleQuery = null;
            if (title != null && title.Trim() != string.Empty)
            {
                titleQuery = new QueryContainerDescriptor<ArticleObject>()
                    .Match(m => m
                    .Field("title")
                    .Query(title));
                combination = titleQuery;
            }

            QueryContainer authorsQuery = null;
            if (authors != null && authors.Count > 0)
            {
                var authorsDescriptor = new QueryContainerDescriptor<ArticleObject>();
                authorsQuery = authorsDescriptor
                        .Match(m => m
                            .Field("authors")
                            .Query(authors[0])
                        );
                for (int i = 1; i < authors.Count; i++)
                {
                    authorsQuery = authorsQuery || authorsDescriptor
                        .Match(m => m
                            .Field("authors")
                            .Query(authors[i])
                        );
                }
                if (combination != null)
                {
                    combination = combination && authorsQuery;
                }
                else
                {
                    combination = authorsQuery;
                }
            }

            QueryContainer fulltextQuery = null;
            if (fulltext != null && fulltext.Trim().Length > 0)
            {
                fulltextQuery = new QueryContainerDescriptor<ArticleObject>()
                    .Match(m => m
                    .Field("fulltext")
                    .Query(fulltext));
                if (combination != null)
                {
                    combination = combination && fulltextQuery;
                }
                else
                {
                    combination = fulltextQuery;
                }
            }

            ISearchResponse<ArticleObject> searchResults;

            if (combination != null)
            {
                searchResults = elasticClient.Search<ArticleObject>(s => s
                    .Index("articles")
                    .From(from)
                    .Size(amount)
                    .Query(q => combination)
                );
            }
            else
            {
                searchResults = elasticClient.Search<ArticleObject>(s => s
                    .Index("articles")
                    .From(from)
                    .Size(amount)
                    .Query(q => q.MatchAll()));
            }
            return new List<ArticleObject>(searchResults.Documents);
        }

        private static void DeleteArticle(string id)
        {
            elasticClient.DeleteByQuery<ArticleObject>(q => q.
                Index("articles")
                .Query(rq => rq
                    .Match(m => m
                        .Field("id")
                        .Query(id)
                    )
                )
            );
        }

        public static void DeleteArticleId(string id)
        {
            DeleteArticle(id);
        }

    }
}
