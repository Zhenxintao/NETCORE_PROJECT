using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Utilities;
using THMS.Core.API.Models;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto;
using THMS.Core.API.Service.UniformedServices.NetBalanceSystem;

namespace THMS.Core.API.Controllers.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 单元阀信息
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "NetBalance")]
    [ApiController]
    public class UvInfoController : ControllerBase
    {
        /// <summary>
        /// 获取二网平衡完成率
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryTwoNetworkBalancePercent()
        {
            return new UvService().queryTwoNetworkBalancePercent();
        }


        /// <summary>
        /// 获取二网平衡数据
        /// </summary>
        /// <param name="search">查询类，organ_id="-1"为总公司</param>
        /// <returns></returns>
        [HttpPost]
        public object queryTwoNetworkBalance(netsSearch search)
        {
            if (string.IsNullOrEmpty(search.organ_id))
                return "参数不可为空";
            return new UvService().queryTwoNetworkBalance(search);
        }

        /// <summary>
        /// 获取二网平衡历史数据
        /// </summary>
        /// <param name="search">查询类，organ_id="-1"为总公司</param>
        /// <returns></returns>
        [HttpPost]
        public object getNetworkBalanceHisData(netsHisSearch search)
        {
            if (string.IsNullOrEmpty(search.organ_id))
                return "参数不可为空";
            return new UvService().getNetworkBalanceHisData(search);
        }

        /// <summary>
        ///查询所有的单元阀设备信息
        /// </summary>
        /// <param name="searchBase">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryUvDeviceList(ValveSearchBase searchBase)
        {
            return new UvService().queryUvDeviceList(searchBase);
        }

        /// <summary>
        /// 查询已安装的单元阀设备信息司
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryUvInstallList(ValveSearch search)
        {
            return new UvService().queryUvInstallList(search);
        }

        ///// <summary>
        ///// 获取单元阀设备信息
        ///// </summary>
        ///// <param name="search">查询类</param>
        ///// <returns></returns>
        //[HttpPost]
        //public string queryUnitDeviceList(Search search)
        //{
        //    return new UvService().queryUnitDeviceList(search);
        //}

        /// <summary>
        /// 获取单元阀检测点指标列表
        /// </summary>
        /// <param name="unitId">单元阀编码</param>
        /// <returns></returns>
        [HttpGet]
        public string queryUnitCheckpointList(string unitId)
        {
            if (string.IsNullOrEmpty(unitId))
                return "单元阀编码不可为空";
            string result = new UvService().queryUnitCheckpointList(unitId);
            return result;
        }

        /// <summary>
        /// 获取单元阀实时监测数据
        /// </summary>
        /// <param name="unitId">单元阀编码</param>
        /// <returns></returns>
        [HttpGet]
        public string queryUnitRealData(string unitId)
        {
            if(string.IsNullOrEmpty(unitId))
                return "单元阀编码不可为空";

            return new UvService().queryUnitRealData(unitId);
        }

        /// <summary>
        /// 获取单元阀历史监测数据
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryUnitHisData(UnitHisSearch search)
        {
            return new UvService().queryUnitHisData(search);
        }

        /// <summary>
        /// 查询单元阀温差
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> queryUvTempDiff() => await new UvService().queryUvTempDiff();
        /// <summary>
        /// 查询末端温差
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> queryEtTempDiff() => await new UvService().queryEtTempDiff();
    }
}