using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast
{
    /// <summary>
    /// 热源信息
    /// </summary>
    public class ForecastPowerInfo
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 热源id
        /// </summary>
        public int VpnUser_id { get; set; }

        /// <summary>
        /// 参与预测的优先级
        /// </summary>
        public int ForecastSeq { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int OrderSeq { get; set; }

        /// <summary>
        /// 标准负荷高限（GJ/h）
        /// </summary>
        public decimal? MaxPower { get; set; }

        /// <summary>
        /// 标准负荷低限（GJ/h）
        /// </summary>
        public decimal? MinPower { get; set; }

        /// <summary>
        /// True：参与预测，False：不参与预测
        /// </summary>
        public bool IsValid { get; set; }
    }
}
