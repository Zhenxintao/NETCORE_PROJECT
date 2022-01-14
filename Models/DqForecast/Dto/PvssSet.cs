using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    public class PvssSet
    {
        /// <summary>
        /// pvss的id
        /// </summary>
        public string Pvss_id { get; set; }

        /// <summary>
        /// 热指标
        /// </summary>
        public decimal? HeatTarget { get; set; }
    }
}
