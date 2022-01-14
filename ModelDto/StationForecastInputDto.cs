using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    /// <summary>
    /// 负荷预测换热站批量添加输入信息data
    /// </summary>
    public class StationForecastInputDto
    {
        /// <summary>
        /// 换热站Id
        /// </summary>
        public int StationType { get; set; }
        /// <summary>
        /// 面积热指标
        /// </summary>
        public decimal HeatTarget { get; set; }

        /// <summary>
        /// 当年实现的热耗
        /// </summary>
        public decimal OverallHeat { get; set; }

        /// <summary>
        /// 二次供水温度
        /// </summary>
        public decimal SEC_TEMP_S { get; set; }

        /// <summary>
        /// 二次回水温度
        /// </summary>
        public decimal SEC_TEMP_R { get; set; }

        /// <summary>
        /// 二次相对流量
        /// </summary>
        public decimal SeNetworkRelativeFlow { get; set; }

        /// <summary>
        /// 当前室外温度补偿值
        /// </summary>
        public decimal OutdoorAtPresentOffset { get; set; }

        /// <summary>
        /// 调整系数
        /// </summary>
        public decimal AdCoefficient { get; set; }

        /// <summary>
        /// 采暖方式,1地板采暖，2挂片采暖，3混合
        /// </summary>
        public int HeatingType { get; set; }

        /// <summary>
        /// 地板采暖散热管间距类型（1：300mm;2：250mm;3：200mm;4：150mm）
        /// </summary>
        public int RadiatingTubeType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }
    }
}
