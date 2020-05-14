using System.Collections.Generic;
using System.Linq;

namespace HomeLibrary_Avalonia.Services
{
    public static class ConvertersService
    {
        public static List<string> ProvideAuthors(string authors)
        {
            List<string> result;
            if(authors == null || authors.Trim() == string.Empty)
            {
                result = null;
            }
            else
            {
                result = new List<string>(authors.Split(' ').Select(x => x.Trim()));
            }
            return result;
        }
    }
}
