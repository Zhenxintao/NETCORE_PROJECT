using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel.Production
{
    /// <summary>
    /// 热网管径信息响应类
    /// </summary>
    public class PipeNetWorkInfoResponse
    {
        /// <summary>
        /// 一级供水管网信息
        /// </summary>
        public List<PrimaryPipeTgResponse> PrimaryPipeTgResponseList { get; set; }
        /// <summary>
        /// 一级回水管网信息
        /// </summary>
        public List<PrimaryPipeThResponse> PrimaryPipeThResponseList { get; set; }

        /// <summary>
        /// 二级供水管网信息
        /// </summary>
        public List<SecondaryPipeTgResponse> SecondaryPipeTgResponseList { get; set; }
        /// <summary>
        /// 二级回水管网信息
        /// </summary>
        public List<SecondaryPipeThResponse> SecondaryPipeThResponseList { get; set; }
    }
}
