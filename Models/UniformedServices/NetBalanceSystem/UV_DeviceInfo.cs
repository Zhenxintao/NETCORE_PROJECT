// File:    UV_DeviceInfo.cs
// Author:  tscc
// Created: 2019年9月4日 11:26:33
// Purpose: Definition of Class UV_DeviceInfo

using System.Collections.Generic;

namespace THMS.Core.API.Models
{
    ///<summary>
    ///单元阀设备信息
    ///</summary>
    public class UV_DeviceInfo
    {
   
        ///<summary>
        ///Id
        ///</summary>
        public int Id{get;set;}
   
        ///<summary>
        ///GUID
        ///</summary>
        public string UV_DeviceInfo_id{get;set;}
   
        ///<summary>
        ///设备名称
        ///</summary>
        public string DeviceName{get;set;}
   
        ///<summary>
        ///设备编码
        ///</summary>
        public string DeviceCode{get;set;}
   
        ///<summary>
        ///物联网卡号
        ///</summary>
        public string IOTCard{get;set;}
   
        ///<summary>
        ///Ip地址
        ///</summary>
        public string IpAddress{get;set;}
   
        ///<summary>
        ///端口号
        ///</summary>
        public int IpPort{get;set;}
   
        ///<summary>
        ///Ism卡号
        ///</summary>
        public string IMSI{get;set;}
   
        ///<summary>
        ///数据上传间隔（分钟）
        ///</summary>
        public int SendInterval{get;set;}
   
        ///<summary>
        ///是否有效
        ///</summary>
        public bool IsValid{get;set;}
   
        ///<summary>
        ///是否删除
        ///</summary>
       public bool IsDelete { get; set; } = false;
   
        ///<summary>
        ///设备生产商
        ///</summary>
        public string DeviceManufacturer{get;set;}
   
        ///<summary>
        ///通讯协议(规约)
        ///</summary>
        public string CommunitcationProtocol{get;set;}
   
        ///<summary>
        ///设备口径
        ///</summary>
        public string DeviceCaliber{get;set;}
   
        ///<summary>
        ///设备型号
        ///</summary>
        public string DeviceModel{get;set;}
   
        ///<summary>
        ///通讯类型
        ///</summary>
        public string DeviceCommunication{get;set;}
   
        ///<summary>
        ///备注
        ///</summary>
        public string Remarks{get;set;}


        

    }
}