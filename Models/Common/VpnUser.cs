using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ApiModel
{
    /// <summary>
    /// VpnUser
    /// </summary>    
    public class VpnUser
    {
        /// <summary>
        /// VpnUserID
        /// </summary>  
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }

        /// <summary>
        /// 站点的编号
        /// </summary>        
        public virtual string PcName { get; set; }


        /// <summary>
        /// IP地址
        /// </summary>        
        public virtual string PcIP { get; set; }


        /// <summary>
        /// 端口号
        /// </summary>        
        public virtual int PcPort { get; set; }


        /// <summary>
        /// 站点在热源上的位置
        /// </summary>        
        public virtual int StationNumber { get; set; }


        /// <summary>
        /// 经度
        /// </summary>        
        public virtual decimal StationLongitude { get; set; }


        /// <summary>
        /// 纬度
        /// </summary>        
        public virtual decimal StationLatitude { get; set; }


        /// <summary>
        /// 通信服务器的IP地址
        /// </summary>        
        public virtual string StationComsvIP { get; set; }


        /// <summary>
        /// 站名称
        /// </summary>        
        public virtual string StationName { get; set; }


        /// <summary>
        /// 建站日期
        /// </summary>        
        public virtual DateTime BuildDate { get; set; }


        /// <summary>
        /// 所属分公司
        /// </summary>        
        public virtual int Organization_id { get; set; }
        //public virtual string Organization { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>        
        public virtual string StationSabb { get; set; }


        /// <summary>
        /// 地址
        /// </summary>        
        public virtual string StationAddress { get; set; }


        /// <summary>
        /// 登录计数
        /// </summary>        
        public virtual int PcLoinCount { get; set; }


        /// <summary>
        /// 最后登录时间
        /// </summary>        
        public virtual DateTime PcLastLogin { get; set; }


        /// <summary>
        /// 数据传输电话号码
        /// </summary>        
        public virtual string CommunicationPhone { get; set; }


        /// <summary>
        /// RTU传输次数
        /// </summary>        
        public virtual int RtuTransCount { get; set; }


        /// <summary>
        /// RTU传输时间
        /// </summary>        
        public virtual DateTime RtuTransTime { get; set; }


        /// <summary>
        /// 是否重点站
        /// </summary>        
        public virtual bool IsPoint { get; set; }


        /// <summary>
        /// 是否生活水
        /// </summary>        
        public virtual bool IsLifeWater { get; set; }


        /// <summary>
        /// 是否组合站
        /// </summary>        
        public virtual bool IsCombo { get; set; }


        /// <summary>
        /// 是否有效
        /// </summary>        
        public virtual bool IsValid { get; set; }


        /// <summary>
        /// 站点是否在前台显示
        /// </summary>        
        public virtual int StationShowValid { get; set; }


        /// <summary>
        /// 站点类型 0、人工站；1、监测站；2、管线监测点；3、监控站；98、热网；99、热源
        /// </summary>        
        public virtual int StationStandard { get; set; }


        /// <summary>
        /// 通讯方式 ADSL 1;GPRS  2; CDMA 3;  光纤传输 4; 3G路由器 5
        /// </summary>        
        public virtual int StationCommunicationType { get; set; }

        /// <summary>
        /// 通讯运营商：移动 1;联通 2;电信 3;广电 4;铁通  5
        /// </summary>
        public virtual int Carrieroperator { get; set; }

        /// <summary>
        /// 站描述
        /// </summary>        
        public virtual string StationDescription { get; set; }


        /// <summary>
        /// 录入人
        /// </summary>        
        public virtual string AuditMan { get; set; }


        /// <summary>
        /// 录入时间
        /// </summary>        
        public virtual DateTime AuditDate { get; set; }


        /// <summary>
        /// 修改人
        /// </summary>        
        public virtual string AuthorizeMan { get; set; }


        /// <summary>
        /// 修改时间
        /// </summary>        
        public virtual DateTime AuthorizeDate { get; set; }


        /// <summary>
        /// 自控面积
        /// </summary>        
        public virtual decimal StationHotArea { get; set; }


        /// <summary>
        /// 建筑面积
        /// </summary>        
        public virtual decimal StationOnNetAllArea { get; set; }


        /// <summary>
        /// 收费面积
        /// </summary>        
        public virtual decimal StationOnNetUseArea { get; set; }


        /// <summary>
        /// 修正面积
        /// </summary>        
        public virtual decimal StationHotUseArea { get; set; }


        /// <summary>
        /// 站点的联系人
        /// </summary>        
        public virtual string StationDutyMan { get; set; }


        /// <summary>
        /// 站点联系人电话
        /// </summary>        
        public virtual string StationDutyManContact { get; set; }


        /// <summary>
        /// 站机组总数
        /// </summary>        
        public virtual int StationArrayCount { get; set; }


        /// <summary>
        /// 站点在CAD地图上的X坐标
        /// </summary>        
        public virtual decimal WebX { get; set; }


        /// <summary>
        /// 站点在CAD地图上的Y坐标
        /// </summary>        
        public virtual decimal WebY { get; set; }


        /// <summary>
        /// 通信协议的供应商:天时自定义 1;天时平台 2;PVSS  3
        /// </summary>        
        public virtual int IPROVIDER { get; set; }


        /// <summary>
        /// 协议类型
        /// </summary>        
        public virtual int Ricategory { get; set; }
        /// <summary>
        /// 工艺图对应的图片路径
        /// </summary>
        public virtual string FlowChart { get; set; }

        public virtual string IsMonitor { get; set; }
        /// <summary>
        /// 枪机地址
        /// </summary>
        public virtual string GunCamera { get; set; }
        /// <summary>
        /// 球机地址
        /// </summary>
        public virtual string PtzCamera { get; set; }

        public virtual int StationTypeID { get; set; }

        public virtual bool IsSendCmd { get; set; }


        /// <summary>
        /// 热力站是否加入全网平衡(1:是;0:否)
        /// 1：二次供回均温控制 2：二次回温控制 3：室外温度控制
        /// </summary>        
        public virtual int IsJionHeatBalance { get; set; }
        /// <summary>
        /// 站点阀门控制类型1：总阀控；2：分阀控
        /// </summary>        
        public virtual int StaitionValveType { get; set; }

        /// <summary>
        /// 排序字段（天津能源用）
        /// </summary>
        public virtual int OrderByData { get; set; }
        /// <summary>
        /// 机组站（站点按机组添加）机组站名称
        /// </summary>
        public virtual string StationGroupName { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public string Xaxis { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public string Yaxis { get; set; }

        /// <summary>
        /// 热网类型（集中供热系统/区域供热系统）
        /// </summary>
        public virtual int HeatNetType { get; set; }

        /// <summary>
        /// 热源类型（电厂/燃气锅炉房/煤气锅炉房）
        /// </summary>
        public virtual int SourceType { get; set; }

        /// <summary>
        /// 热站类型（住宅区/工业区）
        /// </summary>
        public virtual int StationType { get; set; }

        /// <summary>
        /// 是否为自管站
        /// </summary>
        public virtual int IsSelfManage { get; set; }
        /// <summary>
        /// 换热站归属热网
        /// </summary>
        public virtual int HeatNetID { get; set; }
        /// <summary>
        /// 设计功率
        /// </summary>
        public virtual string DisignPower { get; set; }
    }
}
