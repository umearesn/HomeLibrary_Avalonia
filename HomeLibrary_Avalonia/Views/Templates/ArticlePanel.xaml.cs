using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.Models.Response;

namespace HomeLibrary_Avalonia.Views
{
    public class ArticlePanel : UserControl
    {
        private ArticleObject article;

        public ArticlePanel()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
