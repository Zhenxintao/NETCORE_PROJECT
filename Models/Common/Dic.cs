using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class Dic
    {
        /// <summary>
        /// Id 自增编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 字典名称  唯一
        /// </summary>
        public string DicName { get; set; }
        /// <summary>
        /// 字典描述
        /// </summary>
        public string Description { get; set; }

    }
}
