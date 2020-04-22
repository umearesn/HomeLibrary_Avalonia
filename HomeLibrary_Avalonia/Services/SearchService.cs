using HomeLibrary_Avalonia.Repositories;
using Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLibrary_Avalonia.Services
{

    class SearchService
    {
        static private CoreRepository repo = new CoreRepository();

        static public string BuildRequest(SearchMode mode, string title = null, IEnumerable<string> authors = null)
        {
            GetRequest request = new GetRequest(mode);
            if (title != null) request.SetTitle(new List<string>() { title });
            if (authors != null) request.SetAuthors(new List<string>(authors));
            return request.ToString();
        }
    }
}
