using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Analysis
{
    /// <summary>
    /// 全网平衡运行质量列表
    /// </summary>
    public class VirtualNetStationM
    {
        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 供热类型
        /// </summary>
        public int HeatType { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public int VpnUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ValveType { get; set; }

        /// <summary>
        /// 机组名称
        /// </summary>
        public string StationBranchName { get; set; }

        /// <summary>
        /// 机组编号
        /// </summary>
        public int NarrayNo { get; set; }

        /// <summary>
        /// 机组面积
        /// </summary>
        public double StationBranchHeatArea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double TargetTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ControlType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// 时段补偿，当前时段补偿温度
        /// </summary>
        public double TimeV { get; set; }
        /// <summary>
        /// 补偿值
        /// </summary>
        public double CorrectValue { get; set; }
        /// <summary>
        /// 供温(补偿)
        /// </summary>
        public double SecTempSV { get; set; }

        /// <summary>
        /// 回温(补偿)
        /// </summary>
        public double SecTempRV { get; set; }

        /// <summary>
        /// 均温(补偿)
        /// </summary>
        public double SecTempAvgV { get; set; }

        /// <summary>
        /// 供温
        /// </summary>
        public double SecTempS { get; set; }

        /// <summary>
        /// 回温
        /// </summary>
        public double SecTempR { get; set; }

        /// <summary>
        /// 二次供回均温
        /// </summary>
        public double SecTempRs { get; set; }

        /// <summary>
        /// 阀门实际开度
        /// </summary>
        public double Opening { get; set; }

        /// <summary>
        /// 阀门设定开度
        /// </summary>
        public double Setting { get; set; }

        /// <summary>
        /// 站点是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Complete { get; set; }

        /// <summary>
        /// 通讯IP
        /// </summary>
        public string FameViewIp { get; set; }

        /// <summary>
        /// 通讯端口
        /// </summary>
        public int FameViewPort { get; set; }

        /// <summary>
        /// 阀门下发标识位
        /// </summary>
        public string ValveSigeValve { get; set; }

        /// <summary>
        /// 全网信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 默认都参与全网平衡
        /// </summary>
        /// <summary>
        /// 是否参与全网平衡
        /// </summary>
        public bool IsJoinBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double CorrectionTemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double TempDiff { get; set; }
        /// <summary>
        /// 目标温度
        /// </summary>
        public double DesTemp { get; set; }

        
    }
}
