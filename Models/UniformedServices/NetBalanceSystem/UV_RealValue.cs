// File:    UV_RealValue.cs
// Author:  tscc
// Created: 2019年9月4日 11:26:33
// Purpose: Definition of Class UV_RealValue

namespace THMS.Core.API.Models
{
    ///<summary>
    ///单元阀实时数据
    ///</summary>
    public class UV_RealValue
    {
   
        ///<summary>
        ///Id
        ///</summary>
        public int Id{get;set;}
   
        ///<summary>
        ///单元阀GUID
        ///</summary>
        public string UV_DeviceInfo_id{get;set;}
   
        ///<summary>
        ///存储标识
        ///</summary>
        public string AiValue{get;set;}
   
        ///<summary>
        ///参量描述
        ///</summary>
        public string AiDesc{get;set;}
   
        ///<summary>
        ///参量类型
        ///</summary>
        public string AiType{get;set;}
   
        ///<summary>
        ///参量标签名
        ///</summary>
        public string TagName{get;set;}
   
        ///<summary>
        ///实际值
        ///</summary>
        public string RealValue{get;set;}
   
        ///<summary>
        ///单位
        ///</summary>
        public string Unit{get;set;}
   
        ///<summary>
        ///高报警
        ///</summary>
        public decimal HiHi{get;set;}
   
        ///<summary>
        ///低报警
        ///</summary>
        public decimal LoLo{get;set;}
   
        ///<summary>
        ///是否参与报警
        ///</summary>
        public bool IsAlarm{get;set;}
   
        ///<summary>
        ///开始字节
        ///</summary>
        public int StartByte{get;set;}
   
        ///<summary>
        ///字节长度
        ///</summary>
        public int DataLength{get;set;}
   
        ///<summary>
        ///数据类型
        ///</summary>
        public string DataType{get;set;}
   
        ///<summary>
        ///排序编号
        ///</summary>
        public int ValueSort{get;set;}
   
        ///<summary>
        ///参量编码（对接第三方）
        ///</summary>
        public string Io_Code{get;set;}

    }
}