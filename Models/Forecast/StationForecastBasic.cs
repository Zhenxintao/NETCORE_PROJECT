using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiModel
{
    /// <summary>
    /// 换热站负荷预测基础数据表（StationForecastBasic）
    /// </summary>
    public class StationForecastBasic
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 供暖季名称
        /// </summary>
        public string HeatingSeason { get; set; }

        /// <summary>
        /// 采暖天数
        /// </summary>
        public int HeatingDays { get; set; }

        /// <summary>
        /// 采暖季室外设计温度
        /// </summary>
        public decimal OutdoorTemperature { get; set; }

        /// <summary>
        /// 采暖季室外平均温度
        /// </summary>
        public decimal OutdoorActualAvgTemp { get; set; }

        /// <summary>
        /// 室内设计温度
        /// </summary>
        public decimal IndoorCalculationTemp { get; set; }

        /// <summary>
        /// 散热器系数
        /// </summary>
        public decimal RadiatorCoefficient { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
    }
}
