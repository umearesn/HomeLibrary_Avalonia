using System.Collections.Generic;

namespace Network
{
    public class GetRequest : IRequest<GetRequest>
    {

        private string url;

        private string apikey;

        private List<string> queryArgs;

        private SearchMode mode;

        private bool isFulltext = false;

        public string GetAsString(int page = 1)
        {
            return url + RequestParts.GetPath(mode, queryArgs) + $"?fulltext={isFulltext}&" + RequestParts.GetQuery(page) + apikey;
        }

        private static string ArgumentsCombiner(string argName, List<string> arguments)
        {
            string res = "";
            if (arguments != null && arguments.Count > 0)
            {
                res = $"{argName}:{arguments[0]}";
                for (int i = 1; i < arguments.Count; i++)
                {
                    res += $" or {argName}:{arguments[i]}";
                }
            }
            return res;
        }

        public GetRequest(string url, string apikey, SearchMode mode)
        {
            this.url = url;
            this.apikey = apikey;
            this.mode = mode;
            this.queryArgs = new List<string>();
        }

        public GetRequest SetId(string id)
        {
            queryArgs.Add($"id:{id}");
            return this;
        }

        public GetRequest RequireFulltext()
        {
            isFulltext = true;
            return this;
        }

        public GetRequest SetTitle(List<string> title)
        {
            queryArgs.Add(ArgumentsCombiner("title", title));
            return this;
        }

        public GetRequest SetDescription(string description)
        {
            queryArgs.Add($"description:{description}");
            return this;
        }

        public GetRequest SetText(string fullText)
        {
            queryArgs.Add($"fullText:{fullText}");
            return this;
        }

        public GetRequest SetAuthors(List<string> authors)
        {
            queryArgs.Add(ArgumentsCombiner("authors", authors));
            return this;
        }

        public GetRequest SetPublisher(List<string> publisher)
        {
            queryArgs.Add(ArgumentsCombiner("publisher", publisher));
            return this;
        }

        public GetRequest SetRepoId(List<string> repoId)
        {
            queryArgs.Add(ArgumentsCombiner("repositories.id", repoId));
            return this;
        }

        public GetRequest SetRepoName(List<string> repoName)
        {
            queryArgs.Add(ArgumentsCombiner("repositories.name", repoName));
            return this;
        }

        public GetRequest SetDoi(string doi)
        {
            queryArgs.Add($"doi:{doi}");
            return this;
        }

        public GetRequest SetOai(string oai)
        {
            queryArgs.Add($"oai:{oai}");
            return this;
        }

        public GetRequest SetIdentifiers(List<string> identifiers)
        {
            queryArgs.Add(ArgumentsCombiner("identifiers", identifiers));
            return this;
        }

        public GetRequest SetLanguage(List<string> language)
        {
            queryArgs.Add(ArgumentsCombiner("language", language));
            return this;
        }

        public GetRequest SetYear(List<int> year)
        {
            List<string> syears = new List<string>();
            foreach (var elem in year)
            {
                syears.Add(elem.ToString());
            }
            queryArgs.Add(ArgumentsCombiner("year", syears));
            return this; ;
        }
    }
}