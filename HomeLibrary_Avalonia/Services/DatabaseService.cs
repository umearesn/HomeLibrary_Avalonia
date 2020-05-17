using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Services
{
    static class DatabaseService
    {

        public static Task<List<ArticleObject>> SearchAsync(string title, string authors, string fulltext, int start, int amount = 10)
        {
            return ElasticRepository
                .SearchArticlesAsync(title, ConvertersService.ProvideAuthors(authors), fulltext, start, amount);
        }

        public static Task<bool> CheckIfPresentedAsync(ArticleObject article)
        {
            return ElasticRepository.IsAddedAsync(article);
        }

    }
}
