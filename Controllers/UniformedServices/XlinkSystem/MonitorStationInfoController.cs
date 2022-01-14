using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using THMS.Core.API.Configuration;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;
using THMS.Core.API.Service.UniformedServices.XlinkSystem;
using THMS.Core.API.Logs;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;

namespace THMS.Core.API.Controllers.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 换热站监测数据信息控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "XlinkSystem")]
    [ApiController]
    [EnableCors("any")]
    public class MonitorStationInfoController : ControllerBase
    {
        /// <summary>
        /// 热源及换热站检测点指标列表
        /// </summary>
        /// <param name="staionId"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryStationFirstCheckpointList(int staionId)
        {
            string resultJson = new MonitorStationInfoServices().queryStationFirstCheckpointList(staionId);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 换热站一次实时监测数据
        /// </summary>
        /// <param name="sourceId">换热站Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryStationFirstRealData(int sourceId)
        {
            string resultJson = new MonitorStationInfoServices().queryStationFirstRealData(sourceId);
            var resultJsonData = JsonConvert.DeserializeObject(resultJson);
            return resultJsonData;
        }

        /// <summary>
        /// 换热站二次检测点指标列表
        /// </summary>
        /// <param name="staionId">换热站Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryStationSecondCheckpointList(int staionId)
        { 
            string resultJson = new MonitorStationInfoServices().queryStationSecondCheckpointList(staionId);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 换热站一次侧历史数据
        /// </summary>
        /// <param name="historyFirstParamesDto"></param>
        /// <returns></returns>
        [HttpPost]
        public object queryStationFirstHisData(HistoryFirstParamesDto historyFirstParamesDto)
        {
            var result = new MonitorStationInfoServices().queryStationFirstHisData(historyFirstParamesDto);
            return result;
        }

        /// <summary>
        /// 换热站二次历史监测数据
        /// </summary>
        /// <param name="historySecondParamesDto"></param>
        /// <returns></returns>
        [HttpPost]
        public object queryStationSecondHisData(HistorySecondParamesDto historySecondParamesDto)
        {
            var result = new MonitorStationInfoServices().queryStationSecondHisData(historySecondParamesDto);
            return result;
        }

        /// <summary>
        /// 换热站二次实时监测数据
        /// </summary>
        /// <param name="staionId"></param>
        /// <param name="narray_no"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object queryStationSecondRealData(int staionId , int narray_no)
        {
            string resultJson = new MonitorStationInfoServices().queryStationSecondRealData(staionId , narray_no);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

        /// <summary>
        /// 获取全网平衡实时数据
        /// </summary>
        /// <param name="id">热网Id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object getNetworkBalanceRealData(int id)
        {
            string resultJson = new MonitorStationInfoServices().getNetworkBalanceRealData(id);
            var resultJsonArray = JsonConvert.DeserializeObject(resultJson);
            return resultJsonArray;
        }

       

        /// <summary>
        /// 换热站能耗数据生成.txt文件定时执行
        /// </summary>
        /// <param name="cron">Cron表达式</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public string StationEnergyFileCreate(string cron)
        {
            //获取换热站能耗数据生成应用方法
            EnergyFileCreateService energyFileCreateService = new EnergyFileCreateService();
            //注册表达式树用于定时调度执行
            Expression<Action> func = () => energyFileCreateService.FilesCreate();
            if (HangFireTimeService.Period(func,cron) == ResultCode.Success)
            {
                return ResultMessageInfo.SuccessMessage;
            }
            else
            {
                return ResultMessageInfo.ErrorMessage;
            }
        }

        /// <summary>
        /// 换热站能耗数据生成.txt文件定时执行
        /// </summary>
        /// <param name="cron">Cron表达式</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public string TestStationEnergyFileCreate(string cron)
        {

            //获取换热站能耗数据生成应用方法
            EnergyFileCreateService energyFileCreateService = new EnergyFileCreateService();
            energyFileCreateService.FilesCreate();
            return "success";
            ////注册表达式树用于定时调度执行
            //Expression<Action> func = () => energyFileCreateService.FilesCreate();
            //if (HangFireTimeService.Period(func, cron) == ResultCode.Success)
            //{
            //    return ResultMessageInfo.SuccessMessage;
            //}
            //else
            //{
            //    return ResultMessageInfo.ErrorMessage;
            //}
        }
        /// <summary>
        /// FTP测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void Ceshi()
        {
            new EnergyFileCreateService().FilesCreate();
        }

        /// <summary>
        /// Hangfire定时任务测试
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        //[HttpGet]
        //public string Hangfire(string cron) {
        //    EnergyFileCreateService energyFileCreateService = new EnergyFileCreateService();
        //    Expression<Action> func = () => energyFileCreateService.HangfireCeshi();
        //    HangFireTimeService.Period(func, cron);
        //    return "执行完毕";
        //}

        /// <summary>
        /// 读取.txt文件流 HeatSource_yyyyMMddHH
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFileDataTxtHeatStation()
        {
            string filepath = $@"FileData/{DqHxConfigSetting.HxFileNameHeatStation}{DateTime.Now.ToString("yyyyMMddHH")}.txt";
            if (string.IsNullOrEmpty(filepath)) filepath = @"FileData/ErrorMessage.txt";
            var provider = new FileExtensionContentTypeProvider();
            FileInfo fileInfo = new FileInfo(filepath);
            var ext = fileInfo.Extension;
            new FileExtensionContentTypeProvider().Mappings.TryGetValue(ext, out var contenttype);
            Logger.Info($"接口文件:{filepath}已被调取;调取时间为:{DateTime.Now}");
            return File(System.IO.File.ReadAllBytes(filepath), contenttype ?? "application/octet-stream", fileInfo.Name);
        }

        /// <summary>
        /// 换热站指标展示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object queryHeatStationIndex(int stationId)
        {
            var resultJson = new MonitorStationInfoServices().queryHeatStationIndex(stationId);
            var result = JsonConvert.DeserializeObject(resultJson);
            return result;
        }

        /// <summary>
        /// 获取换热站水利平衡信息
        /// </summary>
        /// <param name="id">-1为全部换热站信息,如获取某换热站信息直接传id编号</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object querySlphHeatplantCalcHistory(int id)
        {
            return new MonitorStationInfoServices().querySlphHeatplantCalcHistory(id);
        }

        /// <summary>
        /// 一级网管径信息添加
        /// </summary>
        /// <param name="primaryPipeNetwork"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int InsertPrimaryPipeNetwork(List<PrimaryPipeNetwork> primaryPipeNetwork)
        {
            return new MonitorStationInfoServices().InsertPrimaryPipeNetwork(primaryPipeNetwork);
        }

        /// <summary>
        /// 一级网管径信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public int DeletePrimaryPipeNetworkById(int id)
        {
            return new MonitorStationInfoServices().DeletePrimaryPipeNetworkById(id);
        }

        /// <summary>
        /// 一级网管径信息更改
        /// </summary>
        /// <param name="primaryPipeNetwork"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int UpdPrimaryPipeNetwork(PrimaryPipeNetwork primaryPipeNetwork)
        {
            return new MonitorStationInfoServices().UpdPrimaryPipeNetwork(primaryPipeNetwork);
        }

        /// <summary>
        /// 二级网管径信息添加
        /// </summary>
        /// <param name="secondaryPipeNetwork"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int InsertSecondaryPipeNetwork(List<SecondaryPipeNetwork> secondaryPipeNetwork)
        {
            return new MonitorStationInfoServices().InsertSecondaryPipeNetwork(secondaryPipeNetwork);
        }

        /// <summary>
        /// 二级网管径信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public int DeleteSecondaryPipeNetworkById(int id)
        {
            return new MonitorStationInfoServices().DeleteSecondaryPipeNetworkById(id);
        }

        /// <summary>
        /// 二级网管径信息更改
        /// </summary>
        /// <param name="secondaryPipeNetwork"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int UpdSecondaryPipeNetwork(SecondaryPipeNetwork secondaryPipeNetwork)
        {
            return new MonitorStationInfoServices().UpdSecondaryPipeNetwork(secondaryPipeNetwork);
        }
    }
}