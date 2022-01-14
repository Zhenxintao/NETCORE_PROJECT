using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    /// <summary>
    /// 小时天气预报
    /// </summary>    
    public class RealTemperature
    {

        /// <summary>
        /// RealTemperatureID
        /// </summary>  
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }

        /// <summary>
        /// 小时温度平均值
        /// </summary>        
        public virtual decimal HourAvgTemperature { get; set; }


        /// <summary>
        /// 预测温度
        /// </summary>        
        public virtual decimal ForecastHourAvgTemperature { get; set; }


        /// <summary>
        /// 风力(0-12级)
        /// </summary>        
        public virtual string Wind { get; set; }


        /// <summary>
        /// 风速
        /// </summary>        
        public virtual string WindVelocity { get; set; }


        /// <summary>
        /// 风向
        /// </summary>        
        public virtual string WindDirection { get; set; }


        /// <summary>
        /// 天气状况
        /// </summary>        
        public virtual string WeatherConditions { get; set; }


        /// <summary>
        /// 雪况(0-4级)
        /// </summary>        
        public virtual int SnowStatus { get; set; }


        /// <summary>
        /// 湿度
        /// </summary>        
        public virtual int HumiDity { get; set; }


        /// <summary>
        /// 采集时间
        /// </summary>        
        public virtual DateTime NcapTime { get; set; }

        /// <summary>
        /// 采集点的名称
        /// </summary>        
        public virtual string CollectName { get; set; }

        /// <summary>
        /// 是否是预报气温
        /// </summary>
        public virtual bool IsForecast { get; set; }

        /// <summary>
        /// 天气编码
        /// </summary>
        public virtual string WeatherCode { get; set; }
    }
}
