using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 同步记录信息表
    /// </summary>
    public class synchrorecord
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 外键Id
        /// </summary>
        public string Guid_Id { get; set; }

        /// <summary>
        /// 表类型，1：小区表，2：楼栋表，3：单元表，4：住户表
        /// </summary>
        public int TableType { get; set; }

        /// <summary>
        /// 同步类型，1：新增，2：更新，3：删除
        /// </summary>
        public int SynchroType { get; set;
        }
    }
}
