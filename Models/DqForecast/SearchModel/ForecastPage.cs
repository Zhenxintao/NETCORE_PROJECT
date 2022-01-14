﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.DqForecast.SearchModel
{
    /// <summary>
    /// 分页
    /// </summary>
    public class ForecastPage : ForecastBaseSearch
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 一页几行
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// 排序类型（正序：ASC ；倒序：DESC）
        /// </summary>
        public string SortType { get; set; }
    }
}
