using ApiModel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.DqForecast;
using THMS.Core.API.Models.DqForecast.ReturnModel;
using THMS.Core.API.Models.DqForecast.SearchModel;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.DqForecast
{
    /// <summary>
    /// 预测历史
    /// </summary>
    public class ForecastDayHisService : DbContextSqlSugar
    {
        /// <summary>
        /// 查询预测历史信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<object> GetData(ForecastDateSearch search)
        {
            var total = 0;
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<VpnUser, ForecastDataHis>((v, f) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id
                })
                    .Where((v, f) => v.StationStandard == 98)
                    .Where((v, f) => v.IsValid == true)
                    .WhereIF(search.HeatNet_id > 0, (v, f) => f.VpnUser_id == search.HeatNet_id)
                    .WhereIF(DateTime.TryParse(search.beginTime.ToString("yyyy-MM-dd 00:00:00.000"), out DateTime _beginTime), (v, f) => f.ForecastDate >= _beginTime)
                    .WhereIF(DateTime.TryParse(search.endTime.ToString("yyyy-MM-dd 23:59:59.000"), out DateTime _endTime), (v, f) => f.ForecastDate <= _endTime)
                    .Select((v, f) => new
                    {
                        v.StationName,
                        f.Id,
                        f.VpnUser_id,
                        f.ForecastDate,
                        f.HotArea,
                        f.HeatTarget,
                        f.StandardTemp,
                        f.OutDoorTemp,
                        f.RealHeat,
                        f.ForecastHeat
                    })
                    .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "ForecastDate desc" : search.SortColumn + " " + search.SortType)
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
        /// 查询预测历史信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<object> GetChartData(ForecastDateSearch search)
        {
            var res = new ForecastRes();
            try
            {
                var list = await Db.Queryable<VpnUser, ForecastDataHis>((v, f) => new object[] {
                 JoinType.Inner,v.Id==f.VpnUser_id
                })
                    .Where((v, f) => v.StationStandard == 98)
                    .Where((v, f) => v.IsValid == true)
                    .WhereIF(search.HeatNet_id > 0, (v, f) => f.VpnUser_id == search.HeatNet_id)
                    .WhereIF(DateTime.TryParse(search.beginTime.ToString("yyyy-MM-dd 00:00:00.000"), out DateTime _beginTime), (v, f) => f.ForecastDate >= _beginTime)
                    .WhereIF(DateTime.TryParse(search.endTime.ToString("yyyy-MM-dd 23:59:59.000"), out DateTime _endTime), (v, f) => f.ForecastDate <= _endTime)
                    .OrderBy((v, f) => f.ForecastDate, OrderByType.Asc)
                    .Select((v, f) => new
                    {
                        v.StationName,
                        f.ForecastDate,
                        f.OutDoorTemp,
                        f.RealHeat,
                        f.ForecastHeat
                    })
                    .ToListAsync();

                var _list = new List<object>();
                var groups = list.GroupBy(p => Convert.ToDateTime(Convert.ToDateTime(p.ForecastDate).ToString("yyyy-MM-dd")));
                foreach (var group in groups)
                {
                    var Time = "";
                    var Tags = new List<object>();
                    foreach (var model in group)
                    {
                        Time = Convert.ToDateTime(model.ForecastDate).ToString("yyyy-MM-dd");
                        Tags.Add(new
                        {
                            Name = model.StationName + " - 室外温度",
                            Value = model.OutDoorTemp,
                            Unit = "℃"
                        });
                        Tags.Add(new
                        {
                            Name = model.StationName + " - 实际瞬时热量",
                            Value = model.RealHeat,
                            Unit = "GJ/h"
                        });
                        Tags.Add(new
                        {
                            Name = model.StationName + " - 预测瞬时热量",
                            Value = model.ForecastHeat,
                            Unit = "GJ/h"
                        });
                    }
                    _list.Add(new
                    {
                        Time = Time,
                        Tags = Tags
                    });
                }

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
    }
}
