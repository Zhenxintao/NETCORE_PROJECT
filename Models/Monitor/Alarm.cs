using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Monitor
{
    /// <summary>
    /// 报警模块
    /// </summary>
    public class Alarm
    {

        /// <summary>
        /// 主键Id
        /// </summary>
        /// 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public int VpnUser_id { get; set; }

        /// <summary>
        /// 报警参量TagName
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 报警开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 报警类型
        /// </summary>
        public string AlarmType { get; set; }

        /// <summary>
        /// 报警数值
        /// </summary>
        public decimal AlarmValue { get; set; }

        /// <summary>
        /// 报警描述
        /// </summary>
        public string AlarmDesc { get; set; }

        /// <summary>
        /// 报警高低限
        /// </summary>
        public string AlarmSetting { get; set; }

        /// <summary>
        /// 是否确认
        /// </summary>
        public int AlarmConfirm { get; set; }

        /// <summary>
        /// 报警确认人
        /// </summary>
        public string ConfirmMan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkMan { get; set; }

        /// <summary>
        /// 报警确认时间
        /// </summary>
        public DateTime ConfirmDate { get; set; }

        /// <summary>
        /// 报警级别
        /// </summary>
        public int AlarmCategory { get; set; }

        /// <summary>
        /// 是否继续弹出
        /// </summary>
        public bool IsAlert { get; set; }
        
        ///// <summary>
        ///// 热源名称
        ///// </summary>
        //public string PowerName { get; set; }

        ///// <summary>
        ///// 组织机构名称
        ///// </summary>
        //public string OrganizationName { get; set; }

        ///// <summary>
        ///// 站点名称
        ///// </summary>
        //public string StationName { get; set; }
        ///// <summary>
        ///// 站点名称简拼
        ///// </summary>
        //public string StationSabb { get; set; }

    }
}
