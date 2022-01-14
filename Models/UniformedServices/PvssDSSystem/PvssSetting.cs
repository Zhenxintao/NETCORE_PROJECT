using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.PvssDSSystem
{
    /// <summary>
    /// PVSS同步换热站表Id
    /// </summary>
    public class PvssSetting
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 换热站Id
        /// </summary>
        public string VpnUser_id { get; set; }

        /// <summary>
        /// PvssId
        /// </summary>
        public string Pvss_id { get; set; }
    }
}
