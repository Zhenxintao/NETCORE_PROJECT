using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Forecast
{
    public class StationForecastInputService:DbContextSqlSugar
    {
        /// <summary>
        /// 查询换热站负荷预测输入表
        /// </summary>
        /// <param name="vpnuserid"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="heatType"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Tuple<object,int> SelStationForecastInput(int vpnuserid, string startTime, string endTime, int heatType, int pageSize, int pageIndex)
        {
            var totalCount = 0;
            var list = Db.Queryable<StationForecastInput, StationBranch>((s,sbr)=>new object[] { JoinType.Left,s.VpnUserId==sbr.VpnUser_id && s.StationBranchArrayNumber==sbr.StationBranchArrayNumber})
                .WhereIF(!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime), (s, sbr) => SqlFunc.Between(s.CreateTime,startTime,endTime))
                .WhereIF(SqlFunc.HasNumber(vpnuserid), (s, sbr) => s.VpnUserId == vpnuserid)
                .WhereIF(SqlFunc.HasNumber(heatType), (s, sbr) => s.HeatingType == heatType)
                .Select((s, sbr)=>new { s.Id,s.VpnUserId,s.StationName,sbr.StationBranchName,s.HeatArea,s.HeatTarget,s.OverallHeat,s.SEC_TEMP_S,s.SEC_TEMP_R,s.SeNetworkRelativeFlow,s.OutdoorAtPresentOffset,s.AdCoefficient,s.HeatingType,s.RadiatingTubeType,s.CreateTime,s.CreateUser,s.IsValid,s.StationBranchArrayNumber})
                .ToPageList(pageIndex,pageSize,ref totalCount);
            return new Tuple<object, int>(list, totalCount);
        }

        /// <summary>
        /// 查询各个换热站对应机组采暖方式信息
        /// </summary>
        /// <param name="vpnuserid"></param>
        /// <param name="heatType"></param>
        /// <param name="StationBranchArrayNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Tuple<object,int> SelStationForecastMessge(int vpnuserid, int heatType, int StationBranchArrayNumber, int pageSize,int pageIndex)
        {

            var totalCount = 0;
            var list = Db.Queryable<VpnUser, StationBranch, StationForecastInput>((v,s,t)=>new object[] { JoinType.Left,v.Id==s.VpnUser_id,JoinType.Left,s.VpnUser_id==t.VpnUserId && s.StationBranchArrayNumber==t.StationBranchArrayNumber })
                .WhereIF(SqlFunc.HasNumber(StationBranchArrayNumber), (v,s,t) => s.StationBranchArrayNumber == StationBranchArrayNumber)
                .WhereIF(SqlFunc.HasNumber(vpnuserid), (v, s,t) => v.Id == vpnuserid)
                .WhereIF(SqlFunc.HasNumber(heatType), (v, s,t) => t.HeatingType == heatType)
                .Where((v, s,t) => v.IsValid  == true && s.StationBranchArrayNumber!=0)
                .Select((v, s,t) => new { IsValid= SqlFunc.IsNull<bool>(t.IsValid, false),StationForecastInputId = t.Id,  HeatArea=s.StationBranchHeatArea, t.HeatTarget, t.OverallHeat, t.SEC_TEMP_S, t.SEC_TEMP_R, t.SeNetworkRelativeFlow, t.OutdoorAtPresentOffset, t.AdCoefficient, t.HeatingType, t.RadiatingTubeType, t.CreateTime, t.CreateUser, Vpnuserid = v.Id, v.StationName, s.StationBranchName, s.StationBranchArrayNumber, StationBranchId=s.Id, StationSabb = v.StationSabb }).ToPageList(pageIndex,pageSize,ref totalCount);
            return new Tuple<object, int>(list, totalCount);
        }

        /// <summary>
        /// 添加换热站负荷预测输入表
        /// </summary>
        /// <param name="stationForecastInput"></param>
        /// <returns></returns>
        public bool AddStationForecastInput(StationForecastInput stationForecastInput)
        {
            var stationForecastBasic = Db.Queryable<StationForecastBasic>().Where(s => s.IsValid == true).OrderBy(s => s.HeatingSeason, OrderByType.Desc).First();
            int con = 0;
            var result = Db.Queryable<StationForecastInput>().Where(s => s.VpnUserId == stationForecastInput.VpnUserId && s.StationBranchArrayNumber==stationForecastInput.StationBranchArrayNumber).First();
            if (result!=null)
            {
                StationForecastInput s = new StationForecastInput();
                if (stationForecastInput.HeatTarget == 0)
                {
                    stationForecastInput.HeatTarget = (stationForecastInput.OverallHeat * Convert.ToDecimal(Math.Pow(10, 6)) / (24 * stationForecastBasic.HeatingDays * 3.6M)) * (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorActualAvgTemp);
                }
                s.Id = result.Id;
                s.VpnUserId = stationForecastInput.VpnUserId;
                s.StationName = stationForecastInput.StationName;
                s.HeatArea = stationForecastInput.HeatArea;
                s.HeatTarget = stationForecastInput.HeatTarget;
                s.OverallHeat = stationForecastInput.OverallHeat;
                s.SEC_TEMP_S = stationForecastInput.SEC_TEMP_S;
                s.SEC_TEMP_R = stationForecastInput.SEC_TEMP_R;
                s.SeNetworkRelativeFlow = stationForecastInput.SeNetworkRelativeFlow;
                s.OutdoorAtPresentOffset = stationForecastInput.OutdoorAtPresentOffset;
                s.AdCoefficient = stationForecastInput.AdCoefficient;
                s.HeatingType = stationForecastInput.HeatingType;
                s.RadiatingTubeType = stationForecastInput.RadiatingTubeType;
                s.CreateTime = stationForecastInput.CreateTime;
                s.CreateUser = stationForecastInput.CreateUser;
                s.IsValid = stationForecastInput.IsValid;
                s.StationBranchArrayNumber = stationForecastInput.StationBranchArrayNumber;
              int updCon =  Db.Updateable<StationForecastInput>(s).ExecuteCommand();
                return updCon > 0 ? true : false; 
            }
            
            else
            {
                if (stationForecastInput.HeatTarget == 0)
                {
                    stationForecastInput.HeatTarget = (stationForecastInput.OverallHeat * Convert.ToDecimal(Math.Pow(10, 6)) / (24 * stationForecastBasic.HeatingDays * 3.6M)) * (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorActualAvgTemp);
                }
                con = Db.Insertable(stationForecastInput).ExecuteCommand();
            }
            
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 批量添加换热站负荷预测输入表(xlink5,0专用)
        /// </summary>
        /// <param name="stationForecastInput"></param>
        /// <returns></returns>
        public bool AddToArrayStationForecastInput(List<StationForecastInput> stationForecastInput)
        {
            var stationForecastBasic = Db.Queryable<StationForecastBasic>().Where(s => s.IsValid == true).OrderBy(s => s.HeatingSeason, OrderByType.Desc).First();
            List<StationForecastInput> addToarryList = new List<StationForecastInput>();
            List<StationForecastInput> updToarryList = new List<StationForecastInput>();
            int updcon = 0;
            foreach (var item in stationForecastInput)
            {
                var result = Db.Queryable<StationForecastInput>().Where(s => s.VpnUserId == item.VpnUserId && s.StationBranchArrayNumber==item.StationBranchArrayNumber).First();
                if (result != null)
                {
                    StationForecastInput s = new StationForecastInput();
                    if (item.HeatTarget == 0)
                    {
                        item.HeatTarget = (item.OverallHeat * Convert.ToDecimal(Math.Pow(10, 6)) / (24 * stationForecastBasic.HeatingDays * 3.6M)) * (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorActualAvgTemp);
                    }
                    s.Id = result.Id;
                    s.VpnUserId = item.VpnUserId;
                    s.StationName = item.StationName;
                    s.HeatArea = result.HeatArea;
                    s.HeatTarget = item.HeatTarget;
                    s.OverallHeat = item.OverallHeat;
                    s.SEC_TEMP_S = item.SEC_TEMP_S;
                    s.SEC_TEMP_R = item.SEC_TEMP_R;
                    s.SeNetworkRelativeFlow = item.SeNetworkRelativeFlow;
                    s.OutdoorAtPresentOffset = item.OutdoorAtPresentOffset;
                    s.AdCoefficient = item.AdCoefficient;
                    s.HeatingType = item.HeatingType;
                    s.RadiatingTubeType = item.RadiatingTubeType;
                    s.CreateTime = item.CreateTime;
                    s.CreateUser = item.CreateUser;
                    s.IsValid = item.IsValid;
                    s.StationBranchArrayNumber = item.StationBranchArrayNumber;
                    updToarryList.Add(s);
                }
                else
                {
                    if (item.HeatTarget == 0)
                    {
                        item.HeatTarget = (item.OverallHeat * Convert.ToDecimal(Math.Pow(10, 6)) / (24 * stationForecastBasic.HeatingDays * 3.6M)) * (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorActualAvgTemp);
                    }
                       
                    addToarryList.Add(item);
                }
               
            }
            if (updToarryList.Count>0 )
            {
                 updcon = Db.Updateable<StationForecastInput>(updToarryList.ToArray()).ExecuteCommand();
            }
             if (addToarryList.Count>0)
            {
                int con = Db.Insertable(addToarryList.ToArray()).ExecuteCommand();
                return con > 0 ? true : false;
            }
            return updcon > 0 ? true : false;
        }
        /// <summary>
        /// 批量添加换热站负荷预测输入表(新版Web)
        /// </summary>
        /// <param name="station">输入参数</param>
        /// <returns></returns>
        public bool AddToArrayStationForecastInputWeb(StationForecastInputDto station)
        {
            var stationForecastBasic = Db.Queryable<StationForecastBasic>().Where(s => s.IsValid == true).OrderBy(s => s.HeatingSeason, OrderByType.Desc).First();
            List<StationForecastInput> addToarryList = new List<StationForecastInput>();
            List<StationForecastInput> updToarryList = new List<StationForecastInput>();
            int updcon = 0;
            List<StationForecastInput> stationForecastInput = Db.Queryable<VpnUser, StationBranch>((v, s) => new object[] { JoinType.Left, v.Id == s.VpnUser_id })
                .WhereIF(station.StationType == -1, "1=1")
                .WhereIF(station.StationType != -1, (v, s) => v.Id == station.StationType)
                .Where((v,s)=>s.StationBranchArrayNumber!=0)
                .Select((v,s)=>new StationForecastInput { VpnUserId = v.Id ,StationName=v.StationName, HeatArea =s.StationBranchHeatArea, HeatTarget = station.HeatTarget, OverallHeat =station.OverallHeat, SEC_TEMP_S =station.SEC_TEMP_S, SEC_TEMP_R=station.SEC_TEMP_R, SeNetworkRelativeFlow=station.SeNetworkRelativeFlow, OutdoorAtPresentOffset=station.OutdoorAtPresentOffset, AdCoefficient =station.AdCoefficient , HeatingType =station.HeatingType , RadiatingTubeType =station.RadiatingTubeType , CreateTime =station.CreateTime , CreateUser =station.CreateUser , IsValid =station.IsValid , StationBranchArrayNumber=s.StationBranchArrayNumber}).ToList();

            foreach (var item in stationForecastInput)
            {
                var result = Db.Queryable<StationForecastInput>().Where(s => s.VpnUserId == item.VpnUserId && s.StationBranchArrayNumber == item.StationBranchArrayNumber).First();
                if (result != null)
                {
                    StationForecastInput s = new StationForecastInput();
                    if (item.HeatTarget == 0)
                    {
                        item.HeatTarget = (item.OverallHeat * Convert.ToDecimal(Math.Pow(10, 6)) / (24 * stationForecastBasic.HeatingDays * 3.6M)) * (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorActualAvgTemp);
                    }
                    s.Id = result.Id;
                    s.VpnUserId = item.VpnUserId;
                    s.StationName = item.StationName;
                    s.HeatArea = result.HeatArea;
                    s.HeatTarget = item.HeatTarget;
                    s.OverallHeat = item.OverallHeat;
                    s.SEC_TEMP_S = item.SEC_TEMP_S;
                    s.SEC_TEMP_R = item.SEC_TEMP_R;
                    s.SeNetworkRelativeFlow = item.SeNetworkRelativeFlow;
                    s.OutdoorAtPresentOffset = item.OutdoorAtPresentOffset;
                    s.AdCoefficient = item.AdCoefficient;
                    s.HeatingType = item.HeatingType;
                    s.RadiatingTubeType = item.RadiatingTubeType;
                    s.CreateTime = item.CreateTime;
                    s.CreateUser = item.CreateUser;
                    s.IsValid = item.IsValid;
                    s.StationBranchArrayNumber = item.StationBranchArrayNumber;
                    updToarryList.Add(s);
                }
                else
                {
                    if (item.HeatTarget == 0)
                    {
                        item.HeatTarget = (item.OverallHeat * Convert.ToDecimal(Math.Pow(10, 6)) / (24 * stationForecastBasic.HeatingDays * 3.6M)) * (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorTemperature) / (stationForecastBasic.IndoorCalculationTemp - stationForecastBasic.OutdoorActualAvgTemp);
                    }

                    addToarryList.Add(item);
                }

            }
            if (updToarryList.Count > 0)
            {
                updcon = Db.Updateable<StationForecastInput>(updToarryList.ToArray()).ExecuteCommand();
            }
            if (addToarryList.Count > 0)
            {
                int con = Db.Insertable(addToarryList.ToArray()).ExecuteCommand();
                return con > 0 ? true : false;
            }
            return updcon > 0 ? true : false;
        }


        /// <summary>
        /// 换热站负荷预测输入表更新修改信息
        /// </summary>
        /// <param name="stationForecastInput"></param>
        /// <returns></returns>
        public bool UpdStationForecastInput(StationForecastInput stationForecastInput)
        {

            int con = Db.Updateable(stationForecastInput).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 修改机组表中采暖方式以及散热情况
        /// </summary>
        /// <param name="staitonbranch"></param>
        /// <returns></returns>
        public bool UpdStationBransh(StationBranch staitonbranch)
        {
            int con = Db.Updateable(staitonbranch).UpdateColumns(it => new { it.HeatingType, it.RadiatingTubeType }).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 批量修改机组表中采暖方式以及散热情况
        /// </summary>
        /// <param name="staitonbranch"></param>
        /// <returns></returns>
        public bool UpdToArryStationBransh(List<StationBranch> staitonbranch)
        {
            int con = Db.Updateable(staitonbranch.ToArray()).UpdateColumns(it => new { it.HeatingType, it.RadiatingTubeType }).ExecuteCommand();
            return con > 0 ? true : false;
        }


    }
}
