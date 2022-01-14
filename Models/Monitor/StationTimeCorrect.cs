using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Monitor
{
    /// <summary>
    /// 时段补偿
    /// </summary>
    public class StationTimeCorrect
    {
        /// <summary>
        /// 表id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public int VpnUserId { get; set; }

        /// <summary>
        /// 机组号
        /// </summary>
        public int NarrayNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Time0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time6 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time8 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time12 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time16 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time18 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time20 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Time22 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remarks { get; set; }
    }
}
