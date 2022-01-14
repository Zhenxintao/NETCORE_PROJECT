using ApiModel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.DqForecast;
using THMS.Core.API.Models.DqForecast.Dto;
using THMS.Core.API.Models.DqForecast.ReturnModel;
using THMS.Core.API.Models.DqForecast.SearchModel;
using THMS.Core.API.Models.UniformedServices.PvssDSSystem;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.DqForecast
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastHomeService : DbContextSqlSugar
    {
        /// <summary>
        /// 获取预测数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> getData(ForecastBaseSearch search)
        {
            var res = new ForecastRes();
            var list = new List<ForecastHeatNetSevenDataDto>();
            try
            {
                var _heatNetForecastData = await getHeatNetForecastData(search);
                var _powerForecastData = await getPowerForecastData(search);

                var heatNetGroups = _heatNetForecastData.GroupBy(x => x.VpnUser_id);
                foreach (var group in heatNetGroups)
                {
                    var _heatMode = new ForecastHeatNetSevenDataDto();
                    var _powerList = new List<ForecastPowerSevenDataDto>();
                    var _today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    foreach (var item in group)
                    {
                        var _forecastdate = Convert.ToDateTime(Convert.ToDateTime(item.ForecastDate).ToString("yyyy-MM-dd"));
                        switch (_forecastdate.Subtract(_today).Days)
                        {
                            case 0:
                                _heatMode.Id = item.Id;
                                _heatMode.VpnUser_id = item.VpnUser_id;
                                _heatMode.StationName = item.StationName;
                                _heatMode.HotArea = item.HotArea;
                                _heatMode.HeatTarget = item.HeatTarget;
                                _heatMode.RealHeat = item.RealHeat;
                                _heatMode.TodayForecastHeat = item.ForecastHeat;
                                break;
                            case 1:
                                _heatMode.SecondDayForecastHeat = item.ForecastHeat;
                                break;
                            case 2:
                                _heatMode.ThirdDayForecastHeat = item.ForecastHeat;
                                break;
                            case 3:
                                _heatMode.ForthDayForecastHeat = item.ForecastHeat;
                                break;
                            case 4:
                                _heatMode.FivthDayForecastHeat = item.ForecastHeat;
                                break;
                            case 5:
                                _heatMode.SixthDayForecastHeat = item.ForecastHeat;
                                break;
                            case 6:
                                _heatMode.SeventhDayForecastHeat = item.ForecastHeat;
                                break;
                            default:
                                break;
                        }
                    }
                    if (_heatMode.VpnUser_id > 0)
                    {
                        var powerForecastData = _powerForecastData.Where(x => x.ParentId == _heatMode.VpnUser_id).ToList();
                        if (powerForecastData.Count > 0)
                        {
                            var powerGroups = powerForecastData.GroupBy(x => x.VpnUser_id);
                            foreach (var powerGroup in powerGroups)
                            {
                                var _powerMode = new ForecastPowerSevenDataDto();
                                foreach (var jtem in powerGroup)
                                {
                                    var _powerforecastdate = Convert.ToDateTime(Convert.ToDateTime(jtem.ForecastDate).ToString("yyyy-MM-dd"));
                                    switch (_powerforecastdate.Subtract(_today).Days)
                                    {
                                        case 0:
                                            _powerMode.Id = jtem.Id;
                                            _powerMode.VpnUser_id = jtem.VpnUser_id;
                                            _powerMode.StationName = jtem.StationName;
                                            _powerMode.HotArea = jtem.HotArea;
                                            _powerMode.HeatTarget = jtem.HeatTarget;
                                            _powerMode.RealHeat = jtem.RealHeat;
                                            _powerMode.TodayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        case 1:
                                            _powerMode.SecondDayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        case 2:
                                            _powerMode.ThirdDayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        case 3:
                                            _powerMode.ForthDayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        case 4:
                                            _powerMode.FivthDayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        case 5:
                                            _powerMode.SixthDayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        case 6:
                                            _powerMode.SeventhDayForecastHeat = jtem.ForecastHeat;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                _powerList.Add(_powerMode);
                            }
                            _heatMode.children = _powerList;
                        }
                    }
                    list.Add(_heatMode);
                }

                res.Code = 200;
                res.Message = "查询成功";
                res.Data = list;
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }


        /// <summary>
        /// 获取预测曲线数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<object> getChartData(ForecastBaseSearch search)
        {
            var res = new ForecastRes();
            try
            {
                var _heatNetForecastData = await getHeatNetForecastData(search);
                var _powerForecastData = await getPowerForecastData(search);

                var seriesList = new List<object>();
                var Time = new string[7];
                for (int i = 0; i < 7; i++)
                {
                    Time[i] = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd");
                }

                //循环计算实际瞬时热量和天气预报chart数据
                var _today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                decimal?[] realArr = new decimal?[7];
                decimal?[] TempArr = new decimal?[7];
                var _name = "";
                foreach (var item in _heatNetForecastData)
                {
                    var _forecastdate = Convert.ToDateTime(Convert.ToDateTime(item.ForecastDate).ToString("yyyy-MM-dd"));
                    switch (_forecastdate.Subtract(_today).Days)
                    {
                        case 0:
                            _name = item.StationName + " - 实际瞬时热量";
                            realArr[0] = item.RealHeat;
                            TempArr[0] = item.OutDoorTemp;
                            break;
                        case 1:
                            realArr[1] = item.RealHeat;
                            TempArr[1] = item.OutDoorTemp;
                            break;
                        case 2:
                            realArr[2] = item.RealHeat;
                            TempArr[2] = item.OutDoorTemp;
                            break;
                        case 3:
                            realArr[3] = item.RealHeat;
                            TempArr[3] = item.OutDoorTemp;
                            break;
                        case 4:
                            realArr[4] = item.RealHeat;
                            TempArr[4] = item.OutDoorTemp;
                            break;
                        case 5:
                            realArr[5] = item.RealHeat;
                            TempArr[5] = item.OutDoorTemp;
                            break;
                        case 6:
                            realArr[6] = item.RealHeat;
                            TempArr[6] = item.OutDoorTemp;
                            break;
                        default:
                            break;
                    }
                }
                seriesList.Add(new
                {
                    type = "column",
                    name = _name,
                    data = realArr,
                    stack = "RealHeat"
                });
                seriesList.Add(new
                {
                    type = "spline",
                    name = "室外温度",
                    data = TempArr,
                    stack = "ForecastHeat"
                });

                //循环计算预测瞬时热量chart数据
                var groups = _powerForecastData.GroupBy(x => x.VpnUser_id);
                var _stack = "ForecastHeat";
                foreach (var group in groups)
                {
                    decimal?[] arr = new decimal?[7];
                    string name = "";
                    foreach (var item in group)
                    {
                        var _powerforecastdate = Convert.ToDateTime(Convert.ToDateTime(item.ForecastDate).ToString("yyyy-MM-dd"));
                        switch (_powerforecastdate.Subtract(_today).Days)
                        {
                            case 0:
                                name = item.StationName + " - 预测瞬时热量";
                                arr[0] = item.ForecastHeat;
                                break;
                            case 1:
                                arr[1] = item.ForecastHeat;
                                break;
                            case 2:
                                arr[2] = item.ForecastHeat;
                                break;
                            case 3:
                                arr[3] = item.ForecastHeat;
                                break;
                            case 4:
                                arr[4] = item.ForecastHeat;
                                break;
                            case 5:
                                arr[5] = item.ForecastHeat;
                                break;
                            case 6:
                                arr[6] = item.ForecastHeat;
                                break;
                            default:
                                break;
                        }
                    }
                    seriesList.Add(new
                    {
                        type= "column",
                        name = name,
                        data = arr,
                        stack = _stack
                    });
                }


                var result = new
                {
                    Time = Time,
                    Data = seriesList
                };

                res.Code = 200;
                res.Message = "查询成功";
                res.Data = result;
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        /// <summary>
        /// 获取热网预测数据
        /// </summary>
        /// <returns></returns>
        private async Task<List<ForecastDataDto>> getHeatNetForecastData(ForecastBaseSearch search)
        {
            var list = new List<ForecastDataDto>();
            try
            {
                list = await Db.Queryable<VpnUser, ForecastData, PowerInfo>((v, f, p) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id,
                 JoinType.Inner,v.Id==p.VpnUser_id
                })
                    .Where((v, f, p) => v.StationStandard == 98)
                    .Where((v, f, p) => v.IsValid == true)
                    .WhereIF(search.HeatNet_id > 0, (v, f, p) => f.VpnUser_id == search.HeatNet_id)
                    .OrderBy((v, f, p) => f.ForecastDate, OrderByType.Asc)
                    .Select((v, f, p) => new ForecastDataDto
                    {
                        Id = f.Id,
                        VpnUser_id = f.VpnUser_id,
                        StationName = v.StationName,
                        ParentId = p.ParentID,
                        ForecastDate = f.ForecastDate,
                        HotArea = f.HotArea,
                        HeatTarget = f.HeatTarget,
                        StandardTemp = f.StandardTemp,
                        OutDoorTemp = f.OutDoorTemp,
                        RealHeat = f.RealHeat,
                        ForecastHeat = f.ForecastHeat
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                return list;
            }
            return list;
        }


        /// <summary>
        /// 获取热源预测数据
        /// </summary>
        /// <returns></returns>
        private async Task<List<ForecastDataDto>> getPowerForecastData(ForecastBaseSearch search)
        {
            var list = new List<ForecastDataDto>();
            try
            {
                list = await Db.Queryable<VpnUser, ForecastData, PowerInfo>((v, f, p) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id,
                 JoinType.Inner,v.Id==p.VpnUser_id
                })
                    .Where((v, f, p) => v.StationStandard == 99)
                    .Where((v, f, p) => v.IsValid == true)
                    .WhereIF(search.HeatNet_id > 0, (v, f, p) => p.ParentID == search.HeatNet_id)
                    .OrderBy((v, f, p) => f.ForecastDate, OrderByType.Asc)
                    .Select((v, f, p) => new ForecastDataDto
                    {
                        Id = f.Id,
                        VpnUser_id = f.VpnUser_id,
                        StationName = v.StationName,
                        ParentId = p.ParentID,
                        ForecastDate = f.ForecastDate,
                        HotArea = f.HotArea,
                        HeatTarget = f.HeatTarget,
                        StandardTemp = f.StandardTemp,
                        OutDoorTemp = f.OutDoorTemp,
                        RealHeat = f.RealHeat,
                        ForecastHeat = f.ForecastHeat
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                return list;
            }
            return list;
        }

        /// <summary>
        /// 从pvss同步热网热指标
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<object> PvssConfigSet(List<PvssSet> list)
        {
            var res = new ForecastRes();
            try
            {
                var updList = new List<ForecastHeatNetInfo>();
                var resList = new List<string>();

                var pvssConfig = await Db.Queryable<PvssSetting>().ToListAsync();
                var heatInfo = await Db.Queryable<ForecastHeatNetInfo>().ToListAsync();

                foreach (var item in list)
                {
                    var pvssModel = pvssConfig.Where(x => x.Pvss_id == item.Pvss_id).FirstOrDefault();
                    if (pvssModel == null || string.IsNullOrEmpty(pvssModel.VpnUser_id))
                    {
                        resList.Add(item.Pvss_id + " - 同步失败，同步配置表中没有相关信息");
                        continue;
                    }
                    var heatMode = heatInfo.Where(x => x.VpnUser_id.ToString() == pvssModel.VpnUser_id).FirstOrDefault();
                    heatMode.HeatTarget = item.HeatTarget;
                    updList.Add(heatMode);
                    resList.Add(item.Pvss_id + " - 同步成功");
                }

                var execRes = await Db.Updateable<ForecastHeatNetInfo>(updList).ExecuteCommandAsync() > 0 ? true : false;

                if (execRes)
                {
                    res.Code = 200;
                    res.Message = JsonConvert.SerializeObject(resList);
                }
                else
                {
                    res.Code = 200;
                    res.Message = "同步失败";
                }
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }


        /// <summary>
        /// 获取热源最大、最小负荷
        /// </summary>
        /// <returns></returns>
        public async Task<object> getPowerFh()
        {
            var res = new ForecastRes();
            var list = new List<ForecastFhDto>();
            try
            {
                var powerData = await Db.Queryable<VpnUser, ForecastPowerInfo>((v, f) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id
                })
                    .Where((v, f) => v.StationStandard == 99)
                    .Where((v, f) => v.IsValid == true)
                    .Where((v, f) => f.IsValid == true)
                    .OrderBy((v, f) => v.Id, OrderByType.Asc)
                    .Select((v, f) => new ForecastFhDto
                    {
                        VpnUser_id = f.VpnUser_id,
                        StationName = v.StationName,
                        MaxPower = f.MaxPower,
                        MinPower = f.MinPower
                    })
                    .ToListAsync();

                var boilerData = await Db.Queryable<ForecastBoilerInfo>().Where(x => x.IsValid == true).ToListAsync();

                foreach (var item in powerData)
                {
                    var _boilerData = boilerData.Where(x => x.VpnUser_id == item.VpnUser_id).ToList();
                    item.MaxPower += _boilerData.Sum(x => x.MaxPower);
                    item.MinPower += _boilerData.Sum(x => x.MinPower);
                    list.Add(item);
                }
                res.Code = 200;
                res.Message = "查询成功";
                res.Data = list;
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

    }
}
