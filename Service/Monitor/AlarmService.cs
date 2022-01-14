using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Monitor;

namespace THMS.Core.API.Service.Monitor
{
    /// <summary>
    /// 报警相关
    /// </summary>
    public class AlarmService : DbContext.DbContextSqlSugar
    {
        AlarmDto _alarm = new AlarmDto();

        /// <summary>
        /// 报警日志查询
        /// </summary>
        /// <param name="startdate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="isConfirm">是否确认 -1：全部，1：已确认</param>
        /// <param name="vpnUserId">站点id</param>
        /// <param name="powerId">热源id</param>
        /// <param name="organizationId">组织结构id</param>
        /// <returns></returns>
        public List<AlarmDto> GetAlarmList(DateTime startdate, DateTime enddate, int isConfirm = -1, int vpnUserId = -1, string powerId = "-1", string organizationId = "-1")
        {
            var list = new List<AlarmDto>();
            try
            {
                var sql = @"SELECT P.StationName PowerName,
                                   O.OrganizationName,
                                   V.Id VpnUser_id,
                                   V.StationName,
                                   V.StationSabb,
                                   A.Id,
                                   A.TagName,
                                   A.StartDate,
                                   A.EndDate,
                                   AlarmType,
                                   A.AlarmValue,
                                   A.AlarmDesc,
                                   A.AlarmSetting,
                                   A.AlarmConfirm,
                                   A.ConfirmMan,
                                   A.ConfirmDate,
                                   A.AlarmCategory
                            FROM Alarm A
                                JOIN VpnUser V
                                    ON A.VpnUser_id = V.Id
                                JOIN Station S
                                    ON V.Id = S.VpnUser_id
                                JOIN VpnUser P
                                    ON S.PowerInfo_id = P.Id
                                JOIN Organization O
                                    ON V.Organization_id = O.Id
                            WHERE (
                                      V.Id = @vid
                                      OR @vid = -1
                                  )
                                  AND (V.IsValid = 1)
                                  AND
                                  (
                                      V.Organization_id IN ( @oid )
                                      OR '-1'IN ( @oid  )
                                  )
                                  AND
                                  (
                                      A.AlarmConfirm = @isConfirm
                                      OR @isConfirm = -1
                                  )
                                  AND
                                  (
                                      S.PowerInfo_id IN ( @pid )
                                      OR '-1' IN ( @pid ) 
                                  )
                                  AND StartDate
                                  BETWEEN @stime AND @etime
                            ORDER BY StartDate DESC;";

                list = Db.Ado.SqlQuery<AlarmDto>(sql, new SugarParameter[]{
                    new SugarParameter("@stime", startdate),
                    new SugarParameter("@etime", enddate),
                    new SugarParameter("@vid", vpnUserId),
                    new SugarParameter("@oid", organizationId.Split(",")),
                    new SugarParameter("@isConfirm", isConfirm),
                    new SugarParameter("@pid", powerId.Split(","))});
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// 实时报警信息查询
        /// </summary>
        /// <returns></returns>
        public List<Alarm> SelAlarmInfoList()
        {
            List<Alarm> alarmInfoList = Db.Queryable<Alarm>().Where(s => s.AlarmConfirm == 0).ToList();
            return alarmInfoList;
        }

        /// <summary>
        /// 报警状态修改
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public bool UpdateAlarm(Alarm alarm)
        {
            var result = Db.Updateable<Alarm>(alarm).UpdateColumns(s=>new { s.AlarmConfirm }).ExecuteCommand();
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 报警弹框状态修改
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public bool UpdateIsAlarm(Alarm alarm)
        {
            var result = Db.Updateable<Alarm>(alarm).UpdateColumns(s => new { s.IsAlert }).ExecuteCommand();
            return result > 0 ? true : false;
        }
    }
}
