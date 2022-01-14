using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 一级管网信息
    /// </summary>
    public class PrimaryPipeNetwork
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 具体安装区域名称
        /// </summary>
        public string XQMC { get; set; }
        /// <summary>
        /// 类别描述
        /// </summary>
        public string L_TYPE { get; set; }
        /// <summary>
        /// 类别id（1供温管；2回温管）
        /// </summary>
        public int L_TYPE_ID { get; set; }
        /// <summary>
        /// 物业公司名称
        /// </summary>
        public string MUS { get; set; }
        /// <summary>
        /// 所属分公司名称
        /// </summary>
        public string SU { get; set; }
        /// <summary>
        /// 情况描述
        /// </summary>
        public string B_C { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string D_S { get; set; }
        /// <summary>
        /// 规格字典Id
        /// </summary>
        public int D_S_ID { get; set; }
        /// <summary>
        /// 安装类别描述
        /// </summary>
        public string D_TYPE { get; set; }

        /// <summary>
        /// 钢管类型描述
        /// </summary>
        public string MATERIAL { get; set; }

        /// <summary>
        /// 钢管原材料描述
        /// </summary>
        public string BMATERIAL { get; set; }

        /// <summary>
        /// 钢管安装方式描述
        /// </summary>
        public string P_W { get; set; }
        /// <summary>
        /// 所属电厂名称
        /// </summary>
        public string H_S { get; set; }
        /// <summary>
        /// 某时间
        /// </summary>
        public string MDATE { get; set; }
        /// <summary>
        /// 某时间
        /// </summary>
        public string C_TIME { get; set; }
        /// <summary>
        /// 某时间
        /// </summary>
        public string P_T { get; set; }
        /// <summary>
        /// 所属检测公司名称
        /// </summary>
        public string P_U { get; set; }
        /// <summary>
        /// 安装方式
        /// </summary>
        public string MEMOL { get; set; }

        /// <summary>
        /// 安装长度
        /// </summary>
        public string SHAPE_LEN { get; set; }
        /// <summary>
        /// 热网名称
        /// </summary>
        public string KH { get; set; }

        /// <summary>
        /// 热网ID
        /// </summary>
        public int KHid { get; set; }

        /// <summary>
        /// 第三方同步Id
        /// </summary>
        public int Hxid { get; set; }

    }
}
