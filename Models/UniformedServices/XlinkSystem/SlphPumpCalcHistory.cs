using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    [SugarTable("slph_pump_calc_history")]
    public class SlphPumpCalcHistory
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public int Pid { get; set; }

        public DateTime Aligntime { get; set; }

        /// <summary>
        /// 计算流量
        /// </summary>
        [SugarColumn(ColumnName = "ca_009q")]
        public string Ca009q { get; set; }

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

        /// <summary>
        /// 计算进口压力
        /// </summary>
        [SugarColumn(ColumnName = "ca_001p")]
        public string Ca001p { get; set; }

        /// <summary>
        /// 计算出口压力
        /// </summary>
        [SugarColumn(ColumnName = "ca_002p")]
        public string Ca002p { get; set; }

    }
}
