using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Network
{
    public class Request
    {
        private const string basement = "https://core.ac.uk:443/api-v2/search/";

        public string ServerRequest{ get; internal set; }

        //private NameValueCollection queryInfo = HttpUtility.ParseQueryString(string.Empty);

        internal NameValueCollection QueryInfo { get; set; } = HttpUtility.ParseQueryString(string.Empty); 

        public Request()
        {
            ServerRequest = basement;
        }

        public override string ToString()
        {
            return $"{ServerRequest}{QueryInfo.ToString()}?apiKey=LY3jJXVTbtixDHlyFoSe14hKs7kNQRAm";
        }
    }
}