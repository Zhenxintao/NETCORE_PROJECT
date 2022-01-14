using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Configuration
{
    /// <summary>
    /// 读取配置信息类
    /// </summary>
    public class ConfigAppsetting
    {
        /// <summary>
        /// 创建对象读取json配置文件
        /// </summary>
        public static IConfigurationBuilder Builder { get; set; } = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
        /// <summary>
        /// 把json文件中的所有数据项保存在Configuration中，读取key实例:Configuration["A:B"]
        /// </summary>
        public static IConfigurationRoot Configuration { get; set; } = Builder.Build();

        /// <summary>
        /// 获取json配置文件中SQL Server数据库连接字符串
        /// </summary>
        public static string SqlServerConfig { get; set; } = Configuration["appSettings:ConnectionStringSqlServer"];

        /// <summary>
        /// redis连接串
        /// </summary>
        public static string RedisConfig { get; set; } = Configuration["appSettings:RedisCaching"];
        /// <summary>
        /// 获取json配置文件中MySql数据库连接字符串
        /// </summary>
        public static string MySqlConfig { get; set; } = Configuration["appSettings:ConnectionStringMysql"];

        /// <summary>
        /// 获取json配置文件中二网的MySql数据库连接字符串
        /// </summary>
        public static string MySqlConfigEw { get; set; } = Configuration["appSettings:ConnectionStringMysqlEw"];

        /// <summary>
        /// 获取json配置文件中SQL Server数据库中天气预报表中预测天气数据值
        /// </summary>
        public static string WeatherForecast { get; set; } = Configuration["appSettings:Forecast"];
        /// <summary>
        /// 获取json配置文件中SQL Server数据库中天气预报表中实时天气数据值
        /// </summary>
        public static string WeatherReal { get; set; } = Configuration["appSettings:Real"];
        /// <summary>
        /// 获取json配置文件中与华夏数据FTP对接IP连接数据
        /// </summary>
        public static string FTPHxInterFaceIp { get; set; } = Configuration["appSettings:FTPHxInterFaceIp"];
        /// <summary>
        /// 获取json配置文件中与华夏数据FTP对接端口连接数据
        /// </summary>
        public static string FTPHxInterFaceProt { get; set; } = Configuration["appSettings:FTPHxInterFaceProt"];
        /// <summary>
        /// 获取json配置文件中HangFire定时任务的Ip地址
        /// </summary>
        public static string HangFireUrl { get; set; } = Configuration["appSettings:HangFireUrl"];
        //PvssWebApi同步数据WebApi地址
        public static string PvssWebApi { get; set; } = Configuration["appSettings:PvssWebApi"];
        //华夏基础数据同步WebApi地址
        public static string HxWebApi { get; set; } = Configuration["appSettings:HxWebApi"];
        //统一服务平台实时数据推送WebApi地址
        public static string SignalrWebApi { get; set; } = Configuration["appSettings:SignalrWebApi"];
        //xlink自动生成点表类型：1系统站; 2机组站
        public static string XlinkDbType { get; set; } = Configuration["appSettings:XlinkDbType"];
        //xlink自动生成点表系统站数量
        public static string XlinkDbNarrayNumber { get; set; } = Configuration["appSettings:XlinkDbNarrayNumber"];

        // Ftp 用户名密码
        public static string FTPHxUserName { get; set; } = Configuration["appSettings:FTPHxUserName"];
        public static string FTPHxPassWord { get; set; } = Configuration["appSettings:FTPHxPassWord"];

        // 视频监控同步接口WebAPI地址
        public static string ViewAPI { get; set; } = Configuration["appSettings:ViewWebApi"];

        // 视频监控AK/SK
        public static string appkey { get; set; } = Configuration["appSettings:appkey"];
        public static string secret { get; set; } = Configuration["appSettings:secret"];


    }
}
