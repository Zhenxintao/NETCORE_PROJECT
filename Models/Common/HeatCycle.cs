using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
   public class HeatCycle
    {
        /// <summary>
        /// Id 自增编号
        /// </summary>
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual int Id { get; set; }
        /// <summary>
        /// 供暖期名称  唯一
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 供暖期开始日期
        /// </summary>
        public virtual DateTime StartDate { get; set; }
        /// <summary>
        /// 供暖期结束日期
        /// </summary>
        public virtual DateTime EndDate { get; set; }
        /// <summary>
        /// 供暖期描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 自行维护换热站数量
        /// </summary>
        public virtual int StationQuantity { get; set; }
        /// <summary>
        /// 供暖期是否有效
        /// </summary>
        public virtual bool IsValid { get; set; }
    }
}
