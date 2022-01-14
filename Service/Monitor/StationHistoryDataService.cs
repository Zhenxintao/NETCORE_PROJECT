using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.Monitor;
using THMS.Core.API.Service.Common;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Monitor
{
    /// <summary>
    /// 历史数据相关
    /// </summary>
    public class StationHistoryDataService : Controller
    {
        StationParameterService _stationParameterService = new StationParameterService();
        CommonService _commonService = new CommonService();

        /// <summary>
        /// 数据库连接串
        /// </summary>
        DbContextSqlSugar DbContext = new DbContextSqlSugar();

        /// <summary>
        /// 获取单个站点历史数据（返回List）
        /// </summary>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="narrayNo">机组编号</param>
        /// <param name="userconfig_id">用户列表配置id</param>
        /// <param name="chartDateType">历史数据类型</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public List<dynamic> GetHistoryData4Station(int vpnuser_id, int narrayNo, int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            //首先查询温度压力所对应的Aivalue值
            var paraList = _stationParameterService.GetStandardParameterList(userconfig_id);

            const string format =
            @"SELECT A.VpnUser_id VpnUserId,
                     V.StationName,
                     SB.StationBranchName,
                     CONVERT(DECIMAL(18, 2), SB.StationBranchHeatArea) AS StationBranchHeatArea,
                     {0}
              FROM dbo.{1} A
                  LEFT JOIN dbo.{2} B
                      ON A.VpnUser_id = B.VpnUser_id
                         AND A.CreateTime = B.CreateTime
                  LEFT JOIN dbo.VpnUser V
                      ON A.VpnUser_id = V.Id
                  JOIN dbo.StationBranch SB
                      ON A.VpnUser_id = SB.VpnUser_id
                         AND B.NarrayNo = SB.StationBranchArrayNumber
              WHERE A.VpnUser_id = @vpnUserId
                    AND B.NarrayNo = @narrayNo
                    AND A.CreateTime >= @begin
                    AND A.CreateTime <= @end;";


            string tableName0 = "";
            string tableName1 = "";

            var cols0 = new List<string>();
            var cols1 = new List<string>();

            switch (chartDateType)
            {
                case ChartDateType.Minutes:
                    tableName0 = "HistoryFirstData";
                    tableName1 = "HistorySecondData";
                    break;
                case ChartDateType.Hours:

                    tableName0 = "HistoryFirstDataHour";
                    tableName1 = "HistorySecondDataHour";
                    break;
                case ChartDateType.Days:
                    tableName0 = "HistoryFirstDayAvg";
                    tableName1 = "HistorySecondDayAvg";
                    break;
            }
            cols0.Add("a.CreateTime AS CreateTime,b.NarrayNo AS 'NarrayNo'");

            foreach (var desc in paraList)
            {
                if (desc.NarrayNo == 0)
                {
                    cols1.Add(string.Format(" a.{0} AS {1} ", desc.AiValue, desc.TagName.ToUpper()));
                }
                else
                {
                    cols1.Add(string.Format(" b.{0} AS {1} ", desc.AiValue, desc.TagName.ToUpper()));
                }
            }

            var sql = string.Format(format, string.Join(",", cols0) + "," + string.Join(",", cols1), tableName0,
                tableName1, " ORDER BY A.CreateTime DESC ");

            var ViewList = DbContext.Db.Ado.SqlQuery<dynamic>(sql, new SugarParameter[]{
                    new SugarParameter("@vpnUserId", vpnuser_id),
                    new SugarParameter("@narrayNo", narrayNo),
                    new SugarParameter("@begin", startDate),
                    new SugarParameter("@end", endDate)});

            var points = new List<HistoryDataInfo>() { };

            return ViewList;
        }

        /// <summary>
        /// 获取单个热源历史数据（返回List）
        /// </summary>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="narrayNo">机组编号</param>
        /// <param name="userconfig_id">用户列表配置id</param>
        /// <param name="chartDateType">历史数据类型</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public List<dynamic> GetHistoryData4Power(int vpnuser_id, int narrayNo, int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            //01首先查询温度压力所对应的Aivalue值
            var paraList = _stationParameterService.GetStandardParameterList(userconfig_id);

            const string format =
            @"SELECT A.VpnUser_id VpnUserId,
                     V.StationName,
                     SB.StationBranchName,
                     CONVERT(DECIMAL(18, 2), SB.StationBranchHeatArea) AS StationBranchHeatArea,
                     {0}
              FROM dbo.{1} A 
                  LEFT JOIN dbo.{2} B
                      ON A.VpnUser_id = B.VpnUser_id
                         AND A.CreateTime = B.CreateTime
                  LEFT JOIN dbo.VpnUser V
                      ON A.VpnUser_id = V.Id
                  JOIN dbo.StationBranch SB
                      ON A.VpnUser_id = SB.VpnUser_id
                         AND B.NarrayNo = SB.StationBranchArrayNumber
              WHERE A.VpnUser_id = @vpnUserId
                    AND B.NarrayNo = @narrayNo
                    AND A.CreateTime >= @begin
                    AND A.CreateTime <= @end;";


            string tableName0 = "";
            string tableName1 = "";

            var cols0 = new List<string>();
            var cols1 = new List<string>();

            switch (chartDateType)
            {
                case ChartDateType.Minutes:
                    tableName0 = "PowerHistoryFirstData";
                    tableName1 = "PowerHistorySecondData";
                    break;
                case ChartDateType.Hours:

                    tableName0 = "PowerHistoryFirstDataHour";
                    tableName1 = "PowerHistorySecondDataHour";
                    break;
                case ChartDateType.Days:
                    tableName0 = "PowerHistoryFirstDayAvg";
                    tableName1 = "PowerHistorySecondDayAvg";
                    break;
            }
            cols0.Add("a.CreateTime AS CreateTime,b.NarrayNo AS 'NarrayNo'");

            foreach (var desc in paraList)
            {
                if (desc.NarrayNo == 0)
                {
                    cols1.Add(string.Format(" a.{0} AS {1} ", desc.AiValue, desc.TagName.ToUpper()));
                }
                else
                {
                    cols1.Add(string.Format(" b.{0} AS {1} ", desc.AiValue, desc.TagName.ToUpper()));
                }
            }

            var sql = string.Format(format, string.Join(",", cols0) + "," + string.Join(",", cols1), tableName0,
                tableName1, " ORDER BY A.CreateTime DESC ");

            var ViewList = DbContext.Db.Ado.SqlQuery<dynamic>(sql, new SugarParameter[]{
                    new SugarParameter("@vpnUserId", vpnuser_id),
                    new SugarParameter("@narrayNo", narrayNo),
                    new SugarParameter("@begin", startDate),
                    new SugarParameter("@end", endDate)});

            var points = new List<HistoryDataInfo>() { };

            return ViewList;
        }

        /// <summary>
        /// 获取站点历史数据
        /// </summary>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="tagname">tagname</param>
        /// <param name="chartDateType">0:分钟、1:小时、2:天</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public List<dynamic> GetHistoryData4Station(int vpnuser_id, string tagname, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            //首先查询温度压力所对应的Aivalue值
            var paraList = _stationParameterService.GetStandardParameterList(tagname).First();

            const string format =
           @"SELECT 
                    V.StationName,
                    S.StationBranchName,
                    A.VpnUser_id VpnUserId,
                    A.CreateTime,
                    {0} AS Value,
                    A.NarrayNo
             FROM dbo.{1} A LEFT JOIN vpnuser V ON A.VpnUser_id = V.id LEFT JOIN StationBranch S ON V.id = S.VpnUser_id AND A.NarrayNo = S.StationBranchArrayNumber
             WHERE 1 = 1
                   AND A.CreateTime
                   BETWEEN @begin AND @end
                   AND A.VpnUser_id = @vpnUserId
             ORDER BY A.CreateTime, A.NarrayNo ASC;";

            string tableName0 = "";
            string tableName1 = "";

            var cols0 = new List<string>();
            var cols1 = new List<string>();

            switch (chartDateType)
            {
                case ChartDateType.Minutes:
                    tableName0 = "HistoryFirstData";
                    tableName1 = "HistorySecondData";
                    break;
                case ChartDateType.Hours:

                    tableName0 = "HistoryFirstDataHour";
                    tableName1 = "HistorySecondDataHour";
                    break;
                case ChartDateType.Days:
                    tableName0 = "HistoryFirstDayAvg";
                    tableName1 = "HistorySecondDayAvg";
                    break;
            }
            if (paraList.NarrayNo == 0)
            {
                cols0.Add(tableName0);
            }
            else
            {
                cols0.Add(tableName1);
            }

            var sql = string.Format(format, string.Join(",", string.Format(" A.{0} ", paraList.AiValue)), string.Join(",", cols0));

            var ViewList = DbContext.Db.Ado.SqlQuery<dynamic>(sql, new SugarParameter[] {
                new SugarParameter("@vpnUserId", vpnuser_id),
                //new SugarParameter("@narrayNo", paraList.NarrayNo),
                new SugarParameter("@begin", startDate),
                new SugarParameter("@end", endDate)});

            return ViewList;
        }



















        /// <summary>
        /// 获取单个站点历史数据（返回曲线）
        /// </summary>
        /// <param name="vpnuser_id">站点id</param>
        /// <param name="narrayNo">机组编号</param>
        /// <param name="userconfig_id">用户列表配置id</param>
        /// <param name="chartDateType">历史数据类型</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public JsonResult GetHistoryData4StationLineChart(int vpnuser_id, int narrayNo, int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            var stations = new List<dynamic>();
            var createtimes = new List<dynamic>();

            var list = GetHistoryData4Station(vpnuser_id, narrayNo, userconfig_id, chartDateType, startDate, endDate);

            //01首先查询温度压力所对应的Aivalue值
            var paraList = _stationParameterService.GetStandardParameterList(userconfig_id);

            //返回曲线格式
            foreach (var tagItem in paraList)
            {
                var series = new List<dynamic>();

                createtimes = new List<dynamic>();

                foreach (var item in list)
                {
                    //xAxisData
                    createtimes.Add(Convert.ToDateTime(item.CreateTime).ToString("yyyy/MM/dd HH:mm:ss"));

                    series.Add(_commonService.GetDynamicValue(item, tagItem.TagName));
                }
                //数据
                dynamic dobj = new System.Dynamic.ExpandoObject();
                var dic = (IDictionary<string, object>)dobj;
                dic["name"] = tagItem.AiDesc;
                dic["date"] = series;

                stations.Add(dobj);
            }

            return Json(new
            {
                xAxisData = createtimes,
                series = stations,
                type = "line",
            });
        }
    }
}
