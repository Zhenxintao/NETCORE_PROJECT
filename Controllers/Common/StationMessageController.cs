using ApiModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.Common;

namespace THMS.Core.API.Controllers.Common
{
    /// <summary>
    /// 基础公共方法控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName= "Common")]
    [ApiController]
    [EnableCors("any")]
    public class StationMessageController : ControllerBase
    {
        StationMessageService s = new StationMessageService();
        CommonService commonService = new CommonService();
        /// <summary>
        /// 机组下拉框
        /// </summary>
        /// <returns></returns>
        ///  
        [HttpGet]
        public object SelStationBranch()
        {
            var result = s.SelStationBranch();          
            return result;
        }

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="value">简拼</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object SelVpnuser(string value)
        {
            var result = s.SelVpnuser(value);
            return result;
        }
        /// <summary>
        /// 返回热源树
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetPowerTrees()
        {
            var result = commonService.GetPowerTrees();
            return result;
        }
        /// <summary>
        /// 返回换热站树
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetStationTrees()
        {
            var result = commonService.GetStationTrees();
            return result;
        }
        /// <summary>
        /// 返回公司组织结构树
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public object GetOrganizationTree()
        {
            var result = commonService.GetOrganizationTree();
            return result;
        }

        /// <summary>
        /// 热源基础信息
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public dynamic GetpowerInfos()
        { 
            var result = commonService.GetpowerInfos();
            return result;  
        }
        /// <summary>
        /// 获取标准参量表
        /// </summary>
        /// <param name="userconfig_id">UserConfig数据表id</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public List<StandardParameter> GetStandardParameterList(int userconfig_id)
        {
            var result = new StationParameterService().GetStandardParameterList(userconfig_id);
            return result;
        }

        /// <summary>
        /// 获取换热站基础信息
        /// </summary>
        /// <param name="typeid">全部信息：-1，换热站：3，热源：99；热网：98</param>
        /// <returns></returns>
        [HttpGet]
        public List<VpnUser> GetStationList(int typeid)
        {
            List<VpnUser> result = new CommonService().GetStationList(typeid);
            return result;
        }
    }
}