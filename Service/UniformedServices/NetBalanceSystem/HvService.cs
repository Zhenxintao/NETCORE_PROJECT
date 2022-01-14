using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SqlSugar;
using System.Text;
using THMS.Core.API.Models;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem;

namespace THMS.Core.API.Service.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 户阀实现
    /// </summary>
    public class HvService : DbContextMySqlEw
    {
        /// <summary>
        ///查询所有的户阀设备信息
        /// </summary>
        /// <param name="searchBase">查询类</param>
        /// <returns></returns>
        public string queryHvDeviceList(ValveSearchBase searchBase)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<hv_deviceinfo>()
                .WhereIF(!string.IsNullOrEmpty(searchBase.DeviceCode) && searchBase.DeviceCode != "string", (uvd) => uvd.DeviceCode == searchBase.DeviceCode)
                .OrderBy(string.IsNullOrEmpty(searchBase.SortColumn) || string.IsNullOrEmpty(searchBase.SortType) || searchBase.SortColumn == "string" || searchBase.SortType == "string" ? "id asc" : searchBase.SortColumn + " " + searchBase.SortType)
            .ToPageListAsync(searchBase.PageIndex == 0 ? 1 : searchBase.PageIndex, searchBase.PageSize == 0 ? 30 : searchBase.PageSize, total);

            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return JsonConvert.SerializeObject(resultList);
        }

        /// <summary>
        /// 查询已安装的户阀设备信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryHvInstallList(ValveSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<hv_devicebasic>()
                .WhereIF(!string.IsNullOrEmpty(search.DeviceCode) && search.DeviceCode != "string", (uvd) => uvd.DeviceCode == search.DeviceCode)
                .WhereIF(!string.IsNullOrEmpty(search.StationName) && search.StationName != "string", (uvd) => uvd.StationName == search.StationName)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNo_id) && search.UnitNo_id != "string", (uvd) => uvd.UnitNo_id == search.UnitNo_id)
                .WhereIF(!string.IsNullOrEmpty(search.BuildingName) && search.BuildingName != "string", (uvd) => uvd.BuildingName == search.BuildingName)
                .WhereIF(!string.IsNullOrEmpty(search.VpnUser_id) && search.VpnUser_id != "string", (uvd) => uvd.VpnUser_id == search.VpnUser_id)
                .WhereIF(search.NarrayNo > 0, (uvb) => uvb.NarrayNo == search.NarrayNo)
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (uvd) => uvd.Community_id == search.Community_id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (uvd) => uvd.CommunityName == search.CommunityName)
                .WhereIF(!string.IsNullOrEmpty(search.Building_id) && search.Building_id != "string", (uvd) => uvd.Building_id == search.Building_id)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNoName) && search.UnitNoName != "string", (uvd) => uvd.UnitNoName == search.UnitNoName)
            .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "DeviceCode asc" : search.SortColumn + " " + search.SortType)
            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);
            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return JsonConvert.SerializeObject(resultList);
        }

        /// <summary>
        /// 获取户阀设备信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryHvDeviceList(ValveSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<hv_deviceinfo, hv_devicebasic>((hvd, hvb) => new object[] { JoinType.Left, hvd.HV_DeviceInfo_id == hvb.HV_DeviceInfo_id })
                .WhereIF(!string.IsNullOrEmpty(search.DeviceCode) && search.DeviceCode != "string", (hvd, hvb) => hvd.DeviceCode == search.DeviceCode)
                .WhereIF(!string.IsNullOrEmpty(search.StationName) && search.StationName != "string", (hvd, hvb) => hvb.StationName == search.StationName)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNo_id) && search.UnitNo_id != "string", (hvd, hvb) => hvb.UnitNo_id == search.UnitNo_id)
                .WhereIF(!string.IsNullOrEmpty(search.BuildingName) && search.BuildingName != "string", (hvd, hvb) => hvb.BuildingName == search.BuildingName)
                .WhereIF(!string.IsNullOrEmpty(search.VpnUser_id) && search.VpnUser_id != "string", (hvd, hvb) => hvb.VpnUser_id == search.VpnUser_id)
                .WhereIF(search.NarrayNo > 0, (hvd, hvb) => hvb.NarrayNo == search.NarrayNo)
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (hvd, hvb) => hvb.Community_id == search.Community_id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (hvd, hvb) => hvb.CommunityName == search.CommunityName)
                .WhereIF(!string.IsNullOrEmpty(search.Building_id) && search.Building_id != "string", (hvd, hvb) => hvb.Building_id == search.Building_id)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNoName) && search.UnitNoName != "string", (hvd, hvb) => hvb.UnitNoName == search.UnitNoName)
            .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "id asc" : search.SortColumn + " " + search.SortType)
            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);
            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return JsonConvert.SerializeObject(resultList);
        }

        /// <summary>
        /// 获取户阀检测点指标列表
        /// </summary>
        /// <param name="hvId">户阀编码</param>
        /// <returns></returns>
        public string queryHvCheckpointList(string hvId)
        {
            var list = DbMysql.Queryable<hv_deviceinfo, hv_realvalue>((hvd, hvr) => new object[] { JoinType.Left, hvd.HV_DeviceInfo_id == hvr.HV_DeviceInfo_id })
                .Where((hvd, hvr) => hvd.DeviceCode == hvId)
                .Select((hvd, hvr) => new ResultDto { key = hvr.TagName, type = hvd.DeviceName, name = hvr.AiDesc }).ToList();

            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 获取户阀实时监测数据
        /// </summary>
        /// <param name="hvId">户阀编码</param>
        /// <returns></returns>
        public string queryHvRealData(string hvId)
        {
            StringBuilder sb = new StringBuilder();

            var listUvd = DbMysql.Queryable<hv_devicebasic>().Where(l => l.DeviceCode == hvId).ToList();
            if (listUvd.Count > 0)
            {
                sb.Append("{");
                sb.Append("\"DeviceCode\":" + "\"" + listUvd[0].DeviceCode + "\"" + ",");
                sb.Append("\"DeviceName\":" + "\"" + listUvd[0].DeviceName + "\"" + ",");
            }
            else
                return "";

            var listUvr = DbMysql.Queryable<hv_realvalue>().Where(l => l.HV_DeviceInfo_id == listUvd[0].HV_DeviceInfo_id).ToList();

            foreach (var item in listUvr)
            {
                sb.Append("\"" + item.TagName + "\"" + ":" + "\"" + item.RealValue + "\"" + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
