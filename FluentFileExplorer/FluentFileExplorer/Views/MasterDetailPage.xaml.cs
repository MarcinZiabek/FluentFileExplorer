using System;

using FluentFileExplorer.ViewModels;

using Windows.UI.Xaml.Controls;

namespace FluentFileExplorer.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MasterDetailViewModel ViewModel => DataContext as MasterDetailViewModel;

        public MasterDetailPage()
        {
            InitializeComponent();
        }
    }
}
