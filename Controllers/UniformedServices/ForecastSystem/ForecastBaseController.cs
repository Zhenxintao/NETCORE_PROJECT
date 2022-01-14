using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.DqForecast;
using THMS.Core.API.Models.DqForecast.Dto;
using THMS.Core.API.Models.DqForecast.SearchModel;
using THMS.Core.API.Service.DqForecast;

namespace THMS.Core.API.Controllers.UniformedServices.ForecastSystem
{
    /// <summary>
    /// 热网负荷预测配置控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "ForecastSystem")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastBaseController : ControllerBase
    {
        ForecastBaseService service = new ForecastBaseService();

        #region 下拉框

        /// <summary>
        /// 获取热网下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getHeatNetSelect() => await service.getHeatNetSelect();

        /// <summary>
        /// 获取热源下拉框数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getPowerSelect(ForecastBaseSearch search) => await service.getPowerSelect(search);

        #endregion

        #region 初始化

        /// <summary>
        /// 预测基本信息初始化
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> InitData() => await service.InitData();

        #endregion

        #region 热网信息维护
        /// <summary>
        /// 获取热网信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getHeatNet(ForecastPage search) => await service.getHeatNet(search);

        /// <summary>
        /// 修改热网信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> updHeatNet(ForecastHeatNetInfo model) => await service.updHeatNet(model);

        #endregion

        #region 热源信息维护
        /// <summary>
        /// 获取热源信息
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getPowerInfo(ForecastPage search) => await service.getPowerInfo(search);

        /// <summary>
        /// 设置热源及锅炉信息
        /// </summary>
        /// <param name="dto">热源信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> updPowerAndBolerInfo(EditBoilerDto dto) => await service.updPowerAndBolerInfo(dto);
      

        #endregion

        #region 自动预测配置信息维护

        /// <summary>
        /// 查询自动预测配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> getForecastConfig() => await service.getForecastConfig();

        /// <summary>
        /// 修改自动预测配置信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> updForecastConfig(ForecastConfig model) => await service.updForecastConfig(model);

        #endregion

        #region 预测方法

        /// <summary>
        /// 手动预测
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> ManualForecast() => await service.ManualForecast();

        /// <summary>
        /// 自动预测
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> AutoForecast() => await service.AutoForecast();

        #endregion

    }
}
