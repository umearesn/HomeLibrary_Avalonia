using System.Collections.Generic;

namespace Network
{
    public enum SearchMode { all, articles, journals, repositories };

    public interface IRequest<out T>
    {
        T SetTitle(List<string> title);

        T SetAuthors(List<string> authors);

        T SetId(string id);

    }
}