using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.Monitor;

namespace THMS.Core.API.Controllers.Common
{
    /// <summary>
    /// 自定义列表公共方法控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    [EnableCors("any")]
    public class StationCustomController : ControllerBase
    {
        /// <summary>
        /// 自定义用户配置
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserConfig>> SelMonitorCustomList(int listType)
        {
            List<UserConfig> result =await new MonitorCustomListService().SelMonitorCustomList(listType);
            return result;
        }

        /// <summary>
        /// 自定义列表参数配置信息
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<StationPra> SelStationPra()
        {
            //获取自定义列表参数
            StationPra result = await new MonitorCustomListService().SelStationPra();
            return result;
        }

        /// <summary>
        /// 自定义列表换热站信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<List<StationInfo>> SelStationInfo(string searchName)
        {
            List<StationInfo> result = await new MonitorCustomListService().SelStationInfo(searchName);
            return result;
        }

        /// <summary>
        /// 自定义列表信息修改
        /// </summary>
        /// <param name="userConfigs"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool AddCustomList(UserConfig userConfigs)
        {
            return new MonitorCustomListService().AddCustomList(userConfigs);
        }

        /// <summary>
        /// 自定义列表信息删除
        /// </summary>
        /// <param name="id">信息表Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public bool RemoveCustomList(int id)
        {
            return new MonitorCustomListService().RemoveCustomList(id);
        }

        /// <summary>
        /// 机组信息查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<List<StationBranch>> SelStationBranchList(int id)
        {
            return await new MonitorCustomListService().SelStationBranchList(id);
        }
    }
}