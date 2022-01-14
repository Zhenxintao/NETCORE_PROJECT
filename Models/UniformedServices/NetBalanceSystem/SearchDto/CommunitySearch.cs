using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models
{
    /// <summary>
    /// 小区信息查询类
    /// </summary>
    public class CommunitySearch : Page
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 小区id（guid）
        /// </summary>
        public string Community_id { get; set; }
    }
}
