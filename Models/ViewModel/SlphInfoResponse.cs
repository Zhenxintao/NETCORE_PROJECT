using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel
{
    /// <summary>
    /// 水利平衡相应实体
    /// </summary>
    public class SlphInfoResponse
    {
        /// <summary>
        /// 水利平衡Id
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// xlink换热站Id
        /// </summary>
        //[SugarColumn(ColumnName = "VpnUser_Id")]
        public int VpnUser_Id { get; set; }
        public string StationName { get; set; }
        public DateTime Aligntime { get; set; }

        /// <summary>
        /// 计算流量
        /// </summary>
        public string Ca003q { get; set; }

        /// <summary>
        /// 计算供温
        /// </summary>
        public string Ca001t { get; set; }

        /// <summary>
        /// 计算回温
        /// </summary>

        public string Ca002t { get; set; }
        /// <summary>
        /// 计算供压
        /// </summary>

        public string Ca001p { get; set; }

        /// <summary>
        /// 计算回压
        /// </summary>
  
        public string Ca002p { get; set; }

        /// <summary>
        /// 散热量
        /// </summary>
 
        public string Ca003qc { get; set; }

        /// <summary>
        /// 计算流速
        /// </summary>

        public string Ca001w { get; set; }

        /// <summary>
        /// 计算压差
        /// </summary>

        public string Ca008p { get; set; }
    }
}
