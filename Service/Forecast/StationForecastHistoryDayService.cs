using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Forecast
{
    public class StationForecastHistoryDayService:DbContextSqlSugar
    {
        /// <summary>
        /// 查询历史信息天表信息
        /// </summary>
        /// <param name="vpnuserid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="heatType"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Tuple<object, int> SelStationForecastHistoryHour(int vpnuserid, string startTime, string endTime, int heatType, int pageSize, int pageIndex)
        {
            var totalCount = 0;
            var list = Db.Queryable<StationForecastHistoryDay,StationBranch>((s, sbr) => new object[] { JoinType.Left, s.VpnUserId == sbr.VpnUser_id && s.StationBranchArrayNumber == sbr.StationBranchArrayNumber })
                .WhereIF(!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime), (s,sbr) => SqlFunc.Between(s.ForecastDateDay, startTime, endTime))
                .WhereIF(SqlFunc.HasNumber(vpnuserid), (s,sbr) => s.VpnUserId == vpnuserid)
                .WhereIF(SqlFunc.HasNumber(heatType), (s, sbr) => s.HeatingType == heatType)
                .Select((s, sbr) => new { s.Id, s.VpnUserId, s.StationName, sbr.StationBranchName, s.ForecastDateDay, s.HeatArea, s.ForecastHeatTarget, s.RealHeatTarget, s.FloorSurfaceAvgTemp, s.ForecastOutdoorTemp, s.RealOutdoorTemp, s.ForecastSecFlow, s.ForecastFlow, s.LoadRate, s.HourlyHeatDay, s.ForecastSecSendTemp, s.ForecastSecReturnTemp, s.RealSecSendTemp, s.RealSecReturnTemp, s.HeatingType, s.CreateTime, s.CreateUser, s.IsValid,sbr.StationBranchArrayNumber })
                .ToPageList(pageIndex, pageSize, ref totalCount);
            return new Tuple<object, int>(list, totalCount);
        }
    }
}
