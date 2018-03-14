using System;

using FluentFileExplorer.ViewModels;

using Windows.UI.Xaml.Controls;

namespace FluentFileExplorer.Views
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel => DataContext as SettingsViewModel;

        //// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere

        public SettingsPage()
        {
            InitializeComponent();
        }
    }
}
