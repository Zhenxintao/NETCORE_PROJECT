using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    /// <summary>
    /// 客服收费实时表
    /// </summary>
    public class CustomerSeverice
    {
        /// <summary>
        /// 楼ID
        /// </summary>        
        public int buildID { get; set; }
        /// <summary>
        /// 小区ID
        /// </summary>        
        public int communityId { get; set; }

        /// <summary>
        /// 分公司ID
        /// </summary>        
        public int filialeId { get; set; }

        /// <summary>
        /// 投诉类型
        /// </summary>        
        public string ComplaintType { get; set; }

        /// <summary>
        /// 投诉数量
        /// </summary>        
        public int ComplaintCount { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChargeSeverice
    {
        /// <summary>
        /// 楼ID
        /// </summary>        
        public int buildID { get; set; }
        /// <summary>
        /// 小区ID
        /// </summary>        
        public int communityId { get; set; }

        /// <summary>
        /// 分公司ID
        /// </summary>        
        public int filialeId { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>        
        public decimal Quantity { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>        
        public decimal RealQuantity { get; set; }

        /// <summary>
        /// 收费率
        /// </summary>        
        public decimal ChargeRate { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>        
        public string UpdateTime { get; set; }
    }

}
