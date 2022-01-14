using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    /// <summary>
    /// 返回结果消息
    /// </summary>
    static public class ResultMessageInfo
    {
        /// <summary>
        /// 成功状态消息
        /// </summary>
        public static string SuccessMessage { get; set; } = "数据请求成功!";
        /// <summary>
        /// 失败状态消息
        /// </summary>
        public static string ErrorMessage { get; set; } = "数据请求失败!";
    }
}
