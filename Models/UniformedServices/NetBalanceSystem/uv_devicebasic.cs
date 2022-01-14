using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models
{
    public class uv_devicebasic
    {
        /// <summary>
        /// Desc:站点名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationName { get; set; }

        /// <summary>
        /// Desc:机组名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BranchName { get; set; }

        /// <summary>
        /// Desc:楼栋编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? BuildingNum { get; set; }

        /// <summary>
        /// Desc:小区名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CommunityName { get; set; }

        /// <summary>
        /// Desc:补偿值
        /// Default:0.00
        /// Nullable:True
        /// </summary>           
        public decimal? CorrectedValue { get; set; }

        /// <summary>
        /// Desc:单元编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? UnitNoNum { get; set; }

        /// <summary>
        /// Desc:楼栋名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BuildingName { get; set; }

        /// <summary>
        /// Desc:阀门开度上限
        /// Default:95.00
        /// Nullable:True
        /// </summary>           
        public decimal? OpenUpperLimit { get; set; }

        /// <summary>
        /// Desc:单元名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UnitNoName { get; set; }

        /// <summary>
        /// Desc:阀门开度下限
        /// Default:30.00
        /// Nullable:True
        /// </summary>           
        public decimal? OpenLowerLimit { get; set; }

        /// <summary>
        /// Desc:唯一标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UnitNo_id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public int? Id { get; set; }

        /// <summary>
        /// Desc:单元阀id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UV_DeviceInfo_id { get; set; }

        /// <summary>
        /// Desc:住户小区id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Community_id { get; set; }

        /// <summary>
        /// Desc:调控模式(0 手动、1 全网、2 分时分控)
        /// Default:0
        /// Nullable:True
        /// </summary>           
        public int? ControlMode { get; set; }

        /// <summary>
        /// Desc:设备名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DeviceName { get; set; }

        /// <summary>
        /// Desc:站点id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string VpnUser_id { get; set; }

        /// <summary>
        /// Desc:系统分区编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? NarrayNo { get; set; }

        /// <summary>
        /// Desc:设备编码(唯一)
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DeviceCode { get; set; }

        /// <summary>
        /// Desc:小区楼栋id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Building_id { get; set; }
    }
}
