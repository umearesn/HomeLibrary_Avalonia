using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.ViewModels;

namespace HomeLibrary_Avalonia.Views
{
    public class SearchWindow : Window
    {
        public SearchWindow()
        {
            DataContext = new SearchVindowViewModel();
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
