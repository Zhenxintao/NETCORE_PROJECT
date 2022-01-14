using ApiModel;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Forecast
{
    public class StationForecastRealDayService: DbContextSqlSugar
    {
        /// <summary>
        /// 定时更改天历史表以及天实时展示表
        /// </summary>
        /// <returns></returns>
        /// 
        public static IConfigurationRoot Configuration { get; set; }
        public string AddStationForecastRealDayService()
        {
           
            //获取未来天气（小时表）中数据配置名称
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            //var forecast = Configuration["appSettings:Forecast"];
            var real = Configuration["appSettings:Real"];
            
            try
            {
                //查询实时表中当前时间之前的数据，准备移到历史表中并清空历史数据。
                var realDayListResult = Db.Queryable<StationForecastRealDay>().Where(s => s.ForecastDateDay < DateTime.Now).Select<StationForecastHistoryDay>().ToList();

                if (realDayListResult.Count > 0)
                {
                    //查询实时的历史数据插入到历史表中
                    int conHistoryDayResult = Db.Insertable<StationForecastHistoryDay>(realDayListResult.ToArray()).ExecuteCommand();
                    //判断是否插入历史表成功，并删除实时表中的所有数据
                    if (conHistoryDayResult > 0)
                    {
                        int conRealDay = Db.Deleteable<StationForecastRealDay>().ExecuteCommand();
                    }
                }

                //获取StationForecastBasic基础信息表中的输入值用于计算
                var stationForecastBasic = Db.Queryable<StationForecastBasic>().Where(s => s.IsValid == true).OrderBy(s => s.HeatingSeason, OrderByType.Desc).First();


                //先获取StationForecastInput换热站负荷预测数据输入表中所有的站点信息，并往StationForecastRealDay表中插入
                var stationForecastInputList = Db.Queryable<StationForecastInput>().Where(s=>s.IsValid==true).ToList();

                //再获取RealTemperature未来天气（小时表）的各个小时温度信息，并插入到各个站中
                //var WeatherForecastList = Db.Queryable<WeatherForecast>().Where(s=>s.WeatherType==2 &&  SqlFunc.Between(s.ForecastTime, DateTime.Now, DateTime.Now.AddDays(6))).ToList();
                List<WeatherForecast> WeatherForecastList = new List<WeatherForecast>();
                string weatherforeSql = $@"SELECT ForecastTime, MAX(CheckDate) AS 'CheckDate'  FROM WeatherForecast WHERE ForecastTime BETWEEN getdate() AND  dateadd(day,6,getdate()) AND WeatherType = 2 GROUP BY ForecastTime ORDER BY CheckDate DESC";
                var WeatherForecas = Db.Ado.SqlQuery<WeatherForecast>(weatherforeSql).ToList();

                foreach (var Weather in WeatherForecas)
                {
                    var list = Db.Queryable<WeatherForecast>().Where(s => s.ForecastTime == Weather.ForecastTime && s.CheckDate == Weather.CheckDate && s.WeatherType == 2).First();
                    WeatherForecastList.Add(list);
                }

                //定义一个List集合用来存放即将运算的所有换热站实时表信息
                List<StationForecastRealDay> stationForecastRealDayList = new List<StationForecastRealDay>();


                //循环遍历StationForecastInput输入表中的所有的换热站信息并添加到StationForecastRealDay实时数据表中。
                foreach (var station in stationForecastInputList)
                {
                    foreach (var weather in WeatherForecastList)
                    {
                        StationForecastRealDay stationForecastRealDay = new StationForecastRealDay();

                        #region 匹配的各项值

                        //换热站ID
                        stationForecastRealDay.VpnUserId = station.VpnUserId;

                        //换热站
                        stationForecastRealDay.StationName = station.StationName;

                        //机组号
                        stationForecastRealDay.StationBranchArrayNumber = station.StationBranchArrayNumber;

                        //预测时间
                        stationForecastRealDay.ForecastDateDay = weather.ForecastTime;

                        //面积
                        stationForecastRealDay.HeatArea = station.HeatArea;

                        //二次供水温度
                        stationForecastRealDay.RealSecSendTemp = station.SEC_TEMP_S;

                        //二次回水温度
                        stationForecastRealDay.RealSecReturnTemp = station.SEC_TEMP_R;


                        //热指标
                        stationForecastRealDay.ForecastHeatTarget = station.HeatTarget;

                        //负荷率
                        stationForecastRealDay.LoadRate = (stationForecastBasic.IndoorCalculationTemp - (weather.AvgTemperature - station.OutdoorAtPresentOffset)) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature);

                        //当前热指标
                        stationForecastRealDay.RealHeatTarget = stationForecastRealDay.ForecastHeatTarget * stationForecastRealDay.LoadRate * station.AdCoefficient;

                        //日用热量
                        stationForecastRealDay.HourlyHeatDay = stationForecastRealDay.RealHeatTarget * stationForecastRealDay.HeatArea * 3.6M * Convert.ToDecimal(Math.Pow(10, -6))*24;

                        //预测室外温度
                        stationForecastRealDay.ForecastOutdoorTemp = weather.AvgTemperature;

                        //实际室外温度，不确定这个字段是否需要存在。
                        var RealTemp = Db.Queryable<RealTemperature>().Where(s => SqlFunc.Between(s.NcapTime, weather.NcapTime, SqlFunc.DateAdd(weather.NcapTime, 1)) && s.CollectName == real).Avg(s => s.ForecastHourAvgTemperature);
                        stationForecastRealDay.RealOutdoorTemp = RealTemp;

                        #endregion

                        //得到该换热站的平均水温
                        decimal avgTemp = (stationForecastRealDay.RealSecSendTemp + stationForecastRealDay.RealSecReturnTemp) / 2;
                        //换热站为地暖
                        if (station.HeatingType == 1 )
                        {
                            //获得地面面积的散热量
                            var FloorSurfaceAvgTemp =new StationForecastRealHourService().RadiatingTube(avgTemp, stationForecastBasic.IndoorCalculationTemp, station.RadiatingTubeType);

                            //地板表面平均温度
                            if (station.RadiatingTubeType == 1)
                            {
                                stationForecastRealDay.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeTH) / 100, 0.969));
                            }
                            else if (station.RadiatingTubeType == 2)
                            {
                                stationForecastRealDay.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeSHF) / 100, 0.969));
                            }
                            else if (station.RadiatingTubeType == 3)
                            {
                                stationForecastRealDay.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeSH) / 100, 0.969));
                            }
                            else if (station.RadiatingTubeType == 4)
                            {
                                stationForecastRealDay.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeOHF) / 100, 0.969));
                            }

                            //预测二次供水温度
                            stationForecastRealDay.ForecastSecSendTemp =
                                stationForecastBasic.IndoorCalculationTemp
                                + (stationForecastRealDay.FloorSurfaceAvgTemp - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealDay.LoadRate), 0.969))
                                + (((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2) - stationForecastRealDay.FloorSurfaceAvgTemp + ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2 * station.SeNetworkRelativeFlow))
                                * stationForecastRealDay.LoadRate;

                            //预测二次回水温度
                            stationForecastRealDay.ForecastSecReturnTemp =
                                stationForecastBasic.IndoorCalculationTemp
                                + (stationForecastRealDay.FloorSurfaceAvgTemp - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealDay.LoadRate), 0.969))
                                + (((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2) - stationForecastRealDay.FloorSurfaceAvgTemp - ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2 * station.SeNetworkRelativeFlow))
                                * stationForecastRealDay.LoadRate;

                            //换热站二次侧循环流量
                            stationForecastRealDay.ForecastSecFlow =
                                stationForecastRealDay.RealHeatTarget
                                * station.HeatArea
                                / (1.163M * (stationForecastRealDay.ForecastSecSendTemp - stationForecastRealDay.ForecastSecReturnTemp))
                                *
                                Convert.ToDecimal(Math.Pow(10, -3));
                            //预测流量
                            stationForecastRealDay.ForecastFlow = stationForecastRealDay.ForecastSecFlow * station.SeNetworkRelativeFlow;
                            //采暖方式
                            stationForecastRealDay.HeatingType = station.HeatingType;
                        }
                        ////换热站为挂片采暖或混合供暖，偏挂片采暖
                        else if (station.HeatingType == 2 || station.HeatingType == 3)
                        {
                            //预测二次供水温度
                            stationForecastRealDay.ForecastSecSendTemp =
                                stationForecastBasic.IndoorCalculationTemp
                                + ((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2 - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealDay.LoadRate), Convert.ToDouble(1 / (1 + stationForecastBasic.RadiatorCoefficient))))
                                + ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2)
                                * (stationForecastRealDay.LoadRate / station.SeNetworkRelativeFlow);

                            //预测二次回水温度
                            stationForecastRealDay.ForecastSecReturnTemp =
                                 stationForecastBasic.IndoorCalculationTemp
                                + ((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2 - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealDay.LoadRate), Convert.ToDouble(1 / (1 + stationForecastBasic.RadiatorCoefficient))))
                                - ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2)
                                * (stationForecastRealDay.LoadRate / station.SeNetworkRelativeFlow);

                            //换热站二次侧循环流量
                            stationForecastRealDay.ForecastSecFlow =
                                stationForecastRealDay.RealHeatTarget
                                * station.HeatArea
                                / (1.163M * (stationForecastRealDay.ForecastSecSendTemp - stationForecastRealDay.ForecastSecReturnTemp))
                                *
                                Convert.ToDecimal(Math.Pow(10, -3));
                            //预测流量
                            stationForecastRealDay.ForecastFlow = stationForecastRealDay.ForecastSecFlow * station.SeNetworkRelativeFlow;
                            //采暖方式
                            stationForecastRealDay.HeatingType = station.HeatingType;
                        }

                        //采集时间
                        stationForecastRealDay.CreateTime = station.CreateTime;

                        //创建人
                        stationForecastRealDay.CreateUser = station.CreateUser;

                        //是否展示
                        stationForecastRealDay.IsValid = station.IsValid;

                        //批量添加到List集合中
                        stationForecastRealDayList.Add(stationForecastRealDay);

                    }
                }

                //进行批量插入
                int conresult = Db.Insertable(stationForecastRealDayList.ToArray()).ExecuteCommand();
                if (conresult > 0)
                {
                    return "实时天信息表更新成功！";
                }
                return null;
            }


            catch (Exception ex)
            {
                return "Day异常信息为：" + ex;
            }


        }

        public object ceshi()
        {
            //再获取RealTemperature未来天气（小时表）的各个小时温度信息，并插入到各个站中
            //var WeatherForecastList = Db.Queryable<WeatherForecast>("SELECT ForecastTime, MAX(CheckDate) AS 'CheckDate'  FROM WeatherForecast GROUP BY ForecastTime  ORDER BY CheckDate DESC").Where(s => s.WeatherType == 2 && SqlFunc.Between(s.ForecastTime, DateTime.Now, DateTime.Now.AddDays(6))).ToList();
            List<WeatherForecast> WeatherForecastList = new List<WeatherForecast>();
            string weatherforeSql = $@"SELECT ForecastTime, MAX(CheckDate) AS 'CheckDate'  FROM WeatherForecast WHERE ForecastTime BETWEEN getdate() AND  dateadd(day,6,getdate()) AND WeatherType = 2 GROUP BY ForecastTime ORDER BY CheckDate DESC";
            var WeatherForecas = Db.Ado.SqlQuery<WeatherForecast>(weatherforeSql).ToList();

            foreach (var Weather in WeatherForecas)
            {
               var list= Db.Queryable<WeatherForecast>().Where(s => s.ForecastTime == Weather.ForecastTime && s.CheckDate == Weather.CheckDate && s.WeatherType==2).First();
                WeatherForecastList.Add(list);
            }
            return WeatherForecastList;
            //SELECT ForecastTime, MAX(CheckDate) AS 'CheckDate',MAX(AvgTemperature) AS AvgTemperature FROM WeatherForecast WHERE ForecastTime BETWEEN '2019-11-18 17:08:57' AND '2019-11-24 17:08:57' AND WeatherType = 2 GROUP BY ForecastTime ORDER BY CheckDate DESC

        }
        /// <summary>
        /// 查询实时信息天表信息
        /// </summary>
        /// <param name="vpnuserid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="heatType"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Tuple<object, int> SelStationForecastRealHour(int vpnuserid, string startTime, string endTime, int heatType, int pageSize, int pageIndex)
        {
            var totalCount = 0;

            var list = Db.Queryable<StationForecastRealDay,StationBranch>((s,sbr)=>new object[] {JoinType.Left,s.VpnUserId==sbr.VpnUser_id && s.StationBranchArrayNumber==sbr.StationBranchArrayNumber})
                .WhereIF(!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime), (s,sbr) => SqlFunc.Between(s.ForecastDateDay, startTime, endTime))
                .WhereIF(SqlFunc.HasNumber(vpnuserid), (s,sbr) => s.VpnUserId == vpnuserid)
                .WhereIF(SqlFunc.HasNumber(heatType), (s,sbr) => s.HeatingType == heatType)
                .Select((s,sbr)=>new { s.Id,s.VpnUserId,s.StationName, sbr.StationBranchName, s.ForecastDateDay,s.HeatArea,s.ForecastHeatTarget,s.RealHeatTarget,s.FloorSurfaceAvgTemp,s.ForecastOutdoorTemp,s.RealOutdoorTemp,s.ForecastSecFlow,s.ForecastFlow,s.LoadRate,s.HourlyHeatDay,s.ForecastSecSendTemp,s.ForecastSecReturnTemp,s.RealSecSendTemp,s.RealSecReturnTemp,s.HeatingType,s.CreateTime,s.CreateUser,s.IsValid,sbr.StationBranchArrayNumber})
                .ToPageList(pageIndex, pageSize, ref totalCount);
            return new Tuple<object, int>(list, totalCount);
        }
    }
}
