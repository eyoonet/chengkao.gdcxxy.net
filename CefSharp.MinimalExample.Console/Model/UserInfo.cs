using CefSharp.MinimalExample.Console.Helper;
using Stylet;
using System;
using System.Collections.Generic;
using System.Text;

namespace console.Model
{
    public class UserInfo : PropertyChangedBase
    {
        public string UserName { get => userName; set => SetAndNotify(ref userName, value); }
        private string userName = "";

        public string Password { get => password; set => SetAndNotify(ref password, value); }
        private string password = "";


        private int processId = 0;
        public int ProcessId { get => processId; set => SetAndNotify(ref processId, value); }


        public CefClientProcess Process { get; set; } = new CefClientProcess();
    }
}
