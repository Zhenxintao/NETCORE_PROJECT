using SqlSugar;
using System;

namespace ApiModel
{

    /// <summary>
    /// 换热站负荷预测实时天表（StationForecastRealDay）
    /// </summary>
    public class StationForecastRealDay
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
        /// 预测时间
        /// </summary>
        public DateTime ForecastDateDay { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public decimal HeatArea { get; set; }

        /// <summary>
        /// 预测热指标
        /// </summary>
        public decimal ForecastHeatTarget { get; set; }

        /// <summary>
        /// 当前热指标
        /// </summary>
        public decimal RealHeatTarget { get; set; }

        /// <summary>
        /// 地板表面平均温度
        /// </summary>
        public decimal FloorSurfaceAvgTemp { get; set; }

        /// <summary>
        /// 预测室外温度
        /// </summary>
        public decimal ForecastOutdoorTemp { get; set; }

        /// <summary>
        /// 实际室外温度
        /// </summary>
        public decimal RealOutdoorTemp { get; set; }

        /// <summary>
        /// 换热站二次侧循环流量
        /// </summary>
        public decimal ForecastSecFlow { get; set; }

        /// <summary>
        /// 预测流量
        /// </summary>
        public decimal ForecastFlow { get; set; }

        /// <summary>
        /// 负荷率
        /// </summary>
        public decimal LoadRate { get; set; }

        /// <summary>
        /// 日用热量
        /// </summary>
        public decimal HourlyHeatDay { get; set; }

        /// <summary>
        /// 预测二次供水温度
        /// </summary>
        public decimal ForecastSecSendTemp { get; set; }

        /// <summary>
        /// 预测二次回水温度
        /// </summary>
        public decimal ForecastSecReturnTemp { get; set; }

        /// <summary>
        /// 实际二次供水温度
        /// </summary>
        public decimal RealSecSendTemp { get; set; }

        /// <summary>
        /// 实际二次回水温度
        /// </summary>
        public decimal RealSecReturnTemp { get; set; }

        /// <summary>
        /// 采暖方式
        /// </summary>
        public int HeatingType { get; set; }

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
