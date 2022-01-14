using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 标准参量列表
    /// </summary>
    public class StandardParameter
    {
        /// <summary>
        /// StandardParameterID
        /// </summary>  
        public int Id { get; set; }

        /// <summary>
        /// 采集量描述
        /// </summary>        
        public string AiDesc { get; set; }

        /// <summary>
        /// 存储代码
        /// </summary>        
        public string AiValue { get; set; }

        /// <summary>
        /// 类型
        /// </summary>        
        public string AiType { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>        
        public string TagName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>        
        public string Unit { get; set; }

        /// <summary>
        /// 机组号
        /// </summary>        
        public int NarrayNo { get; set; }

        /// <summary>
        /// 是否参与报表
        /// </summary>        
        public bool IsReport { get; set; }

        /// <summary>
        /// 参量是否显示
        /// </summary>        
        public bool IsShow { get; set; }

        /// <summary>
        /// 是否作为主要参量显示（用于参数纵览）
        /// </summary>        
        public bool IsMainParametersShow { get; set; }

        /// <summary>
        /// 排序标准 -1隐藏
        /// </summary>        
        public int ValueSequence { get; set; }

        /// <summary>
        /// 解析类型
        /// </summary>        
        public int DataLength { get; set; }

        /// <summary>
        /// 数据类型倍率“1”、参数是实际原值*1 进行计算；“10”、参数是实际原值*10 进行计算；“100”、参数是实际原值*100 进行计算；“1000”、参数是实际原值*1000 进行计算；
        /// “ByteAlarm”、 DI开关量；“TimeStamp”、采集时间；“TimeCheck”、巡岗时间
        /// </summary>        
        public string DataType { get; set; }

        /// <summary>
        /// 是否参与超标
        /// </summary>
        public bool IsOverproofShow { get; set; }

        /// <summary>
        /// 是否显示曲线
        /// </summary>
        public bool IsCurveShow { get; set; }
    }
}
