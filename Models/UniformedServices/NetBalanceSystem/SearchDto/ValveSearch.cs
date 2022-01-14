using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models
{
    /// <summary>
    ///  查询类
    /// </summary>
    public class ValveSearch : ValveSearchBase
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public string VpnUser_id { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 机组
        /// </summary>
        public int NarrayNo { get; set; }

        /// <summary>
        /// Desc:住户小区id
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
        /// Desc:单元id
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
    }
}
