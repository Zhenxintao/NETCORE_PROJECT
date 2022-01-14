using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using THMS.Core.API.Logs;

namespace THMS.Core.API.ExceptionFilter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 发生异常时进入
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                Logger.Info("全局异常过滤器："+context.Exception.Message);
                context.Result = new ContentResult
                {
                    //Content = context.Exception.Message,//这里是把异常抛出。也可以不抛出。
          StatusCode = StatusCodes.Status200OK,
                    ContentType = "text/html;charset=utf-8"
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
