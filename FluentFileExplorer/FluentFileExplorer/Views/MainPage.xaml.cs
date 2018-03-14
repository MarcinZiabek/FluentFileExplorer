using System;

using FluentFileExplorer.ViewModels;

using Windows.UI.Xaml.Controls;

namespace FluentFileExplorer.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
