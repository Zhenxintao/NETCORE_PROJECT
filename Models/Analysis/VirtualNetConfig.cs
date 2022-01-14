using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Analysis
{
    /// <summary>
    /// 全网平衡配置表
    /// </summary>
    public class VirtualNetConfig
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 虚拟网名字
        /// </summary>
        public string VirtualNetName { get; set; }

        /// <summary>
        /// 目标温度
        /// </summary>
        public decimal TargetTemp { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 控制策略
        /// </summary>
        public int ControlType { get; set; }

        /// <summary>
        /// 控制策略目标值是否自动生成
        /// </summary>
        public bool IsAuto { get; set; }

        /// <summary>
        /// 本次调控时间
        /// </summary>
        public DateTime ExcuteTime { get; set; }

        /// <summary>
        /// 调控周期
        /// </summary>
        public int ExcuteCycle { get; set; }

        /// <summary>
        /// 下次调控周期
        /// </summary>
        public DateTime NextExcuteTime { get; set; }
    }
}
