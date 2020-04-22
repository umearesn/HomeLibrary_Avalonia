using HomeLibrary_Avalonia.Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Repositories
{
    interface RepositoryInterface
    {
        Task<HttpResponseMessage> GetAllObjects(string request);

        Task<(string, ResponseBody<ArticleObject>)> GetArticlesAsync(string request);
    }
}
