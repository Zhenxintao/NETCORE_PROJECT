using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    [SugarTable("slph_xLinkId")]
    public class SlphXlinkId
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 水利平衡Id
        /// </summary>
        public int Pid { get; set; }

       /// <summary>
       /// xlink换热站Id
       /// </summary>
        [SugarColumn(ColumnName ="VpnUser_Id")]
        public int VpnUserId { get; set; }

        /// <summary>
        /// 0换热站 1 热源
        /// </summary>
        public int Type { get; set; }
    }
}
