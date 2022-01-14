using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    /// <summary>
    /// 换热站地板采暖散热量数据表
    /// </summary>
    public class StationForecastRadiatingTube
    {
        /// <summary>
        /// 自增
        /// </summary>
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 平均水温
        /// </summary>
        public decimal Avgtemperature { get; set; }

        /// <summary>
        /// 室内温度
        /// </summary>
        public decimal IndoorCalculationTemp { get; set; }

        /// <summary>
        /// 散热管间距300
        /// </summary>
        public decimal RadiatingTubeTH { get; set; }

        /// <summary>
        /// 散热管间距250
        /// </summary>
        public decimal RadiatingTubeSHF { get; set; }

        /// <summary>
        /// 散热管间距200
        /// </summary>
        public decimal RadiatingTubeSH { get; set; }

        /// <summary>
        /// 散热管间距150
        /// </summary>
        public decimal RadiatingTubeOHF { get; set; }
    }
}
