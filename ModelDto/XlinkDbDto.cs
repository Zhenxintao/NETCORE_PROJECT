using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    public class XlinkDbDto
    {
        //public string VpnUserId { get; set; }
        //public string AiValue { get; set; }
        //public string NarrayNo { get; set; }
        //public string AiDesc { get; set; }
        //public string AiType { get; set; }
        //public string TagName { get; set; }
        //public string Unit { get; set; }
        //public string PvssTagName { get; set; }

        /// <summary>
        /// ValueDescID
        /// </summary>  
        //public virtual int Id { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>        
        public virtual string VpnUser_id { get; set; }
        //public virtual VpnUser VpnUser { get; set; }


        /// <summary>
        /// 存储代码
        /// </summary>        
        public virtual string AiValue { get; set; }


        /// <summary>
        /// 机组号
        /// </summary>        
        public virtual string NarrayNo { get; set; }


        /// <summary>
        /// 描述
        /// </summary>        
        public virtual string AiDesc { get; set; }


        /// <summary>
        /// 存储代码类型
        /// </summary>        
        public virtual string AiType { get; set; }


        /// <summary>
        /// 标签名
        /// </summary>        
        public virtual string TagName { get; set; }


        /// <summary>
        /// 实际值
        /// </summary>        
        public virtual string RealValue { get; set; }


        /// <summary>
        /// 单位
        /// </summary>        
        public virtual string Unit { get; set; }


        /// <summary>
        /// 报警标志
        /// </summary>        
        public virtual int SignWarn { get; set; }


        /// <summary>
        /// 事故高报警值
        /// </summary>        
        public virtual decimal HiHi { get; set; }


        /// <summary>
        /// 运行高报警值
        /// </summary>        
        public virtual decimal Hi { get; set; }


        /// <summary>
        /// 事故低报警值
        /// </summary>        
        public virtual decimal LoLo { get; set; }


        /// <summary>
        /// 运行低报警值
        /// </summary>        
        public virtual decimal Lo { get; set; }


        /// <summary>
        /// 报警确认
        /// </summary>        
        public virtual int AlarmConfirm { get; set; }


        /// <summary>
        /// 报警ID
        /// </summary>        
        public virtual int AlarmID { get; set; }


        /// <summary>
        /// 计算标志
        /// </summary>        
        public virtual string SignValue { get; set; }


        /// <summary>
        /// 量程高
        /// </summary>        
        public virtual decimal RangeHi { get; set; }


        /// <summary>
        /// 量程低
        /// </summary>        
        public virtual decimal RangeLo { get; set; }


        /// <summary>
        /// 物理高
        /// </summary>        
        public virtual decimal ScaleHi { get; set; }


        /// <summary>
        /// 物理低
        /// </summary>        
        public virtual decimal ScaleLo { get; set; }


        /// <summary>
        /// 是否报警
        /// </summary>        
        public virtual bool IsAlarm { get; set; }


        /// <summary>
        /// 是否显示曲线
        /// </summary>        
        public virtual bool IsImage { get; set; }


        /// <summary>
        /// FIX值类型
        /// </summary>        
        public virtual string FixType { get; set; }


        /// <summary>
        /// 显示顺序（-1、只在后台显示）
        /// </summary>        
        public virtual int ValueSeq { get; set; }


        /// <summary>
        /// 用于存储颜色值
        /// </summary>        
        public virtual string MachineType { get; set; }


        /// <summary>
        /// 是否浮动显示
        /// </summary>        
        public virtual bool IsFloatShow { get; set; }


        /// <summary>
        /// 传输组数
        /// </summary>        
        public virtual int TransArray { get; set; }


        /// <summary>
        /// 开始字节
        /// </summary>        
        public virtual int StartByte { get; set; }


        /// <summary>
        /// 数据长度1、单字节整数；2、双字节整数；3、有符号整数；4、长整型；5、带符号整型；7、浮点数
        /// </summary>        
        public virtual int DataLength { get; set; }


        /// <summary>
        /// 数据类型“1”、参数是实际原值*1 进行计算；“10”、参数是实际原值*10 进行计算；“100”、参数是实际原值*100 进行计算；“1000”、参数是实际原值*1000 进行计算；
        /// “ByteAlarm”、 DI开关量；“TimeStamp”、采集时间；“TimeCheck”、巡岗时间
        /// </summary>        
        public virtual string DataType { get; set; }


        /// <summary>
        /// DI点采集量0意义
        /// </summary>        
        public virtual string ZeroMean { get; set; }


        /// <summary>
        /// DI点采集量1意义
        /// </summary>        
        public virtual string OneMean { get; set; }


        /// <summary>
        /// 值排序
        /// </summary>        
        public virtual int ValueSort { get; set; }
        /// <summary>
        /// 在工艺图的坐标X
        /// </summary>
        public virtual double Xval { get; set; }
        /// <summary>
        /// 在工艺图中的坐标Y
        /// </summary>
        public virtual double Yval { get; set; }

        /// <summary>
        /// 工艺图显示（1：显示）
        /// </summary>  
        public virtual int GYTShow { get; set; }
        /// <summary>
        /// 工艺图公共量显示（1：显示）
        /// </summary>  
        public virtual int GGLShow { get; set; }
        /// <summary>
        /// PVSS标示
        /// </summary>
        public virtual string PvssTagName { get; set; }

        /// <summary>
        /// 数据处理方式
        /// </summary>
        public virtual int DataFmtDesc { get; set; }

        /// <summary>
        /// 寄存器地址
        /// </summary>
        public virtual int MBAddress { get; set; }

        /// <summary>
        /// 吉林用量程
        /// </summary>
        public virtual string profix { get; set; }

        public virtual int GYTTagShow { get; set; }
    }
}
