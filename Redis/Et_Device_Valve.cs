using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace THMS.Core.API.Redis
{
    /// <summary>
    /// 末端温采实时数据redis实体类
    /// </summary>
    public class Et_Device_Valve
    {
        /// <summary>
        /// 安装Id
        /// </summary>
        public int? ConfigId { get; set; }

        /// <summary>
        /// Desc:安装位置
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ET_Manufacturer { get; set; }

        /// <summary>
        /// Desc:环网中的位置
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string NetManufacturer { get; set; }
        /// <summary>
        /// Desc:设备名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DeviceName { get; set; }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Layer { get; set; }

        /// <summary>
        /// Desc:唯一标识
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UnitNo_id { get; set; }

        /// <summary>
        /// Desc:单元名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string UnitNoName { get; set; }

        /// <summary>
        /// Desc:单元编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? UnitNoNum { get; set; }
        /// <summary>
        /// Desc:楼栋编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? BuildingNum { get; set; }

        /// <summary>
        /// Desc:小区楼栋id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Building_id { get; set; }

        /// <summary>
        /// Desc:楼栋名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BuildingName { get; set; }

        /// <summary>
        /// Desc:住户小区id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Community_id { get; set; }

        /// <summary>
        /// Desc:小区名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CommunityName { get; set; }

        /// <summary>
        /// Desc:机组名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BranchName { get; set; }

        /// <summary>
        /// Desc:机组编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? NarrayNo { get; set; }

        /// <summary>
        /// Desc:站点名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationName { get; set; }
        /// <summary>
        /// Desc:站点id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string VpnUser_id { get; set; }

        /// <summary>
        /// Desc:运行高报警
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? HiHi { get; set; }
        /// <summary>
        /// Desc:运行低报警
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? LoLo { get; set; }

        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceCode { get; set; }
        /// <summary>
        /// 采集量描述
        /// </summary>
        public string AiDesc { get; set; }
        /// <summary>
        /// 采集量类型
        /// </summary>
        public string AiType { get; set; }
        /// <summary>
        /// 采集量单位
        /// </summary>
        public string Unit { get; set; }


        /// <summary>
        /// 末端温采设备id
        /// </summary>
        public string ET_DeviceInfo_id { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public decimal? TEMP { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? TIMESTAMP { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 是否断线
        /// </summary>
        public bool Status { get; set; }
        //public bool Status => this.TIMESTAMP.HasValue && this.TIMESTAMP.Value.AddHours(3) >= DateTime.Now;

    }
}
