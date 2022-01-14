using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto
{
    /// <summary>
    /// 全网平衡历史查询类
    /// </summary>
    public class netsHisSearch : Page
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string organ_id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }
    }
}
