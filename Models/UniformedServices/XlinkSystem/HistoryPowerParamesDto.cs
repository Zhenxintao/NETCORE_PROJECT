using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 热源历史数据
    /// </summary>
    public class HistoryPowerParamesDto
    {
        /// <summary>
        /// 热源Id
        /// </summary>
        public int sourceId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }
        /// <summary>
        /// 检测点string集合
        /// </summary>
        public string[] keyList { get; set; }
    }
}
