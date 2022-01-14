using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using THMS.Core.API.Service.UniformedServices.IndoorSystem;

namespace THMS.Core.API.Controllers.UniformedServices.IndoorSystem
{
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "IndoorSystem")]
    [ApiController]
    [EnableCors("any")]
    public class RealTempController : ControllerBase
    {
        RealTempService real = new RealTempService();
        //[HttpGet]
        //public HttpResponseMessage MessageList()
        //{

        //    var JsonStr = "1.1.1.1.1.1";
        //    //返回纯文本text/plain  ,返回json application/json  ,返回xml text/xml
        //    HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(JsonStr, Encoding.GetEncoding("UTF-8"), "text/plain") };
        //    return result;
        //}

        /// <summary>
        /// 生成实时室内温度txt
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetRealTemp() =>  real.GetRealTemp();




    }
}