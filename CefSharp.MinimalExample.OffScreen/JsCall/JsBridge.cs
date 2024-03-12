using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.MinimalExample.OffScreen.JsCall
{
    public class JsBridge
    {
        private ChromiumWebBrowser browser;

        public JsBridge(ChromiumWebBrowser browser)
        {
            this.browser = browser;
        }

        /// <summary>
        /// 模拟单击浏览器
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void click(int x = 0, int y = 0)
        {
            var host = browser.GetBrowser().GetHost();
            //鼠标单次点击，需要按下并松开，所以需要两个指令，false为按下，true为松开
            host.SendMouseClickEvent(x, y, MouseButtonType.Left, false, 1, CefEventFlags.None);
            Thread.Sleep(300);
            host.SendMouseClickEvent(x, y, MouseButtonType.Left, true, 1, CefEventFlags.None);
        }

        /// <summary>
        /// 获取计算机名称
        /// </summary>
        /// <returns></returns>
        public string GetHomeName() {
            return Dns.GetHostName();
        }
    }
}
