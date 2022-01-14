using ApiModel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.DqForecast;
using THMS.Core.API.Models.DqForecast.Dto;
using THMS.Core.API.Models.DqForecast.ReturnModel;
using THMS.Core.API.Models.DqForecast.SearchModel;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.DqForecast
{
    /// <summary>
    /// 预测配置
    /// </summary>
    public class ForecastBaseService : DbContextSqlSugar
    {
        #region 下拉框
        /// <summary>
        /// 获取热网下拉框数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> getHeatNetSelect()
        {
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<VpnUser, ForecastHeatNetInfo>((v, f) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id
                })
                    .Where((v, f) => v.StationStandard == 98)
                    .Where((v, f) => v.IsValid == true)
                    .OrderBy((v, f) => f.OrderSeq, OrderByType.Asc)
                    .Select((v, f) => new
                    {
                        label = v.StationName,
                        value = f.VpnUser_id.ToString()
                    })
                    .ToListAsync();

                var _list = new List<object>();
                _list.Add(new
                {
                    label = "全部",
                    value = "-1"
                });
                _list.AddRange(list);

                res.Code = 200;
                res.Message = "查询成功";
                res.Data = _list;
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        /// <summary>
        /// 获取热源下拉框数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> getPowerSelect(ForecastBaseSearch search)
        {
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<VpnUser, PowerInfo, ForecastPowerInfo>((v, p, f) => new object[] {
                 JoinType.Inner,v.Id==p.VpnUser_id,
                 JoinType.Inner,v.Id==f.VpnUser_id
                })
                    .Where((v, p, f) => v.StationStandard == 99)
                    .Where((v, p, f) => v.IsValid == true)
                    .WhereIF(search.HeatNet_id > 0, (v, p, f) => p.ParentID == search.HeatNet_id)
                    .WhereIF(search.PowerInfo_id > 0, (v, p, f) => f.VpnUser_id == search.PowerInfo_id)
                    .OrderBy((v, p, f) => f.OrderSeq, OrderByType.Asc)
                    .Select((v, p, f) => new
                    {
                        label = v.StationName,
                        value = f.VpnUser_id.ToString()
                    })
                    .ToListAsync();

                var _list = new List<object>();
                _list.Add(new
                {
                    label = "全部",
                    value = "-1"
                });
                _list.AddRange(list);

                res.Code = 200;
                res.Message = "查询成功";
                res.Data = _list;
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        #endregion


        #region 初始化

        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <returns></returns>
        public async Task<object> InitData()
        {
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<VpnUser>().Where(x => x.StationStandard > 10 && x.IsValid == true).ToListAsync();
                var forecastHeatNetList = await Db.Queryable<ForecastHeatNetInfo>().ToListAsync();
                var forecastPowerList = await Db.Queryable<ForecastPowerInfo>().ToListAsync();

                //初始化热网信息
                var heatNetList = list.Where(x => x.StationStandard == 98).ToList();
                var _forecastHeatNetList = new List<ForecastHeatNetInfo>();
                foreach (var item in heatNetList)
                {
                    var _forecastHeatNet = forecastHeatNetList.Where(x => x.VpnUser_id == item.Id).FirstOrDefault();
                    if (_forecastHeatNet == null)
                    {
                        var _heatNetModel = new ForecastHeatNetInfo();
                        _heatNetModel.VpnUser_id = item.Id;
                        _heatNetModel.HeatTarget = (decimal)0.00;
                        _heatNetModel.StandardTemp = (decimal)0.00;
                        _heatNetModel.OrderSeq = item.Id;
                        _heatNetModel.IsValid = true;
                        _forecastHeatNetList.Add(_heatNetModel);
                    }
                }
                if (_forecastHeatNetList.Count > 0)
                {
                    await Db.Insertable<ForecastHeatNetInfo>(_forecastHeatNetList).ExecuteCommandAsync();
                }


                //初始化热源信息
                var powerList = list.Where(x => x.StationStandard == 99).ToList();
                var _forecastPowerList = new List<ForecastPowerInfo>();
                foreach (var item in powerList)
                {
                    var _forecastPower = forecastPowerList.Where(x => x.VpnUser_id == item.Id).FirstOrDefault();
                    if (_forecastPower == null)
                    {
                        var _powerModel = new ForecastPowerInfo();
                        _powerModel.VpnUser_id = item.Id;
                        _powerModel.ForecastSeq = item.Id;
                        _powerModel.OrderSeq = item.Id;
                        _powerModel.MaxPower = (decimal)0.00;
                        _powerModel.MinPower = (decimal)0.00;
                        _powerModel.IsValid = true;
                        _forecastPowerList.Add(_powerModel);
                    }
                }
                if (_forecastHeatNetList.Count > 0)
                {
                    await Db.Insertable<ForecastPowerInfo>(_forecastPowerList).ExecuteCommandAsync();
                }

                //初始化锅炉信息
                await Db.Deleteable<ForecastBoilerInfo>().Where(x => x.Id > 0).ExecuteCommandAsync();
                forecastPowerList = await Db.Queryable<ForecastPowerInfo>().ToListAsync();
                var _boilerList = new List<ForecastBoilerInfo>();
                foreach (var item in forecastPowerList)
                {
                    for (int i = 1; i < 7; i++)
                    {
                        var _boilerMode = new ForecastBoilerInfo();
                        _boilerMode.VpnUser_id = item.VpnUser_id;
                        _boilerMode.BoilerName = i.ToString() + "#锅炉";
                        _boilerMode.MaxPower = (decimal)0.00;
                        _boilerMode.MinPower = (decimal)0.00;
                        _boilerMode.OrderSeq = i;
                        _boilerMode.IsValid = false;
                        _boilerList.Add(_boilerMode);
                    }
                }
                if (_boilerList.Count > 0)
                    await Db.Insertable<ForecastBoilerInfo>(_boilerList).ExecuteCommandAsync();


                //初始化自动预测配置信息
                var configData = await Db.Queryable<ForecastConfig>().ToListAsync();
                if (configData.Count < 1)
                {
                    var _configMode = new ForecastConfig();
                    _configMode.ForecastTime = Convert.ToDateTime("08:00:00.000");
                    _configMode.IsValid = false;
                    await Db.Insertable<ForecastConfig>(_configMode).ExecuteCommandAsync();
                }

                res.Code = 200;
                res.Message = "基本信息初始化完成";
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }


        #endregion


        #region 热网信息维护

        /// <summary>
        /// 获取热网信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<object> getHeatNet(ForecastPage search)
        {
            var total = 0;
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<VpnUser, ForecastHeatNetInfo>((v, f) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id
                })
                    .Where((v, f) => v.StationStandard == 98)
                    .Where((v, f) => v.IsValid == true)
                    .Where((v, f) => f.IsValid == true)
                    .WhereIF(search.HeatNet_id > 0, (v, f) => f.VpnUser_id == search.HeatNet_id)
                    .Select((v, f) => new
                    {
                        v.StationName,
                        f.Id,
                        f.VpnUser_id,
                        f.HeatTarget,
                        f.StandardTemp,
                        f.OrderSeq,
                        f.IsValid
                    })
                    .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "OrderSeq asc" : search.SortColumn + " " + search.SortType)
                    .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);


                var data = new
                {
                    Total = total,
                    Data = list
                };

                res.Code = 200;
                res.Message = "查询成功";
                res.Data = data;
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }


        /// <summary>
        /// 修改热网信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> updHeatNet(ForecastHeatNetInfo model)
        {
            var res = new ForecastRes();
            try
            {
                var result = await Db.Updateable<ForecastHeatNetInfo>(model).ExecuteCommandAsync();

                res.Code = 200;
                if (result > 0)
                    res.Message = "更新成功";
                else
                    res.Message = "更新失败";
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        #endregion

        #region 热源信息维护
        /// <summary>
        /// 获取热源信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<object> getPowerInfo(ForecastPage search)
        {
            var res = new ForecastRes();
            var total = 0;
            var list = new List<object>();
            var powerList = new List<ForecastPowerInfoDto>();
            var boilerList = new List<ForecastBoilerInfo>();
            try
            {
                boilerList = await getBoilerInfoData(0);

                powerList = await Db.Queryable<VpnUser, ForecastPowerInfo, PowerInfo, VpnUser, ForecastHeatNetInfo>((v, f, p, n, m) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id,
                 JoinType.Inner,v.Id==p.VpnUser_id,
                 JoinType.Inner,p.ParentID==n.Id,
                 JoinType.Inner,n.Id==m.VpnUser_id
                })
                            .Where((v, f, p, n, m) => v.StationStandard == 99)
                            .Where((v, f, p, n, m) => v.IsValid == true)
                            .WhereIF(search.HeatNet_id > 0, (v, f, p, n, m) => n.Id == search.HeatNet_id)
                            .WhereIF(search.PowerInfo_id > 0, (v, f, p, n, m) => f.VpnUser_id == search.PowerInfo_id)
                            .OrderBy((v, f, p, n, m) => m.OrderSeq, OrderByType.Asc)
                            .OrderBy((v, f, p, n, m) => f.OrderSeq, OrderByType.Asc)
                            .Select((v, f, p, n, m) => new ForecastPowerInfoDto
                            {
                                Id = f.Id,
                                VpnUser_id = f.VpnUser_id,
                                HeatNetName = n.StationName,
                                HeatNet_id = n.Id,
                                StationName = v.StationName,
                                ForecastSeq = f.ForecastSeq,
                                OrderSeq = f.OrderSeq,
                                MaxPower = f.MaxPower,
                                MinPower = f.MinPower,
                                IsValid = f.IsValid
                            })
                            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 100 : search.PageSize, total);


                foreach (var item in powerList)
                {
                    var _boilerData = boilerList.Where(x => x.VpnUser_id == item.VpnUser_id).ToList();
                    var EditData = new
                    {
                        PowerData = item,
                        BoilerData = _boilerData
                    };

                    var _boilerValidData = _boilerData.Where(x => x.IsValid == true).ToList();
                    item.MaxPower = item.MaxPower + _boilerValidData.Sum(x => x.MaxPower);
                    item.MinPower = item.MinPower + _boilerValidData.Sum(x => x.MinPower);

                    list.Add(new
                    {
                        TableData = item,
                        EditData = EditData
                    });
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
        /// 获取锅炉信息
        /// </summary>
        /// <param name="vpnId">热源id</param>
        /// <returns></returns>
        public async Task<List<ForecastBoilerInfo>> getBoilerInfoData(int vpnId)
        {
            var list = new List<ForecastBoilerInfo>();
            try
            {
                list = await Db.Queryable<ForecastBoilerInfo>()
                    .WhereIF(vpnId > 0, f => f.VpnUser_id == vpnId)
                    .OrderBy(f => f.OrderSeq, OrderByType.Asc)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return list;
            }
            return list;
        }

        /// <summary>
        /// 设置热源及锅炉信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<object> updPowerAndBolerInfo(EditBoilerDto dto)
        {
            var res = new ForecastRes();
            try
            {
                if (dto.PowerInfoDto != null && dto.PowerInfoDto.Id > 0)
                {
                    ForecastPowerInfo pinfo = new ForecastPowerInfo();
                    pinfo.Id = dto.PowerInfoDto.Id;
                    pinfo.VpnUser_id = dto.PowerInfoDto.VpnUser_id;
                    pinfo.ForecastSeq = dto.PowerInfoDto.ForecastSeq;
                    pinfo.OrderSeq = dto.PowerInfoDto.OrderSeq;
                    pinfo.MaxPower = dto.PowerInfoDto.MaxPower;
                    pinfo.MinPower = dto.PowerInfoDto.MinPower;
                    pinfo.IsValid = dto.PowerInfoDto.IsValid;
                    await Db.Updateable<ForecastPowerInfo>(pinfo).ExecuteCommandAsync();
                }

                if (dto.BoilerInfoDto != null && dto.BoilerInfoDto.Count > 0)
                    await Db.Updateable<ForecastBoilerInfo>(dto.BoilerInfoDto).ExecuteCommandAsync();


                res.Code = 200;
                res.Message = "更新成功";
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        #endregion


        #region 自动预测配置信息维护

        /// <summary>
        /// 查询自动预测配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<object> getForecastConfig()
        {
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<ForecastConfig>().ToListAsync();
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
        /// 修改自动预测配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> updForecastConfig(ForecastConfig model)
        {
            var res = new ForecastRes();
            try
            {
                var result = await Db.Updateable<ForecastConfig>(model).ExecuteCommandAsync();

                res.Code = 200;
                if (result > 0)
                    res.Message = "更新成功";
                else
                    res.Message = "更新失败";
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        #endregion


        #region 预测方法

        /// <summary>
        /// 手动预测
        /// </summary>
        /// <returns></returns>
        public async Task<object> ManualForecast()
        {
            var res = new ForecastRes();
            try
            {
                var re = await Db.Ado.UseStoredProcedure().GetDataTableAsync("ForecastCalculate", new { funtype = 0 });
                res.Code = 200;
                res.Message = "预测成功";
            }
            catch (Exception e)
            {
                res.Code = 500;
                res.Message = e.Message;
            }
            return JsonConvert.SerializeObject(res);
        }

        /// <summary>
        /// 自动预测
        /// </summary>
        /// <returns></returns>
        public async Task<object> AutoForecast()
        {
            var res = new ForecastRes();
            try
            {
                var re = await Db.Ado.UseStoredProcedure().GetDataTableAsync("ForecastCreateAutoPlan");

                res.Code = 200;
                res.Message = "自动预测创建成功";
            }
            catch (Exception e)
            {
                if (e.Message == "指定的 @job_name ('AutoForecast')不存在。")
                {
                    res.Code = 200;
                    res.Message = "自动预测创建成功";
                }
                else
                {
                    res.Code = 500;
                    res.Message = e.Message;
                }

            }
            return JsonConvert.SerializeObject(res);
        }

        #endregion
    }
}
