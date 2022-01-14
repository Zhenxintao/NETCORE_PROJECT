using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel
{
    /// <summary>
    /// 热源及换热站检测点指标列表
    /// </summary>
    public class MonitorPointDto
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

        /// <summary>
        /// 检测点排序
        /// </summary>
        public int order { get; set; }
    }

    /// <summary>
    /// 热源实时监测数据
    /// </summary>
    public class SourceDataResult
    {
        /// <summary>
        /// 检测点关键字
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 检测类型（单位）
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 检测类型（单位）
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// VpnUser_id
        /// </summary>
        public int VpnUser_id { get; set; }
    }
}
