using System.Collections.Generic;
using HomeLibrary_Avalonia.Models.Response;

namespace Network.ArticlesRequest
{
    // Unsure if it is neccessary.
    public class ArticleRequest : BaseRequest
    {
        public ArticleRequest SetTitle(string title)
        {
            queryArgs.Add($"title:{title}");
            return this;
        }
        
        public ArticleRequest SetAuthors(List<string> authors)
        {
            queryArgs.Add(ArgumentsCombiner("authors",authors));
            return this;
        }
        
        //journals (array[ArticleJournal], optional): List of journals this article belongs to,
        public ArticleRequest SetJournals(List<string> journals)
        {
            queryArgs.Add(ArgumentsCombiner("journals", journals));
            return this;
        }
        
        
    }
}