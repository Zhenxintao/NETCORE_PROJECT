using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    public class da_historyday
    {
        /// <summary>
        /// 
        /// </summary>
        public da_historyday()
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
        public System.String DeviceNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal TValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal Humidity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime RecordTime { get; set; }
    }
}
