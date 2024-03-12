using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.MinimalExample.Console.Model
{
    public class AjaxResult
    {
        public int code { get; set; }

        public string msg { get; set; }

        public Dictionary<string, object> data;

        public List<JObject> rows { get; set; }
    }
}
