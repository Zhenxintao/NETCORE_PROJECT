using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 组织结构表
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// 组织结构ID
        /// </summary>  
        public int Id { get; set; }

        /// <summary>
        /// 组织结构编码
        /// </summary>        
        public int OrganizationCode { get; set; }

        /// <summary>
        /// 组织结构名称
        /// </summary>        
        public string OrganizationName { get; set; }

        /// <summary>
        /// 组织机构描述
        /// </summary>        
        public string OrganizationDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>        
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>        
        public bool IsValid { get; set; }

        /// <summary>
        /// 面积
        /// </summary>        
        public decimal HeatArea { get; set; }

        /// <summary>
        /// 部门的父部门ID(用于部门管理的递归树)
        /// </summary>        
        public int ParentDepID { get; set; }

        /// <summary>
        /// 部门的级别
        /// </summary>        
        public int DepLevel { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>        
        public string CreateUser { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public string Xaxis { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public string Yaxis { get; set; }

        /// <summary>
        /// 办公人数
        /// </summary>
        public int OfficeNumber { get; set; }

        public string ClassList { get; set; }

        public List<Organization> children { get; set; } = new List<Organization>();
    }
}
