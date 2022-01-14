using Newtonsoft.Json;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Models;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.ReturnDto;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto;
using THMS.Core.API.Redis;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;

namespace THMS.Core.API.Service.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 单元阀实现
    /// </summary>
    public class UvService : DbContextMySqlEw
    {
        /// <summary>
        /// 获取二网平衡完成率
        /// </summary>
        /// <returns></returns>
        public object queryTwoNetworkBalancePercent()
        {
            try
            {
                var data = DbMysql.Queryable<nets_controlstrategy>().ToList();

                //投入总数
                var partakeControlValve = 0;
                //设备总数
                var valveTotal = 1;


                //投入总数
                var _partakeControlValve = data.Sum(x => x.PartakeControlValve);
                partakeControlValve = _partakeControlValve == null ? 0 : Convert.ToInt32(_partakeControlValve);


                //设备总数
                var _valveTotal = data.Sum(x => x.ValveTotal);
                valveTotal = _valveTotal == null ? 1 : Convert.ToInt32(_valveTotal);
                if (valveTotal < 1)
                    valveTotal = 1;

                var _val = ((double)partakeControlValve / valveTotal);

                return new
                {
                    Code = 200,
                    total = 0,
                    message = "操作成功",
                    data = Math.Round(Convert.ToDouble(_val * 100), 2)
                };
            }
            catch (Exception e)
            {
                return new
                {
                    Code = 500,
                    total = 0,
                    message = e.Message,
                    data = "0"
                };
            }
        }



        /// <summary>
        /// 获取二网平衡数据
        /// </summary>
        /// <param name="search">查询类，organ_id="-1"为总公司</param>
        /// <returns></returns>
        public object queryTwoNetworkBalance(netsSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<nets_controlstrategy, vpnuser, organization, stationbranch>((n, v, o, s) => new object[] {
                JoinType.Inner,n.VpnUser_id==v.VpnUser_id,
                JoinType.Inner,v.Organization_id==o.Id,
                JoinType.Inner,v.VpnUser_id==s.VpnUser_id,
                JoinType.Inner,n.NarrayNo==s.ArrayNumber
            })
            //.Where((n, v, o, s) => o.Id.ToString() == search.organ_id)
            .WhereIF(search.organ_id != "-1", (n, v, o, s) => o.Id.ToString() == search.organ_id)
            .Select((n, v, o, s) => new
            {
                Id = n.Id,
                VpnUser_id = v.VpnUser_id,
                StationName = v.StationName,
                BranchName = s.BranchName,
                NarrayNo = s.ArrayNumber,
                OrganizationName = o.OrganizationName,
                TargetTemp = n.TargetTemp,
                ValveTotal = n.ValveTotal,
                PartakeBalanceValve = n.PartakeBalanceValve,
                BalanceDegree = n.BalanceDegree
            })
             .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "id asc" : search.SortColumn + " " + search.SortType)
            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);
            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return resultList;
            //return JsonConvert.SerializeObject(resultList);


        }

        /// <summary>
        /// 获取二网平衡历史数据
        /// </summary>
        /// <param name="search">查询类，organ_id="-1"为总公司</param>
        /// <returns></returns>
        public object getNetworkBalanceHisData(netsHisSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<Nets_ControlStrategyRecord, vpnuser, organization, stationbranch>((n, v, o, s) => new object[] {
                JoinType.Inner,n.VpnUser_id==v.VpnUser_id,
                JoinType.Inner,v.Organization_id==o.Id,
                JoinType.Inner,v.VpnUser_id==s.VpnUser_id,
                JoinType.Inner,n.NarrayNo==s.ArrayNumber
            })
            //.Where((n, v, o, s) => o.Id.ToString() == search.organ_id)
            .WhereIF(search.organ_id != "-1", (n, v, o, s) => o.Id.ToString() == search.organ_id)
            .Where((n, v, o, s) => n.RegulationDate >= search.startTime)
            .WhereIF(!string.IsNullOrEmpty(search.endTime.ToString()), (n, v, o, s) => n.RegulationDate <= search.endTime)
            .Select((n, v, o, s) => new
            {
                Id = n.Id,
                VpnUser_id = v.VpnUser_id,
                StationName = v.StationName,
                BranchName = s.BranchName,
                NarrayNo = s.ArrayNumber,
                OrganizationName = o.OrganizationName,
                TargetTemp = n.TargetTemp,
                ValveTotal = n.ValveTotal,
                PartakeBalanceValve = n.PartakeControlValve,
                BalanceDegree = n.BalanceDegree,
                RegulationDate = n.RegulationDate
            })
             .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "id asc" : search.SortColumn + " " + search.SortType)
            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);
            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return resultList;
            //return JsonConvert.SerializeObject(resultList);
        }


        /// <summary>
        ///查询所有的单元阀设备信息
        /// </summary>
        /// <param name="searchBase">查询类</param>
        /// <returns></returns>
        public string queryUvDeviceList(ValveSearchBase searchBase)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<UV_DeviceInfo>()
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
        /// 查询已安装的单元阀设备信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryUvInstallList(ValveSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<uv_devicebasic>()
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
        /// 获取单元阀设备信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryUnitDeviceList(ValveSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<UV_DeviceInfo, uv_devicebasic>((uvd, uvb) => new object[] { JoinType.Left, uvd.UV_DeviceInfo_id == uvb.UV_DeviceInfo_id })
                .WhereIF(!string.IsNullOrEmpty(search.DeviceCode) && search.DeviceCode != "string", (uvd, uvb) => uvd.DeviceCode == search.DeviceCode)
                .WhereIF(!string.IsNullOrEmpty(search.StationName) && search.StationName != "string", (uvd, uvb) => uvb.StationName == search.StationName)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNo_id) && search.UnitNo_id != "string", (uvd, uvb) => uvb.UnitNo_id == search.UnitNo_id)
                .WhereIF(!string.IsNullOrEmpty(search.BuildingName) && search.BuildingName != "string", (uvd, uvb) => uvb.BuildingName == search.BuildingName)
                .WhereIF(!string.IsNullOrEmpty(search.VpnUser_id) && search.VpnUser_id != "string", (uvd, uvb) => uvb.VpnUser_id == search.VpnUser_id)
                .WhereIF(search.NarrayNo > 0, (uvd, uvb) => uvb.NarrayNo == search.NarrayNo)
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (uvd, uvb) => uvb.Community_id == search.Community_id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (uvd, uvb) => uvb.CommunityName == search.CommunityName)
                .WhereIF(!string.IsNullOrEmpty(search.Building_id) && search.Building_id != "string", (uvd, uvb) => uvb.Building_id == search.Building_id)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNoName) && search.UnitNoName != "string", (uvd, uvb) => uvb.UnitNoName == search.UnitNoName)
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
        /// 获取单元阀检测点指标列表
        /// </summary>
        /// <param name="unitId">单元阀编码</param>
        /// <returns></returns>
        public string queryUnitCheckpointList(string unitId)
        {
            var list = DbMysql.Queryable<UV_DeviceInfo, UV_RealValue>((uvd, uvr) => new object[] { JoinType.Left, uvd.UV_DeviceInfo_id == uvr.UV_DeviceInfo_id })
                .Where((uvd, uvr) => uvd.DeviceCode == unitId)
                .Select((uvd, uvr) => new ResultDto { key = uvr.TagName, type = uvd.DeviceName, name = uvr.AiDesc }).ToList();

            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 获取单元阀实时监测数据
        /// </summary>
        /// <param name="unitId">单元阀编码</param>
        /// <returns></returns>
        public string queryUnitRealData(string unitId)
        {
            StringBuilder sb = new StringBuilder();

            var listUvd = DbMysql.Queryable<uv_devicebasic>().Where(l => l.DeviceCode == unitId).ToList();
            if (listUvd.Count > 0)
            {
                sb.Append("{");
                sb.Append("\"DeviceCode\":" + "\"" + listUvd[0].DeviceCode + "\"" + ",");
                sb.Append("\"DeviceName\":" + "\"" + listUvd[0].DeviceName + "\"" + ",");
            }
            else
                return "";

            var listUvr = DbMysql.Queryable<UV_RealValue>().Where(l => l.UV_DeviceInfo_id == listUvd[0].UV_DeviceInfo_id).ToList();

            foreach (var item in listUvr)
            {
                sb.Append("\"" + item.TagName + "\"" + ":" + "\"" + item.RealValue + "\"" + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 获取单元阀历史监测数据
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryUnitHisData(UnitHisSearch search)
        {
            try
            {
                var url = DqHxConfigSetting.HxUnitHis;
                if (string.IsNullOrEmpty(url))
                    return "尚未部署ElasticSearch历史库";
                var result = HttpHelper.HttpClientPost(url, search);
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        RedisCacheManager _redisCacheManager = new RedisCacheManager();

        /// <summary>
        /// 查询单元阀温差
        /// </summary>
        /// <returns></returns>
        public async Task<object> queryUvTempDiff()
        {
            try
            {
                //var time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));

                var redisData = await _redisCacheManager.GetPatternAsync<Uv_Device_Valve>("UvValueData*");
                //redisData = redisData.Where(x => Convert.ToDateTime(x.TIMESTAMP) >= time).ToList();

                var stationData = await DbMysql.Queryable<valuedescsecond>().Where(l => l.TagName == "SEC_TEMP_R").ToListAsync();

                var diffData = from u in redisData
                               from t in stationData
                               where u.VpnUser_id == t.VpnUser_id && u.NarrayNo == t.NarrayNo
                               select new
                               {
                                   DeviceCode = u.DeviceCode,
                                   StationName = u.StationName,
                                   NarrayNo = u.NarrayNo,
                                   TIMESTAMP = u.TIMESTAMP,
                                   Diff = decimal.TryParse(t.RealValue, out decimal val) ? val - u.TEMP_R : 0
                               };
                var res = new
                {
                    Code = 500,
                    Data = diffData,
                    Desc = "查询成功"
                };
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception e)
            {
                var res = new
                {
                    Code = 500,
                    Data = "",
                    Desc = e.Message
                };
                return JsonConvert.SerializeObject(res);
            }
        }

        /// <summary>
        /// 查询末端温差
        /// </summary>
        /// <returns></returns>
        public async Task<object> queryEtTempDiff()
        {
            try
            {
                //var time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:00:00"));

                var redisData = await _redisCacheManager.GetPatternAsync<Et_Device_Valve>("EtValueData*");
                //redisData = redisData.Where(x => Convert.ToDateTime(x.TIMESTAMP) >= time).ToList();

                var stationData = await DbMysql.Queryable<valuedescsecond>().Where(l => l.TagName == "SEC_TEMP_R").ToListAsync();

                var diffData = from u in redisData
                               from t in stationData
                               where u.VpnUser_id == t.VpnUser_id && u.NarrayNo == t.NarrayNo
                               select new
                               {
                                   DeviceCode = u.DeviceCode,
                                   StationName = u.StationName,
                                   NarrayNo = u.NarrayNo,
                                   TIMESTAMP = u.TIMESTAMP,
                                   Diff = decimal.TryParse(t.RealValue, out decimal val) ? val - u.TEMP : 0
                               };
                var res = new
                {
                    Code = 500,
                    Data = diffData,
                    Desc = "查询成功"
                };
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception e)
            {
                var res = new
                {
                    Code = 500,
                    Data = "",
                    Desc = e.Message
                };
                return JsonConvert.SerializeObject(res);
            }
        }
    }
}
