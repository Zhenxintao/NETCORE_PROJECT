using System;
using System.Collections.Generic;
using System.Text;

namespace THMS.Core.API.Redis
{
   public  class Uv_Device_Valve
    {
      
        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public int? ControlMode { get; set; }

        /// <summary>
        /// Desc:站点名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CommunityName { get; set; }

        /// <summary>
        /// Desc:Valve_Set
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BuildingName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UnitNoName { get; set; }

        /// <summary>
        /// Desc:唯一标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UnitNo_id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DeviceName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DeviceCode { get; set; }
        public int? NarrayNo { get; set; }
        /// <summary>
        /// Desc:机组名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BranchName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? CorrectedValue { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? OpenUpperLimit { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? OpenLowerLimit { get; set; }

        /// <summary>
        /// Desc:单元阀Guid
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UV_DeviceInfo_id { get; set; }

        /// <summary>
        /// Desc:供水温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? TEMP_S { get; set; }

        /// <summary>
        /// Desc:回水温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? TEMP_R { get; set; }

        /// <summary>
        /// Desc:供水压力
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PRESS_S { get; set; }

        /// <summary>
        /// Desc:回水压力
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? PRESS_R { get; set; }

        /// <summary>
        /// Desc:瞬时热量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HEAT_S { get; set; }

        /// <summary>
        /// Desc:瞬时流量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? FLOW_S { get; set; }

        /// <summary>
        /// Desc:累计热量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HEAT_TOTAL { get; set; }

        /// <summary>
        /// Desc:累计流量
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? FLOW_TOTAL { get; set; }

        /// <summary>
        /// Desc:阀门实际开度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? VALVE_FACT { get; set; }

        /// <summary>
        /// Desc:阀门设定开度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? VALVE_SET { get; set; }

        /// <summary>
        /// Desc:目标温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? TARGET_TEMP { get; set; }

        /// <summary>
        /// Desc:偏差值
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? DEVIATION_VALUE { get; set; }

        /// <summary>
        /// Desc:平均室温
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? MEANROOM_TEMP { get; set; }

        /// <summary>
        /// Desc:修正温度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? CORRECTION_TEMP { get; set; }

        /// <summary>
        /// Desc:加权值
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? WEIGHTED_VALUE { get; set; }

        /// <summary>
        /// Desc:采集时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? TIMESTAMP { get; set; }

        /// <summary>
        /// Desc:工作模式：0正常，1节能
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? WORK_MODE { get; set; }

        /// <summary>
        /// Desc:扩展列，自定义json格式
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string ExtendedColumn { get; set; }


        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 安装Id
        /// </summary>
        public int? ConfigId { get; set; }
        public string Address => $"{StationName}{CommunityName}{BuildingName}{UnitNoName}";

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Community_id { get; set; }
        /// <summary>
        /// Desc:站点id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string VpnUser_id { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Building_id { get; set; }

        public int? BuildingNum { get; set; }
        public int? UnitNoNum { get; set; }

        public string Layer { get; set; }
        public bool Status => this.TIMESTAMP.HasValue && this.TIMESTAMP.Value.AddMinutes(35) >= DateTime.Now;
    }
}
