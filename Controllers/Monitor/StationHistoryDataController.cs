using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models.Monitor;
using THMS.Core.API.Service.Monitor;

namespace THMS.Core.API.Controllers.Monitor
{
    /// <summary>
    /// 历史数据控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Monitor")]
    [ApiController]
    [EnableCors("any")]
    public class StationHistoryDataController : Controller
    {
        StationHistoryDataService stationHistoryDataService = new StationHistoryDataService();

        /// <summary>
        /// 获取单个站点历史数据（表格）
        /// </summary>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="narrayNo">机组编号</param>
        /// <param name="userconfig_id">用户列表配置id</param>
        /// <param name="chartDateType">历史数据类型</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        [HttpGet]
        public List<dynamic> GetHistoryData4Station(int vpnuser_id, int narrayNo, int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            var result = stationHistoryDataService.GetHistoryData4Station(vpnuser_id, narrayNo, userconfig_id, chartDateType, startDate, endDate);

            return result;
        }
    }
}