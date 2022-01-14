using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    [SugarTable("slph_heatplant_calc_history")]
    public class SlphHeatplantCalcHistory
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public int Pid { get; set; }
        public DateTime Aligntime { get; set; }

        /// <summary>
        /// 计算流量
        /// </summary>
        [SugarColumn(ColumnName = "ca_003q")]
        public string Ca003q { get; set; }

        /// <summary>
        /// 计算供温
        /// </summary>
        [SugarColumn(ColumnName = "ca_001t")]
        public string Ca001t { get; set; }

        /// <summary>
        /// 计算回温
        /// </summary>
        [SugarColumn(ColumnName = "ca_002t")]
        public string Ca002t { get; set; }
        /// <summary>
        /// 计算供压
        /// </summary>
        [SugarColumn(ColumnName = "ca_001p")]
        public string Ca001p { get; set; }

        /// <summary>
        /// 计算回压
        /// </summary>
        [SugarColumn(ColumnName = "ca_002p")]
        public string Ca002p { get; set; }

        /// <summary>
        /// 散热量
        /// </summary>
        [SugarColumn(ColumnName = "ca_003qc")]
        public string Ca003qc { get; set; }

        /// <summary>
        /// 计算流速
        /// </summary>
        [SugarColumn(ColumnName = "ca_001w")]
        public string Ca001w { get; set; }

        /// <summary>
        /// 计算压差
        /// </summary>
        [SugarColumn(ColumnName = "ca_008p")]
        public string Ca008p { get; set; }
    }
}
