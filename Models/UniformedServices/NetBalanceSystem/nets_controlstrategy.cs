using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 全网平衡
    /// </summary>
    public class nets_controlstrategy
    {
        /// <summary>
        /// Desc:实时数据有效时间
        /// Default:30
        /// Nullable:True
        /// </summary>           
        public int? EffectiveTime { get; set; }

        /// <summary>
        /// Desc:最小调控幅度
        /// Default:1.00
        /// Nullable:True
        /// </summary>           
        public decimal? MinRange { get; set; }

        /// <summary>
        /// Desc:运行质量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? Quality { get; set; }

        /// <summary>
        /// Desc:分区系统二次供水温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? SEC_TEMP_S { get; set; }

        /// <summary>
        /// Desc:最终目标温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? FinalTargetTemp { get; set; }

        /// <summary>
        /// Desc:换热站机组分区编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? NarrayNo { get; set; }

        /// <summary>
        /// Desc:调控周期（分钟）
        /// Default:30
        /// Nullable:True
        /// </summary>           
        public int? RegulationCycle { get; set; }

        /// <summary>
        /// Desc:目标温度死区负值
        /// Default:-0.50
        /// Nullable:True
        /// </summary>           
        public decimal? TargetDeadbandNegative { get; set; }

        /// <summary>
        /// Desc:所供面积
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HotArea { get; set; }

        /// <summary>
        /// Desc:平衡策略记录iD
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Nets_ControlRecord_id { get; set; }

        /// <summary>
        /// Desc:目标温度调控模式：0手动，1自动,2 二网回温
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? RegulationMode { get; set; }

        /// <summary>
        /// Desc:目标温度死区正值
        /// Default:0.50
        /// Nullable:True
        /// </summary>           
        public decimal? TargetDeadbandPositive { get; set; }

        /// <summary>
        /// Desc:总户数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? Households { get; set; }

        /// <summary>
        /// Desc:二网平均温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? NetsAvgTemp { get; set; }

        /// <summary>
        /// Desc:调控状态：true 开启
        /// Default:b'0'
        /// Nullable:True
        /// </summary>           
        public bool? RegulationState { get; set; }

        /// <summary>
        /// Desc:目标温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? TargetTemp { get; set; }

        /// <summary>
        /// Desc:户阀总数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? HouseholdValveTotal { get; set; }

        /// <summary>
        /// Desc:下一调控时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? NextRegulatoryTime { get; set; }

        /// <summary>
        /// Desc:备注
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Remarks { get; set; }

        /// <summary>
        /// Desc:目标温度补偿值
        /// Default:0.00
        /// Nullable:True
        /// </summary>           
        public decimal? TargetTempCompensate { get; set; }

        /// <summary>
        /// Desc:自增id
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:参与平衡阀数量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? PartakeBalanceValve { get; set; }

        /// <summary>
        /// Desc:分区平均室内温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? RoomTempAvg { get; set; }

        /// <summary>
        /// Desc:调节阀总数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ValveTotal { get; set; }

        /// <summary>
        /// Desc:平衡度(百分比)
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? BalanceDegree { get; set; }

        /// <summary>
        /// Desc:调试)阀门下发：true 是
        /// Default:b'0'
        /// Nullable:True
        /// </summary>           
        public bool? IsSendOpen { get; set; }

        /// <summary>
        /// Desc:参与调控阀数量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? PartakeControlValve { get; set; }

        /// <summary>
        /// Desc:室温达标率
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? RoomTempPassRate { get; set; }

        /// <summary>
        /// Desc:换热站ID
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string VpnUser_id { get; set; }

        /// <summary>
        /// Desc:控制策略：1回温
        /// Default:1
        /// Nullable:True
        /// </summary>           
        public int? ControlPolicy { get; set; }

        /// <summary>
        /// Desc:最大调控幅度
        /// Default:5.00
        /// Nullable:True
        /// </summary>           
        public decimal? MaxRange { get; set; }

        /// <summary>
        /// Desc:换热站系统分区平衡度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PartitionBalanceDegree { get; set; }

        /// <summary>
        /// Desc:分区系统二次回水温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? SEC_TEMP_R { get; set; }
    }
}
