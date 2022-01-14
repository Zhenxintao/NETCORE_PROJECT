using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 管网规格字典
    /// </summary>
    public class DicPipeNetwork
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string D_S { get; set; }
        /// <summary>
        /// 规格字典Id
        /// </summary>
        public string D_S_ID { get; set; }
    }
}
