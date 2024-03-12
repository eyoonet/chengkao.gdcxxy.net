using CefSharp.MinimalExample.Console.Helper;
using CefSharp.MinimalExample.Console.Model;
using console.Model;
using Newtonsoft.Json;
using Stylet;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace console
{
    public class CopilotViewModel : Screen
    {

        public BindableCollection<LogItemViewModel> LogItemViewModels { get; set; }

        public BindableCollection<UserInfo> UserInfos { get=> _userInfos; set=> SetAndNotify(ref _userInfos, value); }
        private BindableCollection<UserInfo> _userInfos = new BindableCollection<UserInfo>();

        public CopilotViewModel() {
            DisplayName = "控制台";
            LogItemViewModels = new BindableCollection<LogItemViewModel>();
            UserInfos = new BindableCollection<UserInfo>();
            Echo("服务启动完成");
        }


        /// <summary>
        /// Adds log.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="color">The font color.</param>
        /// <param name="weight">The font weight.</param>
        public void Echo(string content, string color = LogColor.Trace, string weight = "Regular")
        {
            LogItemViewModels.Add(new LogItemViewModel(content, color, weight));
            //LogItemViewModels.Insert(0, new LogItemViewModel(content, color, weight));
        }


        public string Domain
        {
            get => _domain;
            set
            {
                _ = SetAndNotify(ref _domain, value);
                ViewStatusStorage.Set("Domain", value);
            }
        }
        private string _domain = ViewStatusStorage.Get("Domain", "http://127.0.0.1:8080");



        public void RequestUserlist()
        {
            try
            {
                var httpWebRequest = WebRequest.Create($@"{Domain}/api/account/{Dns.GetHostName()}/list") as HttpWebRequest;
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                var httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                // 获取输入输出流
                using (var sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    var text = sr.ReadToEnd();
                    var responseObject = JsonConvert.DeserializeObject<AjaxResult>(text);

                    foreach (var item in responseObject.rows)
                    {
                        var userInfo = new UserInfo {
                            UserName = item["userName"].ToString(), 
                            Password= item["password"].ToString() 
                        };
                        UserInfos.Add(userInfo);
                    }
                }
            }
            catch (Exception)
            {
                //return string.Empty;
            }
        }


        public void ClearUserInfos()
        {
            UserInfos.Clear();
        }

        public void DoNewUserLogin_Click() {
            try
            {
                var processs = new CefClientProcess() { };
                //CefSharp.MinimalExample.WinForms.netcore.exe | CefSharp.MinimalExample.OffScreen.netcore.exe
                processs.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "CefSharp.MinimalExample.WinForms.netcore.exe");
                processs.StartInfo.Arguments = "pip_test";
                processs.StartInfo.UseShellExecute = false;
                processs.EnableRaisingEvents = true;
                processs.Exited += delegate
                {
                    Echo(string.Format("进程 {0} 退出 {1} ", processs.Id, processs.ExitCode));
                    var u = UserInfos.First(x=>x.ProcessId == processs.Id);
                    UserInfos.Remove(u);
                };
                processs.Start();
                var UserInfo = new UserInfo { ProcessId = processs.Id ,Process= processs};
                UserInfos.Add(UserInfo);
            }
            catch (Exception ex)
            {
                Echo(ex.Message, LogColor.Error);
            }
        }
    }
}
