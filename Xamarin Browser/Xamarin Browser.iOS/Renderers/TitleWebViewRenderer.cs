using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin_Browser.iOS.Renderers
{
    public class TitleWebViewRenderer : WkWebViewRenderer, IWKScriptMessageHandler
    {
        WKUserContentController userController;

        public TitleWebViewRenderer() : this(new WKWebViewConfiguration())
        {
        }

        public TitleWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
            userController = config.UserContentController;
            var script = new WKUserScript(new NSString("document.title"), WKUserScriptInjectionTime.AtDocumentEnd, false);
            userController.AddUserScript(script);
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                userController.RemoveAllUserScripts();
            }
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            ((WebViewWithTitle)Element).PageTitle = ((WebViewWithTitle)Element).EvaluateJavaScriptAsync("document.title").GetAwaiter().GetResult();
        }
    }
}