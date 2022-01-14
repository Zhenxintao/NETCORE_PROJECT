using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.MultiStationParameters;

namespace THMS.Core.API.Controllers.MultiStationParameters
{
    /// <summary>
    /// 多站参数
    /// 
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "MultiStation")]
    [ApiController]
    [EnableCors("any")]
    public class MultiStationController : Controller
    {
        readonly MultiStationServices multiStation = new MultiStationServices();

        [HttpPost]
        public dynamic GetMultiStationTable(int pageIndex, int pageSize , string OranId = "-1",string sortName = "TIMESTAMP", string sortType = "DESC") => multiStation.GetMultiStationTable(pageIndex,  pageSize, OranId, sortName, sortType);
    }
}
