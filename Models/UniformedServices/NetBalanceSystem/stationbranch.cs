using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 机组信息表
    /// </summary>
    public class stationbranch
    {
        /// <summary>
        /// Desc:自增id
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:true:删除
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? IsDelete { get; set; }

        /// <summary>
        /// Desc:站点id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string VpnUser_id { get; set; }

        /// <summary>
        /// Desc:机组编号
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ArrayNumber { get; set; }

        /// <summary>
        /// Desc:机组名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BranchName { get; set; }
    }
}
