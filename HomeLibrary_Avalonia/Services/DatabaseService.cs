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
            using (StreamWriter sw = new StreamWriter("databaseServiceDebug.txt", true))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(title);
                sw.WriteLine(ConvertersService.ProvideAuthors(authors));
                sw.WriteLine(fulltext);
                sw.WriteLine();
            }
            return ElasticRepository
                .SearchArticlesAsync(title, ConvertersService.ProvideAuthors(authors), fulltext, start, amount);
        }

        public static Task<bool> CheckIfPresentedAsync(ArticleObject article)
        {
            return ElasticRepository.IsAddedAsync(article);
        }

    }
}
