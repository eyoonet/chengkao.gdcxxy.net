using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CefSharp.MinimalExample.Console.Helper
{
    public class CefClientProcess : Process
    {

        public void Stop()
        {
            CloseMainWindow();
            Close();
            OnExited();
        }
    }
}
