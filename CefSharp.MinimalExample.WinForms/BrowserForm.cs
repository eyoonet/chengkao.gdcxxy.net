// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using cef.protocol;
using CefSharp.DevTools.DOM;
using CefSharp.DevTools.IO;
using CefSharp.DevTools.Network;
using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.MinimalExample.WinForms.JsCall;
using CefSharp.WinForms;
using Grpc.Core;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ChromiumWebBrowser = CefSharp.WinForms.ChromiumWebBrowser;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class BrowserForm : Form
    {
#if DEBUG
        private const string Build = "Debug";
#else
        private const string Build = "Release";
#endif
        private readonly string title = "CefSharp.MinimalExample.WinForms (" + Build + ")";
        private readonly ChromiumWebBrowser browser;
        public CefProtocolService.CefProtocolServiceClient GrpcClient { get; set; }
        public BrowserForm(CefProtocolService.CefProtocolServiceClient grpcClient)
        {
            InitializeComponent();
            GrpcClient = grpcClient;
            Text = title;
            WindowState = FormWindowState.Normal;

            browser = new ChromiumWebBrowser("https://chengkao.gdcxxy.net/gdcx/login2.php");
            toolStripContainer.ContentPanel.Controls.Add(browser);
            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadingStateChanged += OnLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.LoadError += OnBrowserLoadError;
            browser.FrameLoadEnd += OnFrameLoadEnd;
            browser.JsDialogHandler = new MyJsDialogHandler();
            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "JsBridge")
                {
                    var upload = new JsBridge(browser,this);
                    repo.Register("JsBridge", upload, options: BindingOptions.DefaultBinder);
                }
            };
            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
               Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

#if NETCOREAPP
            // .NET Core
            var environment = string.Format("Environment: {0}, Runtime: {1}",
                System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant(),
                System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
#else
            // .NET Framework
            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var environment = String.Format("Environment: {0}", bitness);
#endif

            DisplayOutput(string.Format("{0}, {1}", version, environment));
        }

        /// <summary>
        /// 页面加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine("进入{0}", e.Url);
            var url = new Uri(e.Url);
            //注入公共js
            string common_path = "./" + url.DnsSafeHost + "/common";

            if (System.IO.Directory.Exists(common_path))
            {
                var files = System.IO.Directory.GetFiles(common_path, "*.js");
                foreach (string script_path in files)
                {
                    transfuseScript(url.AbsolutePath, script_path);
                }
            }
            //注入页面js
            string path = "./" + url.DnsSafeHost + url.AbsolutePath;
            if (System.IO.Directory.Exists(path))
            {
                var files = System.IO.Directory.GetFiles(path, "*.js");
                foreach (string script_path in files)
                {
                    transfuseScript(url.AbsolutePath, script_path);
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private void transfuseScript(string absolutePath, string script_path)
        {
            //遍历frame窗口 注入js代码
            var list = browser.GetBrowser().GetFrameNames();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var frame = browser.GetBrowser().GetFrame(item);
                    if (frame.Url == "")
                    {
                        continue;
                    }
                    Uri url = new Uri(frame.Url);
                    if (url.AbsolutePath == absolutePath)
                    {
                        Console.WriteLine("注入页面:{0} 注入Script路径:{1}", absolutePath, script_path);
                        var script = System.IO.File.ReadAllText(script_path);
                        frame.ExecuteJavaScriptAsync(script);
              
                    }
                }
            }
        }
        private void OnBrowserLoadError(object sender, LoadErrorEventArgs e)
        {
            //Actions that trigger a download will raise an aborted error.
            //Aborted is generally safe to ignore
            if (e.ErrorCode == CefErrorCode.Aborted)
            {
                return;
            }

            var errorHtml = string.Format("<html><body><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              e.FailedUrl, e.ErrorText, e.ErrorCode);

            _ = e.Browser.SetMainFrameDocumentContentAsync(errorHtml);

            //AddressChanged isn't called for failed Urls so we need to manually update the Url TextBox
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = e.FailedUrl);
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            var b = ((ChromiumWebBrowser)sender);

            this.InvokeOnUiThreadIfRequired(() => b.Focus());
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            string msgStr = string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message);
            Task.Run(async () => {
                var call = GrpcClient.Echo();
                await call.RequestStream.WriteAsync(new LogItem { Msg= msgStr }); // 发送第一个消息
                await call.RequestStream.CompleteAsync();
            });
           DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Text = title + " - " + args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private void SetCanGoBack(bool canGoBack)
        {
            this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading)
        {
            goButton.Text = isLoading ?
                "Stop" :
                "Go";
            goButton.Image = isLoading ?
                Properties.Resources.nav_plain_red :
                Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        public void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string urlString)
        {
            // No action unless the user types in some sort of url
            if (string.IsNullOrEmpty(urlString))
            {
                return;
            }

            Uri url;

            var success = Uri.TryCreate(urlString, UriKind.RelativeOrAbsolute, out url);

            // Basic parsing was a success, now we need to perform additional checks
            if (success)
            {
                // Load absolute urls directly.
                // You may wish to validate the scheme is http/https
                // e.g. url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps
                if (url.IsAbsoluteUri)
                {
                    browser.LoadUrl(urlString);

                    return;
                }

                // Relative Url
                // We'll do some additional checks to see if we can load the Url
                // or if we pass the url off to the search engine
                var hostNameType = Uri.CheckHostName(urlString);

                if (hostNameType == UriHostNameType.IPv4 || hostNameType == UriHostNameType.IPv6)
                {
                    browser.LoadUrl(urlString);

                    return;
                }

                if (hostNameType == UriHostNameType.Dns)
                {
                    try
                    {
                        var hostEntry = Dns.GetHostEntry(urlString);
                        if (hostEntry.AddressList.Length > 0)
                        {
                            browser.LoadUrl(urlString);

                            return;
                        }
                    }
                    catch (Exception)
                    {
                        // Failed to resolve the host
                    }
                }
            }

            // Failed parsing load urlString is a search engine
            var searchUrl = "https://www.google.com/search?q=" + Uri.EscapeDataString(urlString);

            browser.LoadUrl(searchUrl);
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }
    }
}
