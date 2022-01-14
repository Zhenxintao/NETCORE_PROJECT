using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Service.UniformedServices.IndoorSystem;

namespace THMS.Core.API.Controllers.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 室温测试
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "IndoorSystem")]
    [ApiController]
    [EnableCors("any")]
    public class DemoController : ControllerBase
    {
        /// <summary>
        /// 室温测试
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ResultData GetListDevice()
        {
            var resultList = new Demo().GetListDevice();

            if (resultList.Count>=0)
            {
                ResultData result = new ResultData { Data = resultList, Code = ResultCode.Success, Message = ResultMessageInfo.SuccessMessage };
                return result;
            }
            else
            {
                ResultData result = new ResultData {  Code = ResultCode.Error, Message = ResultMessageInfo.ErrorMessage };
                return result;
            }
        }
       

    }
}