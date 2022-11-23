using Android.Content;
using Android.OS;
using Android.Webkit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin_Browser;
using Xamarin_Browser.Droid.Renderer;

[assembly: ExportRenderer(typeof(WebViewWithTitle), typeof(TitleWebViewRenderer))]

namespace Xamarin_Browser.Droid.Renderer
{
    public class TitleWebViewRenderer : WebViewRenderer
    {
        public TitleWebViewRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Settings.SetSupportMultipleWindows(true);
                Control.SetWebViewClient(new TitleWebViewClient(this));
                Control.SetWebChromeClient(new TitleWebChromeClient());
            }
        }

        internal class TitleWebViewClient : FormsWebViewClient
        {
            readonly TitleWebViewRenderer titleWebViewRenderer;

            internal TitleWebViewClient(TitleWebViewRenderer titleWebViewRenderer) : base(titleWebViewRenderer)
            {
                this.titleWebViewRenderer = titleWebViewRenderer;
            }

            public override void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                base.OnPageFinished(view, url);
                ((IElementController)titleWebViewRenderer.Element).SetValueFromRenderer(WebViewWithTitle.PageTitleProperty, view.Title);
            }

            public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, IWebResourceRequest request)
            {
                var url = request.Url;
                if (url == null || !url.IsOpaque && (url.IsRelative || url.Path?.Contains(MainPage.webPageUrl) == false))
                    return base.ShouldOverrideUrlLoading(view, request);

                try
                {
                    Intent intent = new Intent(Intent.ActionView, url);
                    view.Context.StartActivity(intent);
                    return true;
                }
                catch (System.Exception)
                {
                    return base.ShouldOverrideUrlLoading(view, request);
                }
            }
        }

        internal class TitleWebChromeClient : WebChromeClient
        {
            public override bool OnCreateWindow(Android.Webkit.WebView view, bool isDialog, bool isUserGesture, Message resultMsg)
            {
                try
                {
                    var result = view.GetHitTestResult();
                    var text = result.Extra;
                    if (result.Type == HitTestResult.EmailType)
                        text = "mailto:" + text;
                    Android.Net.Uri uri = Android.Net.Uri.Parse(text);
                    Intent intent = new Intent(Intent.ActionView, uri);
                    view.Context.StartActivity(intent);
                }
                catch (System.Exception)
                {

                }
                return base.OnCreateWindow(view, isDialog, isUserGesture, resultMsg);
            }
        }
    }
}