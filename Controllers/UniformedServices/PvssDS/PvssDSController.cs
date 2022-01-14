using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.UniformedServices.PvssDSSystem;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Service.UniformedServices.PvssDSSystem;

namespace THMS.Core.API.Controllers.UniformedServices.PvssDS
{
    /// <summary>
    /// PVSS数据同步控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "PVSS")]
    [ApiController]
    [EnableCors("any")]
    public class PvssDSController : ControllerBase
    {
        PvssDSServices pvssDS = new PvssDSServices();

        /// <summary>
        /// 同步热网，没有则新增热网信息，有则修改热网信息（PVSS用）
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int CopyPvss_Rw(PvssDSDto.MdjRw_PVSS item)
        {
            return pvssDS.CopyPvss_Rw(item);
        }

        /// <summary>
        /// 同步热源，没有则新增热源信息，有则修改热源信息（PVSS用）
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int CopyPvss_Power(PvssDSDto.MdjPower_PVSS item)
        {
            return pvssDS.CopyPvss_Power(item);
        }

        /// <summary>
        /// 同步站点，没有则新增站点信息和采集量，有则修改站点信息（PVSS用）
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int CopyPvss_Station(PvssDSDto.Station_PVSS station)
        {
            return pvssDS.CopyPvss_Station(station);
        }

        /// <summary>
        /// 新增组织结构（pvss用）
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int InsertOrgan(PvssDSDto.Organization organization)
        {
            return pvssDS.InsertOrgan(organization);
        }

        /// <summary>
        /// 修改组织结构（pvss用）
        /// </summary>
        /// <param name="organ"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int UpdateOrgan(PvssDSDto.UpdateOrgPvss organ)
        {
            return pvssDS.UpdateOrgan(organ);
        }

        #region 废弃热网、热源添加修改控制器
        ///// <summary>
        ///// 新增热网信息（pvss用）
        ///// </summary>
        ///// <param name="powerpvssw"></param>
        ///// <returns></returns>
        ///// 
        //[HttpPost]
        //public int PostPVSS_RW(PvssDSDto.PowerRW_PVSS powerpvssw)
        //{
        //    return pvssDS.PostPVSS_RW(powerpvssw);
        //}

        ///// <summary>
        ///// 修改热网信息（pvss用）
        ///// </summary>
        ///// <param name="powerInfo"></param>
        ///// <returns></returns>
        ///// 
        //[HttpPut]
        //public int PutPVSS_RW(PvssDSDto.PowerRW_PVSS powerInfo)
        //{
        //    return pvssDS.PutPVSS_RW(powerInfo);
        //}

        ///// <summary>
        ///// 新增热源信息（pvss用）
        ///// </summary>
        ///// <param name="powerpvss"></param>
        ///// <returns></returns>
        ///// 
        //[HttpPost]
        //public int PostPVSS_RY(PvssDSDto.PowerRY_PVSS powerpvss)
        //{
        //    return pvssDS.PostPVSS_RY(powerpvss);
        //}

        ///// <summary>
        ///// 修改热源信息（pvss用）
        ///// </summary>
        ///// <param name="powerInfo"></param>
        ///// <returns></returns>
        ///// 
        //[HttpPut]
        //public int PutPVSS_RY(PvssDSDto.PowerRY_PVSS powerInfo)
        //{
        //    return pvssDS.PutPVSS_RY(powerInfo);
        //}
        #endregion

        /// <summary>
        /// 删除热网、热源、换热站信息
        /// </summary>
        /// <param name="PcName"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public int DeletePVSS(string PcName, string isValid)
        {
            return pvssDS.DeletePVSS(PcName, isValid);
        }

        /// <summary>
        ///  恢复热网、热源、换热站状态，并往HX添加新数据
        /// </summary>
        /// <param name="pvssId">pvssId</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public int ReNewPVSS(string pvssId)
        {
            return pvssDS.ReNewPVSS(pvssId);
        }

        /// <summary>
        /// 新增、更新全网平衡信息
        /// </summary>
        /// <param name="netBalance"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int NetBalancePVSS(NetBalance netBalance)
        {
            return pvssDS.NetBalancePVSS(netBalance);
        }

        /// <summary>
        /// 修改XY坐标
        /// </summary>
        /// <param name="orgCode">公司pvsscode码</param>
        /// <param name="xaxis">x轴坐标</param>
        /// <param name="yaxis">y轴坐标</param>
        /// <param name="officeNumber">办公人数</param>
        /// <param name="depLevel">公司级别标识</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int UpdateOrgXY(int orgCode , string xaxis , string yaxis , int officeNumber, int depLevel)
        {
            return pvssDS.UpdateOrgXY(orgCode,xaxis,yaxis,officeNumber, depLevel);
        }

        /// <summary>
        /// 接入控制目标和失调度
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public int UpdateScheduOrControlList(List<OrganSchedu> list)
        {
            return pvssDS.UpdateScheduOrControlList(list);
        }
    }
}