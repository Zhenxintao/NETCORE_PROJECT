using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    ///<summary>
    ///住户单元信息表
    ///</summary>
    public class Sys_UnitNo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        ///<summary>
        ///UnitNo_id
        ///</summary>
        public string UnitNo_id{get;set;}
   
        ///<summary>
        ///所属小区楼栋ID
        ///</summary>
        public string Building_id{get;set;}
   
        ///<summary>
        ///单元编号
        ///</summary>
        public int UnitNoNum{get;set;}
   
        ///<summary>
        ///单元显示名称
        ///</summary>
        public string UnitNoName{get;set;}
   
        ///<summary>
        ///建筑面积
        ///</summary>
        public decimal BuiltArea { get;set;}
   
        ///<summary>
        ///收费面积
        ///</summary>
        public decimal ChargeArea{get;set;}
   
       

    }
}