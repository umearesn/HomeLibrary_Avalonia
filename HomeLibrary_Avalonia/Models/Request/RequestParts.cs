using System;
using System.Collections.Generic;

namespace Network
{
    public static class RequestParts
    {
        public static string GetQuery(string apiKey, int page = 1, int pageSize = 10, bool isFulltext = false)
        {
            return $"page={page}&pageSize={pageSize}&apiKey={apiKey}";
        }

        public static string GetPath(SearchMode mode, List<string> queryArguments)
        {
            string path = "/api-v2";
            string query = null;
            if (mode != SearchMode.all)
            {
                path += $"/{mode}";
            }

            path += "/search";
            foreach (var arg in queryArguments)
            {
                if (arg != "")
                {
                    if (query != null)
                    {
                        query += $" and {arg}";
                    }
                    else
                    {
                        query = arg;
                    }
                }
            }

            return $"{path}/{Uri.EscapeDataString(query)}";
        }
    }
}