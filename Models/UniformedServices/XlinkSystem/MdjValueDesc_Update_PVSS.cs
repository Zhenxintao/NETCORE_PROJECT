using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 更新采集量实时值
    /// </summary>
    public class MdjValueDesc_Update_PVSS
    {
        //机组编号
        public int NarrayNo { get; set; }

        //标签名称
        public string TagName { get; set; }

        //实时值
        public string RealValue { get; set; }
    }
}
