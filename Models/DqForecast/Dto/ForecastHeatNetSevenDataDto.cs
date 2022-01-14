using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastHeatNetSevenDataDto: ForecastPowerSevenDataDto
    {
        /// <summary>
        /// 子列表
        /// </summary>
        public object children { get; set; }
    }
}
