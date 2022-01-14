using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast
{
    /// <summary>
    /// 预测信息
    /// </summary>
    public class ForecastData
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 热网、热源id
        /// </summary>
        public int VpnUser_id { get; set; }

        /// <summary>
        /// 预测日期
        /// </summary>
        public DateTime? ForecastDate { get; set; }

        /// <summary>
        /// 供热面积
        /// </summary>
        public decimal? HotArea { get; set; }

        /// <summary>
        /// 热指标
        /// </summary>
        public decimal? HeatTarget { get; set; }

        /// <summary>
        /// 室温基准值
        /// </summary>
        public decimal? StandardTemp { get; set; }

        /// <summary>
        /// 室外温度
        /// </summary>
        public decimal? OutDoorTemp { get; set; }

        /// <summary>
        /// 实际瞬时热量
        /// </summary>
        public decimal? RealHeat { get; set; }

        /// <summary>
        /// 预测瞬时热量
        /// </summary>
        public decimal? ForecastHeat { get; set; }
    }
}
