using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto
{
    /// <summary>
    /// 楼栋信息查询类
    /// </summary>
    public class BuildingSearch : CommunitySearch
    {
        /// <summary>
        /// 楼栋id（guid）
        /// </summary>
        public string Building_id { get; set; }

        ///<summary>
        ///楼栋编号
        ///</summary>
        public int BuildingNum { get; set; }

        ///<summary>
        ///楼栋显示名称
        ///</summary>
        public string BuildingName { get; set; }
    }
}
