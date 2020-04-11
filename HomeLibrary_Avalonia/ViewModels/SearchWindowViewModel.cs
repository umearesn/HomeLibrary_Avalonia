using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace HomeLibrary_Avalonia.ViewModels
{
    //public IValueConverterConverter { get; set; }

    public class AuthorsAsString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> authorsList = value as List<string>;
            if (authorsList == null || authorsList.Count == 0)
                return "No data about authors.";
            
            string res = authorsList[0];
            int i = 1;
            while (i < authorsList.Count)
            {
                res += $", {authorsList[i]}";
                i++;
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
          => throw new NotImplementedException();
    }

    public class SearchVindowViewModel : ReactiveObject
    {
        public string Greeting => "Hello World!";

        public List<ArticleObject> SearchResult { get; set; } = new List<ArticleObject> {
            new ArticleObject()
            {
                Title = "some title",
                Authors = new List<string> { "some authors" }
            },

            new ArticleObject()
            {
                Title = "another title",
                Authors = new List<string> { "second author", "third author" }
            },

            new ArticleObject()
            {
                Title = "third title",
                Authors = new List<string> { "forth author", "fifth author" }
            }
        };

        public ArticleObject OnePoorArticle = new ArticleObject()
        {
            Title = "some title",
            Authors = new List<string> { "some authors" }
        };

        //public ListBox SearchResult { get; set; }
        
        public void AddArticle()
        {
            
        }

        /*
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/

        public void GoBack()
        {
            
        }

    }
}