using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    /// <summary>
    /// 返回结果码
    /// </summary>
    static public class ResultCode
    {
        /// <summary>
        /// 成功状态码
        /// </summary>
        public  static int Success { get; set; } = 200;
        /// <summary>
        /// 失败状态码
        /// </summary>
        public static  int Error { get; set; } = 500;
    }
}
