using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cef.protocol;
using console;
using console.Model;
using Grpc.Core;

namespace CefSharp.MinimalExample.Console.GrpcService
{
    class ProcessService : CefProtocolService.CefProtocolServiceBase
    {
        public async override Task<CommonResponse> Echo(IAsyncStreamReader<LogItem> requestStream, ServerCallContext context)
        {
            var vm = IoC.Get<CopilotViewModel>();
            try
            {
                while (await requestStream.MoveNext())
                {
                    LogItem log = requestStream.Current;
                    vm.Echo(log.Msg, log.LogColor);
                }
                return new CommonResponse { Code = 0, Msg = "接收成功" };
            }
            catch (Exception)
            {
                return new CommonResponse { Code = -1, Msg = "接收失败" };
            }
        }

        public override Task<CommonResponse> SetUserInfo(UserInfoRequest request, ServerCallContext context)
        {
            var vm = IoC.Get<CopilotViewModel>();
            if (vm.UserInfos.Count > 0)
            {
                var userInfo = vm.UserInfos.First(x => x.ProcessId == request.ProcessId);
                userInfo.UserName = request.UserName;
                userInfo.Password = request.Password;
            }
            return Task.FromResult(new CommonResponse() { Code = 1 });
        }

        public override Task<SerialResponse> GetSerial(SerialRequest request, ServerCallContext context)
        {
            var vm = IoC.Get<CopilotViewModel>();
            if (vm.UserInfos.Count > 0) {
                var userInfo = vm.UserInfos.First(x => x.ProcessId == request.Id);
                return Task.FromResult(new SerialResponse() { Serial = vm.UserInfos.IndexOf(userInfo) });
            }
            return Task.FromResult(new SerialResponse() { Serial = -1 });
        }
    }
}
