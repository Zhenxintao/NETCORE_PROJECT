using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.ReturnModel
{
    public class ForecastRes
    {
        /// <summary>
        /// 200:正常，500:报错
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据集合
        /// </summary>
        public object Data { get; set; }
    }
}
