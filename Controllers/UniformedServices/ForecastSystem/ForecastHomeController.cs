using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.DqForecast.Dto;
using THMS.Core.API.Models.DqForecast.SearchModel;
using THMS.Core.API.Service.DqForecast;

namespace THMS.Core.API.Controllers.UniformedServices.ForecastSystem
{
    /// <summary>
    /// 热网负荷预测首页
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "ForecastSystem")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastHomeController : ControllerBase
    {
        ForecastHomeService service = new ForecastHomeService();

        /// <summary>
        /// 获取预测数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getData(ForecastBaseSearch search) => await service.getData(search);

        /// <summary>
        /// 获取预测曲线数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getChartData(ForecastBaseSearch search) => await service.getChartData(search);


        /// <summary>
        /// 从pvss同步热网热指标
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> PvssConfigSet(List<PvssSet> list) => await service.PvssConfigSet(list);


        /// <summary>
        /// 获取热源最大、最小负荷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getPowerFh() => await service.getPowerFh();
    }
}
