using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;

namespace THMS.Core.API.ModelDto
{
    //public class MonitorCustomListDto
    //{
    //    public List<StationMessage> stationMessage { get; set; }
    //    public List<StationDisposition> stationDisposition { get; set; }

    //}
    /// <summary>
    /// 自定义列表配置项
    /// </summary>
    //public class StationDisposition
    //{
    //    public int Id { get; set; }
    //    public string ConfigName { get; set; }
    //    public string IncludeSta { get; set; }
    //    public string IncludeParaMDL { get; set; }
    //}

    /// <summary>
    /// 自定义列表换热站信息
    /// </summary>
    public class StationInfo
    {
        public int VpnUserId { get; set; }
        public string StationName { get; set; }

        public string OrganizationName { get; set; }

        public int SavePowerType { get; set; }
        public string ItemName { get; set; }
        
        public string PowerInfoName { get; set; }


    }
    public class StationPra {
        public List<StandardParameter> stationPublicPra { get; set; }

        public List<StandardParameter> stationBranchPra { get; set; }

    }
    ///// <summary>
    ///// 自定义列表公共参数
    ///// </summary>
    //public class StationPublicPra
    //{
    //    public string AiDesc { get; set; }
    //    public string TagName { get; set; }
    //}
    ///// <summary>
    ///// 自定义列表机组参数
    ///// </summary>
    //public class StationBranchPra
    //{
    //    public string AiDesc { get; set; }
    //    public string TagName { get; set; }

    //}
}
