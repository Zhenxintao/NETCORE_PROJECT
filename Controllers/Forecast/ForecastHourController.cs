using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Service.Forecast;

namespace THMS.Core.API.Controllers.Forecast
{
    /// <summary>
    /// 换热站负荷预测小时数据控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Forecast")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastHourController : ControllerBase
    {
        StationForecastRealHourService s = new StationForecastRealHourService();
        StationForecastHistoryHourService sh = new StationForecastHistoryHourService();

        /// <summary>
        /// 查询实时信息小时表信息
        /// </summary>
        /// <param name="vpnuserid">站点ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="heatType">采暖类型</param>
        /// <param name="rows">分页条数</param>
        /// <param name="page">当前页数</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelStationForecastRealHour(int vpnuserid, string startTime, string endTime, int heatType, int rows, int page)
        {
            var result = s.SelStationForecastRealHour(vpnuserid, startTime, endTime, heatType, rows, page);
            ResultMessgeData m = new ResultMessgeData();
            m.rows = result.Item1;
            m.total = result.Item2;
            return m;
        }

        /// <summary>
        /// 查询历史信息小时表信息
        /// </summary>
        /// <param name="vpnuserid">站点ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="heatType">采暖类型</param>
        /// <param name="rows">分页条数</param>
        /// <param name="page">当前页数</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelStationForecastHistoryHour(int vpnuserid, string startTime, string endTime, int heatType, int rows, int page)
        {
            var result = sh.SelStationForecastHistoryHour(vpnuserid, startTime, endTime, heatType, rows, page);
            ResultMessgeData m = new ResultMessgeData();
            m.rows = result.Item1;
            m.total = result.Item2;
            return m;
        }

        [HttpGet]
        public object Ceshi()
        {
           return new StationForecastRealHourService().AddStationForecastRealHourService();
        }
    }
}