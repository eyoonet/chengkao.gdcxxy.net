﻿using cef.protocol;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#pragma warning disable CA1416 // 验证平台兼容性
namespace CefSharp.MinimalExample.WinForms.JsCall
{
    public class JsBridge
    {
        private ChromiumWebBrowser browser;
        private BrowserForm browserForm;
        public JsBridge(ChromiumWebBrowser browser, BrowserForm browserForm)
        {
            this.browser = browser;
            this.browserForm = browserForm;
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
            var response = browserForm.GrpcClient.GetSerial(new SerialRequest() { Id = Process.GetCurrentProcess().Id });
            return Dns.GetHostName() + "-" + response.Serial.ToString();
        }


        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void SetUserInfo(string userName, string password) {
            browserForm.GrpcClient.SetUserInfo(new UserInfoRequest { ProcessId= Process.GetCurrentProcess().Id, UserName = userName, Password = password });
        }

    }
}
