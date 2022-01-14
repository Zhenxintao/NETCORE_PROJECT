using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
   public class PlanEnergyDto
    {
        /// <summary>
        /// 计划总能耗
        /// </summary>
        public decimal PlanTotal { get; set; }
        /// <summary>
        /// 计划总指标
        /// </summary>
        public decimal PlanTarget { get; set; }
        /// <summary>
        /// 当前总能耗
        /// </summary>
        public decimal NowTotal { get; set; }

        /// <summary>
        /// 当前总指标
        /// </summary>
        public decimal NowTarget { get; set; }

        /// <summary>
        /// 完成能耗比例
        /// </summary>
        public decimal RatioTotal { get; set; } 
        /// <summary> 
        /// 完成指标比例
        /// </summary>                                       
        public decimal RatioTarget { get; set; }

        /// <summary>
        /// 剩余能耗
        /// </summary>
        public decimal ResidueTotal { get; set; }
        /// <summary>
        /// 可用天数
        /// </summary>
        public decimal UseDays { get; set; }
    }
}
