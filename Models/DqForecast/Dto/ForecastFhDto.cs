using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    public class ForecastFhDto
    {
        /// <summary>
        /// 热源id
        /// </summary>
        public int VpnUser_id { get; set; }

        /// <summary>
        /// 热源名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 标准负荷高限（GJ/h）
        /// </summary>
        public decimal? MaxPower { get; set; }

        /// <summary>
        /// 标准负荷低限（GJ/h）
        /// </summary>
        public decimal? MinPower { get; set; }
    }
}
