using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Models.ViewModel;
using THMS.Core.API.Service.UniformedServices.ServiceProcessing;

namespace THMS.Core.API.Controllers.UniformedServices.ServiceProcessing
{
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "PVSS")]
    [ApiController]
    [EnableCors("any")]
    public class ServiceProcessing : ControllerBase
    {
        ServiceData service = new ServiceData();

        /// <summary>
        /// 统一服务实时数据传输信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<UniformedServicesInfo> serachList()
        {

            List<UniformedServicesInfo> body = service.serachList();
            return body;
        }

        /// <summary>
        /// 统一服务各平台今日数据传数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UniformedServiceResponse UniformedServiceResponse()
        {
            return service.UniformedServiceResponse();
        }

    }
}
