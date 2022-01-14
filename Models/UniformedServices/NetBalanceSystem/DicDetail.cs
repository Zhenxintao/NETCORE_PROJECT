using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 字典明细表
    /// </summary>
    public class DicDetail
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
        /// 主表关联id
        /// </summary>
        public Int32 Dic_id
        {
            set;
            get;
        }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string ItemName
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
        /// 排序
        /// </summary>
        public Int32 SortIndex
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
        /// <summary>
        /// true:默认
        /// </summary>
        public bool IsDefault
        {
            set;
            get;
        }
    }
}
