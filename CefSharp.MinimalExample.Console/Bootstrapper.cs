using cef.protocol;
using CefSharp.MinimalExample.Console.GrpcService;
using console;
using Grpc.Core;
using Stylet;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace console
{
    /// <summary>
    /// The bootstrapper.
    /// </summary>
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        private Server? server;
        /// <inheritdoc/>
        /// <remarks>初始化些啥自己加。</remarks>
        protected override void OnStart()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            base.OnStart();
            ViewStatusStorage.Load();
            // grpc
            string ip = ViewStatusStorage.Get("grpc.ip", "localhost");
            int port = Convert.ToInt32(ViewStatusStorage.Get("grpc.port", "8099"));
            server = new Server()
            {
                Services = {
                    CefProtocolService.BindService(new ProcessService())
                },
                Ports = { new ServerPort(ip, port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        protected override void Configure()
        {
            base.Configure();
            IoC.GetInstance = Container.Get;
            IoC.BuildUp = Container.BuildUp;
            IoC.GetAllInstances = Container.GetAll;
        }

        /// <inheritdoc/>
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<CopilotViewModel>().ToSelf().InSingletonScope();
            builder.Bind<SettingsViewModel>().ToSelf().InSingletonScope();
        }

        /// <inheritdoc/>
        /// <remarks>退出时执行啥自己加。</remarks>
        protected override void OnExit(ExitEventArgs e)
        {
            ViewStatusStorage.Save();
        }
        /// <inheritdoc/>
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            // 抛异常了，可以打些日志
        }
    }
}