using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("valuedescsecond")]
    public partial class valuedescsecond
    {
           public valuedescsecond(){


           }
           /// <summary>
           /// Desc:量程高
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? RangeHi {get;set;}

           /// <summary>
           /// Desc:开关量解析方式，默认0:字节位解析,3:对应杰控3方式
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? DataFmtDesc {get;set;}

           /// <summary>
           /// Desc:数据类型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string DataType {get;set;}

           /// <summary>
           /// Desc:数据长度
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? DataLength {get;set;}

           /// <summary>
           /// Desc:开始字节
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? StartByte {get;set;}

           /// <summary>
           /// Desc:传输组数
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? TransArray {get;set;}

           /// <summary>
           /// Desc:在工艺图的坐标Y
           /// Default:
           /// Nullable:True
           /// </summary>           
           public float? Yval {get;set;}

           /// <summary>
           /// Desc:在工艺图的坐标X
           /// Default:
           /// Nullable:True
           /// </summary>           
           public float? Xval {get;set;}

           /// <summary>
           /// Desc:是否浮动显示
           /// Default:
           /// Nullable:True
           /// </summary>           
           public bool? IsFloatShow {get;set;}

           /// <summary>
           /// Desc:显示排序
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? ValueSeq {get;set;}

           /// <summary>
           /// Desc:是否报警v
           /// Default:
           /// Nullable:True
           /// </summary>           
           public bool? IsAlarm {get;set;}

           /// <summary>
           /// Desc:DI点意义
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string DiMean {get;set;}

           /// <summary>
           /// Desc:量程低
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? RangeLo {get;set;}

           /// <summary>
           /// Desc:自增id
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id {get;set;}

           /// <summary>
           /// Desc:运行低报警
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? Lo {get;set;}

           /// <summary>
           /// Desc:事故低报警
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? LoLo {get;set;}

           /// <summary>
           /// Desc:运行高报警
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? Hi {get;set;}

           /// <summary>
           /// Desc:事故高报警
           /// Default:
           /// Nullable:True
           /// </summary>           
           public decimal? HiHi {get;set;}

           /// <summary>
           /// Desc:单位
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Unit {get;set;}

           /// <summary>
           /// Desc:实际值
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string RealValue {get;set;}

           /// <summary>
           /// Desc:标签名称
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string TagName {get;set;}

           /// <summary>
           /// Desc:存储代码类型
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string AiType {get;set;}

           /// <summary>
           /// Desc:采集量描述
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string AiDesc {get;set;}

           /// <summary>
           /// Desc:机组编号
           /// Default:
           /// Nullable:True
           /// </summary>           
           public int? NarrayNo {get;set;}

           /// <summary>
           /// Desc:存储代码
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string AiValue {get;set;}

           /// <summary>
           /// Desc:站点关联id
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string VpnUser_id {get;set;}

    }
}
