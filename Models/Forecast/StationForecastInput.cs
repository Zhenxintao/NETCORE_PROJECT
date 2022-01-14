using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiModel
{
    /// <summary>
    /// 换热站负荷预测数据输入表（StationForecastInput）
    /// </summary>
    public class StationForecastInput
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 换热站Id
        /// </summary>
        public int VpnUserId { get; set; }

        /// <summary>
        /// 换热站名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public decimal HeatArea { get; set; }

        /// <summary>
        /// 面积热指标
        /// </summary>
        public decimal HeatTarget { get; set; }

        /// <summary>
        /// 当年实现的热耗
        /// </summary>
        public decimal OverallHeat { get; set; }

        /// <summary>
        /// 二次供水温度
        /// </summary>
        public decimal SEC_TEMP_S { get; set; }

        /// <summary>
        /// 二次回水温度
        /// </summary>
        public decimal SEC_TEMP_R { get; set; }

        /// <summary>
        /// 二次相对流量
        /// </summary>
        public decimal SeNetworkRelativeFlow { get; set; }

        /// <summary>
        /// 当前室外温度补偿值
        /// </summary>
        public decimal OutdoorAtPresentOffset { get; set; }

        /// <summary>
        /// 调整系数
        /// </summary>
        public decimal AdCoefficient { get; set; }

        /// <summary>
        /// 采暖方式,1地板采暖，2挂片采暖，3混合
        /// </summary>
        public int HeatingType { get; set; }

        /// <summary>
        /// 地板采暖散热管间距类型（1：300mm;2：250mm;3：200mm;4：150mm）
        /// </summary>
        public int RadiatingTubeType { get; set; }

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

        /// <summary>
        /// 机组号
        /// </summary>
        public int StationBranchArrayNumber { get; set; }
    }
}
