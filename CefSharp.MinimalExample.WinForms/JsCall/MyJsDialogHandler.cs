using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.MinimalExample.WinForms.JsCall
{
    public class MyJsDialogHandler : JsDialogHandler
    {
        protected override bool OnJSDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            return true;
        }
    }
}
