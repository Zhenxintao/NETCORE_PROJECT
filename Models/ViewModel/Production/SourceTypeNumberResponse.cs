using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel.Production
{
    /// <summary>
    /// 生产调度热源管理类型数量
    /// </summary>
    public class SourceTypeNumberResponse
    {
        /// <summary>
        /// 总数量
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 集中供热数量
        /// </summary>
        public int TotalCentralizedHeat { get; set; }

        /// <summary>
        /// 区域供热数量
        /// </summary>
        public int TotalRegionalHeating { get; set; }
    }
}
