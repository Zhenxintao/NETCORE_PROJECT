using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 数据颜色配置表
    /// </summary>
    public class SystemBaseConfig
    {
        /// <summary>
        /// Id 自增编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 颜色值 #FF0000
        /// </summary>
        public string ColorValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Hi1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Lo1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Hi2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Lo2 { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
