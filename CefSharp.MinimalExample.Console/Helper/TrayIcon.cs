using HandyControl.Controls;
using System;
using System.Windows;

namespace console
{
    /// <summary>
    /// 托盘图标。
    /// </summary>
    public partial class TrayIcon
    {
        private readonly NotifyIcon notifyIcon = new NotifyIcon();
        private WindowState ws; // 记录窗体状态
        private bool _isMinimizeToTaskbar = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayIcon"/> class.
        /// </summary>
        public TrayIcon()
        {
            InitIcon();
        }

        private void InitIcon()
        {
          
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            ws = System.Windows.Application.Current.MainWindow.WindowState;
            if (ws == WindowState.Minimized)
            {
                SetShowInTaskbar(false);
            }
            else
            {
                SetShowInTaskbar(true);
            }
        }


      

     
        private void App_exit(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void App_show(object sender, EventArgs e)
        {
            SetShowInTaskbar(true);
            ws = System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
            System.Windows.Application.Current.MainWindow.Activate();
        }

        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            if (ws == WindowState.Minimized)
            {
                SetShowInTaskbar(true);
                ws = System.Windows.Application.Current.MainWindow.WindowState = WindowState.Normal;
                System.Windows.Application.Current.MainWindow.Activate();
            }
        }

        private void SetShowInTaskbar(bool state)
        {
            if (_isMinimizeToTaskbar)
            {
                System.Windows.Application.Current.MainWindow.ShowInTaskbar = state;
            }
        }

        

        /// <summary>
        /// Sets whether to minimize to taskbar.
        /// </summary>
        /// <param name="enable">Whether to minimize to taskbar.</param>
        public void SetMinimizeToTaskbar(bool enable)
        {
            _isMinimizeToTaskbar = enable;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            notifyIcon.Icon = null;
            notifyIcon.Dispose();
        }
    }
}
