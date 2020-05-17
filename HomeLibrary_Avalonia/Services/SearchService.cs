using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Repositories;
using Network;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Services
{

    static class SearchService
    {
        static public CoreRepository repo = new CoreRepository();

        static public string BuildRequest(string url, string apikey,
            SearchMode mode, int page = 1, string title = null, IEnumerable<string> authors = null)
        {
            GetRequest request = new GetRequest(url, apikey, mode);
            if (title != null && title.Trim() != string.Empty) request.SetTitle(new List<string>() { title });
            if (authors != null) request.SetAuthors(new List<string>(authors));
            return request.GetAsString(page);
        }

        static public Task<(string, ResponseBody<ArticleObject>)> GetArticlesAsync(
            SearchMode mode = SearchMode.articles, int page = 1, string title = null, string authorsString = null)
        {
            List<string> authors = ConvertersService.ProvideAuthors(authorsString);
            Task<(string, ResponseBody<ArticleObject>)> requestResult;
            (string url, string key) = SettingsService.GetCoreInfo();

            if (url == null || key == null || url == string.Empty || key == string.Empty)
            {
                string result = "Configuration error: Core URL or API key is not given.";
                requestResult = new Task<(string, ResponseBody<ArticleObject>)>(() => { return (result, null); });
            }
            else
            {
                string request = BuildRequest(url, key, mode, page, title, authors);
                requestResult = repo.GetArticlesAsync(request);
            }
            return requestResult;

        }

        static public Task<(string, ResponseBody<ArticleObject>)> GetFulltextArticle(string id)
        {
            Task<(string, ResponseBody<ArticleObject>)> requestResult;
            (string url, string key) = SettingsService.GetCoreInfo();

            if (url == null || key == null)
            {
                string result = "Configuration error: Core URL or API key is not given";
                requestResult = new Task<(string, ResponseBody<ArticleObject>)>(() => (result, null));
            }
            else
            {
                GetRequest request = new GetRequest(url, key, SearchMode.articles);
                request.SetId(id);
                request.RequireFulltext();
                requestResult = repo.GetArticlesAsync(request.GetAsString());
            }
            return requestResult;
        }

        public static async Task<string> LoadPdf(string path, string link)
        {
            return await repo.LoadPDF(path, link);
        }

    }
}
