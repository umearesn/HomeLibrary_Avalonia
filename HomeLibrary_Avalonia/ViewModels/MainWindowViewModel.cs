using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using HomeLibrary_Avalonia.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace HomeLibrary_Avalonia.ViewModels
{
    // was like this: 
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        public void SwitchToSearch()
        {
            var window = new SearchWindow();
            window.Show();
        }

    }
}