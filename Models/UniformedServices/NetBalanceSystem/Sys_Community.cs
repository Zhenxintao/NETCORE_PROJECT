using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    ///<summary>
    ///住户小区信息表
    ///</summary>
    public class Sys_Community
    {
   
        ///<summary>
        ///Id
        ///</summary>
        public int Id{get;set;}
   
        ///<summary>
        ///Community_id
        ///</summary>
        public string Community_id{get;set;}
   
        ///<summary>
        ///小区名称
        ///</summary>
        public string CommunityName{get;set;}
   
        ///<summary>
        ///小区简拼
        ///</summary>
        public string CommunitySabb{get;set;}
   
        ///<summary>
        ///建筑面积
        ///</summary>
        public decimal BuiltArea{get;set;}
   
        ///<summary>
        ///收费面积
        ///</summary>
        public decimal ChargeArea{get;set;}
   
        ///<summary>
        ///物业名称
        ///</summary>
        public string PropertyName{get;set;}
   
        ///<summary>
        ///物业联系人
        ///</summary>
        public string PropertyUserName{get;set;}
   
        ///<summary>
        ///物业电话
        ///</summary>
        public string PropertyPhone{get;set;}
   
        ///<summary>
        ///小区地址
        ///</summary>
        public string Address{get;set;}
   
        ///<summary>
        ///示意图
        ///</summary>
        public string SketchMap{get;set;}
   
        ///<summary>
        ///删除标识：true
        ///</summary>
       public bool IsDelete { get; set; } = false;
   
        ///<summary>
        ///备注
        ///</summary>
        public string Remarks{get;set;}
   
     
    }
}