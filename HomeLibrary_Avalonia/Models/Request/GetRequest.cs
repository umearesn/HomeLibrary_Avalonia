using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX.Direct3D11;

namespace Network
{
    public class GetRequest : IRequest<GetRequest>
    {

        private static string baseUrl;

        private static string apiKey;
        
        public static void InitRequest()
        {
            using (StreamReader sr = new StreamReader("../../../Assets/info.keys"))
            {
                string str = "";
                Dictionary<string, string> values = new Dictionary<string, string>();
                while ((str = sr.ReadLine()) != null)
                {
                    Console.WriteLine(str);
                    string[] vals = str.Split(' ');
                    values.Add(vals[0], vals[1]);
                }
                baseUrl = values["baseUrl"];
                apiKey = values["apiKey"];
            };
        }

        private List<string> queryArgs;

        private SearchMode mode;
        
        
        public override string ToString()
        {
            return baseUrl + RequestParts.GetPath(mode, queryArgs) + RequestParts.GetQuery(apiKey);
        }

        private static string ArgumentsCombiner(string argName, List<string> arguments)
        {
            string res = $"{argName}:{arguments[0]}";
            for (int i = 1; i < arguments.Count; i++)
            {
                res += $" or {argName}:{arguments[i]}";
            }

            return res;
        }
        
        public GetRequest(SearchMode mode)
        {
            this.mode = mode;
            this.queryArgs = new List<string>();
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
            queryArgs.Add(ArgumentsCombiner("authors",authors));
            return this;
        }

        public GetRequest SetPublisher(List<string> publisher)
        {
            queryArgs.Add(ArgumentsCombiner("publisher" , publisher));
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
            queryArgs.Add(ArgumentsCombiner("language.name", language));
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
            return this;;
        }
    }
}