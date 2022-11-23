using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin_Browser
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource reloadTimeoutCancel;

        public static WebView mainWebView;
        public const string webPageUrl = "https://if.ddns.net";

        public MainPage()
        {
            InitializeComponent();
            mainWebView = webView;
            webView.Source = webPageUrl;

            ICommand refreshCommand = new Command(() =>
            {
                BeforeLoadingStart();
                webView.Reload();
            });
            refreshView.Command = refreshCommand;

            BeforeLoadingStart();
        }

        private void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (!webView.CanGoBack)
                return;

            BeforeLoadingStart();
            webView.GoBack();
        }

        private void OnForwardClicked(object sender, EventArgs e)
        {
            if (!webView.CanGoForward)
                return;

            BeforeLoadingStart();
            webView.GoForward();
        }

        private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            refreshView.IsRefreshing = false;

            if (reloadTimeoutCancel == null)
                return;
            reloadTimeoutCancel.Cancel();
            reloadTimeoutCancel.Dispose();
            reloadTimeoutCancel = null;
        }

        private async Task LoadTimeout(CancellationToken cancellationToken)
        {
            await Task.Delay(10_000, cancellationToken);
            refreshView.IsRefreshing = false;
        }

        private void BeforeLoadingStart()
        {
            if (reloadTimeoutCancel != null)
            {
                reloadTimeoutCancel.Cancel();
                reloadTimeoutCancel.Dispose();
            }
            reloadTimeoutCancel = new CancellationTokenSource();
            Task.Run(() => LoadTimeout(reloadTimeoutCancel.Token));

            refreshView.IsRefreshing = true;
        }
    }
}
