using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models;
using THMS.Core.API.Service.UniformedServices.NetBalanceSystem;

namespace THMS.Core.API.Controllers.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 户阀信息
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "NetBalance")]
    [ApiController]
    public class HvInfoController : ControllerBase
    {
        /// <summary>
        ///查询所有的户阀设备信息
        /// </summary>
        /// <param name="searchBase">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryHvDeviceList(ValveSearchBase searchBase)
        {
            return new HvService().queryHvDeviceList(searchBase);
        }

        /// <summary>
        /// 查询已安装的户阀设备信息司
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryHvInstallList(ValveSearch search)
        {
            return new HvService().queryHvInstallList(search);
        }

        ///// <summary>
        ///// 获取户阀设备信息
        ///// </summary>
        ///// <param name="search">查询类</param>
        ///// <returns></returns>
        //[HttpPost]
        //public string queryHvDeviceList(Search search)
        //{
        //    return new HvService().queryHvDeviceList(search);
        //}

        /// <summary>
        /// 获取户阀检测点指标列表
        /// </summary>
        /// <param name="hvId">户阀编码</param>
        /// <returns></returns>
        [HttpGet]
        public string queryHvCheckpointList(string hvId)
        {
            if (string.IsNullOrEmpty(hvId))
                return "单元阀编码不可为空";
            string result = new HvService().queryHvCheckpointList(hvId);
            return result;
        }

        /// <summary>
        /// 获取户阀实时监测数据
        /// </summary>
        /// <param name="hvId">户阀编码</param>
        /// <returns></returns>
        [HttpGet]
        public string queryHvRealData(string hvId)
        {
            if(string.IsNullOrEmpty(hvId))
                return "户阀编码不可为空";

            return new HvService().queryHvRealData(hvId);
        }
    }
}