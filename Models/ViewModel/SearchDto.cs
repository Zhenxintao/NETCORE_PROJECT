using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
   public class SearchDto
    {
        /// <summary>
        /// 能耗类型
        /// </summary>
        public int EnergyType { get; set; }

        /// <summary>
        /// 搜索查询开始采集时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 搜索查询结束采集时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 站点VpnUser_id
        /// </summary>
        public int?  VpnUserid { get; set; }

        /// <summary>
        /// 热源PowerInfoid
        /// </summary>
        public int? PowerInfoid { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public int? Organizationid { get; set; }

        /// <summary>
        /// 当前页标
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 排序列名
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string SortType { get; set; }


    }
}
