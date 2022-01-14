using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast
{
    /// <summary>
    /// 锅炉信息
    /// </summary>
    public class ForecastBoilerInfo
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
        /// 锅炉名称
        /// </summary>
        public string BoilerName { get; set; }

        /// <summary>
        /// 标准负荷高限（GJ/h）
        /// </summary>
        public decimal? MaxPower { get; set; }

        /// <summary>
        /// 标准负荷低限（GJ/h）
        /// </summary>
        public decimal? MinPower { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int OrderSeq { get; set; }

        /// <summary>
        /// True:阶跃，False：不阶跃
        /// </summary>
        public bool IsValid { get; set; }
    }
}
