using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HomeLibrary_Avalonia.Views
{
    public class SearchWindow : Window
    {
        public SearchWindow()
        {
            //DataContext = new SearchWindow();
            this.InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
