using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 室温系统换热站表信息
    /// </summary>
    public class or_station
    {
        /// <summary>
        /// 
        /// </summary>
        public or_station()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.UInt32 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String StationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? SourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? CompanyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Int32? LineId { get; set; }
    }
}
