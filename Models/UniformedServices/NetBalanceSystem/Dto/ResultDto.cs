using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models
{
    /// <summary>
    /// 通用返回类
    /// </summary>
    public class ResultDto
    {
        /// <summary>
        /// 检测点关键字
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 检测类型（单位）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 检测点含义
        /// </summary>
        public string name { get; set; }
    }
}
