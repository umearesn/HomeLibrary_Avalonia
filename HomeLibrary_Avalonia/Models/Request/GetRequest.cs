﻿using System.Collections.Generic;

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
            return url + RequestParts.GetPath(mode, queryArgs) + $"?fulltext={isFulltext.ToString().ToLower()}&" + RequestParts.GetQuery(page) + apikey;
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

        public GetRequest SetAuthors(List<string> authors)
        {
            queryArgs.Add(ArgumentsCombiner("authors", authors));
            return this;
        }
    }
}