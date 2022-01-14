using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.DqForecast.SearchModel;
using THMS.Core.API.Service.DqForecast;

namespace THMS.Core.API.Controllers.UniformedServices.ForecastSystem
{
    /// <summary>
    /// 热网负荷预测历史数据控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "ForecastSystem")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastDayHisController : ControllerBase
    {
        ForecastDayHisService service = new ForecastDayHisService();

        /// <summary>
        /// 查询预测历史数据填充表格
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> GetData(ForecastDateSearch search)
        {
            return await service.GetData(search);
        }

        /// <summary>
        /// 查询预测历史数据填充曲线
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> GetChartData(ForecastDateSearch search)
        {
            return await service.GetChartData(search);
        }
    }
}
