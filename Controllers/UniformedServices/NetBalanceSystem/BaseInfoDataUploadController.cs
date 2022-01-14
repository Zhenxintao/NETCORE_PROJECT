using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.ReturnDto;
using THMS.Core.API.Service.UniformedServices.NetBalanceSystem;

namespace THMS.Core.API.Controllers.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 调用华夏api上传基础信息
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "NetBalance")]
    [ApiController]
    public class BaseInfoDataUploadController : ControllerBase
    {
        /// <summary>
        /// 基础信息上传
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response HxUpload()
        {
            return new BaseInfoDataUploadService().HxUpload();
        }
    }
}