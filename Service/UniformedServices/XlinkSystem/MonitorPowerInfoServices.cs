using ApiModel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Models.ViewModel;
using THMS.Core.API.Service.DbContext;
using static THMS.Core.API.Service.Monitor.ShowMonitorService;

namespace THMS.Core.API.Service.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 热源监测数据信息应用
    /// </summary>
    public class MonitorPowerInfoServices : DbContextSqlSugar
    {
        /// <summary>
        /// 热源检测点指标列表
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public string querySourceCheckpointList(int sourceId)
        {
            var monitorPointDtos = Db.Queryable<ValueDesc>().Where(s => s.VpnUser_id == sourceId && s.NarrayNo == 0).Select(s => new MonitorPointDto { key = s.TagName, type = s.Unit, name = s.AiDesc }).ToList();
            string resultJson = JsonConvert.SerializeObject(monitorPointDtos);
            return resultJson;
        }

        /// <summary>
        /// 热源实时监测数据
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="signo"></param>
        /// <returns></returns>
        public string querySourceRealData(int sourceId,string signo)
        {
            StringBuilder sourceData = new StringBuilder();
            var t = Db.Queryable<VpnUser, PowerInfo>((s, p) => new object[] { JoinType.Inner, s.Id == p.VpnUser_id }).WhereIF(signo == "huaxia", s => s.Id == sourceId).WhereIF(signo == "tianshi", s => s.StationStandard == 99 && s.Id >2).Select(s => new SourceDataResult { name = s.StationName, VpnUser_id = s.Id }).ToList();
            if (t.Count > 1)
                sourceData.Append("[");
            foreach (var item in t)
            {
                sourceData.Append("{");
                string name = "\"name\":\"" + item.name + "\",";
                sourceData.Append("\"sourceId\":" + item.VpnUser_id + ",");
                sourceData.Append(name);
                sourceId = item.VpnUser_id;
                var monitorPointDtos = Db.Queryable<ValueDesc, PowerInfo>((s, p) => new object[] { JoinType.Left, s.VpnUser_id == p.VpnUser_id }).Where(s => s.NarrayNo == 0).WhereIF(sourceId > 0, s => s.VpnUser_id == sourceId).Select(s => new SourceDataResult { key = s.TagName, value = s.RealValue }).ToList();
                foreach (var team in monitorPointDtos)
                {
                    string data = "";
                    if (team.key.Contains("TIMESTAMP") || team.key.Contains("TIMECHECK"))
                        data = "\"" + team.key + "\":\"" + team.value + "\"";
                    else
                        data = "\"" + team.key + "\":" + team.value + "";
                    sourceData.Append(data);
                    sourceData.Append(",");
                }
                sourceData.Remove(sourceData.Length - 1, 1);
                sourceData.Append("},");
            }
            if (sourceData.Length > 0)
                sourceData.Remove(sourceData.Length - 1, 1);
            if (t.Count > 1)
                sourceData.Append("]");
            string resultJson = sourceData.ToString();
            return resultJson;
        }

        ///// <summary>
        ///// 热源检测点指标列表
        ///// </summary>
        ///// <param name="sourceId"></param>
        ///// <param name="starttime"></param>
        ///// <param name="endtime"></param>
        ///// <param name="keylist"></param>
        ///// <returns></returns>
        //public string querySourceHisData(int sourceId , DateTime starttime, DateTime endtime, string[] keylist)
        //{
        //    var monitorPointDtos = Db.Queryable<StandardParameter>().In(s => s.TagName , keylist).Select(s => new SourceDataResult {  value = s.AiValue }).ToList();
        //    string resultJson = JsonConvert.SerializeObject(monitorPointDtos);
        //    List<string> list = new List<string>();
        //    foreach (var item in monitorPointDtos)
        //    {
        //        list.Add(item.value);
        //    }
        //    string[] aivalue = list.ToArray();
        //    //var SourceDataResult = Db.Queryable<PowerHistoryFirstData>().In(s => s.TagName, keylist).Select(s => new SourceDataResult { value = s.AiValue }).ToList();
        //    return resultJson;
        //}

        /// <summary>
        /// 热源历史监测数据
        /// </summary>
        /// <param name="historyPowerParamesDto"></param>
        /// <returns></returns>
        public object querySourceHisData(HistoryPowerParamesDto historyPowerParamesDto)
        {
            var paramesList = Db.Queryable<ValueDesc>().In(s => s.TagName, historyPowerParamesDto.keyList).Where(s => s.VpnUser_id == 56).ToList();
            StringBuilder aiValues = new StringBuilder();
            foreach (var item in paramesList)
            {
                aiValues.Append($@"{item.AiValue} AS {item.TagName},");
            }
            string sql = $@"SELECT {aiValues} VpnUser_id,Dhour FROM PowerHistoryFirstDataHour  WHERE VpnUser_id={ historyPowerParamesDto.sourceId} AND Dhour BETWEEN '{ historyPowerParamesDto.startTime}' AND '{ historyPowerParamesDto.endTime}' ORDER BY Dhour";
            var historyFirstList = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            var jsonresult = JsonConvert.SerializeObject(historyFirstList);
            var DynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonresult);
            List<VpnUser> vpnUserList = Db.Queryable<VpnUser>().Where(s => s.IsValid == true).ToList();
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            keyValues.Add("sourceId", historyPowerParamesDto.sourceId);
            keyValues.Add("name", vpnUserList.Where(s => s.Id == historyPowerParamesDto.sourceId).First().StationName);
            keyValues.Add("datetime", historyFirstList.Select(s => s.Dhour).ToList());
            foreach (var flag in historyPowerParamesDto.keyList)
            {
                List<string> dec = new List<string>();
                foreach (var json in DynamicObject)
                {
                    var result = Convert.ToDecimal(json[flag]).ToString("#0.00");
                    dec.Add(result);
                }
                keyValues.Add(flag, dec);
            }
            var keyValuesJson = JsonConvert.SerializeObject(keyValues);
            var list = JsonConvert.DeserializeObject(keyValuesJson);
            return list;
        }

        /// <summary>
        /// 热源预测热负荷
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public string queryForecastHeatLoad(int sourceId)
        {
            List<PowerForecastHourReal> forecast = Db.Queryable<PowerForecastHourReal>().Where(s => s.VpnUser_id == sourceId).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("datetime", forecast.Select(s => s.ForecastDate).ToList());
            dic.Add("forecast_load", forecast.Select(s => s.ForecastUseHeat).ToList());
            dic.Add("forecast_temperature", forecast.Select(s => s.ForecastOutdoorTemp).ToList());
            string json = JsonConvert.SerializeObject(dic);
            return json;
        }

        /// <summary>
        /// 热源实际热负荷
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string queryActualHeatLoad(int sourceId, DateTime starttime, DateTime endtime)
        {
            List<PowerForecastHistoryHourData> forecast = Db.Queryable<PowerForecastHistoryHourData>().Where(s => s.VpnUser_id == sourceId && SqlFunc.Between(s.ForecastDate, starttime, endtime)).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("datetime", forecast.Select(s => s.ForecastDate).ToList());
            dic.Add("forecast_load", forecast.Select(s => s.RealUseHeat).ToList());
            dic.Add("forecast_temperature", forecast.Select(s => s.RealOutdoorTemp).ToList());
            string json = JsonConvert.SerializeObject(dic);
            return json;
        }

        /// <summary>
        /// 天气预测相关数据(每天)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="weatherType"></param>
        /// <returns></returns>
        public string queryWeatherDate(DateTime starttime, DateTime endtime, int weatherType)
        {
            var weatherForecasts = Db.Queryable<WeatherForecast>().Where(s => SqlFunc.Between(s.ForecastTime, starttime, endtime)).WhereIF(weatherType == 1, s => s.WeatherType == 1).WhereIF(weatherType == 2, s => s.WeatherType == 2).WhereIF(weatherType == -1, "1=1").OrderBy(s => s.ForecastTime).Select(s => new { s.HeatTemperature, s.AvgTemperature, s.LowTemperature, s.Wind, s.WindVelocity, s.WindDirection, s.WeatherConditions, s.SnowStatus, s.HumiDity, s.ForecastTime, s.WeatherType }).ToList();
            string json = JsonConvert.SerializeObject(weatherForecasts);
            return json;
        }
        /// <summary>
        /// 天气预测相关数据(每小时)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string queryWeatherHourDate(DateTime starttime, DateTime endtime, int type)
        {
            var realTemperatureList = Db.Queryable<RealTemperature>().Where(s => SqlFunc.Between(s.NcapTime, starttime, endtime)).WhereIF(type == 1, s => s.CollectName == ConfigAppsetting.WeatherReal).WhereIF(type == 2, s => s.CollectName == ConfigAppsetting.WeatherForecast).WhereIF(type == -1, "1=1").OrderBy(s => s.NcapTime).Select(s => new { s.ForecastHourAvgTemperature, s.Wind, s.WindVelocity, s.WindDirection, s.WeatherConditions, s.SnowStatus, s.HumiDity, s.NcapTime, s.CollectName }).ToList();
            string json = JsonConvert.SerializeObject(realTemperatureList);
            return json;
        }

        /// <summary>
        /// 所有站点或热源的实时数据
        /// </summary>
        /// <param name="sortName">排序名称</param>
        /// <param name="sortType">排序类型</param>
        /// <param name="tagNames">检测点串</param>
        /// <param name="type">类型（参数1为换热站，2为热源）</param>
        /// <returns></returns>
        public string SelShowPower(string sortName, string sortType, string[] tagNames, int type)
        {
            // 获取热源信息及实时参数数据
            var dataList = Db.Queryable<StationBranch, VpnUser, ValueDesc>((s, v, d) => new object[] { JoinType.Left, s.VpnUser_id == v.Id, JoinType.Left, v.Id == d.VpnUser_id && s.StationBranchArrayNumber == d.NarrayNo }).WhereIF(type == 1, (s, v, d) => v.StationStandard < 98).WhereIF(type == 2, (s, v, d) => v.StationStandard == 99).Where((s, v, d) => v.IsValid == true).OrderByIF(!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sortType) && sortName != "Area", $@"case when d.TagName='{sortName}' then 1 ELSE 1 END,1 {sortType}").OrderByIF(!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sortType) && sortName == "Area", $@"{sortName} {sortType}").Select((s, v, d) => new NewDemo { Id = v.Id, StationBranchArrayNumber = s.StationBranchArrayNumber, StationBranchName = s.StationBranchName, StationName = v.StationName, Area = v.StationHotArea, TagName = d.TagName, AiDesc = d.AiDesc, Unit = d.Unit, RealValue = d.RealValue }).ToList();
            if (type == 1)
                type = 3;
            var tagNameList = Db.Queryable<ValueDesc>().In(s => s.TagName, tagNames).Where(s => s.VpnUser_id == type).ToList();

            //获取热源信息
            var powerList = (from c in dataList group c by new { c.Id, c.StationBranchArrayNumber, c.StationName, c.StationBranchName, c.Area } into v select new NewDemo() { Id = v.Key.Id, StationBranchArrayNumber = v.Key.StationBranchArrayNumber, StationBranchName = v.Key.StationBranchName, StationName = v.Key.StationName, Area = v.Key.Area }).ToList();
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            foreach (var power in powerList)
            {
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                keyValues.Add("VpnuserId", power.Id);
                keyValues.Add("StationBranchArrayNumber", power.StationBranchArrayNumber);
                keyValues.Add("StationName", power.StationName);
                keyValues.Add("StationBranchName", power.StationBranchName);
                keyValues.Add("Area", power.Area);

                foreach (var item in tagNameList.Where(s => s.NarrayNo == power.StationBranchArrayNumber).ToList())
                {
                    var body = dataList.Where(s => s.Id == power.Id && s.StationBranchArrayNumber == power.StationBranchArrayNumber && s.TagName == item.TagName).First();
                    keyValues.Add(item.TagName, body.RealValue);
                }
                listResult.Add(keyValues);
            }
            string json = JsonConvert.SerializeObject(listResult);
            return json;
        }

        /// <summary>
        /// 预测热负荷（24小时）
        /// </summary>
        /// <returns></returns>
        public string queryForecast24HourHeatLoad(string signo, string starttime, string endtime, string type, int sourceId)
        {
            string json;
            StringBuilder data = new StringBuilder();
            try
            {
                if (signo == "huaxia")
                {
                    StringBuilder datetime = new StringBuilder();
                    StringBuilder forecast_load = new StringBuilder();
                    StringBuilder heat_consumption = new StringBuilder();
                    StringBuilder forecastOutdoorTemp = new StringBuilder();
                    StringBuilder realOutdoorTemp = new StringBuilder();
                    datetime.Append("["); forecast_load.Append("["); heat_consumption.Append("["); forecastOutdoorTemp.Append("["); realOutdoorTemp.Append("[");
                    string huaxiaTime1 = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd 08:00:00");
                    //string huaxiaTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00");
                    string huaxiaTime = DateTime.Now.ToString("yyyy-MM-dd 08:00:00");
                    string huaxiaTime2 = DateTime.Now.AddHours(12).ToString("yyyy-MM-dd HH:00:00");
                    string sql;
                    if (sourceId > 0)
                    {
                        sql = $@"select p.ForecastDate ,sum( p.ForecastUseHeat ) as ForecastHeatTarget, isnull(sum(p.RealUseHeat),0) as RealHeatTarget,
                                avg( p.ForecastOutdoorTemp ) as ForecastOutdoorTemp,avg( p.RealOutdoorTemp ) as RealOutdoorTemp from PowerForecastHistoryHourData p 
							    where p.ForecastDate between '" + huaxiaTime1 + "' and '" + huaxiaTime + "' and VpnUser_id = " + sourceId + " group by  p.ForecastDate";
                        var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        sql = $@"select p.ForecastDate ,sum( p.ForecastUseHeat ) as ForecastHeatTarget,  isnull(sum(p.RealUseHeat),0) as RealHeatTarget,
                                avg( p.ForecastOutdoorTemp ) as ForecastOutdoorTemp,avg( p.RealOutdoorTemp ) as RealOutdoorTemp from PowerForecastHourReal p 
							    where p.ForecastDate > '" + huaxiaTime1 + "' and p.ForecastDate <= '" + huaxiaTime + "' and VpnUser_id = " + sourceId + " group by  p.ForecastDate";
                        var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        foreach (var item in resultlist)
                        {
                            datetime.Append("\"" + item.ForecastDate + "\",");
                            forecast_load.Append("\"" + item.ForecastHeatTarget + "\",");
                            heat_consumption.Append("\"" + item.RealHeatTarget + "\",");
                            forecastOutdoorTemp.Append("\"" + item.ForecastOutdoorTemp + "\",");
                            realOutdoorTemp.Append("\"" + item.RealOutdoorTemp + "\",");
                        }
                        foreach (var item in list)
                        {
                            datetime.Append("\"" + item.ForecastDate + "\",");
                            forecast_load.Append("\"" + item.ForecastHeatTarget + "\",");
                            heat_consumption.Append("\"" + item.RealHeatTarget + "\",");
                            forecastOutdoorTemp.Append("\"" + item.ForecastOutdoorTemp + "\",");
                            realOutdoorTemp.Append("\"" + item.RealOutdoorTemp + "\",");
                        }
                        datetime.Remove(datetime.Length - 1, 1);
                        forecast_load.Remove(forecast_load.Length - 1, 1);
                        heat_consumption.Remove(heat_consumption.Length - 1, 1);
                        realOutdoorTemp.Remove(realOutdoorTemp.Length - 1, 1);
                        forecastOutdoorTemp.Remove(forecastOutdoorTemp.Length - 1, 1);
                        datetime.Append("]"); forecast_load.Append("]"); heat_consumption.Append("]"); forecastOutdoorTemp.Append("]"); realOutdoorTemp.Append("]");
                        data.Append("{\"datetime\":" + datetime + ",\"forecast_load\":" + forecast_load + ",\"heat_consumption\":" + heat_consumption + ",\"forecastOutdoorTemp\":" + forecastOutdoorTemp + ",\"realOutdoorTemp\":" + realOutdoorTemp + "}");
                        json = data.ToString();
                    }
                    else
                    {
                        sql = $@"select p.ForecastDate ,sum( p.ForecastUseHeat ) as ForecastHeatTarget,  isnull(sum(p.RealUseHeat),0) as RealHeatTarget ,
                                    avg( p.ForecastOutdoorTemp ) as ForecastOutdoorTemp,avg( p.RealOutdoorTemp ) as RealOutdoorTemp from PowerForecastHistoryHourData p 
							        where p.ForecastDate between '" + huaxiaTime1 + "' and '" + huaxiaTime + "' group by  p.ForecastDate";
                        var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        sql = $@"select p.ForecastDate ,sum( p.ForecastUseHeat ) as ForecastHeatTarget,  isnull(sum(p.RealUseHeat),0) as RealHeatTarget ,
                                    avg( p.ForecastOutdoorTemp ) as ForecastOutdoorTemp,avg( p.RealOutdoorTemp ) as RealOutdoorTemp from PowerForecastHourReal p 
							        where p.ForecastDate > '" + huaxiaTime1 + "' and p.ForecastDate <= '" + huaxiaTime + "' group by  p.ForecastDate";
                        var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        foreach (var item in resultlist)
                        {
                            datetime.Append("\"" + item.ForecastDate + "\",");
                            forecast_load.Append("\"" + item.ForecastHeatTarget + "\",");
                            heat_consumption.Append("\"" + item.RealHeatTarget + "\",");
                            forecastOutdoorTemp.Append("\"" + item.ForecastOutdoorTemp + "\",");
                            realOutdoorTemp.Append("\"" + item.RealOutdoorTemp + "\",");
                        }
                        foreach (var item in list)
                        {
                            datetime.Append("\"" + item.ForecastDate + "\",");
                            forecast_load.Append("\"" + item.ForecastHeatTarget + "\",");
                            heat_consumption.Append("\"" + item.RealHeatTarget + "\",");
                            forecastOutdoorTemp.Append("\"" + item.ForecastOutdoorTemp + "\",");
                            realOutdoorTemp.Append("\"" + item.RealOutdoorTemp + "\",");
                        }
                        datetime.Remove(datetime.Length - 1, 1);
                        forecast_load.Remove(forecast_load.Length - 1, 1);
                        heat_consumption.Remove(heat_consumption.Length - 1, 1);
                        realOutdoorTemp.Remove(realOutdoorTemp.Length - 1, 1);
                        forecastOutdoorTemp.Remove(forecastOutdoorTemp.Length - 1, 1);
                        datetime.Append("]"); forecast_load.Append("]"); heat_consumption.Append("]"); forecastOutdoorTemp.Append("]"); realOutdoorTemp.Append("]");
                        data.Append("{\"datetime\":" + datetime + ",\"forecast_load\":" + forecast_load + ",\"heat_consumption\":" + heat_consumption + ",\"forecastOutdoorTemp\":" + forecastOutdoorTemp + ",\"realOutdoorTemp\":" + realOutdoorTemp + "}");
                        json = data.ToString();
                    }
                }
                else if (signo == "xLink")
                {
                    data.Append("[");
                    if (type == "all")
                    {
                        string sql = $@"select ForecastDate ,ISNULL(sum( ForecastUseHeat ),0) as ForecastHeatTarget,ISNULL(sum(ForecastFlow),0) as ForecastFlow,ISNULL(sum(RealFlow),0) as RealFlow, isnull(sum(RealUseHeat),0) as RealHeatTarget,avg(ISNULL(ForecastOutdoorTemp,0)) as RealOutdoorTemp from 
                                PowerForecastHistoryHourData where ForecastDate between '" + starttime + "' and '" + endtime + "'group by  ForecastDate ";
                        var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        sql = $@"select ForecastDate ,ISNULL(sum( ForecastUseHeat ),0) as ForecastHeatTarget,ISNULL(sum(ForecastFlow),0) as ForecastFlow,ISNULL(sum(RealFlow),0) as RealFlow, isnull(sum(RealUseHeat),0) as RealHeatTarget,avg(ISNULL(ForecastOutdoorTemp,0)) as RealOutdoorTemp from 
                                PowerForecastHourReal where ForecastDate between '" + starttime + "' and '" + endtime + "'group by  ForecastDate ";
                        var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        //resultlist.Add(list);
                        foreach (var item in resultlist)
                        {
                            data.Append("{\"ForecastDate\": \"" + item.ForecastDate + "\",\"ForecastHeatTarget\": " + item.ForecastHeatTarget + ",\"ForecastFlow\": " + item.ForecastFlow + ",\"RealFlow\": " + item.RealFlow + ",\"RealHeatTarget\": " + item.RealHeatTarget + ",\"RealOutdoorTemp\": " + item.RealOutdoorTemp + "},");
                        }
                        foreach (var item in list)
                        {
                            data.Append("{\"ForecastDate\": \"" + item.ForecastDate + "\",\"ForecastHeatTarget\": " + item.ForecastHeatTarget + ",\"ForecastFlow\": " + item.ForecastFlow + ",\"RealFlow\": " + item.RealFlow + ",\"RealHeatTarget\": " + item.RealHeatTarget + ",\"RealOutdoorTemp\": " + item.RealOutdoorTemp + "},");
                        }
                        data.Remove(data.Length - 1, 1);
                        data.Append("]");
                        json = data.ToString();
                        //json = JsonConvert.SerializeObject(resultlist);
                    }
                    else if (type == "VpnUser_id")
                    {
                        string sql = $@"select " + type + ", ForecastDate ,ISNULL(sum( ForecastUseHeat ),0) as ForecastHeatTarget,ISNULL(sum(ForecastFlow),0) as ForecastFlow,ISNULL(sum(RealFlow),0) as RealFlow, isnull(sum(RealUseHeat),0) as RealHeatTarget ,ForecastOutdoorTemp as RealOutdoorTemp from PowerForecastHistoryHourData where ForecastDate between '" + starttime + "' and '" + endtime + "'group by  ForecastDate," + type + ",ForecastOutdoorTemp";
                        var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        sql = $@"select " + type + ", ForecastDate ,ISNULL(sum( ForecastUseHeat ),0) as ForecastHeatTarget,ISNULL(sum(ForecastFlow),0) as ForecastFlow,ISNULL(sum(RealFlow),0) as RealFlow, isnull(sum(RealUseHeat),0) as RealHeatTarget ,ForecastOutdoorTemp as RealOutdoorTemp from PowerForecastHourReal where ForecastDate between '" + starttime + "' and '" + endtime + "'group by  ForecastDate," + type + ",ForecastOutdoorTemp";
                        var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                        //resultlist.Add(list);
                        foreach (var item in resultlist)
                        {
                            data.Append("{\"VpnUser_id\": " + item.VpnUser_id + ",\"ForecastDate\": \"" + item.ForecastDate + "\",\"ForecastHeatTarget\": " + item.ForecastHeatTarget + ",\"ForecastFlow\": " + item.ForecastFlow + ",\"RealFlow\": " + item.RealFlow + ",\"RealHeatTarget\": " + item.RealHeatTarget + ",\"RealOutdoorTemp\": " + item.RealOutdoorTemp + "},");
                        }
                        foreach (var item in list)
                        {
                            data.Append("{\"VpnUser_id\": " + item.VpnUser_id + ",\"ForecastDate\": \"" + item.ForecastDate + "\",\"ForecastHeatTarget\": " + item.ForecastHeatTarget + ",\"ForecastFlow\": " + item.ForecastFlow + ",\"RealFlow\": " + item.RealFlow + ",\"RealHeatTarget\": " + item.RealHeatTarget + ",\"RealOutdoorTemp\": " + item.RealOutdoorTemp + "},");
                        }
                        data.Remove(data.Length - 1, 1);
                        data.Append("]");
                        json = data.ToString();
                        //json = JsonConvert.SerializeObject(resultlist);
                    }
                    else json = null;
                }
                else json = null;
            }
            catch (Exception)
            {
                return null;
            }
            return json;

        }

        /// <summary>
        /// 全网平衡完成率
        /// </summary>
        /// <returns></returns>
        public string queryWholeNetworkBalance()
        {
            var weatherForecasts = Db.Queryable<NetBalance>().Select(s => new { heatnetId = s.ComboNetID, heatnetName = s.ComboNetName, stationCount = s.StationNumber, area = s.HeatArea, jfc = s.ImbalancingDegree, netBalancePerComplete = s.NetBalancePerComplete }).ToList();
            string json = JsonConvert.SerializeObject(weatherForecasts);
            return json;
        }

        /// <summary>
        /// 客服板块接口
        /// </summary>
        /// <returns></returns>
        public string queryComplaintTypeList(string type, int signo)
        {
            if (type == "楼栋")
                type = "buildID,communityId,filialeId";
            else if (type == "小区")
                type = "communityId,filialeId";
            else if (type == "分公司")
                type = "filialeId";
            else
                return "{\"Error\":500,\"Message\":\"查询条件错误！\"}";

            string json;
            if (signo == 1)
            {
                string sql = "select " + type + " , ComplaintType , count(ComplaintCount) as ComplaintCount from CustomerSeverice group by " + type + " , ComplaintType";
                var result = Db.Ado.SqlQuery<CustomerSeverice>(sql).ToList();
                json = JsonConvert.SerializeObject(result);
            }
            else
            {
                string sql = "select " + type + " , sum(Quantity) as Quantity , sum(RealQuantity) as RealQuantity ,sum(RealQuantity)/(case sum(Quantity) when 0 then 1 else sum(Quantity) end)*100 as ChargeRate from ChargeSeverice group by " + type + " ";
                var result = Db.Ado.SqlQuery<ChargeSeverice>(sql).ToList();
                json = JsonConvert.SerializeObject(result);
            }
            return json;
        }

        /// <summary>
        /// 近七天投诉工单数
        /// </summary>
        /// <returns></returns>
        public string queryChargeRateLine()
        {
            string sql = $@"select ChargeRate as rate,CONVERT(varchar(7),UpdateTime,120) as dateTime from ChargeSeverice where UpdateTime in 
                            (CONVERT(varchar(100),DATEADD(DAY,-1,'" + DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01") + "'),23),"+
                            "CONVERT(varchar(100),DATEADD(DAY,-1,'" + DateTime.Now.AddMonths(-2).ToString("yyyy-MM-01") + "'),23)," +
                            "CONVERT(varchar(100),DATEADD(DAY,-1,'" + DateTime.Now.AddMonths(-3).ToString("yyyy-MM-01") + "'),23)," +
                            "CONVERT(varchar(100),DATEADD(DAY,-1,'" + DateTime.Now.AddMonths(-4).ToString("yyyy-MM-01") + "'),23)," +
                            "CONVERT(varchar(100),DATEADD(DAY,-1,'" + DateTime.Now.AddMonths(-5).ToString("yyyy-MM-01") + "'),23) ) ";
            var result = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            string json = JsonConvert.SerializeObject(result);
            return json;
        }

        /// <summary>
        /// 近七天投诉工单数
        /// </summary>
        /// <returns></returns>
        public string querySevenDayComlaintRate()
        {
            string sql = $@"select sum(ComplaintCount) as count,CONVERT(varchar(100),CreateTime,23) as dateTime from CustomerSeverice 
                            where CreateTime between CONVERT(varchar(100),DATEADD(DAY,-7,GETDATE()),23) and CONVERT(varchar(100),GETDATE(),23) 
                            group by CONVERT(varchar(100),CreateTime,23) ";
            var result = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            string json = JsonConvert.SerializeObject(result);
            return json;
        }

        /// <summary>
        /// 查询供暖季供热时间
        /// </summary>
        /// <returns></returns>
        public string queryHeatingSeasonDate()
        {
            string sql = $@"select top 1  StartDate , EndDate  from HeatCycle order by id desc ";
            var result = Db.Ado.SqlQuerySingle<dynamic>(sql);
            DateTime StartDate = Convert.ToDateTime(result.StartDate);
            DateTime EndDate = Convert.ToDateTime(result.EndDate);
            int DiffDate = 0;
            if (EndDate >= DateTime.Now)
            {
                DiffDate = DateTime.Now.Subtract(StartDate).Days;
            }
            else
            {
                DiffDate = EndDate.Subtract(StartDate).Days;
            }
           var  list = new List<object>();
            list.Add(new { StartDate, EndDate, DiffDate });
            string json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// <summary>
        /// 客服板块接口(服务商用)
        /// </summary>
        /// <returns></returns>
        public int updateComplaintType(List<CustomerSeverice> customerSeverice)
        {
            StringBuilder insert = new StringBuilder();
            try
            {
                insert.Append("insert into CustomerSeverice (BuildingId,CommunityId,CompanyId,ComplaintType,ComplaintCount,CreateTime) values ");
                foreach (var item in customerSeverice)
                {
                    insert.Append("(" + item.buildID + "," + item.communityId + "," + item.filialeId + ",'" + item.ComplaintType + "'," + item.ComplaintCount + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') ,");
                    if (item.buildID == 0 || item.communityId == 0 || item.filialeId == 0)
                    {
                        return ResultCode.Error;
                    }
                }
                insert.Remove(insert.Length - 1, 1);
                string sql = insert.ToString();
                var t0 = Db.Deleteable<CustomerSeverice>().ExecuteCommand();
                var result = Db.Ado.SqlQuery<CustomerSeverice>(sql);
            }
            catch (Exception)
            {
                return ResultCode.Error;
            }
            return ResultCode.Success;
        }

        /// <summary>
        /// 收费板块接口(服务商用)
        /// </summary>
        /// <returns></returns>
        public int updateCharge(List<ChargeSeverice> chargeSeverice)
        {
            StringBuilder insert = new StringBuilder();
            try
            {
                insert.Append("insert into ChargeSeverice (BuildingId,CommunityId,CompanyId,Quantity,RealQuantity,ChargeRate,CreateTime,UpdateTime) values ");
                foreach (var item in chargeSeverice)
                {
                    insert.Append("(" + item.buildID + "," + item.communityId + "," + item.filialeId + "," + item.Quantity + "," + item.RealQuantity + "," + item.ChargeRate + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + item.UpdateTime + "') ,");
                    if (item.buildID == 0 || item.communityId == 0 || item.filialeId == 0)
                    {
                        return ResultCode.Error;
                    }
                }
                insert.Remove(insert.Length - 1, 1);
                string sql = insert.ToString();
                var t0 = Db.Deleteable<ChargeSeverice>().ExecuteCommand();
                var result = Db.Ado.SqlQuery<ChargeSeverice>(sql);
            }
            catch (Exception)
            {
                return ResultCode.Error;
            }
            return ResultCode.Success;
        }

        /// <summary>
        /// 查询换热站数量和供热面积（页面用）
        /// </summary>
        /// <returns></returns>
        public string queryPowerCountAndArea(int id)
        {
            string json = null;
            StringBuilder list = new StringBuilder();
            list.Append("{");
            if (id == 0)
            {
                string sql = $@"select count(VpnUser_id) as StationNumber , SUM(v.StationHotArea) as TotalArea from Station s inner join VpnUser v on s.VpnUser_id = v.Id where VpnUser_id != 3 ";
                var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                list.Append("\"StationNumber\":" + resultlist[0].StationNumber + ",");
                sql = $@"select SUM(v.StationHotArea) as TotalArea  from  PowerInfo p inner join VpnUser v on p.VpnUser_id = v.Id where VpnUser_id != 2 and v.StationStandard = 99 ";
                resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                list.Append("\"TotalArea\":" + resultlist[0].TotalArea + "}");
                json = list.ToString();
                //json = JsonConvert.SerializeObject(result);
            }
            else
            {
                string sql = $@"select count(VpnUser_id) as StationNumber, SUM(v.StationHotArea) as TotalArea from Station s inner join VpnUser v on s.VpnUser_id = v.Id 
                            where PowerInfo_id = " + id + " group by s.PowerInfo_id";
                var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                list.Append("\"StationNumber\":" + resultlist[0].StationNumber + ",");
                sql = $@"select StationHotArea  from  VpnUser where id = " + id + " ";
                resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                list.Append("\"TotalArea\":" + resultlist[0].StationHotArea + ",\"PowerInfo\":" + id + "}");
                json = list.ToString();
            }
            return json;
        }

        /// <summary>
        /// 查询历史瞬时流量热量（页面）
        /// </summary>
        /// <returns></returns>
        public string queryPowerHeatAndFlow(int id, string starttime, string endtime)
        {
            string json = null;
            if (id > 0)
            {
                string sql = $@"select VpnUser_id,AiValue5 as Flow,AiValue6 as Heat,Dhour from PowerHistoryFirstDataHour where VpnUser_id = " + id + " and Dhour between '" + starttime + "' and '" + endtime + "' ";
                var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                json = JsonConvert.SerializeObject(resultlist);
            }
            return json;
        }

        /// <summary>
        /// 预测/实际热量对比（页面用）
        /// </summary>
        /// <returns></returns>
        public string queryForecastRealHeat(string starttime, string endtime, int sourceId)
        {
            string json;
            StringBuilder data = new StringBuilder();
            try
            {
                if (sourceId > 0)
                {
                    data.Append("[");
                    string sql = $@"select VpnUser_id, ForecastDate ,ISNULL(sum( ForecastUseHeat ),0) as ForecastHeatTarget,ISNULL(sum(ForecastFlow),0) as ForecastFlow,ISNULL(sum(RealFlow),0) as RealFlow, isnull(sum(RealUseHeat),0) as RealHeatTarget ,ForecastOutdoorTemp as RealOutdoorTemp from PowerForecastHistoryHourData where ForecastDate between '" + starttime + "' and '" + endtime + "' and VpnUser_id = " + sourceId + " group by  ForecastDate,VpnUser_id,ForecastOutdoorTemp";
                    var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                    sql = $@"select VpnUser_id, ForecastDate ,ISNULL(sum( ForecastUseHeat ),0) as ForecastHeatTarget,ISNULL(sum(ForecastFlow),0) as ForecastFlow,ISNULL(sum(RealFlow),0) as RealFlow, isnull(sum(RealUseHeat),0) as RealHeatTarget ,ForecastOutdoorTemp as RealOutdoorTemp from PowerForecastHourReal where ForecastDate between '" + starttime + "' and '" + endtime + "' and VpnUser_id = " + sourceId + " group by  ForecastDate,VpnUser_id,ForecastOutdoorTemp";
                    var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                    foreach (var item in resultlist)
                    {
                        data.Append("{\"VpnUser_id\": " + item.VpnUser_id + ",\"ForecastDate\": \"" + item.ForecastDate + "\",\"ForecastHeatTarget\": " + item.ForecastHeatTarget + ",\"ForecastFlow\": " + item.ForecastFlow + ",\"RealFlow\": " + item.RealFlow + ",\"RealHeatTarget\": " + item.RealHeatTarget + ",\"RealOutdoorTemp\": " + item.RealOutdoorTemp + "},");
                    }
                    foreach (var item in list)
                    {
                        data.Append("{\"VpnUser_id\": " + item.VpnUser_id + ",\"ForecastDate\": \"" + item.ForecastDate + "\",\"ForecastHeatTarget\": " + item.ForecastHeatTarget + ",\"ForecastFlow\": " + item.ForecastFlow + ",\"RealFlow\": " + item.RealFlow + ",\"RealHeatTarget\": " + item.RealHeatTarget + ",\"RealOutdoorTemp\": " + item.RealOutdoorTemp + "},");
                    }
                    data.Remove(data.Length - 1, 1);
                    data.Append("]");
                    json = data.ToString();
                }
                else json = null;
            }
            catch (Exception)
            {
                return null;
            }
            return json;
        }

        /// <summary>
        /// 查询单耗
        /// </summary>
        /// <returns></returns>
        public string queryPowerForecastTarget(int id)
        {
            string json = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            StringBuilder datalist = new StringBuilder();
            StringBuilder forecast = new StringBuilder();
            StringBuilder real = new StringBuilder();
            StringBuilder date = new StringBuilder();
            forecast.Append("[");
            real.Append("[");
            date.Append("[");
            if (id == 0)
            {
                string sql = $@"select ISNULL(sum(ForecastHeatTarget),0) as Forecast,ISNULL(sum(RealHeatTarget),0) as Real, ForecastDate as Date 
                        from PowerForecastHistoryHourData where ForecastDate between CONVERT(varchar(100),DATEADD(DAY,-2,GETDATE()),23) and GETDATE() group by ForecastDate";
                var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                sql = $@"select ISNULL(sum(ForecastHeatTarget),0) as Forecast,ISNULL(sum(RealHeatTarget),0) as Real, ForecastDate as Date 
                        from PowerForecastHourReal where  ForecastDate between CONVERT(varchar(100),DATEADD(DAY,-1,GETDATE()),23) and CONVERT(varchar(100),DATEADD(DAY,1,GETDATE()),23) group by ForecastDate";
                var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                foreach (var item in resultlist)
                {
                    forecast.Append(item.Forecast + ",");
                    real.Append(item.Real + ",");
                    date.Append("\"" + item.Date + "\",");
                }
                foreach (var item in list)
                {
                    forecast.Append(item.Forecast + ",");
                    real.Append(item.Real + ",");
                    date.Append("\"" + item.Date + "\",");
                }
                forecast.Remove(forecast.Length - 1, 1);
                real.Remove(real.Length - 1, 1);
                date.Remove(date.Length - 1, 1);
                //dic.Add("Forecast", resultlist.Select(s => s.Forecast).ToList());
                //dic.Add("Real", resultlist.Select(s => s.Real).ToList());
                //dic.Add("Date", resultlist.Select(s => s.Date).ToList());
            }
            else
            {
                string sql = $@"select ISNULL(sum(ForecastHeatTarget),0) as Forecast,ISNULL(sum(RealHeatTarget),0) as Real, ForecastDate as Date from PowerForecastHistoryHourData where ForecastDate between CONVERT(varchar(100),DATEADD(DAY,-2,GETDATE()),23) and GETDATE() and VpnUser_id = " + id + " group by ForecastDate ";
                var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                sql = $@"select ISNULL(sum(ForecastHeatTarget),0) as Forecast,ISNULL(sum(RealHeatTarget),0) as Real, ForecastDate as Date from PowerForecastHourReal where ForecastDate between CONVERT(varchar(100),DATEADD(DAY,-2,GETDATE()),23) and GETDATE() and VpnUser_id = " + id + " group by ForecastDate ";
                var list = Db.Ado.SqlQuery<dynamic>(sql).ToList();
                foreach (var item in resultlist)
                {
                    forecast.Append(item.Forecast + ",");
                    real.Append(item.Real + ",");
                    date.Append("\"" + item.Date + "\",");
                }
                foreach (var item in list)
                {
                    forecast.Append(item.Forecast + ",");
                    real.Append(item.Real + ",");
                    date.Append("\"" + item.Date + "\",");
                }
                forecast.Remove(forecast.Length - 1, 1);
                real.Remove(real.Length - 1, 1);
                date.Remove(date.Length - 1, 1);
                //dic.Add("Forecast", resultlist.Select(s => s.Forecast).ToList());
                //dic.Add("Real", resultlist.Select(s => s.Real).ToList());
                //dic.Add("Date", resultlist.Select(s => s.Date).ToList());
            }
            forecast.Append("]");
            real.Append("]");
            date.Append("]");
            datalist.Append("{\"Forecast\":" + forecast + ",\"Real\":" + real + ",\"Date\":" + date + "}");
            json = datalist.ToString();
            return json;
        }

        /// <summary>
        /// 查询标准参量表字段名
        /// </summary>
        /// <returns></returns>
        public string queryFirstSecondTagname()
        {
            StringBuilder jsonbody = new StringBuilder();
            StringBuilder tem = new StringBuilder();
            StringBuilder press = new StringBuilder();
            StringBuilder heat = new StringBuilder();
            StringBuilder flow = new StringBuilder();
            StringBuilder beng = new StringBuilder();
            StringBuilder otherdata = new StringBuilder();
            jsonbody.Append("{\"全部\":{\"First\":");
            tem.Append("\"温度\":{\"First\":[");
            press.Append("\"压力\":{\"First\":[");
            heat.Append("\"热量\":{\"First\":[");
            flow.Append("\"流量\":{\"First\":[");
            beng.Append("\"泵\":{\"First\":[");
            otherdata.Append("\"其他\":{\"First\":[");
            string json = null;
            string sql = $@"select AiDesc,TagName,Unit from StandardParameter where TagName in ('PRI_TEMP_S','PRI_TEMP_R','PRI_PRESS_S','PRI_PRESS_R','PRI_FLOW_S','HEAT')";
            var resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            foreach (var item in resultlist)
            {
                if (item.TagName.Contains("TEMP"))
                {
                    tem.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    tem.Append("\"TagName\":\"" + item.TagName + "\",");
                    tem.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                if (item.TagName.Contains("PRESS"))
                {
                    press.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    press.Append("\"TagName\":\"" + item.TagName + "\",");
                    press.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                if (item.TagName.Contains("FLOW"))
                {
                    flow.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    flow.Append("\"TagName\":\"" + item.TagName + "\",");
                    flow.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                if (item.TagName.Contains("HEAT"))
                {
                    heat.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    heat.Append("\"TagName\":\"" + item.TagName + "\",");
                    heat.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
            }
            tem.Remove(tem.Length - 1, 1);
            press.Remove(press.Length - 1, 1);
            heat.Remove(heat.Length - 1, 1);
            flow.Remove(flow.Length - 1, 1);
            tem.Append("],\"Second\":[");
            press.Append("],\"Second\":[");
            heat.Append("],\"Second\":[");
            flow.Append("],\"Second\":[");
            beng.Append("],\"Second\":[");
            otherdata.Append("],\"Second\":[");
            json = JsonConvert.SerializeObject(resultlist);
            jsonbody.Append(json);
            jsonbody.Append(",\"Second\":");
            sql = $@"select AiDesc,TagName,Unit from StandardParameter where TagName in ('SEC_TEMP_S1','SEC_TEMP_R1','SEC_PRESS_S1','SEC_PRESS_R1','SEC_FLOW_LOST1','SEC_TOTAL_FLOW_LOST1','PRI_SEC_FILL_FLOW_S1','PRI_SEC_FILL_VOLUME_S1','CYCLE_VVVF_SET_HZ1','CYCLE_VVVF1_FACT_HZ1','CYCLE_VVVF4_FACT_HZ1','CYCLE_VVVF2_FACT_HZ1','CYCLE_VVVF3_FACT_HZ1','CYCLE_VVVF5_FACT_HZ1','CV1_SV1','CV1_U1')";
            resultlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            foreach (var item in resultlist)
            {
                if (item.TagName.Contains("TEMP"))
                {
                    tem.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    tem.Append("\"TagName\":\"" + item.TagName + "\",");
                    tem.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                else if (item.TagName.Contains("PRESS"))
                {
                    press.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    press.Append("\"TagName\":\"" + item.TagName + "\",");
                    press.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                else if (item.TagName.Contains("FLOW") || item.TagName.Contains("FILL"))
                {
                    flow.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    flow.Append("\"TagName\":\"" + item.TagName + "\",");
                    flow.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                else if (item.TagName.Contains("HEAT"))
                {
                    heat.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    heat.Append("\"TagName\":\"" + item.TagName + "\",");
                    heat.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                else if (item.TagName.Contains("CYCLE"))
                {
                    beng.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    beng.Append("\"TagName\":\"" + item.TagName + "\",");
                    beng.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
                else
                {
                    otherdata.Append("{\"AiDesc\":\"" + item.AiDesc + "\",");
                    otherdata.Append("\"TagName\":\"" + item.TagName + "\",");
                    otherdata.Append("\"Unit\":\"" + item.Unit + "\"},");
                }
            }
            tem.Remove(tem.Length - 1, 1);
            press.Remove(press.Length - 1, 1);
            //heat.Remove(heat.Length - 1, 1);
            flow.Remove(flow.Length - 1, 1);
            beng.Remove(beng.Length - 1, 1);
            otherdata.Remove(otherdata.Length - 1, 1);
            tem.Append("]},");
            press.Append("]},");
            heat.Append("]},");
            flow.Append("]},");
            beng.Append("]},");
            otherdata.Append("]}");
            json = JsonConvert.SerializeObject(resultlist);
            jsonbody.Append(json);
            jsonbody.Append("},");
            jsonbody.Append(tem);
            jsonbody.Append(press);
            jsonbody.Append(heat);
            jsonbody.Append(flow);
            jsonbody.Append(beng);
            jsonbody.Append(otherdata);
            jsonbody.Append("}");
            string result = jsonbody.ToString();
            return result;
        }

        /// <summary>
        /// 获取热源水利平衡信息
        /// </summary>
        /// <param name="id">-1为全部热源信息,如获取某热源信息直接传id编号</param>
        /// <returns></returns>
        public object querySlphHeatplantCalcHistory(int id)
        {
            try
            {
                if (id == -1)
                {
                    List<SlphInfoResponse> result = Db.Queryable<SlphHeatplantCalcHistory, SlphXlinkId, VpnUser>((s, x, v) => new JoinQueryInfos(JoinType.Left, s.Pid == x.Pid, JoinType.Left, x.VpnUserId == v.Id)).Where((s, x, v) => v.IsValid == true).Select<SlphInfoResponse>().ToList();
                    return result;
                }
                else
                {
                    SlphInfoResponse result = Db.Queryable<SlphHeatplantCalcHistory, SlphXlinkId, VpnUser>((s, x, v) => new JoinQueryInfos(JoinType.Left, s.Pid == x.Pid, JoinType.Left, x.VpnUserId == v.Id)).Where((s, x, v) => x.VpnUserId == id && v.IsValid == true).Select<SlphInfoResponse>().First();
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }
    }
}
