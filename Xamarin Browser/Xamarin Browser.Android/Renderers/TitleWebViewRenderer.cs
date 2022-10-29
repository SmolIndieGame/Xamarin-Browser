using Android.Content;
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
                Control.SetWebViewClient(new TitleWebViewClient(this));
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
        }
    }
}