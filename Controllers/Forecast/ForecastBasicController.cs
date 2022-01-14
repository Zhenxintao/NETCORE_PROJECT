using System.Collections.Generic;
using ApiModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Service.Forecast;

namespace THMS.Core.API.Controllers.Forecast
{
    /// <summary>
    /// 换热站负荷预测基础数据控制器
    /// </summary>
    [Route("api/[controller]/[action]"),ApiExplorerSettings(GroupName = "Forecast")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastBasicController : ControllerBase
    {
        /// <summary>
        /// 换热站负荷预测基础数据表查询信息
        /// </summary>
        /// <param name="rows">分页条数</param>
        /// <param name="page">当前页数</param>
        /// <returns></returns>
        [HttpGet]
        public object SelStationForecastBasic(int rows, int page)
        {
            var result = new StationForecastBasicService().SelStationForecastBasic(rows, page);
            ResultMessgeData m = new ResultMessgeData();
            m.rows = result.Item1;
            m.total = result.Item2;
            return m;
        }

        /// <summary>
        /// 换热站负荷预测基础数据表新增信息
        /// </summary>
        /// <param name="stationForecastBasic">StationForecastBasic实体类</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool AddStationForecastBasic(StationForecastBasic stationForecastBasic)
        {
            
            bool con = new StationForecastBasicService().AddStationForecastBasic(stationForecastBasic);

            return con;

        }

        /// <summary>
        /// 换热站负荷预测基础数据表删除(5.0使用)
        /// </summary>
        /// <param name="id">自增Id</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool DelStationForecastBasic(int id)
        {
            bool con = new StationForecastBasicService().DelStationForecastBasic(id);
            return con;
        }

        /// <summary>
        /// 换热站负荷预测基础数据表删除(最新版Web使用)
        /// </summary>
        /// <param name="id">自增Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public bool RemoveStationForecastBasic(int id)
        {
            bool con = new StationForecastBasicService().DelStationForecastBasic(id);
            return con;
        }

        /// <summary>
        /// 换热站负荷预测基础数据表更新修改信息
        /// </summary>
        /// <param name="stationForecastBasic">StationForecastBasic实体类</param>
        /// <returns></returns>
        /// 
        [HttpPut]
        public bool UpdStationForecastBasic(StationForecastBasic stationForecastBasic)
        {
            bool con = new StationForecastBasicService().UpdStationForecastBasic(stationForecastBasic);
            return con;
        }
    }
}