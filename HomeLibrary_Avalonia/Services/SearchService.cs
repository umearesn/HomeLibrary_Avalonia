using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Services
{

    class SearchService
    {
        static public CoreRepository repo = new CoreRepository();

        /*static public string BuildRequest(SearchMode mode, string title = null, IEnumerable<string> authors = null)
        {
            GetRequest request = new GetRequest(mode);
            if (title != null) request.SetTitle(new List<string>() { title });
            if (authors != null) request.SetAuthors(new List<string>(authors));
            return request.GetAsString();
        }*/

        static public string BuildRequest(SearchMode mode, int page = 1, string title = null, IEnumerable<string> authors = null)
        {
            GetRequest request = new GetRequest(mode);
            if (title != null) request.SetTitle(new List<string>() { title });
            if (authors != null) request.SetAuthors(new List<string>(authors));
            return request.GetAsString(page);
        }

        static public Task<(string, ResponseBody<ArticleObject>)> GetArticlesAsync(
            SearchMode mode = SearchMode.articles, int page = 1, string title = null, IEnumerable<string> authors = null)
        {
            using(StreamWriter sw = new StreamWriter("SearchService.txt", true))
            {
                sw.WriteLine("Entered!");
                sw.WriteLine($"Current page: {page}");
            }
            return repo.GetArticlesAsync(BuildRequest(mode, page, title, authors));
        }

    }
}
