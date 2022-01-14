using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class DicDetail
    {
        /// <summary>
        /// Id 自增编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 字典ID
        /// </summary>
        public int Dic_id { get; set; }
        /// <summary>
        /// 字典详细名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 字典详细描述
        /// </summary>
        public string Description { get; set; }
        public int Value { get; set; }

    }
}
