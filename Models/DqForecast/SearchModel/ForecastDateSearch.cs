using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.SearchModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastDateSearch : ForecastPage
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime beginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime endTime { get; set; }
    }
}
