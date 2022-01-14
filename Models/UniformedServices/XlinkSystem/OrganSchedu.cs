using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    public class OrganSchedu
    {
        /// <summary>
        /// 
        /// </summary>
        public OrganSchedu()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 分公司或项目部Id
        /// </summary>
        public System.Int32 OrganizationCode { get; set; }

        /// <summary>
        /// 投入换热站数量
        /// </summary>
        public System.Int32 StationNumber { get; set; }

        /// <summary>
        /// 控制目标数量
        /// </summary>
        public System.Int32 ControNumber { get; set; }

        /// <summary>
        /// 失调度（%）
        /// </summary>
        public System.Double Schedu { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime UpdTime { get; set; }
    }
}
