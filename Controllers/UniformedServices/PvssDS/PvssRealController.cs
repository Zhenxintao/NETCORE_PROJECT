using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Service.UniformedServices.PvssDSSystem;

namespace THMS.Core.API.Controllers.UniformedServices.PvssDS
{
    /// <summary>
    /// PVSS数据同步控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "PVSS")]
    [ApiController]
    [EnableCors("any")]
    public class PvssRealController : ControllerBase
    {
        /// <summary>
        /// 实时数据同步
        /// </summary>
        /// <param name="Pvss_id"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int ValueDescUpdate(string Pvss_id, List<MdjValueDesc_Update_PVSS> list)
        {
            return new PvssDSServices().ValueDescUpdate(Pvss_id, list);
        }

    }
}
