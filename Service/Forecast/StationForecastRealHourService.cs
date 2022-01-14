using ApiModel;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Forecast
{
    public class StationForecastRealHourService : DbContextSqlSugar
    {
        /// <summary>
        /// 定时更改小时历史表以及小时实时展示表
        /// </summary>
        /// <returns></returns>
        /// 
        public static IConfigurationRoot Configuration { get; set; }
        public string AddStationForecastRealHourService()
        {
            //获取未来天气（小时表）中数据配置名称
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            var forecast = Configuration["appSettings:Forecast"];
            var real= Configuration["appSettings:Real"];
            try
            {

                //查询实时表中当前时间之前的数据，准备移到历史表中并清空历史数据。
                var realHourListResult = Db.Queryable<StationForecastRealHour>().Where(s => s.ForecastDateHour < DateTime.Now).Select<StationForecastHistoryHour>().ToList();

                if (realHourListResult.Count > 0)
                {
                    //查询实时的历史数据插入到历史表中
                    int conHistoryHourResult = Db.Insertable<StationForecastHistoryHour>(realHourListResult.ToArray()).ExecuteCommand();
                    //判断是否插入历史表成功，并删除实时表中的所有数据
                    if (conHistoryHourResult > 0)
                    {
                        int conRealHour = Db.Deleteable<StationForecastRealHour>().ExecuteCommand();
                    }
                }

                //获取StationForecastBasic基础信息表中的输入值用于计算
                var stationForecastBasic = Db.Queryable<StationForecastBasic>().Where(s => s.IsValid == true).OrderBy(s => s.HeatingSeason, OrderByType.Desc).First();


                //先获取StationForecastInput换热站负荷预测数据输入表中所有的站点信息，并往StationForecastRealHour表中插入
                var stationForecastInputList = Db.Queryable<StationForecastInput>().Where(s => s.IsValid == true).ToList();

                

                //再获取RealTemperature未来天气（小时表）的各个小时温度信息，并插入到各个站中
                var realTemperatureList = Db.Queryable<RealTemperature>().Where(s =>s.CollectName == forecast && SqlFunc.Between(s.NcapTime, DateTime.Now, DateTime.Now.AddDays(6))).ToList();


                //定义一个List集合用来存放即将运算的所有换热站实时表信息
                List<StationForecastRealHour> stationForecastRealHourList = new List<StationForecastRealHour>();


                //循环遍历StationForecastInput输入表中的所有的换热站信息并添加到StationForecastRealHour实时数据表中。
                foreach (var station in stationForecastInputList)
                {
                    foreach (var weather in realTemperatureList)
                    {
                        StationForecastRealHour stationForecastRealHour = new StationForecastRealHour();

                        #region 匹配的各项值

                        //换热站ID
                        stationForecastRealHour.VpnUserId = station.VpnUserId;

                        //换热站
                        stationForecastRealHour.StationName = station.StationName;

                        //机组号
                        stationForecastRealHour.StationBranchArrayNumber = station.StationBranchArrayNumber;

                        //预测时间
                        stationForecastRealHour.ForecastDateHour = weather.NcapTime;

                        //面积
                        stationForecastRealHour.HeatArea = station.HeatArea;

                        //二次供水温度
                        stationForecastRealHour.RealSecSendTemp = station.SEC_TEMP_S;

                        //二次回水温度
                        stationForecastRealHour.RealSecReturnTemp = station.SEC_TEMP_R;


                        //热指标
                        stationForecastRealHour.ForecastHeatTarget = station.HeatTarget;

                        //负荷率
                        stationForecastRealHour.LoadRate = (stationForecastBasic.IndoorCalculationTemp - (weather.ForecastHourAvgTemperature - station.OutdoorAtPresentOffset)) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature);

                        //当前热指标
                        stationForecastRealHour.RealHeatTarget = stationForecastRealHour.ForecastHeatTarget * stationForecastRealHour.LoadRate * station.AdCoefficient;

                        //小时用热量
                        stationForecastRealHour.HourlyHeatHour = stationForecastRealHour.RealHeatTarget * stationForecastRealHour.HeatArea * 3.6M * Convert.ToDecimal(Math.Pow(10, -6));

                        //预测室外温度
                        stationForecastRealHour.ForecastOutdoorTemp = weather.ForecastHourAvgTemperature;

                        //实际室外温度
                       var RealTemp= Db.Queryable<RealTemperature>().Where(s => s.NcapTime == weather.NcapTime && s.CollectName == real).First();
                        if (RealTemp == null)
                        {
                            stationForecastRealHour.RealOutdoorTemp = 0;
                        }
                        else {
                            stationForecastRealHour.RealOutdoorTemp = RealTemp.ForecastHourAvgTemperature;
                        }
           

                        #endregion

                        //得到该换热站的平均水温
                        decimal avgTemp = (stationForecastRealHour.RealSecSendTemp + stationForecastRealHour.RealSecReturnTemp) / 2;
                        //换热站为地暖
                        if (station.HeatingType == 1 )
                        {
                            //获得地面面积的散热量
                            var FloorSurfaceAvgTemp = RadiatingTube(avgTemp, stationForecastBasic.IndoorCalculationTemp, station.RadiatingTubeType);

                            //地板表面平均温度
                            if (station.RadiatingTubeType == 1)
                            {
                                stationForecastRealHour.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeTH) / 100, 0.969));
                            }
                            else if (station.RadiatingTubeType == 2)
                            {
                                stationForecastRealHour.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeSHF) / 100, 0.969));
                            }
                            else if (station.RadiatingTubeType == 3)
                            {
                                stationForecastRealHour.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeSH) / 100, 0.969));
                            }
                            else if (station.RadiatingTubeType == 4)
                            {
                                stationForecastRealHour.FloorSurfaceAvgTemp = stationForecastBasic.IndoorCalculationTemp + 9.82M * Convert.ToDecimal(Math.Pow(Convert.ToDouble(FloorSurfaceAvgTemp.RadiatingTubeOHF) / 100, 0.969));
                            }

                            //预测二次供水温度
                            stationForecastRealHour.ForecastSecSendTemp =
                                stationForecastBasic.IndoorCalculationTemp
                                + (stationForecastRealHour.FloorSurfaceAvgTemp - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealHour.LoadRate), 0.969))
                                + (((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2) - stationForecastRealHour.FloorSurfaceAvgTemp + ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2 * station.SeNetworkRelativeFlow))
                                * stationForecastRealHour.LoadRate;

                            //预测二次回水温度
                            stationForecastRealHour.ForecastSecReturnTemp =
                                stationForecastBasic.IndoorCalculationTemp
                                + (stationForecastRealHour.FloorSurfaceAvgTemp - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealHour.LoadRate), 0.969))
                                + (((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2) - stationForecastRealHour.FloorSurfaceAvgTemp - ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2 * station.SeNetworkRelativeFlow))
                                * stationForecastRealHour.LoadRate;

                            //换热站二次侧循环流量
                            stationForecastRealHour.ForecastSecFlow =
                                stationForecastRealHour.RealHeatTarget
                                * station.HeatArea
                                / (1.163M * (stationForecastRealHour.ForecastSecSendTemp - stationForecastRealHour.ForecastSecReturnTemp))
                                *
                                Convert.ToDecimal(Math.Pow(10, -3));
                            //预测流量
                            stationForecastRealHour.ForecastFlow = stationForecastRealHour.ForecastSecFlow * station.SeNetworkRelativeFlow;
                            //采暖方式
                            stationForecastRealHour.HeatingType = station.HeatingType;
                        }
                        //换热站为挂片采暖或混合供暖，偏挂片采暖
                        else if (station.HeatingType == 2 || station.HeatingType == 3)
                        {
                            //预测二次供水温度
                            stationForecastRealHour.ForecastSecSendTemp =
                                stationForecastBasic.IndoorCalculationTemp
                                + ((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2 - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealHour.LoadRate), Convert.ToDouble(1 / (1 + stationForecastBasic.RadiatorCoefficient))))
                                + ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2)
                                * (stationForecastRealHour.LoadRate / station.SeNetworkRelativeFlow);

                            //预测二次回水温度
                            stationForecastRealHour.ForecastSecReturnTemp =
                                 stationForecastBasic.IndoorCalculationTemp
                                + ((station.SEC_TEMP_S + station.SEC_TEMP_R) / 2 - stationForecastBasic.IndoorCalculationTemp)
                                * Convert.ToDecimal(Math.Pow(Convert.ToDouble(stationForecastRealHour.LoadRate), Convert.ToDouble(1 / (1 + stationForecastBasic.RadiatorCoefficient))))
                                - ((station.SEC_TEMP_S - station.SEC_TEMP_R) / 2)
                                * (stationForecastRealHour.LoadRate / station.SeNetworkRelativeFlow);

                            //换热站二次侧循环流量
                            stationForecastRealHour.ForecastSecFlow =
                                stationForecastRealHour.RealHeatTarget
                                * station.HeatArea
                                / (1.163M * (stationForecastRealHour.ForecastSecSendTemp - stationForecastRealHour.ForecastSecReturnTemp))
                                *
                                Convert.ToDecimal(Math.Pow(10, -3));
                            //预测流量
                            stationForecastRealHour.ForecastFlow = stationForecastRealHour.ForecastSecFlow * station.SeNetworkRelativeFlow;
                            //采暖方式
                            stationForecastRealHour.HeatingType = station.HeatingType;
                        }

                        stationForecastRealHour.CreateTime = station.CreateTime;
                        stationForecastRealHour.CreateUser = station.CreateUser;
                        stationForecastRealHour.IsValid = station.IsValid;


                        stationForecastRealHourList.Add(stationForecastRealHour);

                    }
                }

                int conresult = Db.Insertable(stationForecastRealHourList.ToArray()).ExecuteCommand();
                if (conresult > 0)
                {
                    return "实时小时信息表更新成功！";
                }
                return null;
            }


            catch (Exception ex)
            {
                return "Hour异常信息为：" + ex;
            }


        }

        /// <summary>
        /// 得到散热量
        /// </summary>
        /// <param name="avgTemp">平均水温</param>
        /// <param name="IndoorCalculationTemp">室内温度</param>
        /// <param name="HeatingType">散热管间距类型</param>
        /// <returns></returns>
        public dynamic RadiatingTube(decimal avgTemp, decimal IndoorCalculationTemp, int HeatingType)
        {

            //从地板采暖散热量数据表中匹配对应的平均水温以及室内温度取出对应的散热量
            #region 平均水温35


            if (avgTemp < 37.5M && IndoorCalculationTemp < 17 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp < 17 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp < 17 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp < 17 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //35、18
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //35、20
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            // 35、22
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //35 、24
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp < 37.5M && IndoorCalculationTemp >= 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 35 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            #endregion

            #region 平均水温40

            if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp < 17 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp < 17 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp < 17 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp < 17 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //40、18
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //40、20
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            // 40、22
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //40 、24
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 37.5M && avgTemp < 42.5M && IndoorCalculationTemp >= 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 40 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }


            #endregion

            #region 平均水温45
            if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp < 17 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp < 17 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp < 17 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp < 17 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //45、18
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //45、20
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            // 45、22
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //45 、24
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 42.5M && avgTemp < 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 45 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }

            #endregion

            #region 平均水温50
            if (avgTemp >= 47.5M && IndoorCalculationTemp < 17 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp < 17 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp < 17 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp < 17 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 16).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //50、18
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 17 && IndoorCalculationTemp < 19 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 18).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //50、20
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 19 && IndoorCalculationTemp < 21 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 20).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            // 50、22
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 21 && IndoorCalculationTemp < 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 22).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }
            //50 、24
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 1)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeTH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 2)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSHF }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 3)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeSH }).First();
                return stationForecastRadiatingTubeList;
            }
            else if (avgTemp >= 47.5M && IndoorCalculationTemp >= 23 && HeatingType == 4)
            {
                var stationForecastRadiatingTubeList = Db.Queryable<StationForecastRadiatingTube>().Where(s => s.Avgtemperature == 50 && s.IndoorCalculationTemp == 24).Select(s => new { s.RadiatingTubeOHF }).First();
                return stationForecastRadiatingTubeList;
            }

            #endregion

            return null;
        }



        /// <summary>
        /// 查询实时信息小时表信息
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
            var list = Db.Queryable<StationForecastRealHour,StationBranch>((s, sbr) => new object[] { JoinType.Left, s.VpnUserId == sbr.VpnUser_id && s.StationBranchArrayNumber==sbr.StationBranchArrayNumber })
                .WhereIF(!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime), (s, sbr) => SqlFunc.Between(s.ForecastDateHour, startTime, endTime))
                .WhereIF(SqlFunc.HasNumber(vpnuserid), (s, sbr) => s.VpnUserId == vpnuserid)
                .WhereIF(SqlFunc.HasNumber(heatType), (s, sbr) => s.HeatingType == heatType)
                .Select((s, sbr) => new { s.Id, s.VpnUserId, s.StationName, sbr.StationBranchName, s.ForecastDateHour, s.HeatArea, s.ForecastHeatTarget, s.RealHeatTarget, s.FloorSurfaceAvgTemp, s.ForecastOutdoorTemp, s.RealOutdoorTemp, s.ForecastSecFlow, s.ForecastFlow, s.LoadRate, s.HourlyHeatHour, s.ForecastSecSendTemp, s.ForecastSecReturnTemp, s.RealSecSendTemp, s.RealSecReturnTemp, s.HeatingType, s.CreateTime, s.CreateUser, s.IsValid,  sbr.StationBranchArrayNumber })
                .ToPageList(pageIndex, pageSize, ref totalCount);
            return new Tuple<object, int>(list, totalCount);
        }
    }
}
