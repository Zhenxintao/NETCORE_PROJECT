using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    /// <summary>
    /// 能耗指标配置表
    /// </summary>
   public class MeterTargetConfig
    {
        /// <summary>
        /// Id 自增编号
        /// </summary>
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 供暖期id
        /// </summary>
        public int HeatCycle_id { get; set; }

        /// <summary>
        /// 公司总热耗W/m2
        /// </summary>
        public decimal Heat_ComTarget { get; set; }

        /// <summary>
        /// 公司度日数能耗kJ/(m2•℃•d)
        /// </summary>
        public decimal Heat_ComTotal { get; set; }

        /// <summary>
        /// 民用节能建筑热耗W/m2
        /// </summary>
        public decimal Heat_EnergyTarget { get; set; }

        /// <summary>
        /// 民用节能建筑度日数能耗kJ/(m2•℃•d)
        /// </summary>
        public decimal Heat_EnergyTotal { get; set; }

        /// <summary>
        /// 热力站单位面积耗电量Kwh/万m2.h
        /// </summary>
        public decimal Elec_Target { get; set; }

        /// <summary>
        /// 热力站单位面积耗电量kwh/m2
        /// </summary>

        public decimal Elec_Total { get; set; }

        /// <summary>
        /// 综合水耗T/万m2.天
        /// </summary>

        public decimal Water__Target { get; set; }

        /// <summary>
        /// 综合水耗kg/m2
        /// </summary>
        public decimal Water_Total { get; set; }

    }
}
