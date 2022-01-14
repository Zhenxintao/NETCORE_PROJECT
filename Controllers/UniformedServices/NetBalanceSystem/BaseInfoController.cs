using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Models;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto;
using THMS.Core.API.Service.UniformedServices.NetBalanceSystem;

namespace THMS.Core.API.Controllers.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 基础信息
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "NetBalance")]
    [ApiController]
    public class BaseInfoController : ControllerBase
    {
        /// <summary>
        /// 查询小区信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryCommunityList(CommunitySearch search)
        {
            return new BaseInfoService().queryCommunityList(search);
        }

        /// <summary>
        /// 查询楼栋信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryBuildingList(BuildingSearch search)
        {
            return new BaseInfoService().queryBuildingList(search);
        }

        /// <summary>
        /// 查询单元信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryUnitNoList(UnitNoSearch search)
        {
            return new BaseInfoService().queryUnitNoList(search);
        }

        /// <summary>
        /// 查询住户信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        [HttpPost]
        public string queryResidentUserList(ResidentUserSearch search)
        {
            return new BaseInfoService().queryResidentUserList(search);
        }
    }
}