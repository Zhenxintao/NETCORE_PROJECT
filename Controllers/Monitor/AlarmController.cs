using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models.Monitor;
using THMS.Core.API.Service.Monitor;

namespace THMS.Core.API.Controllers.Monitor
{
    /// <summary>
    /// 报警相关方法
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Monitor")]
    [ApiController]
    [EnableCors("any")]
    public class AlarmController : ControllerBase
    {
        AlarmService _alarm = new AlarmService();

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
        [HttpGet]
        public List<AlarmDto> GetAlarmList(DateTime startdate, DateTime enddate, int isConfirm = -1, int vpnUserId = -1, string powerId = "-1", string organizationId = "-1")
        {
            var result = _alarm.GetAlarmList(startdate, enddate, isConfirm, vpnUserId, powerId, organizationId);

            return result;
        }

        /// <summary>
        /// 实时报警信息查询
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public List<Alarm> SelAlarmInfoList()
        {
            List<Alarm> alarmInfoList = _alarm.SelAlarmInfoList();
            return alarmInfoList;
        }

        /// <summary>
        /// 报警状态修改
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool UpdateAlarm(Alarm alarm)
        {
            var result = _alarm.UpdateAlarm(alarm);
            return result;
        }

        /// <summary>
        /// 报警弹框状态修改
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool UpdateIsAlarm(Alarm alarm)
        {
            var result = _alarm.UpdateIsAlarm(alarm);
            return result;
        }
    }
}