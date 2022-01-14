using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Monitor;

namespace THMS.Core.API.Service.Report
{
    /// <summary>
    /// 历史数据报表
    /// </summary>
    public class HistoryDataReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="powerid">热源id</param>
        /// <param name="organizationid">组织结构id</param>
        /// <param name="StationStandard">站点类型</param>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="userconfig_id">用户列表配置id</param>
        /// <param name="chartDateType">历史数据类型</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public List<dynamic> ReportDataSource(string powerid, string organizationid, int StationStandard, int vpnuser_id, int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {




            return null;
        }
    }
}
