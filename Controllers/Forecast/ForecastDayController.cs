using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Service.Forecast;

namespace THMS.Core.API.Controllers.Forecast
{
    /// <summary>
    /// 换热站负荷预测天数据控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ ApiExplorerSettings(GroupName = "Forecast")]
    [ApiController]
    
    [EnableCors("any")]
    public class ForecastDayController : ControllerBase
    {
        StationForecastRealDayService s = new StationForecastRealDayService();
        StationForecastHistoryDayService sh = new StationForecastHistoryDayService();

        /// <summary>
        /// 查询实时信息天表信息
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
        public object SelStationForecastRealDay(int vpnuserid, string startTime, string endTime, int heatType, int rows, int page)
        {
            var result = s.SelStationForecastRealHour(vpnuserid, startTime, endTime, heatType, rows, page);
            ResultMessgeData m = new ResultMessgeData();
            m.rows = result.Item1;
            m.total = result.Item2;
            return m;
        }

        /// <summary>
        /// 查询历史信息天表信息
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


    }
}