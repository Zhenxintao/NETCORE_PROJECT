using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 字典表
    /// </summary>
    public class Dic
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Int32 Id
        {
            set;
            get;
        }
        /// <summary>
        /// 唯一编码，系统默认
        /// </summary>
        public string ItemCode
        {
            set;
            get;
        }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string DicName
        {
            set;
            get;
        }
        /// <summary>
        /// 类型描述
        /// </summary>
        public string Description
        {
            set;
            get;
        }
        /// <summary>
        /// true:有效
        /// </summary>
        public bool IsValid
        {
            set;
            get;
        }
        /// <summary>
        /// true:删除
        /// </summary>
        public bool IsDelete
        {
            set;
            get;
        }
    }
}
