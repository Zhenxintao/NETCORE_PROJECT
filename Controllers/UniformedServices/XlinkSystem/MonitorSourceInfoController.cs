using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Service.Monitor;
using THMS.Core.API.Service.UniformedServices.XlinkSystem;

namespace THMS.Core.API.Controllers.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 热源监测数据信息控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "XlinkSystem")]
    [ApiController]
    [EnableCors("any")]
    public class MonitorSourceInfoController : ControllerBase
    {
        /// <summary>
        /// 热源检测点指标列表
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object querySourceCheckpointList(int sourceId)
        {
            string resultJson = new MonitorPowerInfoServices().querySourceCheckpointList(sourceId);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 热源实时监测数据
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object querySourceRealData(int sourceId)
        {
            string resultJson = new MonitorPowerInfoServices().querySourceRealData(sourceId,"huaxia");
            var resultJsonData = JsonConvert.DeserializeObject(resultJson);
            return resultJsonData;
        }

        /// <summary>
        /// 热源历史监测数据
        /// </summary>
        /// <param name="historyPowerParamesDto"></param>
        /// <returns></returns>
        [HttpPost]
        public object querySourceHisData(HistoryPowerParamesDto historyPowerParamesDto)
        {
            var resultJson = new MonitorPowerInfoServices().querySourceHisData(historyPowerParamesDto);
            return resultJson;
        }

        /// <summary>
        /// 热源预测热负荷
        /// </summary>
        /// <param name="sourceId">热源Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryForecastHeatLoad(int sourceId)
        {
            string resultJson = new MonitorPowerInfoServices().queryForecastHeatLoad(sourceId);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 热源实际热负荷
        /// </summary>
        /// <param name="sourceId">热源Id</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryActualHeatLoad(int sourceId, DateTime starttime, DateTime endtime)
        {
            string resultJson = new MonitorPowerInfoServices().queryActualHeatLoad(sourceId, starttime, endtime);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 天气预测相关数据(每天)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="weatherType"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryWeatherDate(DateTime starttime, DateTime endtime, int weatherType)
        {
            string resultJson = new MonitorPowerInfoServices().queryWeatherDate(starttime, endtime, weatherType);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }
        /// <summary>
        /// 天气预测相关数据(每小时)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryWeatherHourDate(DateTime starttime, DateTime endtime, int type)
        {
            string resultJson = new MonitorPowerInfoServices().queryWeatherHourDate(starttime, endtime, type);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 所有站点或热源的实时数据
        /// </summary>
        /// <param name="sortName">排序名称</param>
        /// <param name="sortType">排序类型</param>
        /// <param name="tagNames">检测点串</param>
        /// <param name="type">类型（参数1为换热站，2为热源）</param>
        /// <returns></returns>
        [HttpPost]
        public object SelShowPower(string sortName, string sortType, string[] tagNames, int type)
        {
            string resultJson = new MonitorPowerInfoServices().SelShowPower(sortName,sortType, tagNames,type);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 预测热负荷（24小时）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryForecast24HourHeatLoad(int sourceId)
        {
            var resultJson = new MonitorPowerInfoServices().queryForecast24HourHeatLoad("huaxia",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"),DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"), null, sourceId);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 全网平衡完成率
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryWholeNetworkBalance()
        {
            var resultJson = new MonitorPowerInfoServices().queryWholeNetworkBalance();
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 客服板块接口(华夏用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryComplaintTypeList(string type)
        {
            var resultJson = new MonitorPowerInfoServices().queryComplaintTypeList(type,1);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 收费率趋势
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryChargeRateLine()
        {
            var resultJson = new MonitorPowerInfoServices().queryChargeRateLine();
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 近七天投诉工单数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object querySevenDayComlaintRate()
        {
            var resultJson = new MonitorPowerInfoServices().querySevenDayComlaintRate();
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 计费板块接口(华夏用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryChargeTypeList(string type)
        {
            var resultJson = new MonitorPowerInfoServices().queryComplaintTypeList(type,2);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 查询供暖季供热时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryHeatingSeasonDate()
        {
            var resultJson = new MonitorPowerInfoServices().queryHeatingSeasonDate();
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 客服板块接口(服务商用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int updateComplaintType(List<CustomerSeverice> customerSeverice)
        {
            MonitorPowerInfoServices monitorPowerInfoServices = new MonitorPowerInfoServices();
            return monitorPowerInfoServices.updateComplaintType(customerSeverice);
        }

        /// <summary>
        /// 收费板块接口(服务商用)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int updateCharge(List<ChargeSeverice> chargeSeverice)
        {
            MonitorPowerInfoServices monitorPowerInfoServices = new MonitorPowerInfoServices();
            return monitorPowerInfoServices.updateCharge(chargeSeverice);
        }

        /// <summary>
        /// 获取热源水利平衡信息
        /// </summary>
        /// <param name="id">-1为全部热源信息,如获取某热源信息直接传id编号</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object querySlphHeatplantCalcHistory(int id)
        {
            MonitorPowerInfoServices monitorPowerInfoServices = new MonitorPowerInfoServices();
            return monitorPowerInfoServices.querySlphHeatplantCalcHistory(id);
        }
    }
}