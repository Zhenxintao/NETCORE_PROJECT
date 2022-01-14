using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto
{
    /// <summary>
    /// 单元查询类
    /// </summary>
    public class UnitNoSearch : BuildingSearch
    {
        ///<summary>
        ///单元id（guid）
        ///</summary>
        public string UnitNo_id { get; set; }

        ///<summary>
        ///单元编号
        ///</summary>
        public int UnitNoNum { get; set; }

        ///<summary>
        ///单元显示名称
        ///</summary>
        public string UnitNoName { get; set; }
    }
}
