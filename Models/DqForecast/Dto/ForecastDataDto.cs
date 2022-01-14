using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastDataDto : ForecastData
    {
        /// <summary>
        /// 热网、热源名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public int ParentId { get; set; }
    }
}
