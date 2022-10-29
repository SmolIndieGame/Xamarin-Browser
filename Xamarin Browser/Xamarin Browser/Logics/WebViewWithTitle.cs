using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin_Browser
{
    public class WebViewWithTitle : WebView
    {
        public static readonly BindableProperty PageTitleProperty = BindableProperty.Create(
            propertyName: "PageTitle",
            defaultValue: string.Empty,
            returnType: typeof(string),
            declaringType: typeof(string),
            defaultBindingMode: BindingMode.OneWayToSource);

        public string PageTitle
        {
            get => (string)GetValue(PageTitleProperty);
            set => SetValue(PageTitleProperty, value);
        }
    }
}
