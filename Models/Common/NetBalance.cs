using SqlSugar;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class NetBalance
    {
        /// <summary>
        /// 
        /// </summary>
        public NetBalance()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 热网Id
        /// </summary>
        public System.Int32 ComboNetID { get; set; }

        /// <summary>
        /// 热网名称
        /// </summary>
        public System.String ComboNetName { get; set; }

        /// <summary>
        /// 热网面积
        /// </summary>
        public System.Double HeatArea { get; set; }

        /// <summary>
        /// 控制模式
        /// </summary>
        public System.Int32 CalcMode { get; set; }

        /// <summary>
        /// 目标均温
        /// </summary>
        public System.Double SecAvgCurrentTarget { get; set; }

        /// <summary>
        /// 实际均温
        /// </summary>
        public System.Double NetBalanceTemp { get; set; }

        /// <summary>
        /// 网内站点数量
        /// </summary>
        public System.Int32 StationNumber { get; set; }

        /// <summary>
        /// 网内机组数量
        /// </summary>
        public System.Int32 ArrayNumber { get; set; }

        /// <summary>
        /// 全部站点数量
        /// </summary>
        public System.Int32 StationSumNumber { get; set; }

        /// <summary>
        /// 达标站点数量
        /// </summary>
        public System.Int32 StandardStation { get; set; }

        /// <summary>
        /// 未达标站点数量
        /// </summary>
        public System.Int32 UnStandardStation { get; set; }

        /// <summary>
        /// 投运状态
        /// </summary>
        public System.Boolean Active { get; set; }

        /// <summary>
        /// 名称简拼
        /// </summary>
        public System.String ComboNetNameAbb { get; set; }

        /// <summary>
        /// 失调度 （均方差）
        /// </summary>
        public System.Double ImbalancingDegree { get; set; }

        /// <summary>
        /// 传输时间
        /// </summary>
        public System.DateTime TIMESTAMP { get; set; }

        /// <summary>
        /// 全网平衡是否运行
        /// </summary>
        public System.Boolean Running { get; set; }

        /// <summary>
        /// 全网平衡完成率
        /// </summary>
        public System.Double NetBalancePerComplete { get; set; }
    }
}
