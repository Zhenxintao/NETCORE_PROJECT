using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
   public class MeterDataListDto
    {
        //public int VpnUser_id { get; set; }
        //public int PowerInfo_id { get; set; }
        //public string StationName { get; set; }
        public string EnergyName { get; set; }
        public decimal Area { get; set; }
        public List<EnergyData> EnergyDataList { get; set; }
    }

    public class EnergyData
    {
        /// <summary>
        /// 采集时间
        /// </summary>
        public string NcapTime { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Tick { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public decimal EnergyToTal { get; set; }

        /// <summary>
        /// 单耗
        /// </summary>
        public decimal EnergyTarget { get; set; }
    }
}
