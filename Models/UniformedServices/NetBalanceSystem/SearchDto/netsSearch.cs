using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto
{
    /// <summary>
    /// 全网平衡查询类
    /// </summary>
    public class netsSearch : Page
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public string organ_id { get; set; }
    }
}
