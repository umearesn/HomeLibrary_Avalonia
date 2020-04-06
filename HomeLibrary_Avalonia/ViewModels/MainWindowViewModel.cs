using Avalonia.Controls;
using Avalonia.Interactivity;
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
    //public class MainWindowViewModel : INotifyPropertyChanged
    {
        public string Greeting => "Hello World!";

        /*
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/

        public void SwitchToSearch()
        {
            var window = new SearchWindow();
            window.Show();
        }

    }
}