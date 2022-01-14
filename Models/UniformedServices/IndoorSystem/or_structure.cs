using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 室内测温组织结构、热网、线别、热源基础数据信息
    /// </summary>
    public class or_structure
    {
        /// <summary>
        /// 
        /// </summary>
        public or_structure()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public System.String PanyName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public System.Int32 ParentId { get; set; }

        /// <summary>
        /// 等级1:一级公司,N:N级公司
        /// </summary>
        public System.Int32 GradeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32 PanyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32 xLinkId { get; set; }
    }
}
