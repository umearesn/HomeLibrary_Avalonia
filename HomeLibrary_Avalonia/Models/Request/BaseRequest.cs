using System;
using System.Collections.Generic;
using System.IO;

namespace Network
{
    // Unsure if it is neccessary.
    public class BaseRequest
    {
        protected static string baseUrl;

        protected static string apiKey;
        
        protected List<string> queryArgs;
        
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
        
        protected static string ArgumentsCombiner(string argName, List<string> arguments)
        {
            string res = $"{argName}:{arguments[0]}";
            for (int i = 1; i < arguments.Count; i++)
            {
                res += $" or {argName}:{arguments[i]}";
            }

            return res;
        }
        
        public string GetAsString()
        {
            return baseUrl + RequestParts.GetPath(SearchMode.articles, queryArgs) + RequestParts.GetQuery(apiKey);
        }
        
    }
}