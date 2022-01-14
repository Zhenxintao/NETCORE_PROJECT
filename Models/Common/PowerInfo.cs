using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 热源信息表
    /// </summary>
    public class PowerInfo
    {
        /// <summary>
        /// 站点ID
        /// </summary>  
        public int Id { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>        
        public int VpnUser_id { get; set; }
        /// <summary>
        /// 出厂管径
        /// </summary>        
        public decimal PowerDiameter { get; set; }

        /// <summary>
        /// 热量峰值
        /// </summary>        
        public decimal HeatPeak { get; set; }

        /// <summary>
        /// 流量峰值
        /// </summary>        
        public decimal FlowPeak { get; set; }

        /// <summary>
        /// 供温峰值
        /// </summary>        
        public decimal SendHeatPeak { get; set; }

        /// <summary>
        /// 回温峰值
        /// </summary>        
        public decimal RecHeatPeak { get; set; }

        /// <summary>
        /// 热源的父ID(用于递归树)
        /// </summary>        
        public int ParentID { get; set; }

        /// <summary>
        /// 热源的供热类型（0：蒸汽热源；1：高温水热源）
        /// </summary>        
        public int HeatType { get; set; }
    }
}
