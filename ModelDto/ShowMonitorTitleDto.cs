using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    public class ShowMonitorTitleDto
    {
        public string label { get; set; }
        public string id { get; set; }
        public string prop { get; set; }
        public string width { get; set; }
        public string Fixed { get; set; }
        public bool IsHidePara { get; set; }
        public int DisplayFlag { get; set; }
    }
    #region 废弃代码
    //public class MonitorTitleStationName
    //{
    //    public string label { get; set; } = "换热站名称";
    //    public string id { get; set; } = "StationName";
    //    public string prop { get; set; } = "StationName";
    //    public string width { get; set; } = "200px";
    //    //public string Fixed { get; set; } = "align";
    //}
    //public class MonitorTitleStationBranchName
    //{
    //    public string label { get; set; } = "机组名称";
    //    public string id { get; set; } = "StationBranchName";
    //    public string prop { get; set; } = "StationBranchName";
    //    public string width { get; set; } = "200px";
    //    //public string Fixed { get; set; } = "align";
    //}
    //public class MonitorTitleStationArea
    //{
    //    public string label { get; set; } = "换热站面积";
    //    public string id { get; set; } = "StationArea";
    //    public string prop { get; set; } = "StationArea";
    //    public string width { get; set; } = "200px";
    //    //public string Fixed { get; set; } = "align";
    //}
    #endregion

}
