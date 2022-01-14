using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    ///<summary>
    ///住户楼栋信息表
    ///</summary>
    public class Sys_Building
    {
   
        ///<summary>
        ///Id
        ///</summary>
        public int Id{get;set;}
        /// <summary>
        /// guid列
        /// </summary>
        public string Building_id { get; set; }
        ///<summary>
        ///所属小区ID
        ///</summary>
        public string Community_id{get;set;}
   
        ///<summary>
        ///楼栋编号
        ///</summary>
        public int BuildingNum{get;set;}
   
        ///<summary>
        ///楼栋显示名称
        ///</summary>
        public string BuildingName{get;set;}
   
        ///<summary>
        ///建筑面积
        ///</summary>
        public decimal BuiltArea{get;set;}
   
        ///<summary>
        ///收费面积
        ///</summary>
        public decimal ChargeArea{get;set;}
   
    }
}