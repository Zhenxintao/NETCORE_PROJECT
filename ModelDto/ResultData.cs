using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    /// <summary>
    /// 返回结果实体类
    /// </summary>
    public class ResultData
    {
        /// <summary>
        /// 返回值状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回值消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回值数据内容
        /// </summary>
        public List<object> Data { get; set; }
    }
}
