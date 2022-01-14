using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto
{
    /// <summary>
    /// 单元阀历史查询类
    /// </summary>
    public class UnitHisSearch
    {
        /// <summary>
        /// 单元阀编号
        /// </summary>
        public int unitId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        /// 监测指标 tagname
        /// </summary>
        public string keyList { get; set; }
    }
}
