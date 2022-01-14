using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using THMS.Core.API.Logs;
using THMS.Core.API.Service.Forecast;

namespace THMS.Core.API.Controllers.Forecast
{
    /// <summary>
    /// 换热站负荷预测定时程序启停控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Forecast")]
    [ApiController]
    [EnableCors("any")]
    public class ForecastQuartzController : ControllerBase
    {
        private ISchedulerFactory _schedulerFactory;
        private  IScheduler _scheduler;
        public ForecastQuartzController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }
        /// <summary>
        /// 定时任务启停（请勿对该方法进行操作！）
        /// </summary>
        /// <param name="workType">1：启动任务 2：暂停任务 3：恢复任务 4:删除任务</param>
        /// <param name="SetTime">设置任务定时时间</param>
        /// <returns></returns>
        [HttpGet]
       // [Route("QuartzTest")]
        public async  Task<string>  QuartzTest(int workType,string SetTime)
        {
            JobKey jobKey = new JobKey("换热站负荷预测", "齐齐哈尔");
            switch (workType)
            {
                //添加任务
                case 1:
                    var trigger = TriggerBuilder.Create()
                            .WithDescription("触发器描述")
                            .WithIdentity("test")
                            .WithSchedule(CronScheduleBuilder.CronSchedule(SetTime).WithMisfireHandlingInstructionDoNothing())
                           //.WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
                            .Build();
                    // QuartzUtilService.Add(typeof(MyJob), jobKey, trigger);
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
                    var job = JobBuilder.Create(typeof(MyJob))
                        .WithIdentity(jobKey)
                        .Build();

                    await _scheduler.ScheduleJob(job, trigger);
                    Logger.Info($"开始任务：{jobKey.Group},{jobKey.Name}");
                    return "定时任务已开启";
        
                //暂停任务
                case 2:
                    _scheduler = await _schedulerFactory.GetScheduler();
                    await _scheduler.PauseJob(jobKey);
                    Logger.Info($"暂停任务：{jobKey.Group},{jobKey.Name}");
                    return "定时任务已暂停";
          
                //恢复任务
                case 3:
                    _scheduler = await _schedulerFactory.GetScheduler();
                    await _scheduler.ResumeJob(jobKey);
                    Logger.Info($"恢复任务：{jobKey.Group},{jobKey.Name}");
                    return "定时任务已恢复";
                case 4:                   
                    try
                    {
                    _scheduler = await _schedulerFactory.GetScheduler();
                    await _scheduler.DeleteJob(jobKey);
                    Logger.Info($"删除任务：{jobKey.Group},{jobKey.Name}");
                    return "定时任务已删除";

                    }
                    catch (Exception ex)
                    {
                        Logger.Info(ex.Message);
                        throw;
                    }
            }
            return null;
        }

        #region 旧代码
        //[HttpGet]
        //public async Task<string> StationForecastType(int workType)
        //{

        //    string message = "";
        //    //1、通过调度工厂获得调度器
        //    _scheduler = await _schedulerFactory.GetScheduler();

        //    if (workType == 1)
        //    {

        //        //2、开启调度器
        //        await _scheduler.Start();
        //        message = "换热站负荷预测定时程序启动成功!";


        //    }
        //    else if (workType == 0) {
        //       await _scheduler.Shutdown();
        //        _scheduler = null;
        //        //message = "换热站负荷预测定时程序已停止!";
        //        return await Task.FromResult("换热站负荷预测定时程序已停止!");
        //    }


        //    //3、创建一个触发器
        //    var trigger = TriggerBuilder.Create()
        //        .WithCronSchedule("0 01 10 * * ?")
        //      /*.WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())*/   //每两秒执行一次
        //      .Build();

        //    //4、创建任务
        //    var jobDetail = JobBuilder.Create<MyJob>()
        //      .WithIdentity("job", "group")
        //      .Build();

        //    //5、将触发器和任务器绑定到调度器中
        //    await _scheduler.ScheduleJob(jobDetail, trigger);
        //    return await Task.FromResult(message);
        //}
        #endregion
    }

    /// <summary>
    /// //创建IJob的实现类，并实现Excute方法。
    /// </summary>
    public class MyJob : IJob//创建IJob的实现类，并实现Excute方法。
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                Logger.Info("执行齐齐哈尔换热站负荷预定时工作：天表、小时表。");
                string DayResult = new StationForecastRealDayService().AddStationForecastRealDayService();
                Logger.Info("天表信息此次工作任务的情况为:" + DayResult);

                string HourResult = new StationForecastRealHourService().AddStationForecastRealHourService();
                Logger.Info("小时表信息此次工作任务的情况为:" + HourResult);
            });
        }
    }

}