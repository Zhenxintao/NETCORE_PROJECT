using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Configuration
{
    /// <summary>
    /// 大庆项目华夏数据接口对接配置
    /// </summary>
    public class DqHxConfigSetting: ConfigAppsetting
    {
        /// <summary>
        /// 获取华夏.txt文件传输名称（热源能耗信息）
        /// </summary>
        public static string HxFileNameHeatSource { get; set; } = Configuration["appSettings:HxFileNameHeatSource"];
        /// <summary>
        /// 获取华夏.txt文件传输名称（换热站能耗信息）
        /// </summary>
        public static string HxFileNameHeatStation { get; set; } = Configuration["appSettings:HxFileNameHeatStation"];
        /// <summary>
        /// 获取华夏.txt文件传输名称（室内测温能耗信息）
        /// </summary>
        public static string HxFileNameTemperature { get; set; } = Configuration["appSettings:HxFileNameTemperature"];


        /// <summary>
        /// 华夏添加小区方法url
        /// </summary>
        public static string HxCommunityAddAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:CommunityAddAction"];

        /// <summary>
        /// 华夏修改小区方法url
        /// </summary>
        public static string HxCommunityUpdateAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:CommunityUpdateAction"];

        /// <summary>
        /// 华夏删除小区方法url
        /// </summary>
        public static string HxCommunityDeleteAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:CommunityDeleteAction"];

        /// <summary>
        /// 华夏添加楼栋方法url
        /// </summary>
        public static string HxBuildingAddAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:BuildingAddAction"];

        /// <summary>
        /// 华夏修改楼栋方法url
        /// </summary>
        public static string HxBuildingUpdateAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:BuildingUpdateAction"];

        /// <summary>
        /// 华夏修改楼栋方法url
        /// </summary>
        public static string HxBuildingDeleteAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:BuildingDeleteAction"];

        /// <summary>
        /// 华夏添加房产方法url
        /// </summary>
        public static string HxHouseAddAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:HouseAddAction"];

        /// <summary>
        /// 华夏修改房产方法url
        /// </summary>
        public static string HxHouseUpdateAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:HouseUpdateAction"];

        /// <summary>
        /// 华夏修改房产方法url
        /// </summary>
        public static string HxHouseDeleteAction { get; set; } = Configuration["HxApiSet:ApiUrl"] + Configuration["HxApiSet:HouseDeleteAction"];

        /// <summary>
        /// 从es读取华夏需要的单元阀历史
        /// </summary>
        public static string HxUnitHis { get; set; } = Configuration["ElasticSearch:ApiUrl"] + Configuration["ElasticSearch:HxUnitHis"];
    }
}
