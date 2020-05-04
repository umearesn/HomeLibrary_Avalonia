using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using HomeLibrary_Avalonia.Models.Response;
using HomeLibrary_Avalonia.ViewModels;

namespace HomeLibrary_Avalonia.Views.Templates
{
    public class ArticlePanel : ReactiveUserControl<ArticleViewModel>
    {

        private Button addToLibrary;

        public ArticlePanel()
        {
            AvaloniaXamlLoader.Load(this);

            addToLibrary = this.Find<Button>("AddToLibrary");
        }
    }
}
