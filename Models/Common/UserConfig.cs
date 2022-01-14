using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    public class UserConfig
    {
        /// <summary>
        /// 用户配置表Id
        /// </summary>
        /// 
        public string ConfigName { get; set; }
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string IncludePara { get; set; }
        public string IncludeParaMDL { get; set; }
        public string IncludeParaMDLWidth { get; set; }
        public string IncludeSta { get; set; }
        /// <summary>
        /// 类型
        public int ListType
        {
            get; set;
        }
        public int SortIndex { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }

    }

    public enum UserConfigListType
    {
        /// <summary>
        /// 任意参数
        /// </summary>
        AnyTagItem = 0,
        /// <summary>
        /// 多站命令列表
        /// </summary>
        CommandList = 3,
        /// <summary>
        /// 多画面
        /// </summary>
        MultiViews = 8,
        /// <summary>
        /// 排行
        /// </summary>
        TopX = 10,
        /// <summary>
        /// Wpf客户段用换热站参数自定列表
        /// </summary>
        WpfMonitorChartTagItems = 998,
        /// <summary>
        /// Wpf客户段用换热站参数自定列表
        /// </summary>
        WpfMonitorTagItems = 999,
        /// <summary>
        /// wpf客户端换热站自定义报表字段
        /// </summary>
        WpfReportTagItems = 997,
        /// <summary>
        /// 多站自定义参数
        /// </summary>
        WpfMultiStationTagItems = 996,
        /// <summary>
        /// 自定义站点列表
        /// </summary>
        WpfVpnUserList = 995,
        /// <summary>
        /// 地图查看参数
        /// </summary>
        WpfMapTagItem = 994,
        /// <summary>
        /// 热源曲线配置
        /// </summary>
        WpfPowerChartTagItems = 993,
        /// <summary>
        /// 多站参数
        /// </summary>
        WpfAnyTagItems = 992,

    }
}
