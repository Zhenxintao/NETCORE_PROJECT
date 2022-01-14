using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.Dto
{
    public class EditBoilerDto
    {
        /// <summary>
        /// 热源信息
        /// </summary>
        public ForecastPowerInfoDto PowerInfoDto { get; set; }

        /// <summary>
        /// 锅炉信息
        /// </summary>
        public List<ForecastBoilerInfo> BoilerInfoDto { get; set; }
    }
}
