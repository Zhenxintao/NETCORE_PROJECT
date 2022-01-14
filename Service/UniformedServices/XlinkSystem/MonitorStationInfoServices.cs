using ApiModel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Models.ViewModel;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 换热站监测数据信息应用
    /// </summary>
    public class MonitorStationInfoServices: DbContextSqlSugar
    {
        /// <summary>
        /// 热源及换热站检测点指标列表
        /// </summary>
        /// <param name="staionId"></param>
        /// <returns></returns>
        public string queryStationFirstCheckpointList(int staionId)
        {
            var monitorPointDtos = Db.Queryable<ValueDesc>().Where(s=>s.VpnUser_id==staionId && s.NarrayNo==0 && (s.AiType == "AI" || s.AiType == "TX")).Select(s => new MonitorPointDto { key = s.TagName, type = s.Unit, name = s.AiDesc, order = s.ValueSeq }).ToList();
            string resultJson = JsonConvert.SerializeObject(monitorPointDtos);
            return resultJson;
        }

        /// <summary>
        /// 换热站二次检测点指标列表
        /// </summary>
        /// <returns></returns>
        public string queryStationSecondCheckpointList(int staionId)
        {
            var resultList = Db.Queryable<VpnUser, StationBranch>((v, s) => new object[] { JoinType.Left, v.Id == s.VpnUser_id }).Where((v, s) => v.Id == staionId && s.StationBranchArrayNumber != 0).Select((v, s) => new { v.StationName, s.StationBranchName, s.StationBranchArrayNumber }).ToList();
           var narryNoList=   resultList.Select(s=>s.StationBranchArrayNumber).ToList<int>();
            List<ValueDesc> monitorPointDtos = Db.Queryable<ValueDesc>().In(s=>s.NarrayNo,narryNoList).Where(s => s.VpnUser_id == staionId && (s.AiType == "AI" || s.AiType == "TX")).ToList();
            var result = monitorPointDtos.GroupBy(s => s.NarrayNo).ToList();
            List<Dictionary<string, object>> keyValues = new List<Dictionary<string, object>>();
            foreach (var group in result)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                var dto = group.Select(s => new MonitorPointDto { key = s.TagName, type = s.Unit, name = s.AiDesc, order = s.ValueSeq }).ToList();
                dic.Add("staionId", staionId);
                dic.Add("staionName", resultList.Where(s => s.StationBranchArrayNumber == group.Key).First().StationName);
                dic.Add("narray_no", group.Key);
                dic.Add("narrayName", resultList.Where(s => s.StationBranchArrayNumber == group.Key).First().StationBranchName);
                dic.Add("keys", dto);
                keyValues.Add(dic);
            }
            var json = JsonConvert.SerializeObject(keyValues);
            return json;
        }

        /// <summary>
        /// 换热站一次历史监测数据
        /// </summary>
        /// <param name="historyFirstParamesDto"></param>
        /// <returns></returns>
        public object queryStationFirstHisData(HistoryFirstParamesDto historyFirstParamesDto)
        {
            var paramesList = Db.Queryable<ValueDesc>().In(s => s.TagName, historyFirstParamesDto.keyList).Where( s => s.VpnUser_id == historyFirstParamesDto.stationId).ToList();
            StringBuilder aiValues = new StringBuilder();
            foreach (var item in paramesList)
            {
                aiValues.Append($@"{item.AiValue} AS {item.TagName},");
            }
            string sql = $@"SELECT {aiValues} VpnUser_id,Dhour FROM HistoryFirstDataHour  WHERE VpnUser_id={historyFirstParamesDto.stationId} AND Dhour BETWEEN '{historyFirstParamesDto.startTime}' AND '{historyFirstParamesDto.endTime}' ORDER BY Dhour";
            var historyFirstList = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            var jsonresult = JsonConvert.SerializeObject(historyFirstList);
            var DynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonresult);
            List<VpnUser> vpnUserList = Db.Queryable<VpnUser>().Where(s=>s.IsValid==true).ToList();
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            keyValues.Add("stationId", historyFirstParamesDto.stationId);
            keyValues.Add("station_name", vpnUserList.Where(s => s.Id == historyFirstParamesDto.stationId).First().StationName);
            keyValues.Add("datetime", historyFirstList.Select(s => s.Dhour).ToList());
            foreach (var flag in historyFirstParamesDto.keyList)
                {
                List<string> dec = new List<string>();
                foreach (var json in DynamicObject)
                {
                    var result =Convert.ToDecimal(json[flag]).ToString("#0.00");
                    dec.Add(result);
                }
                keyValues.Add(flag, dec);
            }
            var keyValuesJson = JsonConvert.SerializeObject(keyValues);
            var list =  JsonConvert.DeserializeObject(keyValuesJson);
            return list;
        }

        /// <summary>
        /// 换热站二次实时监测数据
        /// </summary>
        /// <param name="staionId"></param>
        /// <param name="narray_no"></param>
        /// <returns></returns>
        public string queryStationSecondRealData(int staionId, int narray_no)
        {
            var t = Db.Queryable<VpnUser, StationBranch>((v, s) => new object[] {JoinType.Left, v.Id == s.VpnUser_id}).WhereIF(SqlFunc.HasNumber(narray_no), (v,s) => s.StationBranchArrayNumber == narray_no).Where((v, s) => s.VpnUser_id == staionId && s.StationBranchArrayNumber != 0).Select((v, s) => new {v.Id,v.StationName, s.StationBranchName,s.StationBranchArrayNumber }).ToList();
            var monitorPointDtos = Db.Queryable<ValueDesc>().WhereIF(SqlFunc.HasNumber(narray_no),s=>s.NarrayNo==narray_no).Where(s => s.VpnUser_id == staionId && (s.AiType == "AI" || s.AiType == "TX")).Select(s => new { key = s.TagName, value = s.RealValue ,s.VpnUser_id,s.NarrayNo}).ToList();
            List<Dictionary<string, object>> valuePairs = new List<Dictionary<string, object>>();
            foreach (var item in t)
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("stationId", item.Id);
                keys.Add("narray_no", item.StationBranchArrayNumber);
                keys.Add("station_name", item.StationName);
                keys.Add("narray_name", item.StationBranchName);
                foreach (var values in monitorPointDtos.Where(s=>s.VpnUser_id==item.Id && s.NarrayNo==item.StationBranchArrayNumber))
                {
                keys.Add(values.key, values.value);
                }
                valuePairs.Add(keys);
            }
            string json = JsonConvert.SerializeObject(valuePairs);
            return json;
        }

        /// <summary>
        /// 换热站二次历史监测数据
        /// </summary>
        /// <param name="historySecondParamesDto"></param>
        /// <returns></returns>
        public object queryStationSecondHisData(HistorySecondParamesDto historySecondParamesDto) 
        {
            var paramesList = Db.Queryable<ValueDesc>().In(s => s.TagName, historySecondParamesDto.keyList).Where( s => s.VpnUser_id == historySecondParamesDto.stationId && s.NarrayNo == historySecondParamesDto.narray_no ).ToList();
            StringBuilder aiValues = new StringBuilder();
            foreach (var item in paramesList)
            {
                aiValues.Append($@"ISNULL({item.AiValue},0) AS {item.TagName},");
            }
            string sql = $@"SELECT {aiValues} VpnUser_id,Dhour FROM HistorySecondDataHour  WHERE VpnUser_id={historySecondParamesDto.stationId} AND NarrayNo={historySecondParamesDto.narray_no} AND  Dhour BETWEEN '{historySecondParamesDto.startTime}' AND '{historySecondParamesDto.endTime}' ORDER BY Dhour";
            var historyFirstList = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            var jsonresult = JsonConvert.SerializeObject(historyFirstList);
            var DynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonresult);
            //获取换热站对应机组信息
            var vpnUserList = Db.Queryable<StationBranch,VpnUser>((s,v)=> new object[] { JoinType.Left,v.Id==s.VpnUser_id}).Where((s,v) => v.IsValid == true && s.VpnUser_id== historySecondParamesDto.stationId && s.StationBranchArrayNumber== historySecondParamesDto.narray_no).Select((s,v)=>new { s.StationBranchName ,v.StationName}).First();
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            keyValues.Add("stationId", historySecondParamesDto.stationId);
            keyValues.Add("narray_no", historySecondParamesDto.narray_no);
            keyValues.Add("narray_name", vpnUserList.StationBranchName);
            keyValues.Add("station_name", vpnUserList.StationName);
            keyValues.Add("datetime", historyFirstList.Select(s => s.Dhour).ToList());
            foreach (var flag in historySecondParamesDto.keyList)
            {
                List<string> dec = new List<string>();
                foreach (var json in DynamicObject)
                {
                    var result = Convert.ToDecimal(json[flag]).ToString("#0.00");
                    dec.Add(result);
                }
                keyValues.Add(flag, dec);
            }
            var keyValuesJson = JsonConvert.SerializeObject(keyValues);
            var list = JsonConvert.DeserializeObject(keyValuesJson);
            return list;
        }

        /// <summary>
        /// 获取全网平衡实时数据
        /// </summary>
        /// <param name="id">热网Id</param>
        /// <returns></returns>
        public string getNetworkBalanceRealData(int id)
        {
            List<NetBalance> netBalancesList = Db.Queryable<NetBalance>().WhereIF(SqlFunc.HasNumber(id),s=>s.ComboNetID==id).ToList();
            string json = JsonConvert.SerializeObject(netBalancesList);
            return json;
        }

        /// <summary>
        /// 换热站指标展示
        /// </summary>
        /// <returns></returns>
        public string queryHeatStationIndex(int stationId)
        {
            string sql = $@"SELECT  TOP (100) PERCENT u.Id,u.StationName ,
			ISNULL(MAX(CASE AiValue WHEN 'AIVALUE33' THEN RealValue ELSE NULL END),0) AS AiValue33, 
            ISNULL(MAX(CASE AiValue WHEN 'AIVALUE34' THEN RealValue ELSE NULL END),0) AS AiValue34, 
            ISNULL(MAX(CASE AiValue WHEN 'AIVALUE35' THEN RealValue ELSE NULL END),0) AS AiValue35,
ISNULL(MAX(CASE AiValue WHEN 'AIVALUE41' THEN RealValue ELSE NULL END),0) AS AiValue41
            FROM     ValueDesc v 
               INNER JOIN  VpnUser u ON v.VpnUser_id = u.Id 
            where StationStandard< 98 and v.VpnUser_id > 81 and v.NarrayNo = 0";
            if (stationId > 0)
                sql += "and v.VpnUser_id = "+stationId;
            sql += "GROUP BY u.Id, v.NarrayNo,u.StationName ";
            string SQL = $@"SELECT  TOP (100) PERCENT u.VpnUser_id,u.StationBranchArrayNumber , u.StationBranchName ,
            ISNULL(MAX(CASE AiValue WHEN 'AiValue71' THEN RealValue ELSE NULL END),0) AS AiValue71
            FROM     ValueDesc v 
               INNER JOIN  StationBranch u ON v.VpnUser_id = u.VpnUser_id and u.StationBranchArrayNumber = v.NarrayNo
			where v.NarrayNo > 0";
            if (stationId > 0)
                SQL += "AND u.ParentId=0 and v.VpnUser_id = " + stationId;
            SQL += "GROUP BY u.Id, v.NarrayNo,u.StationBranchArrayNumber , u.StationBranchName , u.VpnUser_id ";
            var stationlist = Db.Ado.SqlQuery<dynamic>(sql).ToList();
            var narraylist = Db.Ado.SqlQuery<dynamic>(SQL).ToList();
            StringBuilder data = new StringBuilder();
            data.Append("[");
            foreach (var item in stationlist)
            {
                data.Append("{\"heatstationId\":" + item.Id + ",");
                data.Append("\"heatstationName\":\"" + item.StationName + "\",");
                data.Append("\"assetHead\":" + item.AiValue34 + ",");
                data.Append("\"isSatisfyAssetHead\":" + item.AiValue35 + ",");
                data.Append("\"assetHeadValue\":" + item.AiValue33 + ",");
                data.Append("\"narrays\":[");
                if (narraylist.Count>1)
                {
                    data.Append("{\"narray_no\":" + 0 + ",");
                    data.Append("\"narrayName\":\"" + "总阀" + "\",");
                    data.Append("\"valveOpening\":" + item.AiValue41 + "");
                    data.Append("},");
                }
                foreach (var t in narraylist.Where(s => s.VpnUser_id == item.Id))
                {
                    data.Append("{\"narray_no\":" + t.StationBranchArrayNumber + ",");
                    data.Append("\"narrayName\":\"" + t.StationBranchName + "\",");
                    data.Append("\"valveOpening\":" + t.AiValue71 + "");
                    data.Append("},");
                }
                data.Remove(data.Length - 1, 1);
                data.Append("]},");
            }
            data.Remove(data.Length - 1, 1);
            data.Append("]");
            string result = data.ToString();
            return result;
        }

        #region 视频监控代码，暂废弃
        ///// <summary>
        ///// 存储视频监控区域列表
        ///// </summary>
        ///// <param name="response"></param>
        //internal void SaveLocalViewData(byte[] data)
        //{
        //    string str = System.Text.Encoding.Default.GetString(data);
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    ViewByParams datalist = js.Deserialize<ViewByParams>(str);
        //    List<ByParams> byParams = datalist.Data.list;
        //    foreach(var item in byParams)
        //        Db.Saveable<RadioViewByParams>(new RadioViewByParams() { indexCode = item.indexCode ,
        //        name = item.name , parentIndexCode = item.parentIndexCode , available = item.available} ).ExecuteReturnEntity();
        //}
        ///// <summary>
        ///// 存储视频监控区域列表
        ///// </summary>
        ///// <param name="response"></param>
        //internal void SavecameraData(byte[] data)
        //{
        //    string str = System.Text.Encoding.Default.GetString(data);
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    ViewByParams datalist = js.Deserialize<ViewByParams>(str);
        //    List<Camera> camera = datalist.Data.cameralist;
        //    Db.Deleteable<RadioCameraData>().ExecuteCommand();
        //    if (datalist.Data.total > datalist.Data.pageNo * datalist.Data.pageSize)
        //        new EnergyFileCreateService().HttpPost("resource/v1/camera/advance/cameraList", "{\"pageNo\": " + (datalist.Data.pageNo+1) + ",\"pageSize\": 1000}", 15);
        //    Db.Insertable<RadioCameraData>(camera.ToArray()).ExecuteCommand();
        //}
        #endregion

        /// <summary>
        /// 换热站一次实时监测数据
        /// </summary>
        /// <param name="staionId"></param>
        /// <returns></returns>
        public string queryStationFirstRealData(int staionId)
        {
            var t = Db.Queryable<VpnUser>()
                .Where((s) => s.Id == staionId )
                .Select((s) => new { s.Id, s.StationName }).ToList();
            var monitorPointDtos = Db.Queryable<ValueDesc>().Where(s => s.VpnUser_id == staionId && s.NarrayNo == 0 && (s.AiType == "AI" || s.AiType == "TX")).Select(s => new { key = s.TagName, value = s.RealValue, s.VpnUser_id, s.NarrayNo }).ToList();
            StringBuilder data = new StringBuilder();
            data.Append("{\"sourceId\":" + t[0].Id + ",");
            data.Append("\"name\":\"" + t[0].StationName + "\",");
            foreach (var item in monitorPointDtos)
            {
                if (item.key.Contains("TIMESTAMP") || item.key.Contains("TIMECHECK") || item.key.Contains("StationCommQuality"))
                    data.Append("\"" + item.key + "\":\"" + item.value + "\",");
                else
                    data.Append("\"" + item.key + "\":" + item.value + ",");
            }
            data.Remove(data.Length - 1, 1);
            data.Append("}");
            string json = data.ToString();
            return json;
        }

        /// <summary>
        /// 获取换热站水利平衡信息
        /// </summary>
        /// <param name="id">-1为全部换热站信息,如获取某换热站信息直接传id编号</param>
        /// <returns></returns>
        public object querySlphHeatplantCalcHistory(int id)
        {
            try
            {
                if (id == -1)
                {
                    List<SlphInfoResponse> result = Db.Queryable<SlphSubstationCalcHistory, SlphXlinkId, VpnUser>((s, x, v) => new JoinQueryInfos(JoinType.Left, s.Pid == x.Pid, JoinType.Left, x.VpnUserId == v.Id)).Where((s, x, v) => v.IsValid == true && x.Type == 0).Select<SlphInfoResponse>().ToList();
                    return result;
                }
                else
                {
                    SlphInfoResponse result = Db.Queryable<SlphSubstationCalcHistory, SlphXlinkId, VpnUser>((s, x, v) => new JoinQueryInfos(JoinType.Left, s.Pid == x.Pid, JoinType.Left, x.VpnUserId == v.Id)).Where((s, x, v) => x.VpnUserId == id && v.IsValid == true && x.Type == 0).Select<SlphInfoResponse>().First();
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 一级网管径信息添加
        /// </summary>
        /// <param name="primaryPipeNetwork"></param>
        /// <returns></returns>
        public int InsertPrimaryPipeNetwork(List<PrimaryPipeNetwork> primaryPipeNetwork)
        {
            int result = Db.Insertable(primaryPipeNetwork).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 一级网管径信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeletePrimaryPipeNetworkById(int id)
        {
            int result = Db.Deleteable<PrimaryPipeNetwork>().Where(s => s.Hxid == id).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 一级网管径信息更改
        /// </summary>
        /// <param name="primaryPipeNetwork"></param>
        /// <returns></returns>
        public int UpdPrimaryPipeNetwork(PrimaryPipeNetwork primaryPipeNetwork)
        {
            int result = Db.Updateable(primaryPipeNetwork).Where(s => s.Hxid == primaryPipeNetwork.Hxid).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 二级网管径信息添加
        /// </summary>
        /// <param name="secondaryPipeNetwork"></param>
        /// <returns></returns>
        public int InsertSecondaryPipeNetwork(List<SecondaryPipeNetwork> secondaryPipeNetwork)
        {
            int result = Db.Insertable(secondaryPipeNetwork).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 二级网管径信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteSecondaryPipeNetworkById(int id)
        {
            int result = Db.Deleteable<SecondaryPipeNetwork>().Where(s => s.Hxid == id).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 二级网管径信息更改
        /// </summary>
        /// <param name="secondaryPipeNetwork"></param>
        /// <returns></returns>
        public int UpdSecondaryPipeNetwork(SecondaryPipeNetwork secondaryPipeNetwork)
        {
            int result = Db.Updateable(secondaryPipeNetwork).Where(s => s.Hxid == secondaryPipeNetwork.Hxid).ExecuteCommand();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }
    }
}
