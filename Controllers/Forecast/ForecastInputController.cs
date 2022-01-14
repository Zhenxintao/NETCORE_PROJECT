using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using THMS.Core.API.Service.Forecast;
using THMS.Core.API.ModelDto;

namespace THMS.Core.API.Controllers.Forecast
{
    /// <summary>
    /// 换热站负荷预测基础数据输入控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Forecast")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastInputController : ControllerBase
    {
        StationForecastInputService s = new StationForecastInputService();

        /// <summary>
        /// 查询换热站负荷预测输入表
        /// </summary>
        /// <param name="vpnuserid">站点ID</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="heatType">采暖方式</param>
        /// <param name="rows">分页条数</param>
        /// <param name="page">当前页数</param>
        /// <returns></returns>
        [HttpGet]
        public object SelStationForecastInput(int vpnuserid, string startTime, string endTime, int heatType, int rows, int page)
        {
            var result = s.SelStationForecastInput(vpnuserid, startTime, endTime, heatType, rows, page);
            ResultMessgeData m = new ResultMessgeData();
            m.rows = result.Item1;
            m.total = result.Item2;
            return m;
        }

        /// <summary>
        /// 查询各个换热站对应机组采暖方式信息
        /// </summary>
        /// <param name="vpnuserid">站点ID</param>
        /// <param name="heatType">采暖方式</param>
        /// <param name="StationBranchArrayNumber">机组号</param>
        /// <param name="rows">分页条数</param>
        /// <param name="page">当前页数</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelStationForecastMessge(int vpnuserid, int heatType, int StationBranchArrayNumber, int rows, int page)
        {

            var result = s.SelStationForecastMessge(vpnuserid, heatType, StationBranchArrayNumber, rows, page);
            ResultMessgeData m = new ResultMessgeData();
            m.rows = result.Item1;
            m.total = result.Item2;
            return m;

        }

        /// <summary>
        /// 添加换热站负荷预测输入表
        /// </summary>
        /// <param name="stationForecastInput">StationForecastInput 实体类</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool AddStationForecastInput(StationForecastInput stationForecastInput)
        {
            bool result = s.AddStationForecastInput(stationForecastInput);
            return result;
        }

        /// <summary>
        /// 批量添加换热站负荷预测输入表
        /// </summary>
        /// <param name="stationForecastInput">集合List </param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool AddToArrayStationForecastInput(List<StationForecastInput> stationForecastInput)
        {
            bool result = s.AddToArrayStationForecastInput(stationForecastInput);
            return result;
        }
        /// <summary>
        /// 批量添加换热站负荷预测输入表(新版Web)
        /// </summary>
        /// <param name="station">批量添加Data,stationType为-1是全部站点</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool AddToArrayStationForecastInputWeb(StationForecastInputDto station)
        {
            bool result = s.AddToArrayStationForecastInputWeb(station);
            return result;
        }
        /// <summary>
        /// 换热站负荷预测输入表更新修改信息
        /// </summary>
        /// <param name="stationForecastInput"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool UpdStationForecastInput(StationForecastInput stationForecastInput)
        {

            bool result = s.UpdStationForecastInput(stationForecastInput);
            return result;
        }

        /// <summary>
        /// 修改机组表中采暖方式以及散热情况
        /// </summary>
        /// <param name="staitonbranch"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdStationBransh(StationBranch staitonbranch)
        {
            bool result = s.UpdStationBransh(staitonbranch);
            return result;
        }

        /// <summary>
        /// 批量修改机组表中采暖方式以及散热情况
        /// </summary>
        /// <param name="staitonbranch"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool UpdToArryStationBransh(List<StationBranch> staitonbranch)
        {
            bool result = s.UpdToArryStationBransh(staitonbranch);
            return result;
        }
    }
}