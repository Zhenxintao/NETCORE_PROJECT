using ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Service.Energy
{
    public class MeterTargetConfigService:DbContext.DbContextSqlSugar
    {
        /// <summary>
        /// 添加能耗指标配置表
        /// </summary>
        /// <param name="meterTargetConfig"></param>
        /// <returns></returns>
        public bool AddMeterTargetConfig(MeterTargetConfig meterTargetConfig)
        {
            int con = Db.Insertable<MeterTargetConfig>(meterTargetConfig).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 删除能耗指标配置表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelMeterTargetConfig(int id)
        {
            int con = Db.Deleteable<MeterTargetConfig>().Where(s=>s.Id==id).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 修改能耗指标配置表
        /// </summary>
        /// <param name="meterTargetConfig"></param>
        /// <returns></returns>
        public bool UpdMeterTargetConfig(MeterTargetConfig meterTargetConfig)
        {
            int con = Db.Updateable<MeterTargetConfig>(meterTargetConfig).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 查询能耗指标配置表
        /// </summary>
        /// <returns></returns>
        public List<MeterTargetConfig> SelMeterTargetConfig()
        {
            var resultList = Db.Queryable<MeterTargetConfig>().ToList();
            return resultList;
        }
    }
}
