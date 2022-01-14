using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastPowerSevenDataDto
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
        /// 热网、热源名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 供热面积
        /// </summary>
        public decimal? HotArea { get; set; }

        /// <summary>
        /// 热指标
        /// </summary>
        public decimal? HeatTarget { get; set; }

        /// <summary>
        /// 实际瞬时热量
        /// </summary>
        public decimal? RealHeat { get; set; }

        /// <summary>
        /// 当天预测瞬时热量
        /// </summary>
        public decimal? TodayForecastHeat { get; set; }

        /// <summary>
        /// 第二天预测瞬时热量
        /// </summary>
        public decimal? SecondDayForecastHeat { get; set; }

        /// <summary>
        /// 第三天预测瞬时热量
        /// </summary>
        public decimal? ThirdDayForecastHeat { get; set; }

        /// <summary>
        /// 第四天预测瞬时热量
        /// </summary>
        public decimal? ForthDayForecastHeat { get; set; }

        /// <summary>
        /// 第五天预测瞬时热量
        /// </summary>
        public decimal? FivthDayForecastHeat { get; set; }

        /// <summary>
        /// 第六天预测瞬时热量
        /// </summary>
        public decimal? SixthDayForecastHeat { get; set; }

        /// <summary>
        /// 第七天预测瞬时热量
        /// </summary>
        public decimal? SeventhDayForecastHeat { get; set; }
    }
}
