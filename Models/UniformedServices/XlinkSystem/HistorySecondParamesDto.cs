using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    public class HistorySecondParamesDto
    {
        /// <summary>
        /// 换热站Id
        /// </summary>
        public int stationId { get; set; }
        /// <summary>
        /// 机组Id
        /// </summary>
        public int narray_no { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }
        /// <summary>
        /// 检测点string集合指标
        /// </summary>
        public string[] keyList { get; set; }
    }
}
