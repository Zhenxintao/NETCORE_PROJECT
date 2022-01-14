using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiModel
{
    /// <summary>
    /// StationBranch
    /// </summary>    
    public class StationBranch
    {
        //public StationBranch()
        //{
        //    VpnUser = new VpnUser();
        //}

        /// <summary>
        /// StationBranchID
        /// </summary>  
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }

        /// <summary>
        /// VpnUserID
        /// </summary>        
        public virtual int VpnUser_id { get; set; }
        public virtual int ParentId { get; set; }
       // public virtual VpnUser VpnUser { get; set; }

        /// <summary>
        /// StationBranchName
        /// </summary>        
        public virtual string StationBranchName { get; set; }


        /// <summary>
        /// StationBranchArrayNumber
        /// </summary>        
        public virtual int StationBranchArrayNumber { get; set; }


        /// <summary>
        /// StationBranchType
        /// </summary>        
        public virtual int StationBranchType { get; set; }


        /// <summary>
        /// HeatMeterDiameter
        /// </summary>        
        public virtual decimal HeatMeterDiameter { get; set; }


        /// <summary>
        /// HeatNetDiameter
        /// </summary>        
        public virtual decimal HeatNetDiameter { get; set; }


        /// <summary>
        /// HeatMeterNumber
        /// </summary>        
        public virtual int HeatMeterNumber { get; set; }


        /// <summary>
        /// HeatMeterLocation
        /// </summary>        
        public virtual string HeatMeterLocation { get; set; }

        /// <summary>
        /// 机组全网平衡二次供回平均温度修正值
        /// </summary>        
        public virtual decimal ValveCorrectValue { get; set; }

        /// <summary>
        /// 机组供热面积
        /// </summary>
        public virtual decimal StationBranchHeatArea { get; set; }

        /// <summary>
        /// 费用类型 1：生产；2：生活；
        /// </summary>
        public virtual int PayType { get; set; }

        /// <summary>
        /// 采暖方式 1：地暖；2：挂暖；3混合供暖
        /// </summary>
        public virtual int HeatingType { get; set; }

        /// <summary>
        /// 地板采暖散热管间距类型（1：300mm;2：250mm;3：200mm;4：150mm）
        /// </summary>
        public virtual int RadiatingTubeType { get; set; }

        /// <summary>
        /// 用热类型（挂暖/地暖）
        /// </summary>
        public virtual int HeatType { get; set; }

        /// <summary>
        /// 是否节能建筑
        /// </summary>
        public virtual int IsEnergySave { get; set; }
    }
}
