using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 客服信息检修测温表
    /// </summary>
    public class eq_repairinfor
    {
        /// <summary>
        /// 
        /// </summary>
        public eq_repairinfor()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public System.String UserCode { get; set; }

        /// <summary>
        /// 是否报修（0:是1:否）
        /// </summary>
        public System.Int32 IsNoRepair { get; set; }

        /// <summary>
        /// 是否测温（0:是1:否）
        /// </summary>
        public System.Int32 IsNoTem { get; set; }

        /// <summary>
        /// 故障简述
        /// </summary>
        public System.String FailureDescription { get; set; }

        /// <summary>
        /// 是否已受理（0:是1:否）
        /// </summary>
        public System.Int32 IsNoAccepted { get; set; }

        /// <summary>
        /// 报修是否完成（0:是1:否）
        /// </summary>
        public System.Int32 IsNoRepairCarryout { get; set; }

        /// <summary>
        /// 测温是否完成（0:是1:否）
        /// </summary>
        public System.Int32 IsNoTemCarryout { get; set; }

        /// <summary>
        /// 报修完成时间
        /// </summary>
        public System.DateTime RepairCarryoutTime { get; set; }

        /// <summary>
        /// 测温完成时间
        /// </summary>
        public System.DateTime TemCarryoutTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}
