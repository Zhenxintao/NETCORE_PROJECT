using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 站点信息表
    /// </summary>
    public class vpnuser
    {
        /// <summary>
        /// Desc:自增id
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:站点地址
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationAddress { get; set; }

        /// <summary>
        /// Desc:纬度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? StationLatitude { get; set; }

        /// <summary>
        /// Desc:true:共用一次
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? IsCommon { get; set; }

        /// <summary>
        /// Desc:机组总数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? StationArrayCount { get; set; }

        /// <summary>
        /// Desc:经度
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? StationLongitude { get; set; }

        /// <summary>
        /// Desc:true:删除
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? IsDelete { get; set; }

        /// <summary>
        /// Desc:通讯ip
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationComsvIp { get; set; }

        /// <summary>
        /// Desc:站点名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationName { get; set; }

        /// <summary>
        /// Desc:true:有效
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? IsValid { get; set; }

        /// <summary>
        /// Desc:通讯端口
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationComsvPort { get; set; }

        /// <summary>
        /// Desc:离热源距离
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? StationNumber { get; set; }

        /// <summary>
        /// Desc:组织机构id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? Organization_id { get; set; }

        /// <summary>
        /// Desc:站点描述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationDescription { get; set; }

        /// <summary>
        /// Desc:简拼
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationSabb { get; set; }

        /// <summary>
        /// Desc:站点编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string PcName { get; set; }

        /// <summary>
        /// Desc:站点联系人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationDutyMan { get; set; }

        /// <summary>
        /// Desc:站点类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? StationStandard { get; set; }

        /// <summary>
        /// Desc:建站日期
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// Desc:站点排序
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? SortIndex { get; set; }

        /// <summary>
        /// Desc:站点联系人电话
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StationDutyManContact { get; set; }

        /// <summary>
        /// Desc:站点id
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string VpnUser_id { get; set; }

        /// <summary>
        /// Desc:工艺图对应的图片路径
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string FlowChart { get; set; }

        /// <summary>
        /// Desc:站点统称（多个控制柜所在换热站统称）
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string StaionsName { get; set; }

        /// <summary>
        /// Desc:总供热面积
        /// Default:
        /// Nullable:True
        /// </summary>           
        public decimal? StationHotArea { get; set; }
    }
}
