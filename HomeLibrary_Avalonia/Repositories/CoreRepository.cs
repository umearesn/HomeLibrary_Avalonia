using HomeLibrary_Avalonia.Models.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeLibrary_Avalonia.Repositories
{
    public class CoreRepository : RepositoryInterface
    {
        private static HttpClient client = new HttpClient();

        public Task<HttpResponseMessage> GetAllObjects(string request)
        {
            return client.GetAsync(request);
        }

        public async Task<(string, ResponseBody<ArticleObject>)> GetArticlesAsync(string request)
        {

            HttpResponseMessage response = await client.GetAsync(request);

            ResponseBody<ArticleObject> articles = new ResponseBody<ArticleObject>();
            string message = response.StatusCode.ToString();
            string stringContent = await response.Content.ReadAsStringAsync();
            if(response.StatusCode == HttpStatusCode.OK)
            {
                articles = JsonSerializer
                    .Deserialize<ResponseBody<ArticleObject>>(stringContent);
            }
            return (message, articles);
        }
    }
}
