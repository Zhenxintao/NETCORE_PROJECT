using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Monitor
{
    /// <summary>
    /// 历史数据相关
    /// </summary>
    public class HistoryDataInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public int VpnUserId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 机组名称
        /// </summary>
        public string StationBranchName { get; set; }

        /// <summary>
        /// 机组面积
        /// </summary>
        public decimal StationBranchHeatArea { get; set; }

        /// <summary>
        /// 存储时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        public decimal? RealValue { get; set; }
    }

    /// <summary>
    /// 要查询的图表时间类型
    /// </summary>
    public enum ChartDateType
    {
        /// <summary>
        /// 分钟
        /// </summary>
        Minutes = 0,
        /// <summary>
        /// 小时
        /// </summary>
        Hours = 1,
        /// <summary>
        /// 天
        /// </summary>
        Days = 2,
    }
}
