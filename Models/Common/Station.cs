using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    public class Station
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// VpnUserID
        /// </summary>  
        public int VpnUser_id { get; set; }

        /// <summary>
        /// 所属热源
        /// </summary>        
        public int PowerInfo_id { get; set; }

        /// <summary>
        /// 建筑类型
        /// </summary>        
        public int BuildType { get; set; }

        /// <summary>
        /// 收费方式
        /// </summary>        
        public int PaymentType { get; set; }

        /// <summary>
        /// 节能类型 1:节能,0:非节能
        /// </summary>        
        public int SavePowerType { get; set; }

        /// <summary>
        /// 管路布置方式
        /// </summary>        
        public int HomeHeatType { get; set; }

        /// <summary>
        /// 管道情况
        /// </summary>        
        public string PipelineSituation { get; set; }

        /// <summary>
        /// 管道管径
        /// </summary>        
        public decimal PipelineDiameter { get; set; }

        /// <summary>
        /// 离热源距离
        /// </summary>        
        public decimal PipelineDistance { get; set; }

        /// <summary>
        /// 热力站地势
        /// </summary>        
        public string Terrain { get; set; }

        public double TerrainValue { get; set; }

        /// <summary>
        /// 维修情况
        /// </summary>        
        public string ReapairSituation { get; set; }

        /// <summary>
        /// 基础瞬时热量
        /// </summary>        
        public decimal BaseHeat { get; set; }

        /// <summary>
        /// 图像路径
        /// </summary>        
        public string ImageUrl { get; set; }

        /// <summary>
        /// 户线图路径
        /// </summary>        
        public string HouseLineUrl { get; set; }

        /// <summary>
        /// 所供楼名称
        /// </summary>        
        public string HeatBuilding { get; set; }

        /// <summary>
        /// 供暖类型(字典表 供暖类型)
        /// </summary>        
        public int HeatType { get; set; }

        /// <summary>
        /// 管理方式
        /// </summary>        
        public int ManageType { get; set; }

        /// <summary>
        /// 供热方式(1、混供；2、地暖；3、暖气片)
        /// </summary>        
        public int HeatIndexType { get; set; }
        /// <summary>
        /// 回水控制温度
        /// </summary>
        public int YGNY_ReturnWaterTemperature_id { get; set; }
        /// <summary>
        /// 保温类型 0：无保温 1：有保温
        /// </summary>
        public int IsHeatPreservation { get; set; }

        /// <summary>
        /// 取暖方式
        /// </summary>
        public int HeatingType_id { get; set; }

        public decimal UseHeatRate { get; set; }

        public decimal ChargeRate { get; set; }

        public decimal ComplaintsRate { get; set; }
    }
}
