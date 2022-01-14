using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 组织结构信息
    /// </summary>
    public class organization
    {
        /// <summary>
        /// Desc:部门级别
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? DepLevel { get; set; }

        /// <summary>
        /// Desc:父级id
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? ParentDepID { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:机构类型
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? InstitutionType { get; set; }

        /// <summary>
        /// Desc:true:删除
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? IsDelete { get; set; }

        /// <summary>
        /// Desc:true:有效
        /// Default:
        /// Nullable:True
        /// </summary>           
        public bool? IsValid { get; set; }

        /// <summary>
        /// Desc:编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? OrganizationCode { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Desc:描述
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string OrganizationDesc { get; set; }

        /// <summary>
        /// Desc:创建人
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CreateUser { get; set; }

        /// <summary>
        /// Desc:名称
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string OrganizationName { get; set; }
    }
}
