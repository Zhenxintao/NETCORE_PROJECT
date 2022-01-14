using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using THMS.Core.API.Service.Monitor;
using Newtonsoft.Json;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;

namespace THMS.Core.API.Controllers.Monitor
{
    /// <summary>
    /// 监控实时数据控制器
    /// </summary>
    [Route("api/[controller]/[action]"), ApiExplorerSettings(GroupName = "Monitor")]
    [ApiController]
    [EnableCors("any")]

    public class ShowMonitorController : ControllerBase
    {
        ShowMonitorService ShowMonitorService = new ShowMonitorService();

        /// <summary>
        /// 监控实时数据表格内容
        /// </summary>
        /// <param name="id">用户配置表id</param>
        /// <param name="sortName">排序字段</param>
        /// <param name="sortType">排序类型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ShowMonitorDto> SelShowMonitor(int id,string sortName,string sortType)
        {
            var result = await ShowMonitorService.SelShowMonitor(id,sortName,sortType);
            return result;
        }
        /// <summary>
        /// 监控实时热源数据表格
        /// </summary>
        /// <param name="sortName"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ShowMonitorDto SelShowPower(string sortName, string sortType)
        {
            var result =  ShowMonitorService.SelShowPower(sortName, sortType);
            return result;
        }

        /// <summary>
        /// 监控实时热源数据表头信息
        /// </summary>
        /// <param name="type">-1:全部信息,1:选中的信息，2:没有选中的信息</param>
        /// <returns></returns>
        [HttpGet]
        public List<CommonTagItem> SelShowPowerTitle(int type)
        {
            var result = ShowMonitorService.SelShowPowerTitle(type);
            return result;
        }
        /// <summary>
        /// 配置热源表头
        /// </summary>
        /// <param name="commonTagItems">配置数据数组</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public bool UpdCommonTagItem(List<CommonTagItem> commonTagItems)
        {
            var result = ShowMonitorService.UpdCommonTagItem(commonTagItems);
            return result;
        }

        /// <summary>
        /// 工艺图数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="narryid"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public List<GytShowDtoList> GytShowList(int id, int narryid)
        {
            var result = ShowMonitorService.GytShowList(id,narryid);
            return result;
        }

        [HttpGet]
        public  ShowMonitorDto SearchShowMonitor(int id, int listType)
        {
            var result =  ShowMonitorService.SearchShowMonitor(id, listType);
            //return JsonConvert.SerializeObject(result);
            return result;
        }
        /// <summary>
        /// 监控实时数据表格列头
        /// </summary>
        /// <param name="id">用户配置表id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ShowMonitorTitleDto>> SelShowMonitorTitle(int id)
        {
            var result = await ShowMonitorService.SelShowMonitorTitle(id);
            return result;
        }
        /// <summary>
        /// 监控实时数据表格列头宽度保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        [HttpGet]
        public bool AddMontiorTitleWidth(int id, int width, string prop)
        {
            var result =  ShowMonitorService.AddMontiorTitleWidth(id,width,prop);
            return result;
        }
            
            
    }
}