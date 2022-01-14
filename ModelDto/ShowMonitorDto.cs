using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    /// <summary>
    /// 监控首页表格数据
    /// </summary>
    public class ShowMonitorDto
    {
        public List<Dictionary<string,object>> data { get; set; }
        public int count { get; set; }
    }
    public class StationMonitorDto
    {
        public int VpnUserId { get; set; }
        public string StationName { get; set; }

        public int StationBranchId { get; set; }
        public decimal Area { get; set; }

        public string StationBranchName { get; set; }

    }
    public class MonitorData
    {
        public string RealValue { get; set; }
        public decimal HiHi { get; set; }

        public decimal LoLo { get; set; }

        public decimal Hi { get; set; }
        public decimal Lo { get; set; }

    }
}
