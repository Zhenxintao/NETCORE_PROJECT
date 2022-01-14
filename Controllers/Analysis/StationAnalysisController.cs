using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models.Analysis;
using THMS.Core.API.Models.Monitor;
using THMS.Core.API.Service.Analysis;
using THMS.Core.API.Service.Monitor;

namespace THMS.Core.API.Controllers.Analysis
{
    /// <summary>
    /// 监控分析模块
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Analysis")]
    [ApiController]
    [EnableCors("any")]
    public class StationAnalysisController : ControllerBase
    {
        StationAnalysisService stationAnalysisService = new StationAnalysisService();
        StationHistoryDataService stationHistoryDataService = new StationHistoryDataService();

        /// <summary>
        /// 获取多站对比曲线数据
        /// </summary>
        /// <param name="userconfig_id">用户配置id</param>
        /// <param name="chartDateType">0:分钟、1:小时、2:天</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        [HttpGet]
        public List<dynamic> GetMultistationChartList(int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            var result = stationAnalysisService.GetMultistationChartList(userconfig_id, chartDateType, startDate, endDate);

            return result;
        }

        #region 全网平衡分析

        /// <summary>
        /// 虚拟网列表
        /// </summary>
        /// <param name="status">启用状态 0:未启用，1:启用</param>
        /// <returns></returns>
        [HttpGet]
        public List<VirtualNetConfigExt> GetVirtualNetConfigs(int status = -1)
        {
            var result = stationAnalysisService.GetVirtualNetConfigs(status);

            return result;
        }

        /// <summary>
        /// 运行质量
        /// </summary>
        /// <param name="id">虚拟网Id</param>
        /// <returns></returns>
        [HttpGet]
        public List<VirtualNetStationM> GetStationHeatNetRunList(int id)
        {
            var result = stationAnalysisService.GetStationHeatNetRunList(id);

            return result;
        }
        #endregion

        #region 站点排行

        /// <summary>
        /// 站点排行数据表(实时部分)
        /// </summary>
        /// <param name="powerid"></param>
        /// <param name="organizationid"></param>
        /// <param name="tagname"></param>
        /// <param name="ordertype"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public dynamic GetStationRank(string powerid, string organizationid, string tagname, string ordertype)
        {
            var result = stationAnalysisService.GetStationRank(powerid, organizationid, tagname, ordertype);
            return result;
        }

        /// <summary>
        /// 站点排行，列表(历史部分)
        /// </summary>
        /// <param name="powerid">热源id,全部：-1,多选热源：1,2,3</param>
        /// <param name="organizationid">组织机构id,全部：-1,多选组织机构：1,2,3</param>
        /// <param name="tagname">tagname</param>
        /// <param name="ordertype">排序字段,asc\desc</param>
        /// <param name="chartDateType">0:分钟、1:小时、2:天</param>
        /// <param name="createtime">查询日期</param>
        /// <returns></returns>
        [HttpGet]
        public List<dynamic> GetStationRankingList(string powerid, string organizationid, string tagname, string ordertype, ChartDateType chartDateType, DateTime createtime)
        {
            var result = stationAnalysisService.GetStationRankingList(powerid, organizationid, tagname, ordertype, chartDateType, createtime);

            return result;
        }

        /// <summary>
        /// 获取站点历史数据
        /// </summary>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="tagname">tagname</param>
        /// <param name="chartDateType">0:分钟、1:小时、2:天</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        [HttpGet]
        public List<dynamic> GetHistoryData4Station(int vpnuser_id, string tagname, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            var result = stationHistoryDataService.GetHistoryData4Station(vpnuser_id, tagname, chartDateType, startDate, endDate);

            return result;
        }

        /// <summary>
        /// 水压图
        /// </summary>
        /// <param name="powerid">热源id</param>
        /// <returns></returns>
        [HttpGet]
        public List<CustomParamChartModel> GetStationTempPressData4Stations(int powerid)
        {
            var result = stationAnalysisService.GetStationTempPressData4Stations(powerid);

            return result;
        }
        #endregion
    }
}