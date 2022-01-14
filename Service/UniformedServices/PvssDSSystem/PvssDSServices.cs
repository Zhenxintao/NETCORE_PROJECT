using ApiModel;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Logs;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.UniformedServices.PvssDSSystem;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;

namespace THMS.Core.API.Service.UniformedServices.PvssDSSystem
{
    /// <summary>
    /// PVSS数据同步应用
    /// </summary>
    public class PvssDSServices : DbContextSqlSugar
    {

        /// <summary>
        /// 同步热网，没有则新增热网信息，有则修改热网信息（PVSS用）
        /// </summary>
        /// <returns></returns>
        public int CopyPvss_Rw(PvssDSDto.MdjRw_PVSS item)
        {
            string url = "/api/MdjPower/CopyPvss_Rw";
            //查询该参数热网Id是否已存在于数据库，存在则修改热网信息，不存在则新增热网信息
            PvssSetting flag = Db.Queryable<PvssSetting>().Where(s => s.Pvss_id == item.Pvss_id).First();
            int result = HttpClientPvssCommon.HttpPvssPostSharp(item, url);
            if (result == ResultCode.Success)
            {
                //获取最新的站点信息
                var list = Db.Queryable<PvssSetting, VpnUser>((p, v) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id }).Where((p, v) => p.Pvss_id == item.Pvss_id).Select((p, v) => new { id = v.Id, name = v.StationName, heatNetArea = v.StationHotArea, heatNetType = v.HeatNetType }).ToList();
                var listResult = list.First();
                //将更新的数据传到华夏
                if (flag != null)
                {
                    string updStation = ConfigAppsetting.HxWebApi + "updateHeatNet";
                    //将数据传送到热网修改方法
                    return HttpClientCommon.HttpPostSharp(list, updStation);
                }
                else
                {
                    string addStation = "addHeatNet";
                    //将数据传送到热网添加方法
                    return HttpClientCommon.HttpPostSharp(list, addStation);
                }
            }
            else
            {
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// 同步热源，没有则新增热源信息，有则修改热源信息（PVSS用）
        /// </summary>
        /// <returns></returns>
        public int CopyPvss_Power(PvssDSDto.MdjPower_PVSS item)
        {
            string url = "/api/MdjPower/CopyPvss_Power";
            //查询参数热源id是否已经在数据库中存在，存在则修改，不存在则新增。
            PvssSetting flag = Db.Queryable<PvssSetting>().Where(s => s.Pvss_id == item.Pvss_id).First();
            int result = HttpClientPvssCommon.HttpPvssPostSharp(item, url);
            if (result == ResultCode.Success)
            {
                //获取最新的热源信息
                var list = Db.Queryable<PvssSetting, VpnUser, PowerInfo>((p, v, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == f.VpnUser_id }).Where((p, v, f) => p.Pvss_id == item.Pvss_id).Select((p, v, f) => new { id = v.Id, pid = f.ParentID, name = v.StationName, nameFirstPinyin = v.StationSabb, heatingLoad = v.DisignPower, heatingArea = v.StationHotArea, chargeablearea = v.StationHotArea, companyid = v.Organization_id, lon=v.Xaxis, lat=v.Yaxis , sourceType = v.SourceType, heatingMode = v.HeatNetType }).ToList();
                var listResult = list.First();
                var branchBody = item.StationBranch.Select(s => new { narrayNo = s.StationBranchArrayNumber, narrayNoPid = s.ParentId, pid = listResult.id, name = s.StationBranchName, area = s.StationBranchArea }).ToList();
                //将更新的数据传到华夏
                if (flag != null)
                {
                    string updStation = ConfigAppsetting.HxWebApi + "updateHeatsource";
                    //将数据传送到热源修改方法
                    int updResult = HttpClientCommon.HttpPostSharp(list, updStation);
                    //将数据传送到机组修改方法
                    return UpdStationNarry(updResult, branchBody,listResult.id);
                }
                else
                {
                    string addStation = ConfigAppsetting.HxWebApi + "addHeatsource";
                    //将数据传送到热源添加方法
                    int addResult = HttpClientCommon.HttpPostSharp(list, addStation);
                    //将数据传送到机组添加方法
                    return AddStationNarry(addResult, branchBody);
                }
            }
            else
            {
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// 同步站点，没有则新增站点信息和采集量，有则修改站点信息（PVSS用）
        /// </summary>
        /// <returns></returns>
        public int CopyPvss_Station(PvssDSDto.Station_PVSS station)
        {
            string url = "/api/MdjStation/CopyPvss_Station";
            PvssSetting flag = Db.Queryable<PvssSetting>().Where(s => s.Pvss_id == station.Pvss_id).First();
            int result = HttpClientPvssCommon.HttpPvssPostSharp(station, url);
            if (result == ResultCode.Success)
            {
                //获取最新的站点信息
                var list = Db.Queryable<PvssSetting, VpnUser, Station, PowerInfo>((p, v, s, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.PowerInfo_id == f.VpnUser_id }).Where((p, v, s, f) => p.Pvss_id == station.Pvss_id).Select((p, v, s, f) => new { id = v.Id, pid = v.Organization_id, name = v.StationName, nameFirstPinyin = v.StationSabb, heatingLoad = v.DisignPower, heatingArea = v.StationHotArea, chargeablearea = v.StationHotArea, heatsourceid = s.PowerInfo_id, heatnetid = v.HeatNetID,lon = v.Xaxis, lat = v.Yaxis , stationType = v.StationType , isselfmanage = v.IsSelfManage }).ToList();
                var listResult = list.First();
                var branchBody = station.StationBranch.Select(s => new { narrayNo = s.StationBranchArrayNumber, narrayNoPid = s.ParentId , pid = listResult.id, name = s.StationBranchName, area = s.StationBranchArea , heatType = s.HeatType , isenergysave = s.IsEnergySave }).ToList();
                //将更新的数据传到华夏
                if (flag != null)
                {
                    string updStation = ConfigAppsetting.HxWebApi + "updateHeatstation";
                    //将数据传送到换热站修改方法
                    int updResult = HttpClientCommon.HttpPostSharp(list, updStation);
                    //将数据传送到机组修改方法
                    return UpdStationNarry(updResult, branchBody, listResult.id);
                }
                else
                {
                    string addStation = ConfigAppsetting.HxWebApi + "addHeatstation";
                    //将数据传送到换热站添加方法
                    int addResult = HttpClientCommon.HttpPostSharp(list, addStation);
                    //将数据传送到机组添加方法
                    return AddStationNarry(addResult, branchBody);
                }
            }
            else
            {
                return ResultCode.Error;
            }
        }

        /// <summary>
        ///  第三方平台机组添加方法
        /// </summary>
        /// <param name="code">换热站修改方法返回状态码</param>
        /// <param name="stationBranchs">修改数据</param>
        /// <returns></returns>
        public int AddStationNarry(int code, dynamic stationBranchs)
        {
            if (code == ResultCode.Success && stationBranchs.Count > 0)
            {
                string addNarray = ConfigAppsetting.HxWebApi + "addNarray";
                return HttpClientCommon.HttpPostSharp(stationBranchs, addNarray);
            }
            else
            {
                return ResultCode.Success;
            }
        }
        /// <summary>
        ///  第三方平台机组修改方法
        /// </summary>
        /// <param name="code">换热站修改方法返回状态码</param>
        /// <param name="stationBranchs">修改数据</param>
        /// <param name="id">换热站id</param>
        /// <returns></returns>
        public int UpdStationNarry(int code, dynamic stationBranchs, int id)
        {
            if (code == ResultCode.Success && stationBranchs.Count > 0)
            {
                string deleteNarray = ConfigAppsetting.HxWebApi + "deleteNarray";
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("pid", id);
                int delNarryResult = HttpClientCommon.HttpGetSharp(keys, deleteNarray);
                if (delNarryResult == ResultCode.Success)
                {
                    string addNarray = ConfigAppsetting.HxWebApi + "addNarray";
                    return HttpClientCommon.HttpPostSharp(stationBranchs, addNarray);
                }
                else
                {
                    return ResultCode.Error;
                }
            }
            else
            {
                return ResultCode.Success;
            }
        }

        /// <summary>
        /// 新增组织结构（pvss用）
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        public int InsertOrgan(PvssDSDto.Organization organization)
        {
            string url = "/api/Organization/InsertOrgan";
            int result = HttpClientPvssCommon.HttpPvssPostSharp(organization, url);
            if (result == ResultCode.Success)
            {
                var list = Db.Queryable<Organization>().Where(s => s.OrganizationCode == organization.OrganizationCode).Select(s => new { id = s.Id, pid = SqlFunc.IF(s.DepLevel == 1).Return(0).ElseIF(s.DepLevel > 1).Return(s.ParentDepID).End(s.ParentDepID), name = s.OrganizationName, chargeablearea = s.HeatArea, s.DepLevel }).ToList();
                var organList = list.Select(s => new { s.id, s.pid, s.name }).ToList();
                var organ = list.First();
                //传到华夏数据，新增总公司信息
                if (organ.DepLevel == 1)
                {
                    string addOrgan = ConfigAppsetting.HxWebApi + "addCompany";
                    return HttpClientCommon.HttpPostSharp(organList, addOrgan);
                }
                //新增分公司信息
                else if (organ.DepLevel == 2)
                {
                    string addHeatfiliale = ConfigAppsetting.HxWebApi + "addHeatfiliale";
                    return HttpClientCommon.HttpPostSharp(organList, addHeatfiliale);
                }
                //新增项目部信息
                else
                {
                    string addCentralstation = ConfigAppsetting.HxWebApi + "addCentralstation";
                    return HttpClientCommon.HttpPostSharp(organList, addCentralstation);
                }
            }
            else
            {
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// 修改组织结构（pvss用）
        /// </summary>
        /// <param name="organ"></param>
        /// <returns></returns>
        public int UpdateOrgan(PvssDSDto.UpdateOrgPvss organ)
        {
            //设置该公司组织表示
            string type = "";
            //查询该公司目前状态
            var addUpdFlag = Db.Queryable<Organization>().Where(s => s.OrganizationCode == organ.OrganizationCode).Select(s => new { s.IsValid }).First();
            //当前公司状态与参数公司状态相同进行HX接口修改操作
            if (addUpdFlag.IsValid == organ.IsValid)
            {
                type = "update";
            }
            //当前公司状态为有效，参数公司状态为无效，进行HX接口删除操作。
            else if (addUpdFlag.IsValid == true && organ.IsValid == false)
            {
                type = "delete";
            }
            //当前公司状态为无效，参数公司状态为有效，执行HX接口新增操作
            else if (addUpdFlag.IsValid == false && organ.IsValid == true)
            {
                type = "add";
            }
            string url = "/api/Organization/UpdateOrgan";
            //请求内部PVSS数据传输API
            int result = HttpClientPvssCommon.HttpPvssPutSharp(organ, url);
            if (result == ResultCode.Success)
            {
                //获取最新该公司数据信息
                var list = Db.Queryable<Organization>().Where(s => s.OrganizationCode == organ.OrganizationCode).Select(s => new { id = s.Id, pid = SqlFunc.IF(s.DepLevel == 1).Return(0).ElseIF(s.DepLevel > 1).Return(s.ParentDepID).End(s.ParentDepID), name = s.OrganizationName, chargeablearea = s.HeatArea, s.DepLevel, s.IsValid }).ToList();
                var organlist = list.Select(s => new { s.id, s.pid, s.name }).ToList();
                var organbody = list.First();
                //进入HX数据接口转发
                return StatusOrgan(type, organbody, organlist);
            }
            else
            {
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// 组织机构添加、修改或删除
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organBody"></param>
        /// <param name="organList"></param>
        /// <returns></returns>
        public int StatusOrgan(string type, dynamic organBody, dynamic organList)
        {
            if (type == "update")
            {
                //更新公司信息传到华夏数据，修改总公司信息
                if (organBody.DepLevel == 1)
                {
                    string updateCompany = ConfigAppsetting.HxWebApi + "updateCompany";
                    return HttpClientCommon.HttpPostSharp(organList, updateCompany);
                }
                //修改分公司信息
                else if (organBody.DepLevel == 2)
                {
                    string updateHeatfiliale = ConfigAppsetting.HxWebApi + "updateHeatfiliale";
                    return HttpClientCommon.HttpPostSharp(organList, updateHeatfiliale);
                }
                //修改项目部信息
                else
                {
                    string updateCentralstation = ConfigAppsetting.HxWebApi + "updateCentralstation";
                    return HttpClientCommon.HttpPostSharp(organList, updateCentralstation);
                }
            }
            else if (type == "delete")
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("id", organBody.id);
                //删除该公司信息传到华夏数据
                if (organBody.DepLevel == 1)
                {
                    string deleteCompany = ConfigAppsetting.HxWebApi + "deleteCompany";
                    return HttpClientCommon.HttpGetSharp(dic, deleteCompany);
                }
                else if (organBody.DepLevel == 2)
                {
                    string deleteHeatfiliale = ConfigAppsetting.HxWebApi + "deleteHeatfiliale";
                    return HttpClientCommon.HttpGetSharp(dic, deleteHeatfiliale);
                }
                else
                {
                    string deleteCentralstation = ConfigAppsetting.HxWebApi + "deleteCentralstation";
                    return HttpClientCommon.HttpGetSharp(dic, deleteCentralstation);
                }
            }
            else
            {
                //新增公司信息传到华夏数据
                if (organBody.DepLevel == 1)
                {
                    string addCompany = ConfigAppsetting.HxWebApi + "addCompany";
                    return HttpClientCommon.HttpPostSharp(organList, addCompany);
                }
                else if (organBody.DepLevel == 2)
                {
                    string addHeatfiliale = ConfigAppsetting.HxWebApi + "addHeatfiliale";
                    return HttpClientCommon.HttpPostSharp(organList, addHeatfiliale);
                }
                else
                {
                    string addCentralstation = ConfigAppsetting.HxWebApi + "addCentralstation";
                    return HttpClientCommon.HttpPostSharp(organList, addCentralstation);
                }
            }
        }

        #region 废弃原代码
        ///// <summary>
        ///// 新增热网信息（pvss用）
        ///// </summary>
        ///// <param name="powerpvssw"></param>
        ///// <returns></returns>
        //public int PostPVSS_RW(PvssDSDto.PowerRW_PVSS powerpvssw)
        //{
        //    string url = "/api/Power/PostPVSS_RW";
        //    int result = HttpClientPvssCommon.HttpPvssPostSharp(powerpvssw, url);
        //    if (result == ResultCode.Success)
        //    {
        //        //获取最新的热网信息
        //        var list = Db.Queryable<PvssSetting, VpnUser, Station, PowerInfo>((p, v, s, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.PowerInfo_id == f.VpnUser_id }).Where((p, v, s, f) => p.Pvss_id == powerpvssw.PcName).Select((p, v, s, f) => new { id = v.Id, name = v.StationName, heatNetArea = v.StationHotArea }).ToList();
        //        //传到华夏数据
        //        string addHeatNet = ConfigAppsetting.HxWebApi + "addHeatNet";
        //        return HttpClientCommon.HttpPostSharp(list, addHeatNet);
        //    }
        //    else
        //    {
        //        return ResultCode.Error;
        //    }
        //}

        ///// <summary>
        ///// 修改热网信息（pvss用）
        ///// </summary>
        ///// <param name="powerInfo"></param>
        ///// <returns></returns>
        //public int PutPVSS_RW(PvssDSDto.PowerRW_PVSS powerInfo)
        //{
        //    string url =  "/api/Power/PutPVSS_RW";
        //    int result = HttpClientPvssCommon.HttpPvssPutSharp(powerInfo, url);
        //    if (result == ResultCode.Success)
        //    {
        //        //获取最新的热网信息
        //        var list = Db.Queryable<PvssSetting, VpnUser, Station, PowerInfo>((p, v, s, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.PowerInfo_id == f.VpnUser_id }).Where((p, v, s, f) => p.Pvss_id == powerInfo.PcName).Select((p, v, s, f) => new { id = v.Id, name = v.StationName, heatNetArea = v.StationHotArea }).ToList();
        //        //更新热网信息
        //            string updateHeatNet = ConfigAppsetting.HxWebApi + "updateHeatNet";
        //            //传到华夏数据
        //            return HttpClientCommon.HttpPostSharp(list, updateHeatNet);
        //    }
        //    else
        //    {
        //        return ResultCode.Error;
        //    }
        //}

        ///// <summary>
        ///// 新增热源信息（pvss用）
        ///// </summary>
        ///// <param name="powerpvss"></param>
        ///// <returns></returns>
        //public int PostPVSS_RY(PvssDSDto.PowerRY_PVSS powerpvss)
        //{
        //    string addHeatsource = ConfigAppsetting.HxWebApi + "addHeatsource";
        //    string url = "/api/Power/PostPVSS_RY";
        //    int result = HttpClientPvssCommon.HttpPvssPostSharp(powerpvss, url);
        //    if (result == ResultCode.Success)
        //    {
        //        //获取最新的热源信息
        //        var list = Db.Queryable<PvssSetting, VpnUser, Station, PowerInfo>((p, v, s, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.PowerInfo_id == f.VpnUser_id }).Where((p, v, s, f) => p.Pvss_id == powerpvss.PcName).Select((p, v, s, f) => new { id = v.Id, pid = f.ParentID, name = v.StationName, nameFirstPinyin = v.StationSabb, heatingArea = v.StationHotArea, chargeablearea = v.StationOnNetUseArea, companyid = v.Organization_id }).ToList();
        //        //传到华夏数据
        //        return HttpClientCommon.HttpPostSharp(list, addHeatsource);
        //    }
        //    else
        //    {
        //        return ResultCode.Error;
        //    }
        //}

        ///// <summary>
        ///// 修改热源信息（pvss用）
        ///// </summary>
        ///// <param name="powerInfo"></param>
        ///// <returns></returns>
        //public int PutPVSS_RY(PvssDSDto.PowerRY_PVSS powerInfo)
        //{
        //    string updateHeatsource = ConfigAppsetting.HxWebApi + "updateHeatsource";
        //    string url = "/api/Power/PutPVSS_RY";
        //    int result = HttpClientPvssCommon.HttpPvssPutSharp(powerInfo, url);
        //    if (result == ResultCode.Success)
        //    {
        //        //获取最新的热源信息
        //        var list = Db.Queryable<PvssSetting, VpnUser, Station, PowerInfo>((p, v, s, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.PowerInfo_id == f.VpnUser_id }).Where((p, v, s, f) => p.Pvss_id == powerInfo.PcName).Select((p, v, s, f) => new { id = v.Id, pid = f.ParentID, name = v.StationName, nameFirstPinyin = v.StationSabb, heatingArea = v.StationHotArea, chargeablearea = v.StationOnNetUseArea, companyid = v.Organization_id }).ToList();
        //        //传到华夏数据
        //        return HttpClientCommon.HttpPostSharp(list, updateHeatsource);
        //    }
        //    else
        //    {
        //        return ResultCode.Error;
        //    }
        //}

        #endregion

        /// <summary>
        /// 删除热网、热源、换热站信息
        /// </summary>
        /// <param name="PcName"></param>
        /// <param name="isValid"></param>
        /// <returns></returns>
        public int DeletePVSS(string PcName, string isValid)
        {
            string url = "/api/Station/DeletePVSS";
            //获取该PcName的xlink数据Id及该PcName为热网、热源还是换热站
            var vpnuser = Db.Queryable<PvssSetting, VpnUser>((p, v) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id }).Where((p, v) => p.Pvss_id == PcName).Select((p, v) => new { v.Id, v.StationStandard, p.Pvss_id }).First();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PcName", vpnuser.Id.ToString());
            dic.Add("isValid", isValid);
            //调用原PVSSDSWebApi接口
            int result = HttpClientPvssCommon.HttpPvssDeleteSharp(dic, url);
            //把要删除的数据Id进行参数绑定
            Dictionary<string, object> dicId = new Dictionary<string, object>();
            dicId.Add("id", vpnuser.Id);
            //删除HX换热站信息
            if (result == ResultCode.Success && vpnuser.StationStandard < 98)
            {
                string hxStationdel = ConfigAppsetting.HxWebApi + "deleteHeatstation";
                //调用HX换热站删除接口
                int delStationResult = HttpClientCommon.HttpGetSharp(dicId, hxStationdel);
                return delStationResult;
            }
            //删除HX热网信息
            else if (result == ResultCode.Success && vpnuser.StationStandard == 98)
            {
                string hxHeatNetdel = ConfigAppsetting.HxWebApi + "deleteHeatNet";
                //调用HX热网删除接口
                int dicHeatNetResult = HttpClientCommon.HttpGetSharp(dicId, hxHeatNetdel);
                return dicHeatNetResult;
            }
            //删除HX热源信息
            else
            {
                string hxPowerdel = ConfigAppsetting.HxWebApi + "deleteHeatsource";
                //调用HX热源删除接口
                int dicPowerResult = HttpClientCommon.HttpGetSharp(dicId, hxPowerdel);
                return dicPowerResult;
            }
        }

        /// <summary>
        ///  恢复热网、热源、换热站状态，并往HX添加新数据
        /// </summary>
        /// <param name="pvssId">pvssId</param>
        /// <returns></returns>
        public int ReNewPVSS(string pvssId)
        {
            //查看该id为热网、热源还是换热站
            var list = Db.Queryable<PvssSetting, VpnUser>((p, v) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id }).Where((p, v) => p.Pvss_id == pvssId).Select((p, v) => new { id = v.Id, v.StationStandard }).ToList();
            var result = list.First();
            //改变该id所对应的状态为true
            int con = Db.Updateable<VpnUser>().UpdateColumns(s => new VpnUser { IsValid = true }).Where(s => s.Id == result.id).ExecuteCommand();
            //判断是否为换热站
            if (con > 0 && result.StationStandard < 98)
            {
                //查询换热站相关格式信息并准备调用HX换热站添加方法
                var listStation = Db.Queryable<PvssSetting, VpnUser, Station, PowerInfo>((p, v, s, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.PowerInfo_id == f.VpnUser_id }).Where((p, v, s, f) => p.Pvss_id == pvssId).Select((p, v, s, f) => new { id = v.Id, pid = v.Organization_id, name = v.StationName, nameFirstPinyin = v.StationSabb, heating_area = v.StationHotArea, chargeablearea = v.StationHotArea, heatsourceid = s.PowerInfo_id, heatnetid = f.ParentID }).ToList();
                string hxStationdel = ConfigAppsetting.HxWebApi + "addHeatstation";
                //调取换热站添加方法
                int delStationResult = HttpClientCommon.HttpPostSharp(listStation, hxStationdel);
                return delStationResult;
            }
            //判断是否为热网
            else if (con > 0 && result.StationStandard == 98)
            {
                //查询热网相关格式信息并准备调用HX热网添加方法
                var listHeatNet = Db.Queryable<PvssSetting, VpnUser>((p, v) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id }).Where((p, v) => p.Pvss_id == pvssId).Select((p, v) => new { id = v.Id, name = v.StationName, heatNetArea = v.StationHotArea}).ToList();
                string hxHeatNetdel = ConfigAppsetting.HxWebApi + "addHeatNet";
                //调取热网添加方法
                int dicHeatNetResult = HttpClientCommon.HttpPostSharp(listHeatNet, hxHeatNetdel);
                return dicHeatNetResult;
            }
            //热源
            else
            {
                //查询热源相关格式并准备调取HX热源的添加方法
                var listHeatSource = Db.Queryable<PvssSetting, VpnUser, PowerInfo>((p, v, f) => new object[] { JoinType.Left, Convert.ToInt32(p.VpnUser_id) == v.Id, JoinType.Left, v.Id == f.VpnUser_id }).Where((p, v, f) => p.Pvss_id == pvssId).Select((p, v, f) => new { id = v.Id, pid = f.ParentID, name = v.StationName, nameFirstPinyin = v.StationSabb, heatingArea = v.StationHotArea, chargeablearea = v.StationHotArea, companyid = v.Organization_id }).ToList();
                string hxPowerdel = ConfigAppsetting.HxWebApi + "addHeatsource";
                //调取热源的添加方法
                int dicPowerResult = HttpClientCommon.HttpPostSharp(listHeatSource, hxPowerdel);
                return dicPowerResult;
            }
        }

        /// <summary>
        /// 新增、更新全网平衡信息
        /// </summary>
        /// <param name="netBalance"></param>
        /// <returns></returns>
        public int NetBalancePVSS(NetBalance netBalance)
        {
            PvssSetting pvssSetting = Db.Queryable<PvssSetting>().Where(s => s.Pvss_id == netBalance.ComboNetID.ToString()).First();
            netBalance.ComboNetID = Convert.ToInt32(pvssSetting.VpnUser_id);
            NetBalance net = Db.Queryable<NetBalance>().Where(s => s.ComboNetID == netBalance.ComboNetID).First();
            if (net != null)
            {
                netBalance.Id = net.Id;
            }
            var result = Db.Saveable<NetBalance>(netBalance).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 修改XY坐标
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="xaxis"></param>
        /// <param name="yaxis"></param>
        /// <param name="officeNumber"></param>
        /// <param name="depLevel"></param>
        /// <returns></returns>
        public int UpdateOrgXY(int orgCode, string xaxis, string yaxis, int officeNumber,int depLevel)
        {
            var result = Db.Updateable<Organization>().UpdateColumns(s => new Organization { Xaxis = xaxis, Yaxis = yaxis, OfficeNumber = officeNumber }).Where(s => s.OrganizationCode == orgCode).ExecuteCommand();

            if (result>0)
            {
                var list = Db.Queryable<Organization>().Where(s => s.OrganizationCode == orgCode && s.IsValid == true).Select(s => new { id = s.Id, lon = s.Xaxis, lat = s.Yaxis, peoplenumber=s.OfficeNumber }).ToList();
                //更新公司信息传到华夏数据，修改总公司信息
                if (depLevel == 1)
                {
                    string updateCompany = ConfigAppsetting.HxWebApi + "updateCompany";
                    return HttpClientCommon.HttpPostSharp(list, updateCompany);
                }
                //修改分公司信息
                else if (depLevel == 2)
                {
                    string updateHeatfiliale = ConfigAppsetting.HxWebApi + "updateHeatfiliale";
                    return HttpClientCommon.HttpPostSharp(list, updateHeatfiliale);
                }
                //修改项目部信息
                else
                {
                    string updateCentralstation = ConfigAppsetting.HxWebApi + "updateCentralstation";
                    return HttpClientCommon.HttpPostSharp(list, updateCentralstation);
                }
            }
            else
            {
                return ResultCode.Error;
            }
            //return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 修改控制目标失调度
        /// </summary>
        /// <returns></returns>
        public int UpdateScheduOrControlList(List<OrganSchedu> list)
        {
          List<OrganSchedu> organSchedus=  Db.Queryable<OrganSchedu>().ToList();
            List<OrganSchedu> updateList = new List<OrganSchedu>();
            foreach (var item in list)
            {
               OrganSchedu schedu = organSchedus.Where(s => s.OrganizationCode == item.OrganizationCode).FirstOrDefault();
                if (schedu != null)
                {
                     item.Id=schedu.Id;
                    updateList.Add(item);
                }
                else {
                    updateList.Add(item);
                }
            }
           int result = Db.Saveable<OrganSchedu>(list).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 实时数据同步
        /// </summary>
        /// <param name="Pvss_id"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int ValueDescUpdate(string Pvss_id, List<MdjValueDesc_Update_PVSS> list)
        {
            try
            {
                //string url = "/api/MdjValueDesc/ValueDescUpdate?Pvss_id=" + Pvss_id;
                ////请求内部PVSS数据传输API
                //int result = HttpClientPvssCommon.HttpPvssPostSharp(list, url);
                return ResultCode.Success;
            }
            catch (Exception ex)
            {
                Logger.Info("实时数据同步失败!" + ex.Message);
                throw;
            }
     
        }
    }
}
