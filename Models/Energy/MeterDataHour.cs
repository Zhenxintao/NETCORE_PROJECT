using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Energy
{
    /// <summary>
    /// 能耗小时表
    /// </summary>
    public class MeterDataHour
    {
        /// <summary>
        /// Id 自增编号
        /// </summary>
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }
        /// <summary>
        /// 站点ID
        /// </summary>
        public virtual int Vpnuser_id { get; set; }
        /// <summary>
        /// 机组编号
        /// </summary>
        public virtual int NarrayNo { get; set; }
        /// <summary>
        /// 表初码
        /// </summary>
        public virtual decimal TotalStart { get; set; }
        /// <summary>
        /// 表末码
        /// </summary>
        public virtual decimal TotalEnd { get; set; }
        /// <summary>
        /// 表用量
        /// </summary>
        public virtual decimal Total { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        public virtual int UserInfo_id { get; set; }
        /// <summary>
        /// 添加日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 审批日期
        /// </summary>
        public virtual DateTime ApproveDate { get; set; }
        /// <summary>
        /// 是否生成费用0:未生成，1：生成
        /// </summary>
        public virtual bool GeneralCharge { get; set; }
        /// <summary>
        /// 数据状态：0：录入 1：已上报 2：审核中 3：已审核，4：驳回，5：系统生成
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 1:水表2：电表 3：热表：4计量表 101:热源水表 102:热源电表 103:热源热表
        /// </summary>
        public virtual int MeterType { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public virtual decimal Area { get; set; }
        /// <summary>
        /// 水/电/热指标
        /// </summary>
        public virtual decimal Target { get; set; }
        /// <summary>
        /// 数据所属日期
        /// </summary>
        public virtual DateTime MeterDate { get; set; }
    }
}
