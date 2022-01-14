using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    /// <summary>
    ///  日天气预报
    /// </summary>    
    public class WeatherForecast
    {

        /// <summary>
        /// WeatherForcastID
        /// </summary>  
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }

        /// <summary>
        /// 预测最高温度
        /// </summary>        
        public virtual decimal HighTemperature { get; set; }


        /// <summary>
        /// 平均温度
        /// </summary>        
        public virtual decimal AvgTemperature { get; set; }


        /// <summary>
        /// 预测最低温度
        /// </summary>        
        public virtual decimal LowTemperature { get; set; }


        /// <summary>
        /// 风力(0-12级)
        /// </summary>        
        public virtual string Wind { get; set; }


        /// <summary>
        /// 风速
        /// </summary>        
        public virtual string WindVelocity { get; set; }


        /// <summary>
        /// 风向(如：东北风)
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
        /// 每日制定的供热气温
        /// </summary>        
        public virtual decimal HeatTemperature { get; set; }


        /// <summary>
        /// 预测热指标
        /// </summary>        
        public virtual decimal ForecastHeatTarget { get; set; }


        /// <summary>
        /// 预测时间段
        /// </summary>        
        public virtual string ForecastDateSection { get; set; }


        /// <summary>
        /// 天气改变状态
        /// </summary>        
        public virtual int ChangeStatus { get; set; }


        /// <summary>
        /// 是否已此记录作为预测值
        /// </summary>        
        public virtual bool IsForecast { get; set; }


        /// <summary>
        /// 供热气温制定人
        /// </summary>        
        public virtual string CheckMan { get; set; }


        /// <summary>
        /// 供热气温制定时间
        /// </summary>        
        public virtual DateTime CheckDate { get; set; }


        /// <summary>
        /// 天气预报采集时间
        /// </summary>        
        public virtual DateTime NcapTime { get; set; }


        /// <summary>
        /// 天气预报预报时间
        /// </summary>        
        public virtual DateTime ForecastTime { get; set; }

        /// <summary>
        /// 天气类型（1：3天；2：7天）
        /// </summary>        
        public virtual int WeatherType { get; set; }

    }
}
