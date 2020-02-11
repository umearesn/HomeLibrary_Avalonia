﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Network
{
    /// <summary>
    /// Going to be refactored. Functionality is going to be implementd
    /// in Request class itself.
    /// </summary>
    public static class Requests_Builder
    {

        public enum SearchMode { all, articles, journals, repositories};

        private const string basement = "https://core.ac.uk:443/api-v2/search/";

        //private static UriBuilder builder = new UriBuilder();

        // В конец перед apiKey нужно воткнуть вопрос, иначе не авторизируется
        // Пока так закостылил
        /*public static string GetRequest()
        {
            var queryInfo = HttpUtility.ParseQueryString(string.Empty);
            queryInfo["title"] = "psychology";
            queryInfo["language.name"] =  "English";
            //queryInfo["apiKey"] = apiKey;
            return queryInfo.ToString() + $"?apiKey={ApiKeyManipulations.GetApiKey()}";
        }*/

        public static string PostRequest(Dictionary<string, string> query)
        {
            query.Add("apiKey", "LY3jJXVTbtixDHlyFoSe14hKs7kNQRAm");
            return basement + new System.Net.Http.FormUrlEncodedContent(query).ToString();
        }

        public static Request GetRequest()
        {
            return new Request();
        }

        public static Request SetMode(this Request req, SearchMode mode = SearchMode.all)
        {
            if (mode != SearchMode.all)
            {
                req.ServerRequest += $"{mode.ToString()}/";
            }
            return req;
        }

        public static Request SetTitle(this Request req, string title)
        {
            req.QueryInfo["title"] = title;
            return req;
        }

        public static void SetDescription()
        {
           
        }

        public static void SetFullText()
        {

        }

        public static void SetAuthors()
        {

        }

        public static void SetPublisher()
        {

        }

        public static void SetRepositoryID()
        {

        }

        public static void SetRepositoryName() {

        }

        public static void SetYear()
        {
            
        }

        public static Request SetLanguageName(this Request req, string language)
        {
            req.QueryInfo["language.name"] = language;
            return req;
        }

        // Надо подумать, как это использовать
        public static void SetDoi()
        {
            throw new NotImplementedException();
        }

        public static void SetOai()
        {
            throw new NotImplementedException();
        }

        public static void SetIdentifier()
        {
            throw new NotImplementedException();
        }
    }
}