using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Models.ViewModel.Production;
using THMS.Core.API.Service.Common;
using THMS.Core.API.Service.UniformedServices.XlinkSystem;

namespace THMS.Core.API.Controllers.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 生产调度控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "XlinkSystem")]
    [ApiController]
    [EnableCors("any")]
    public class XlinkProductionController : ControllerBase
    {
        XlinkProductionService service = new XlinkProductionService();
        CommonService commonService = new CommonService();

        /// <summary>
        /// 获取前24小时的天气预报
        /// </summary>
        /// <returns></returns>
       [HttpGet]
        public dynamic GetFront24HourWeather() => service.GetFront24HourWeather();

        /// <summary>
        /// 获取任意时间段历史天气
        /// </summary>
        /// <param name="stratdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
       [HttpGet]
        public dynamic GetHourHistroyWeather(DateTime stratdate, DateTime enddate) => service.GetHourHistroyWeather(stratdate, enddate);






        /// <summary>
        ///  所有站点或热源的实时数据(生产调度)
        /// </summary>
        /// <param name="parmas"></param>
        /// <returns></returns>
        [HttpPost]
        public object SelProductionTableData(ProductionTableParmas parmas)
        {
            object resultJson = service.SelShowPower(parmas.sortName, parmas.sortType, parmas.tagNames, parmas.type, parmas.organIds, parmas.pageIndex, parmas.pageSize);
            //var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJson;
        }

        /// <summary>
        /// 站点排行数据表(生产调度)
        /// </summary>
        /// <param name="powerid">热源id，默认全部为-1</param>
        /// <param name="organizationid">公司id串，默认全部为-1</param>
        /// <param name="tagname">检测点名称</param>
        /// <param name="ordertype">排序字段</param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetStationRank(int powerid, string organizationid, string tagname, string ordertype)
        {
            var result = service.GetStationRank(powerid, organizationid, tagname, ordertype);
            return result;
        }

        /// <summary>
        /// 换热站基础信息（生产调度）
        /// </summary>
        /// <param name="vpnuserId">换热站id，默认全部为-1</param>
        /// <param name="organizationid">组织结构id，默认全部为-1</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelStationMessage(int vpnuserId,string organizationid)
        {
            var result = service.SelStationMessage(vpnuserId ,organizationid);
            return result;
        }

        /// <summary>
        /// 站点通讯状态及站点类型饼图（生产调度）
        /// </summary>
        /// <param name="vpnuserId">换热站Id</param>
        /// <param name="powerid">热源id</param>
        /// <param name="organizationid">公司id串</param>
        /// <returns></returns>
        [HttpGet]
        public object SelSatationTypeState(int vpnuserId, int powerid, string organizationid)
        {
            var result = service.SelSatationTypeState(vpnuserId, powerid, organizationid);
            return result;
        }

        /// <summary>
        /// 返回热源树
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetPowerTrees()
        {
            var result = commonService.GetPowerTrees();
            return result;
        }
        /// <summary>
        /// 返回换热站树
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetStationTrees()
        {
            var result = commonService.GetStationTrees();
            return result;
        }
        /// <summary>
        /// 返回公司组织结构树
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetOrganizationTree()
        {
            var result = commonService.GetOrganizationTree();
            return result;
        }

        /// <summary>
        /// 室温温度区间分布
        /// </summary>
        /// <param name="serchType">传参类型：1公司、2热源、3换热站、默认为-1</param>
        /// <param name="serchName">名称，serchType为-1时可以传空</param>
        /// <param name="xlinkId">xlink中的Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public dynamic SelRoomAnalysis(int serchType, string serchName, int xlinkId)
        {
            var result = service.RoomAnalysis(serchType, serchName, xlinkId);
            return result;
        }

        /// <summary>
        /// 热网能耗数据展示
        /// </summary>
        /// <param name="energyType">参数1为水，2为电，3为热</param>
        /// <param name="stationType">默认为-1全部,1：普通住宅 2：节能建筑 10：工业区</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelEnergyProductionList(int energyType, int stationType)
        {
            var result = service.SelEnergyProductionList(energyType, stationType);
            return result;
        }


        /// <summary>
        /// 预测/实际热负荷（页面用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryRealForecastHeat(string type, string starttime, string endtime)
        {
            object resultJsonArray;
            try
            {
                var resultJson = new MonitorPowerInfoServices().queryForecast24HourHeatLoad("xLink", starttime, endtime, type, 0);
                 resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            }
            catch (Exception e)
            {

                return null;
            }
        
           
            return resultJsonArray;
        }

        /// <summary>
        /// 预测/实际热量对比（页面用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryForecastRealHeat(int sourceId, string starttime, string endtime)
        {
            var resultJson = new MonitorPowerInfoServices().queryForecastRealHeat(starttime, endtime, sourceId);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 查询换热站数量和供热面积（页面用）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryPowerCountAndArea(int id)
        {
            var resultJson = new MonitorPowerInfoServices().queryPowerCountAndArea(id);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 查询历史瞬时流量热量（页面）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryPowerHeatAndFlow(int id, string starttime, string endtime)
        {
            var resultJson = new MonitorPowerInfoServices().queryPowerHeatAndFlow(id, starttime, endtime);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 室温历史数据检修
        /// </summary>
        /// <param name="serchName">公司名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageSize">分页条数</param>
        /// <param name="pageIndex">当前页标</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelIndoorHistoryList(string serchName, DateTime startTime, DateTime endTime, int pageSize, int pageIndex)
        {
            return service.SelIndoorHistoryList(serchName, startTime, endTime, pageSize, pageIndex);
        }

        /// <summary>
        /// 查询标准参量表字段名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryFirstSecondTagname()
        {
            var resultJson = new MonitorPowerInfoServices().queryFirstSecondTagname();
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 热源饼图/表格
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        [HttpGet]
        public object querySourceRealData(int sourceId)
        {
            string resultJson = new MonitorPowerInfoServices().querySourceRealData(sourceId, "tianshi");
            var resultJsonData = JsonConvert.DeserializeObject(resultJson);
            return resultJsonData;
        }

        /// <summary>
        /// 保修与测温信息展示
        /// </summary>
        /// <param name="serchName">公司名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页标</param>
        /// <param name="type">参数1为保修信息、2为测温信息、-1为全部信息</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelRepairinforList(string serchName, DateTime startTime, DateTime endTime, int pageSize, int pageIndex, int type)
        {
            return service.SelRepairinforList(serchName, startTime, endTime, pageSize, pageIndex,type);
        }

        /// <summary>
        ///  公司失调度与控制目标柱状图展示
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelOrganScheduOrControlList()
        {
            return service.SelOrganScheduOrControlList();
        }

        /// <summary>
        /// 综合评价雷达图
        /// </summary>
        /// <param name="orgId">-1为全部公司页面</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelSynthesize(int orgId)
        {
            return service.SelSynthesize(orgId);
        }

        /// <summary>
        /// 预测单耗与实际单耗对比
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryPowerForecastTarget(int id)
        {
            var resultJson = new MonitorPowerInfoServices().queryPowerForecastTarget(id);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 三天水电热对比图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryThreeDayWaterPowerHeat(string organizationId,int type, int stationType)
        {
            var resultJson = service.queryThreeDayWaterPowerHeat(organizationId,type,stationType);
            return resultJson;
        }

        /// <summary>
        /// 三天控制目标/失调度
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryThreeDayScheduDay(int organizationId)
        {
            return service.queryThreeDayScheduDay(organizationId);
        }

        /// <summary>
        /// 模糊查询换热站名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryStationName(string queryName)
        {
            return service.queryStationName(queryName);
        }

        /// <summary>
        /// 查询站下视频监控点编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object queryCameraCode(int organizationId)
        {
            return service.queryCameraCode(organizationId);
        }

        /// <summary>
        /// 匹配站id和站编码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public int updateParamsId(int vpnUserId,string code)
        {
            return service.updateParamsId(vpnUserId,code);
        }

        /// <summary>
        /// 查询热源总数量及分类数量
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public SourceTypeNumberResponse sourceTypeNumberResponse()
        {
            return service.sourceTypeNumberResponse();
        }

        /// <summary>
        /// 查询热源瞬时热量饼图信息
        /// </summary>
        /// <param name="heatType">参数0为集中供热，1为区域供热</param>
        /// <param name="paramType">参数1为获取瞬时热量，2为瞬时流量</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object sourceHeatType(int heatType, int paramType)
        {
            return JsonConvert.DeserializeObject(service.sourceHeatType(heatType,paramType));
        }

        /// <summary>
        /// 全部热网管径信息柱状图展示
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object pipeNetWorkCharts(string startTime,string endTime,int type)
        {
            return service.pipeNetWorkCharts(startTime, endTime,type);
        }
        /// <summary>
        /// 全部热网信息
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object heatNetInfo() {
            return service.heatNetInfo();
        }

        /// <summary>
        /// 热网管径信息
        /// </summary>
        /// <param name="id">热网Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public PipeNetWorkInfoResponse pipeNetWorkInfo(int id)
        {
            return service.pipeNetWorkInfo(id);
        }
    }
}