using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 分段温度值表
    /// </summary>
    public class co_threshold
    {
        /// <summary>
        /// 
        /// </summary>
        public co_threshold()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ValueCName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String ValueEName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ValueData { get; set; }
    }
}
