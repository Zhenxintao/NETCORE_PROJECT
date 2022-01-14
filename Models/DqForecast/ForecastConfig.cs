using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast
{
    /// <summary>
    /// 自动预测配置信息
    /// </summary>
    public class ForecastConfig
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 自动预测时间点
        /// </summary>
        public DateTime? ForecastTime { get; set; }

        /// <summary>
        /// True:自动预测，False：不自动预测
        /// </summary>
        public bool IsValid { get; set; }
    }
}
