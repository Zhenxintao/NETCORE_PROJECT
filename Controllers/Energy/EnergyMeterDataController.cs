using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Service.Energy;

namespace THMS.Core.API.Controllers.Energy
{
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Energy")]
    [ApiController]
    public class EnergyMeterDataController : ControllerBase
    {
        /// <summary>
        /// 查询耗热量曲线，柱状图，表格数据。
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public List<MeterDataListDto> ListmeterDataList(SearchDto dto)
        {
            var result = new MeterDataEnergyService().ListmeterDataList(dto);
            return result;
        }

        /// <summary>
        /// 两日能耗对比，同比，环比柱状图
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public List<object> YtdaymeterList(SearchDto dto)
        {
            var result = new MeterDataEnergyService().YtdaymeterList(dto);
            return result;
        }

        /// <summary>
        /// 今年、去年总能耗数据及增长值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public object SumEnergyList(SearchDto dto)
        {
            var result = new MeterDataEnergyService().SumEnergyList(dto);
            return result;
        }

        /// <summary>
        /// 计划指标能耗数据展示
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public object PlanEnergyList(SearchDto dto)
        {
            var result = new MeterDataEnergyService().PlanEnergyList(dto);
            return result;
        }

        /// <summary>
        /// 添加能耗指标配置表
        /// </summary>
        /// <param name="meterTargetConfig"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool AddMeterTargetConfig(MeterTargetConfig meterTargetConfig)
        {
            bool result = new MeterTargetConfigService().AddMeterTargetConfig(meterTargetConfig);
            return result;
        }

        /// <summary>
        /// 删除能耗指标配置表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpDelete]
        public bool DelMeterTargetConfig(int id)
        {
            bool result = new MeterTargetConfigService().DelMeterTargetConfig(id);
            return result;
        }

        /// <summary>
        /// 修改能耗指标配置表
        /// </summary>
        /// <param name="meterTargetConfig"></param>
        /// <returns></returns>
        /// 
        [HttpPut]
        public bool UpdMeterTargetConfig(MeterTargetConfig meterTargetConfig)
        {
            bool result = new MeterTargetConfigService().UpdMeterTargetConfig(meterTargetConfig);
            return result;
        }

        /// <summary>
        /// 查询能耗指标配置表
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public List<MeterTargetConfig> AddMeterTargetConfig()
        {
            var resultList = new MeterTargetConfigService().SelMeterTargetConfig();
            return resultList;
        }
    }
}