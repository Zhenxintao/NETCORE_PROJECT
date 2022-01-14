using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastPowerInfoDto: ForecastPowerInfo
    {
        /// <summary>
        /// 所属热网id
        /// </summary>
        public int HeatNet_id { get; set; }

        /// <summary>
        /// 所属热网名称
        /// </summary>
        public string HeatNetName { get; set; }

        /// <summary>
        /// 热源名称
        /// </summary>
        public string StationName { get; set; }
    }
}
