using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel.Production
{
    /// <summary>
    /// 一次供水管径信息
    /// </summary>
    public class PrimaryPipeTgResponse
    {
        /// <summary>
        /// 规格字典Id
        /// </summary>
        public int DcId { get; set; }
        /// <summary>
        /// 规格名称
        /// </summary>
        public string DcName { get; set; }
        /// <summary>
        /// 规格数量
        /// </summary>
        public int DcNumber { get; set; }
        /// <summary>
        /// 规格总长度
        /// </summary>
        public decimal DcLength { get; set; }
        
    }
}
