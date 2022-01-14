using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast
{
    /// <summary>
    /// 热网信息
    /// </summary>
    public class ForecastHeatNetInfo
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 热网id
        /// </summary>
        public int VpnUser_id { get; set; }

        /// <summary>
        /// 热指标
        /// </summary>
        public decimal? HeatTarget { get; set; }

        /// <summary>
        /// 室温基准值
        /// </summary>
        public decimal? StandardTemp { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int OrderSeq { get; set; }

        /// <summary>
        /// True:参与预测，False:不参与预测
        /// </summary>
        public bool IsValid { get; set; }
    }
}
