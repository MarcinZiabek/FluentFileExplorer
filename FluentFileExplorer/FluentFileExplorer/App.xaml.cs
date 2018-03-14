using System;
using System.Globalization;
using System.Threading.Tasks;

using FluentFileExplorer.Services;
using FluentFileExplorer.Views;

using Microsoft.Practices.Unity;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentFileExplorer
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<ISampleDataService, SampleDataService>();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            return LaunchApplicationAsync(PageTokens.MainPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            Services.ThemeSelectorService.SetRequestedTheme();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            await Task.CompletedTask;
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol && ((ProtocolActivatedEventArgs)args)?.Uri == null && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                if (protocolArgs.Uri.AbsolutePath.Equals("sample", StringComparison.OrdinalIgnoreCase))
                {
                    var secret = "<<I-HAVE-NO-SECRETS>>";

                    try
                    {
                        if (protocolArgs.Uri.Query != null)
                        {
                            // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                            var decoder = new Windows.Foundation.WwwFormUrlDecoder(protocolArgs.Uri.Query);

                            secret = decoder.GetFirstValueByName("secret");
                        }
                    }
                    catch (Exception)
                    {
                        // NullReferenceException if the URI doesn't contain a query
                        // ArgumentException if the query doesn't contain a param called 'secret'
                    }

                    // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                    return LaunchApplicationAsync(PageTokens.UriSchemePage, secret);
                }
                else
                {
                    // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                    OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
                }
            }

            return Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "FluentFileExplorer.ViewModels.{0}ViewModel, FluentFileExplorer", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await base.OnInitializeAsync(args);
        }

        public void SetNavigationFrame(Frame frame)
        {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            return shell;
        }
    }
}
