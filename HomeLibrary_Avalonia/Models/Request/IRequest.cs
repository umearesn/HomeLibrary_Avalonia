using System.Collections.Generic;

namespace Network
{
    public enum SearchMode { all, articles, journals, repositories };

    public interface IRequest<out T>
    {
        T SetTitle(List<string> title);

        T SetDescription(string description);

        T SetText(string fullText);

        T SetAuthors(List<string> authors);

        T SetPublisher(List<string> publisher);

        T SetRepoId(List<string> repoId);

        T SetRepoName(List<string> repoName);

        T SetDoi(string doi);

        T SetOai(string oai);

        T SetIdentifiers(List<string> identifiers);

        T SetLanguage(List<string> language);

        T SetYear(List<int> year);
    }
}