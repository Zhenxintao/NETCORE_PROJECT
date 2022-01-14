using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.SearchModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ForecastBaseSearch
    {
        /// <summary>
        /// 热网id
        /// </summary>
        public int HeatNet_id { get; set; }

        /// <summary>
        /// 热源id
        /// </summary>
        public int PowerInfo_id { get; set; }
    }
}
