using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Logs;
using THMS.Core.API.Service.UniformedServices.ServiceProcessing;

namespace THMS.Core.API.ExceptionFilter
{
    public class RequestResponseWare
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger _logger;
        //private SortedDictionary<string, object> _data;
        private ServiceData _service;
        //private Stopwatch _stopwatch;
        public RequestResponseWare(RequestDelegate next)
        {
            _next = next;
            _service = new ServiceData();
            //_stopwatch = new Stopwatch();
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {

                HttpRequest request = context.Request;
                string path = request.Path.ToString();
                string startTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                #region 废弃代码 SorteDictionary…………
                //_stopwatch.Restart();
                //SortedDictionary<string, object> _data = new SortedDictionary<string, object>();
                //var userIp = await GetClientUserIp(context);
                //_data.Add("userUrl", userIp);
                //_data.Add("path", request.Path.ToString());
                //_data.Add("headers", request.Headers.ToDictionary(x => x.Key, v => string.Join(";", v.Value.ToList())));
                //_data.Add("method", request.Method);
                //_data.Add("executeStartTime", DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                // 获取请求body内容
                //if (request.Method.ToLower().Equals("post"))
                //{
                //    // 启用倒带功能，就可以让 Request.Body 可以再次读取
                //    request.EnableRewind();
                //    Stream stream = request.Body;
                //    byte[] buffer = new byte[request.ContentLength.Value];
                //    stream.Read(buffer, 0, buffer.Length);
                //    _data.Add("body", Encoding.UTF8.GetString(buffer));
                //    request.Body.Position = 0;
                //}
                //else if (request.Method.ToLower().Equals("get"))
                //{
                //    _data.Add("body", request.QueryString.Value);
                //}
                #endregion

                // 获取Response.Body内容
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);
                    await GetResponse(context.Response);
                    //_data.Add("body", await GetResponse(context.Response));
                    //_data.Add("executeEndTime", DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                //响应完成记录时间和存入日志
                context.Response.OnCompleted(() =>
                {
                    //_stopwatch.Stop();
                    //_data.Add("elaspedTime", _stopwatch.ElapsedMilliseconds + "ms");
                    //_data.Add("elaspedTime", _stopwatch.ElapsedMilliseconds);
                    //var json = JsonConvert.SerializeObject(_data);
                    //Logger.Info(json + "&&" + "api" + "&&" + request.Method.ToUpper());
                    string endTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    _service.RequestInfo(path,startTime,endTime);
                    return Task.CompletedTask;
                });
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        /// <summary>
        /// 获取响应内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<string> GetResponse(HttpResponse response)
        {
            //response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        /// <summary>
        /// 获取客户Ip
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> GetClientUserIp(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
    /// <summary>
    /// 扩展中间件
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestResponseWare>();
        }
    }
}
