using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models
{
    /// <summary>
    /// 查询基础类
    /// </summary>
    public class ValveSearchBase : Page
    {
        /// <summary>
        /// 阀门编号
        /// </summary>
        public string DeviceCode { get; set; }
    }
}
