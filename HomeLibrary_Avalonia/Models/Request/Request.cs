using System.Collections.Generic;
using Newtonsoft.Json;

namespace Network
{
    public class Request
    {
        public Dictionary<string, List<string>> Arguments { get; private set; }
            = new Dictionary<string, List<string>>()
            {
                ["title"] = null,
                ["description"] = null,
                ["fullText"] = null,
                ["language.name"] = null,
            };

        public Request(string[] title, string[] language)
        {
            Arguments["title"] = new List<string>(title);
            Arguments["language.name"] = new List<string>(language);
        }

        public string ToJsonString()
        {
            List<string> query = new List<string>();
            foreach (var arg in Arguments)
            {
                if (arg.Value != null)
                {
                    string orString = null;
                    foreach (var value in arg.Value)
                    {
                        if (orString == null)
                        {
                            orString = $"{arg.Key}:{value}";
                        }
                        else
                        {
                            orString += $" or {arg.Key}:{value}";
                        }
                    }
                    
                    query.Add(orString);
                }
            }

            string totalQuery = null;
            foreach (var orTerm in query)
            {
                if (totalQuery == null)
                {
                    totalQuery = orTerm;
                }
                else
                {
                    totalQuery += $" and {orTerm}";
                }
            }

            return "[" +
                   "{" +
                   $"\"query\": \"{totalQuery}\"" +
                   "}" +
                   "]";
        }
    }
}