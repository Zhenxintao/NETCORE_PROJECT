using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.Common;

namespace THMS.Core.API.Service.Monitor
{
    /// <summary>
    /// 
    /// </summary>
    public class ShowMonitorService : DbContext.DbContextSqlSugar
    {
        CommonService commonService = new CommonService();
        //前端返回List格式
        List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();

        /// <summary>
        /// 监控实时数据表格内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortName"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public async Task<ShowMonitorDto> SelShowMonitor(int id, string sortName, string sortType)
        {
            //获取UserConfig 多站配置列表
            var userConfigData = await Db.Queryable<UserConfig>().Where(s => s.Id == id).FirstAsync();

            //IncludeSta == “*” 全部站点列表，IncludeSta != “*”，站点id，逗号分隔
            var includeStaS = userConfigData.IncludeSta.Split(",");

            //includeParaMDLS 参数集合
            var includeParaMDLS = userConfigData.IncludeParaMDL.Split(",");

            //循环增加机组TagName 后缀机组号
            //默认最多6机组
            List<string> ListincludeParaMDLS = new List<string>();
            foreach (var item in includeParaMDLS)
            {
                var newTagName = removeNarrayNo(item);
                ListincludeParaMDLS.Add(item);

                if (item != newTagName)
                {
                    for (int i = 2; i <= 6; i++)
                    {
                        ListincludeParaMDLS.Add(newTagName + i);
                    }
                }
            }

            //执行sql
            var sql = " SELECT * FROM [dbo].[OverViewList] WHERE TagName IN ( @TagName ) {0};  ";

            //站点排序集合
            var StationViewList = new List<ReturnListDate>();

            //数据集合
            string sql_includeStaS = " ";
            //判断是否为全部站点，*代表全部站点，否则为站点id字符串
            if (includeStaS[0].ToString() != "*")
            {
                sql_includeStaS = " AND Id IN ( @includeStaS )";
            }

            //获取数据 视图 OverViewList
            var ViewList = Db.Ado.SqlQuery<ReturnListDate>(string.Format(sql, sql_includeStaS), new SugarParameter[]{
                new SugarParameter("@TagName", ListincludeParaMDLS),
                new SugarParameter("@includeStaS", includeStaS)});

            //排序站点集合
            //基本信息、运行参数
            if (!string.IsNullOrEmpty(sortName))
            {
                if (sortName.ToUpper() == "StationName".ToUpper() || sortName.ToUpper() == "StationBranchHeatArea".ToUpper() || sortName.ToUpper() == "StationBranchName".ToUpper())
                {
                    sql = @"SELECT Id,
                                   StationName,
                                   StationBranchHeatArea,
                                   StationBranchArrayNumber,
                                   StationBranchName
                            FROM [dbo].[OverViewList]
                            WHERE 1 = 1 AND StationBranchArrayNumber > 0 
                            {0}
                            GROUP BY Id,
                                     StationName,
                                     StationBranchHeatArea,
                                     StationBranchArrayNumber,
                                     StationBranchName ORDER BY " + sortName + " " + sortType;
                }
                //采集时间排序，字符串
                else if (sortName.ToUpper() == "TIMESTAMP")
                {
                    sql = "SELECT * FROM [dbo].[OverViewList] WHERE 1=1 {0} AND TagName LIKE ('" + sortName + "_') OR TagName LIKE ('" + sortName + "') ORDER BY RealValue " + sortType + "";
                }
                //运行量排序，数值类型
                else
                {
                    sql = "SELECT * FROM [dbo].[OverViewList] WHERE 1=1 {0} AND ( TagName LIKE ('" + sortName + "_') OR TagName LIKE ('" + sortName + "')) ORDER BY CONVERT(DECIMAL(18, 2), RealValue) " + sortType + "";
                }
                StationViewList = Db.Ado.SqlQuery<ReturnListDate>(string.Format(sql, sql_includeStaS), new SugarParameter[]{
                new SugarParameter("@includeStaS", includeStaS)});
            }
            else
            {
                StationViewList = ViewList;
            } 

            //获取站点集合
            var station = StationViewList
                    .GroupBy(x => new { x.Id, x.StationBranchArrayNumber, x.StationBranchName, x.StationBranchHeatArea, x.StationName }).ToList(); 

            //一次侧TagName排序
            if (!string.IsNullOrEmpty(sortName) && ViewList.Where(t => t.TagName == sortName).Count() > 0)
            {
                foreach (var item in station)
                {
                    var Key = item.Key;
                    var stationinfo = ViewList.Where(t => t.Id == Key.Id).GroupBy(x => new { x.Id, x.StationBranchArrayNumber, x.StationBranchName, x.StationBranchHeatArea, x.StationName }).ToList();
                    foreach (var stationinfoitem in stationinfo)
                    {
                        var stationinfoitemKey = stationinfoitem.Key;
                        if (stationinfoitemKey.StationBranchArrayNumber != 0 || stationinfo.Count == 1) 
                        {
                            //一次侧不返回站点信息
                            Dictionary<string, object> dicObj = new Dictionary<string, object>();
                            dicObj.Add("VpnUserId", stationinfoitemKey.Id);
                            //前端参数传递，需要固定格式 站点id，机组号
                            dicObj.Add("Pass", stationinfoitemKey.Id + "," + stationinfoitemKey.StationBranchArrayNumber);  
                            dicObj.Add("StationName", stationinfoitemKey.StationName);   
                            dicObj.Add("StationBranchHeatArea", stationinfoitemKey.StationBranchHeatArea);
                            dicObj.Add("NarrayNo", stationinfoitemKey.StationBranchArrayNumber);
                            dicObj.Add("StationBranchName", stationinfoitemKey.StationBranchName);

                            var valuedesc = ViewList.Where(t => t.Id == stationinfoitemKey.Id && (t.StationBranchArrayNumber == 0 || t.StationBranchArrayNumber ==  stationinfoitemKey.StationBranchArrayNumber)).ToList();

                            ForeachList(valuedesc, dicObj);
                        }
                    }
                }
            }
            //二次侧TagName排序
            else
            {
                foreach (var item in station)
                {
                    var Key = item.Key;
                    //一次侧不返回站点信息
                    if (Key.StationBranchArrayNumber != 0)
                    {
                        Dictionary<string, object> dicObj = new Dictionary<string, object>();
                        dicObj.Add("VpnUserId", Key.Id);
                        //前端参数传递，需要固定格式 站点id，机组号
                        dicObj.Add("Pass", Key.Id + "," + Key.StationBranchArrayNumber);
                        dicObj.Add("StationName", Key.StationName);
                        dicObj.Add("StationBranchHeatArea", Key.StationBranchHeatArea);
                        dicObj.Add("NarrayNo", Key.StationBranchArrayNumber);
                        dicObj.Add("StationBranchName", Key.StationBranchName);

                        var valuedesc = ViewList.Where(t => t.Id == Key.Id && (t.StationBranchArrayNumber == 0 || t.StationBranchArrayNumber == Key.StationBranchArrayNumber)).ToList();

                        ForeachList(valuedesc, dicObj);
                    }
                }
            }
            ShowMonitorDto showMonitorDto = new ShowMonitorDto() { data = listResult, count = listResult.Count };

            return showMonitorDto;
        }

        void ForeachList(List<ReturnListDate> valuedesc, Dictionary<string, object> dicObj)
        {
            foreach (var desc in valuedesc)
            {
                MonitorData monitorData = new MonitorData()
                {
                    RealValue = desc.AiType.ToUpper() == "DI" ? commonService.GetDIMean(desc.RealValue, desc.ZeroMean, desc.OneMean) : desc.RealValue,
                    HiHi = desc.HiHi,
                    Hi = desc.Hi,
                    LoLo = desc.LoLo,
                    Lo = desc.Lo
                };

                var lastChar = desc.TagName.Substring(desc.TagName.Length - 1, 1);
                int narrayNo = 0;

                if (int.TryParse(lastChar, out narrayNo))
                {
                    dicObj.Add(desc.TagName.Substring(0, desc.TagName.Length - 1), monitorData);
                }
                else
                {
                    dicObj.Add(desc.TagName, monitorData);
                }
            }

            listResult.Add(dicObj);
        }

        /// <summary>
        /// 监控实时热源数据表格
        /// </summary>
        /// <param name="sortName"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public ShowMonitorDto SelShowPower(string sortName, string sortType)
        {
            // 获取热源信息及实时参数数据
            var dataList = Db.Queryable<StationBranch, VpnUser, ValueDesc>((s, v, d) => new object[] { JoinType.Left, s.VpnUser_id == v.Id, JoinType.Left, v.Id == d.VpnUser_id && s.StationBranchArrayNumber == d.NarrayNo }).Where((s, v, d) => v.IsValid == true && v.StationStandard == 99).OrderByIF(!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sortType) && sortName != "Area", $@"case when d.TagName='{sortName}' then 1 ELSE 1 END,1 {sortType}").OrderByIF(!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sortType) && sortName == "Area", $@"{sortName} {sortType}").Select((s, v, d) => new NewDemo { Id = v.Id, StationBranchArrayNumber = s.StationBranchArrayNumber, StationBranchName = s.StationBranchName, StationName = v.StationName, Area = v.StationHotArea, TagName = d.TagName, AiDesc = d.AiDesc, Unit = d.Unit, HiHi = d.HiHi, Hi = d.Hi, Lo = d.Lo, LoLo = d.LoLo, RealValue = d.RealValue }).ToList();
            //获取热源信息
            var powerList = (from c in dataList group c by new { c.Id, c.StationBranchArrayNumber, c.StationName, c.StationBranchName, c.Area } into v select new NewDemo() { Id = v.Key.Id, StationBranchArrayNumber = v.Key.StationBranchArrayNumber, StationBranchName = v.Key.StationBranchName, StationName = v.Key.StationName, Area = v.Key.Area }).ToList();
            // 查询热源列头展示的数据
            var commonTagItem = Db.Queryable<CommonTagItem>().Where(s => s.IsImage == true).OrderBy(s => s.ValueSeq, OrderByType.Asc).ToList();
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            foreach (var power in powerList)
            {
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                keyValues.Add("VpnuserId", power.Id);
                keyValues.Add("StationBranchArrayNumber", power.StationBranchArrayNumber);
                keyValues.Add("StationName", power.StationName);
                keyValues.Add("StationBranchName", power.StationBranchName);
                keyValues.Add("Area", power.Area);

                foreach (var item in commonTagItem)
                {
                    dynamic result = new object();
                    if (item.NarrayNo != 0)
                    {
                        result = dataList.Where(s => s.TagName == item.TagName + item.NarrayNo && s.Id == power.Id && s.StationBranchArrayNumber == power.StationBranchArrayNumber).First();
                    }
                    else
                    {
                        result = dataList.Where(s => s.TagName == item.TagName && s.Id == power.Id && s.StationBranchArrayNumber == power.StationBranchArrayNumber).First();
                    }
                    MonitorData monitorData = new MonitorData() { RealValue = result.RealValue, HiHi = result.HiHi, Hi = result.Hi, LoLo = result.LoLo, Lo = result.Lo };
                    keyValues.Add(item.TagName, monitorData);
                }
                listResult.Add(keyValues);
            }
            ShowMonitorDto showMonitorDto = new ShowMonitorDto { data = listResult };
            return showMonitorDto;
        }
        public class NewDemo
        {
            public int Id { get; set; }
            public int StationBranchArrayNumber { get; set; }
            public string StationBranchName { get; set; }
            public string StationName { get; set; }
            public decimal Area { get; set; }
            public string TagName { get; set; }
            public string AiDesc { get; set; }
            public string Unit { get; set; }
            public decimal HiHi { get; set; }
            public decimal Hi { get; set; }
            public decimal Lo { get; set; }
            public decimal LoLo { get; set; }
            public string RealValue { get; set; }

        }
        /// <summary>
        /// 监控实时热源数据表头信息
        /// </summary>
        /// <returns></returns>
        public List<CommonTagItem> SelShowPowerTitle(int type)
        {
            // 查询热源列头展示的数据
            var commonTagItem = Db.Queryable<CommonTagItem>().WhereIF(type == 1, s => s.IsImage == true).WhereIF(type == 2, s => s.IsImage == false).WhereIF(type == -1, "1=1").OrderBy(s => s.ValueSeq, OrderByType.Asc).ToList();
            return commonTagItem;
        }

        /// <summary>
        /// 配置热源表头
        /// </summary>
        /// <param name="commonTagItems"></param>
        /// <returns></returns>
        public bool UpdCommonTagItem(List<CommonTagItem> commonTagItems)
        {
            var result = Db.Updateable(commonTagItems).UpdateColumns(s => new { s.IsImage, s.AiDesc, s.Unit, s.ValueSeq }).ExecuteCommand();
            return result > 0 ? true : false;
        }

        public ShowMonitorDto SearchShowMonitor(int id, int listType)
        {
            var userConfigData = Db.Queryable<UserConfig>().Where(s => s.Id == id && s.ListType == listType).First();
            var includeStaS = userConfigData.IncludeSta.Split(",");
            var includeParaMDLS = userConfigData.IncludeParaMDL.Split(",");
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            //int[] vpnuserid = Array.ConvertAll(includeStaS, s => int.Parse(s));
            var station = Db.Queryable<VpnUser, StationBranch, ValueDesc>((v, s, d) => new object[] { JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, v.Id == d.VpnUser_id }).In((v, s, d) => d.TagName, includeParaMDLS).Where((v, s, d) => s.StationBranchArrayNumber != 0).OrderBy((v, s, d) => v.StationNumber, OrderByType.Asc).OrderBy((v, s, d) => s.StationBranchArrayNumber, OrderByType.Asc).Select((v, s, d) => new { VpnUserId = v.Id, v.StationName, StationBranchId = s.StationBranchArrayNumber, s.StationBranchHeatArea, s.StationBranchName, d.TagName, d.RealValue, d.Hi, d.Lo, d.HiHi, d.LoLo }).ToList();
            var lastCode = station.Last().GetHashCode();
            // int vpnuserId = 0;
            //int StationBranchArrayNumber = 0;
            string stationFlag = "";
            //var list = station.GroupBy(s => s.StationBranchId);
            //var valueDesc = Db.Queryable<ValueDesc>().In(s => s.VpnUser_id, vpnuserid).In(s => s.TagName, includeParaMDLS).Select(s => new { s.RealValue, s.Hi, s.HiHi, s.Lo, s.LoLo, s.Id, s.VpnUser_id, s.NarrayNo, s.TagName }).ToList();
            Dictionary<string, object> dicObj = new Dictionary<string, object>();
            foreach (var stationBranch in station)
            {
                var newFlag = stationBranch.VpnUserId.ToString() + stationBranch.StationBranchId.ToString();
                //var duplicateValues = listResult.GroupBy(x => x.Value).Where(x => x.Count() > 1);
                //vpnuserId = stationBranch.VpnUserId;
                //StationBranchArrayNumber = stationBranch.StationBranchId;

                if (newFlag != stationFlag || stationBranch.GetHashCode() == lastCode)
                {
                    if (stationFlag != "")
                    {
                        if (stationBranch.GetHashCode() == lastCode)
                        {
                            MonitorData lastmonitorData = new MonitorData() { RealValue = stationBranch.RealValue, HiHi = stationBranch.HiHi, Hi = stationBranch.Hi, LoLo = stationBranch.LoLo, Lo = stationBranch.Lo };
                            dicObj.Add(stationBranch.TagName, lastmonitorData);
                            listResult.Add(dicObj);
                            break;
                        }
                        else
                        {
                            listResult.Add(dicObj);
                            dicObj = new Dictionary<string, object>();
                        }
                    }
                    dicObj.Add("VpnUserId", stationBranch.VpnUserId);
                    dicObj.Add("StationName", stationBranch.StationName);
                    dicObj.Add("Area", stationBranch.StationBranchHeatArea);
                    dicObj.Add("StationBranchId", stationBranch.StationBranchId);
                    dicObj.Add("StationBranchName", stationBranch.StationBranchName);
                    MonitorData monitorData = new MonitorData() { RealValue = stationBranch.RealValue, HiHi = stationBranch.HiHi, Hi = stationBranch.Hi, LoLo = stationBranch.LoLo, Lo = stationBranch.Lo };
                    dicObj.Add(stationBranch.TagName, monitorData);
                    //vpnuserId = stationBranch.VpnUserId;
                    stationFlag = newFlag;
                }
                else
                {
                    //dicObj.Add("VpnUserId", stationBranch.VpnUserId);
                    //dicObj.Add("StationName", stationBranch.StationName);
                    //dicObj.Add("Area", stationBranch.StationBranchHeatArea);
                    //dicObj.Add("StationBranchId", stationBranch.StationBranchId);
                    //dicObj.Add("StationBranchName", stationBranch.StationBranchName);
                    MonitorData monitorData = new MonitorData() { RealValue = stationBranch.RealValue, HiHi = stationBranch.HiHi, Hi = stationBranch.Hi, LoLo = stationBranch.LoLo, Lo = stationBranch.Lo };
                    dicObj.Add(stationBranch.TagName, monitorData);
                }
                //if (Convert.ToInt32(dicObj["VpnUserId"]) == stationBranch.VpnUserId && Convert.ToInt32(dicObj["StationBranchId"])==stationBranch.StationBranchId)
                //{
                //MonitorData monitorData = new MonitorData() { RealValue = stationBranch.RealValue, HiHi = stationBranch.HiHi, Hi = stationBranch.Hi, LoLo = stationBranch.LoLo, Lo = stationBranch.Lo };
                //dicObj.Add(stationBranch.TagName, monitorData);
                //}

                //if (.VpnUser_id == stationBranch.VpnUserId)
                //{
                //    MonitorData monitorData = new MonitorData() { RealValue = tagName.RealValue, HiHi = tagName.HiHi, Hi = tagName.Hi, LoLo = tagName.LoLo, Lo = tagName.Lo };
                //    dicObj.Add(tagName.TagName, monitorData);
                //}

            }
            //var list = listResult.OrderBy(s => s.GetValueOrDefault("Area"));

            ShowMonitorDto showMonitorDto = new ShowMonitorDto() { data = listResult, count = listResult.Count };
            //var resultJson = JsonConvert.SerializeObject(showMonitorDto);
            return showMonitorDto;
        }

        /// <summary>
        /// 监控实时数据表格列头
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ShowMonitorTitleDto>> SelShowMonitorTitle(int id)
        {
            var userConfigData = await Db.Queryable<UserConfig>().Where(s => s.Id == id).FirstAsync();
            var includeParaMDLS = userConfigData.IncludeParaMDL.Split(",");
            List<ShowMonitorTitleDto> showMonitorTitleDtoList = new List<ShowMonitorTitleDto>();
            //var dic= userConfigData.IncludeParaMDLWidth.Split(",");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(userConfigData.IncludeParaMDLWidth))
            {
                var IncludeParaMDLWidth = userConfigData.IncludeParaMDLWidth.Split(",");
                foreach (var item in IncludeParaMDLWidth)
                {
                    var key = item.Split(":")[0];
                    var val = item.Split(":")[1];
                    dic.Add(key, val);
                }
            }
            ShowMonitorTitleDto showMonitorTitleExpand = new ShowMonitorTitleDto();
            showMonitorTitleExpand.label = "操作";
            //showMonitorTitleExpand.Fixed = "left";
            showMonitorTitleExpand.id = "Expand";
            showMonitorTitleExpand.prop = "Expand";
            showMonitorTitleExpand.DisplayFlag = 23;
            showMonitorTitleDtoList.Add(showMonitorTitleExpand);
            ShowMonitorTitleDto showVpnUserIdExpand = new ShowMonitorTitleDto();
            showVpnUserIdExpand.label = "站点及机组ID";
            //showMonitorTitleExpand.Fixed = "left";
            showVpnUserIdExpand.id = "Pass";
            showVpnUserIdExpand.prop = "Pass";
            showVpnUserIdExpand.IsHidePara = true;
            //showMonitorTitleExpand.DisplayFlag = 10;
            showMonitorTitleDtoList.Add(showVpnUserIdExpand);

            ShowMonitorTitleDto showMonitorTitleName = new ShowMonitorTitleDto();
            showMonitorTitleName.label = "站点名称";
            //showMonitorTitleName.Fixed = "left";           
            showMonitorTitleName.id = "StationName";
            showMonitorTitleName.prop = "StationName";
            showMonitorTitleName.DisplayFlag = 10;

            if (dic.ContainsKey("StationName"))
            {
                showMonitorTitleName.width = dic["StationName"] + "px";
            }
            showMonitorTitleDtoList.Add(showMonitorTitleName);

            ShowMonitorTitleDto showMonitorTitleStationBranchName = new ShowMonitorTitleDto();
            showMonitorTitleStationBranchName.label = "机组名称";
            showMonitorTitleStationBranchName.id = "StationBranchName";
            showMonitorTitleStationBranchName.prop = "StationBranchName";
            if (dic.ContainsKey("StationBranchName"))
            {
                showMonitorTitleStationBranchName.width = dic["StationBranchName"] + "px";
            }
            showMonitorTitleDtoList.Add(showMonitorTitleStationBranchName);

            ShowMonitorTitleDto showMonitorTitleArea = new ShowMonitorTitleDto();
            showMonitorTitleArea.label = "机组面积";
            //showMonitorTitleName.Fixed = "left";
            showMonitorTitleArea.id = "StationBranchHeatArea";
            showMonitorTitleArea.prop = "StationBranchHeatArea";
            if (dic.ContainsKey("StationBranchHeatArea"))
            {
                showMonitorTitleArea.width = dic["StationBranchHeatArea"] + "px";
            }
            showMonitorTitleDtoList.Add(showMonitorTitleArea);
            foreach (var tagName in includeParaMDLS)
            {
                var newTagName = removeNarrayNo(tagName);
                var StandardParameter = await Db.Queryable<StandardParameter>().Where(s => s.TagName == tagName).FirstAsync();
                ShowMonitorTitleDto showMonitorTitleTagName = new ShowMonitorTitleDto();
                showMonitorTitleTagName.label = StandardParameter.AiDesc + " " + StandardParameter.Unit;
                showMonitorTitleTagName.id = newTagName;
                showMonitorTitleTagName.prop = newTagName;
                if (dic.ContainsKey(newTagName))
                {
                    showMonitorTitleTagName.width = dic[newTagName] + "px";
                }
                //showMonitorTitleTagName.Fixed = "align";
                showMonitorTitleDtoList.Add(showMonitorTitleTagName);
            }
            return showMonitorTitleDtoList;
        }

        /// <summary>
        /// 监控实时数据表格列头宽度保存
        /// </summary>
        /// <param name="id"></param>
        /// <param name="width"></param>
        /// <param name="prop"></param>
        public bool AddMontiorTitleWidth(int id, int width, string prop)
        {


            UserConfig userConfig = Db.Queryable<UserConfig>().Where(s => s.Id == id).First();
            string newIncludeParaMDLWidth = "";
            if (userConfig.IncludeParaMDLWidth == null)
            {
                newIncludeParaMDLWidth = prop + ":" + width;

            }
            else
            {
                var IncludeParaMDLWidth = userConfig.IncludeParaMDLWidth.Split(",");

                foreach (var item in IncludeParaMDLWidth)
                {
                    var key = item.Split(":")[0];
                    var val = item.Split(":")[1];
                    if (key == prop)
                    {
                        newIncludeParaMDLWidth = userConfig.IncludeParaMDLWidth.Replace(key + ":" + val, key + ":" + width);
                        break;
                    }
                }
                if (newIncludeParaMDLWidth == "")
                {
                    newIncludeParaMDLWidth += userConfig.IncludeParaMDLWidth + "," + prop + ":" + width;
                }
            }

            UserConfig user = new UserConfig() { Id = id, IncludeParaMDLWidth = newIncludeParaMDLWidth };
            int result = Db.Updateable(user).UpdateColumns(u => new { u.IncludeParaMDLWidth }).Where(u => u.Id == id).ExecuteCommand();
            return result > 0;
            //string obj = prop + ":" + width;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public string removeNarrayNo(string tagName)
        {

            var newTagName = "";


            var lastChar = tagName.Substring(tagName.Length - 1, 1);
            int narrayNo = 0;

            if (int.TryParse(lastChar, out narrayNo))
            {
                newTagName = tagName.Substring(0, tagName.Length - 1);
            }
            else
            {
                newTagName = tagName;
            }
            return newTagName;
        }

        /// <summary>
        /// 工艺图数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="narryid"></param>
        /// <returns></returns>
        public List<GytShowDtoList> GytShowList(int id, int narryid)
        {
            var resultlist = Db.Queryable<VpnUser, StationBranch, ValueDesc>((v, s, d) => new object[] { JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, v.Id == d.VpnUser_id && s.StationBranchArrayNumber == d.NarrayNo }).WhereIF(narryid != -1, (v, s, d) => new int[] { 0, narryid }.Contains(s.StationBranchArrayNumber)).Where((v, s, d) => v.IsValid == true && d.ValueSeq != -1 && v.Id == id).OrderBy((v, s, d) => d.AiType, OrderByType.Asc).OrderBy((v, s, d) => d.ValueSeq, OrderByType.Asc).Select((v, s, d) => new GytShowDtoList() { Id = v.Id, NarrayNo = s.StationBranchArrayNumber, StationName = v.StationName, StationBranchName = s.StationBranchName, AiDesc = d.AiDesc, TagName = d.TagName, RealValue = d.RealValue, Unit = d.Unit, ValueSeq = d.ValueSeq, AiType = d.AiType, GYTShow = d.GYTShow, Xval = d.Xval, Yval = d.Yval, FlowChart = v.FlowChart, StationStandard = v.StationStandard, OneMean = d.OneMean, ZeroMean = d.ZeroMean, GYTTagShow = d.GYTTagShow, GGLShow = d.GGLShow, TimeSjk = SqlFunc.GetDate() }).ToList();
            GytShowDtoList gytTime = resultlist.Where(s => s.TagName == "TIMESTAMP" && s.NarrayNo == 0).First();
            DateTime d1 = Convert.ToDateTime(gytTime.RealValue);
            DateTime d2 = gytTime.TimeSjk;
            TimeSpan d3 = d2.Subtract(d1);

            foreach (var item in resultlist)
            {
                if (d3.Days >= 1)
                {   
                    item.IsOnline = false;
                }
                else if (d3.Hours >= 1)
                {
                    item.IsOnline = false;
                }
                else if (d3.Minutes > 5)
                {
                    item.IsOnline = false;
                }
                else
                {
                    item.IsOnline = true;
                }
            }
            return resultlist;
        }

    }

    /// <summary>
    /// 多站列表返回
    /// </summary>
    public class ReturnListDate
    {
        /// <summary>
        /// 热源名称
        /// </summary>
        public string PowerName { get; set; }
        /// <summary>
        /// 热源id
        /// </summary>
        public int PowerId { get; set; }
        /// <summary>
        /// 站点id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 是否加入全网
        /// </summary>
        public int IsJionHeatBalance { get; set; }
        /// <summary>
        /// 站点IP地址
        /// </summary>
        public string PcIP { get; set; }
        /// <summary>
        /// 机组面积
        /// </summary>
        public string StationBranchHeatArea { get; set; }
        /// <summary>
        /// 组织机构id
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// 组织结构名称
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 机组名称
        /// </summary>
        public string StationBranchName { get; set; }
        /// <summary>
        /// 机组编号
        /// </summary>
        public int StationBranchArrayNumber { get; set; }
        /// <summary>
        /// 采集量描述
        /// </summary>
        public string AiDesc { get; set; }
        /// <summary>
        /// 采集量分类
        /// </summary>
        public string AiType { get; set; }
        /// <summary>
        /// TagName
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public string RealValue { get; set; }
        /// <summary>
        /// 高高限
        /// </summary>
        public decimal HiHi { get; set; }
        /// <summary>
        /// 高限
        /// </summary>
        public decimal Hi { get; set; }
        /// <summary>
        /// 低低限
        /// </summary>
        public decimal LoLo { get; set; }
        /// <summary>
        /// 低限
        /// </summary>
        public decimal Lo { get; set; }
        /// <summary>
        /// Di点1含义
        /// </summary>
        public string OneMean { get; set; }
        /// <summary>
        /// Di点0含义
        /// </summary>
        public string ZeroMean { get; set; }

    }
}
