using SqlSugar;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PowerForecastHistoryHourData
    {
        /// <summary>
        /// 
        /// </summary>
        public PowerForecastHistoryHourData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIdentity = true)]
        public System.Int32 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public System.Int32 VpnUser_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String StationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public System.DateTime ForecastDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal HeatArea { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastHeatTarget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealHeatTarget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal IndoorTemperature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastOutdoorTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealOutdoorTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal LoadRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastUseHeat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealUseHeat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastFlow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealFlow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal PRI_RelativeFlow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal SEC_RelativeFlow { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal Constant_E { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastFirstSendTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealFirstSendTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastFirstReturnTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealFirstReturnTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastSecSendTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealSecSendTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal ForecastSecReturnTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.Decimal RealSecReturnTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.String CreateUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}
