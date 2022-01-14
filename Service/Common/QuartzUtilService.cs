using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Logs;

namespace THMS.Core.API.Service.Common
{
    public class QuartzUtilService
    {
        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        public QuartzUtilService(ISchedulerFactory schedulerFactory) {
            _schedulerFactory = schedulerFactory;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="type">类</param>
        /// <param name="jobKey">键</param>
        /// <param name="trigger">触发器</param>
        public static async Task Add(Type type, JobKey jobKey, ITrigger trigger = null)
        {
            //Init();
            _scheduler = await _schedulerFactory.GetScheduler();

            await _scheduler.Start();

            if (trigger == null)
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity("april.trigger")
                    .WithDescription("default")
                    .WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow().WithRepeatCount(-1))
                    .Build();
            }
            var job = JobBuilder.Create(type)
                .WithIdentity(jobKey)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public static async Task Resume(JobKey jobKey)
        {
            //Init();
            _scheduler = await _schedulerFactory.GetScheduler();
            Logger.Info($"恢复任务{jobKey.Group},{jobKey.Name}");
            await _scheduler.ResumeJob(jobKey);
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public static async Task Stop(JobKey jobKey)
        {
            //Init();
            _scheduler = await _schedulerFactory.GetScheduler();
            Logger.Info($"暂停任务{jobKey.Group},{jobKey.Name}");
            await _scheduler.PauseJob(jobKey);
        }



        
        //private static void Init()
        //{
        //    if (_schedulerFactory == null)
        //    {
        //        _schedulerFactory = AprilConfig.ServiceProvider.GetService<ISchedulerFactory>();
        //    }
        //}
    }
}
