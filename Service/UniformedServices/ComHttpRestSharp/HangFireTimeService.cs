using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using THMS.Core.API.Logs;
using THMS.Core.API.ModelDto;

namespace THMS.Core.API.Service.UniformedServices.ComHttpRestSharp
{
    /// <summary>
    /// HangFire定时任务调度
    /// </summary>
    public class HangFireTimeService 
    {
        /// <summary>
        /// 周期性任务调度
        /// </summary>
        /// <param name="func">执行方法</param>
        /// <param name="cron">Cron表达式</param>
        public static int Period(Expression<Action> func, string cron)
        {
            RecurringJob.AddOrUpdate(func,cron);
            Logger.Info($"周期性任务调度开始:{DateTime.Now}");
            return ResultCode.Success;
        }

        /// <summary>
        /// 立即执行任务调度
        /// </summary>
        public static int Enqueue(Expression<Action> func)
        {
            BackgroundJob.Enqueue(func);
            Logger.Info($"立即执行任务调度:{DateTime.Now}");
            return ResultCode.Success;
        }
    }
}
