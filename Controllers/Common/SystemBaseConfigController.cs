using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.Common;

namespace THMS.Core.API.Controllers.Common
{
    /// <summary>
    /// 数值颜色配置
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    [EnableCors("any")]
    public class SystemBaseConfigController : ControllerBase
    {
        /// <summary>
        /// 获取数值颜色配置表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<SystemBaseConfig>> GetSystemBaseConfigList()
        {
            List<SystemBaseConfig> result = await new SystemBaseConfigService().GetSystemBaseConfigList();

            return result;
        }

        /// <summary>
        /// 修改数值颜色配置表
        /// </summary>
        /// <param name="SystemBaseConfig">SystemBaseConfig Model</param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateSystemBaseConfig(SystemBaseConfig SystemBaseConfig)
        {
            return new SystemBaseConfigService().UpdateSystemBaseConfig(SystemBaseConfig);
        }
    }
}
