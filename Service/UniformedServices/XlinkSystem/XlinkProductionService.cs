using ApiModel;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using SqlSugar;
using Sugar.Enties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.Energy;
using THMS.Core.API.Models.UniformedServices.IndoorSystem;
using THMS.Core.API.Models.UniformedServices.PvssDSSystem;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Models.ViewModel.Production;
using THMS.Core.API.Service.DbContext;
using static THMS.Core.API.Service.Monitor.ShowMonitorService;

namespace THMS.Core.API.Service.UniformedServices.XlinkSystem
{
    public class XlinkProductionService : DbContextSqlSugar
    {

        /// <summary>
        /// 获取前24小时的天气预报
        /// </summary>
        /// <returns></returns>
        public dynamic GetFront24HourWeather() {
            List<dynamic> temp;
            try
            {
                temp = Db.Ado.SqlQuery<dynamic>(@"SELECT HourAvgTemperature,
                                                      NcapTime
                                                FROM dbo.RealTemperature
                                                WHERE NcapTime
                                                      BETWEEN DATEADD(DAY, -1, GETDATE()) AND GETDATE()
                                                      AND CollectName = '实时天气';").ToList();
            }
            catch (Exception e)
            {

                temp = null;
            }
            return temp;
        }
        /// <summary>
        /// 获取任意时间段历史天气
        /// </summary>
        /// <param name="stratdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public dynamic GetHourHistroyWeather(DateTime stratdate, DateTime enddate)
        {
            List<dynamic> temp;
            try
            {
                temp = Db.Ado.SqlQuery<dynamic>(@"SELECT HourAvgTemperature,
                                                      NcapTime
                                                FROM dbo.RealTemperature
                                                WHERE NcapTime
                                                      BETWEEN @stratdate AND @enddate
                                                      AND CollectName = '实时天气';",new { stratdate, enddate }).ToList();
            }
            catch (Exception e)
            {

                temp = null;
            }
            return temp;
        }


        /// <summary>
        /// 所有站点或热源的实时数据(生产调度)
        /// </summary>
        /// <param name="sortName">排序名称</param>
        /// <param name="sortType">排序类型</param>
        /// <param name="tagNames">检测点串</param>
        /// <param name="type">类型（参数1为换热站，2为热源）</param>
        /// <param name="organIds">公司id 参数-1为全部</param>
        /// <param name="pageIndex">当前页标</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public object SelShowPower(string sortName, string sortType, string[] tagNames, int type, string organIds, int pageIndex, int pageSize)
        {
            // 获取热源信息及实时参数数据
            var dataList = Db.Queryable<StationBranch, VpnUser, ValueDesc, Organization>((s, v, d, o) => new object[] { JoinType.Left, s.VpnUser_id == v.Id, JoinType.Left, v.Id == d.VpnUser_id && s.StationBranchArrayNumber == d.NarrayNo, JoinType.Left, v.Organization_id == o.Id })
                .WhereIF(type == 1, (s, v, d) => v.StationStandard < 98).WhereIF(type == 2, (s, v, d, o) => v.StationStandard == 99)
                .WhereIF(organIds == "-1", "1=1")
                .WhereIF(organIds != "-1", (s, v, d, o) => organIds.Contains(v.Organization_id.ToString()))
                .Where((s, v, d, o) => v.IsValid == true)
                .In((s, v, d, o) => d.AiValue, new string[] { "AIVALUE1", "AIVALUE2", "AIVALUE20", "AIVALUE23", "AIVALUE24", "AIVALUE3", "AIVALUE4", "AIVALUE5", "AIVALUE6", "AIVALUE78" })
                //.OrderByIF(!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sortType) && sortName != "Area", $@"case when d.TagName='{sortName}' then 1 ELSE 1 END,1 {sortType}")
                //.OrderByIF(!string.IsNullOrEmpty(sortName) && !string.IsNullOrEmpty(sortType) && sortName == "Area", $@"{sortName} {sortType}")
                .Select((s, v, d, o) => new NewBodys { Id = v.Id, StationBranchArrayNumber = s.StationBranchArrayNumber, StationBranchName = s.StationBranchName, StationName = v.StationName, Area = v.StationHotArea, TagName = d.TagName, AiDesc = d.AiDesc, Unit = d.Unit, RealValue = d.RealValue, OrganizationName = o.OrganizationName }).ToList();
            //var valueDescList = Db.Queryable<ValueDesc>
            //var tagNameList = Db.Queryable<StandardParameter>().In(s => s.TagName, tagNames).Where(s => s.NarrayNo!=0).Select(s => new { s.NarrayNo, s.TagName }).ToList();

            //获取热源信息
            var powerList = (from c in dataList group c by new { c.Id, c.StationBranchArrayNumber, c.StationName, c.StationBranchName, c.Area, c.OrganizationName } into v select new NewBodys() { Id = v.Key.Id, StationBranchArrayNumber = v.Key.StationBranchArrayNumber, StationBranchName = v.Key.StationBranchName, StationName = v.Key.StationName, Area = v.Key.Area, OrganizationName = v.Key.OrganizationName }).ToList();
            List<Dictionary<string, object>> listResult = new List<Dictionary<string, object>>();
            foreach (var power in powerList.Where(s => s.StationBranchArrayNumber != 0))
            {
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                keyValues.Add("VpnuserId", power.Id);
                keyValues.Add("OrganizationName", power.OrganizationName);
                keyValues.Add("StationBranchArrayNumber", power.StationBranchArrayNumber);
                keyValues.Add("StationName", power.StationName);
                keyValues.Add("StationBranchName", power.StationBranchName);
                keyValues.Add("Area", power.Area);

                foreach (var item in tagNames)
                {

                    var body = dataList.Where(s => s.Id == power.Id && s.TagName == item).FirstOrDefault();
                    if (body != null)
                    {
                        keyValues.Add(item, body.RealValue);
                    }
                    else
                    {
                        var body1 = dataList.Where(s => s.Id == power.Id && s.StationBranchArrayNumber == power.StationBranchArrayNumber && s.TagName == item + power.StationBranchArrayNumber).FirstOrDefault();
                        if (body1 != null)
                            keyValues.Add(item, body1.RealValue);
                    }


                    //foreach (var t in tagNames)
                    //{
                    //    if (power.StationBranchArrayNumber > 0) 
                    //    {
                    //        var body1 = dataList.Where(s => s.Id == power.Id && s.StationBranchArrayNumber == power.StationBranchArrayNumber && s.TagName == t + power.StationBranchArrayNumber).FirstOrDefault();
                    //        if (body1 == null)
                    //        {
                    //            //keyValues.Add(item, "无该点位");
                    //        }
                    //        else
                    //        {
                    //            keyValues.Add(t, body1.RealValue);
                    //        }
                    //    }
                    //}
                }
                listResult.Add(keyValues);
            }
            int total = listResult.Count;
            var pageList = listResult.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            //string json = JsonConvert.SerializeObject(pageList);
            return new { pageList, total };
        }
        public class NewBodys
        {
            public int Id { get; set; }
            public int StationBranchArrayNumber { get; set; }
            public string StationBranchName { get; set; }
            public string OrganizationName { get; set; }

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
        /// 站点排行数据表(生产调度)
        /// </summary>
        /// <param name="powerid">热源id，默认全部为-1</param>
        /// <param name="organizationid">公司id串，默认全部为-1</param>
        /// <param name="tagname">检测点名称</param>
        /// <param name="ordertype">排序字段</param>
        /// <returns></returns>
        public dynamic GetStationRank(int powerid, string organizationid, string tagname, string ordertype)
        {
            //判断查询参数
            string flag = tagname.Substring(tagname.Length - 1, 1);
            int narrayNo = 0;
            if (int.TryParse(flag, out narrayNo))
            {
                narrayNo = 1;
            }
            else
            {
                narrayNo = 0;
            }
            List<string> ids = new List<string>();
            if (organizationid!="-1")
            {
                ids = organizationid.Split(",").ToList<string>();
            }
            string testname = tagname.Substring(0, tagname.Length - 1) + "_";
            var stationRankList = Db.Queryable<VpnUser, StationBranch, Station, VpnUser, Organization, ValueDesc>((v, b, s, t, o, d) => new object[] {
          JoinType.Left,v.Id == b.VpnUser_id,JoinType.Left,v.Id ==s.VpnUser_id,JoinType.Left,s.PowerInfo_id==t.Id,JoinType.Left,v.Organization_id==o.Id,JoinType.Left,v.Id==d.VpnUser_id && b.StationBranchArrayNumber==d.NarrayNo
          }).WhereIF(organizationid != "-1", (v, b, s, t, o, d) => ids.Contains(v.Organization_id.ToString()))
                .WhereIF(powerid != -1, (v, b, s, t, o, d) => s.PowerInfo_id == powerid)
                .WhereIF(narrayNo == 0, (v, b, s, t, o, d) => d.TagName == tagname)
                .WhereIF(narrayNo != 0, $"d.TagName LIKE '{tagname.Substring(0, tagname.Length - 1) + "_"}'")
                .Where((v, b, s, t, o, d) => v.StationStandard < 98 && v.IsValid == true)
                .Select((v, b, s, t, o, d) => new { v.Id, PowerName = t.StationName, o.OrganizationName, v.StationName, b.StationBranchName, d.RealValue }).OrderBy($@"CONVERT(float,d.RealValue) {ordertype}").Take(10).ToList();

            //var timestamp = Db.Queryable<ValueDesc>().Where(s => s.TagName == "TIMESTAMP").Select(s => new { s.VpnUser_id, s.RealValue }).ToList();
            //List<StationRankListDto> stationRankListDtoList = new List<StationRankListDto>();
            //foreach (var item in stationRankList)
            //{
            //    foreach (var time in timestamp.Where(s => s.VpnUser_id == item.Id))
            //    {
            //        StationRankListDto stationRankListDto = new StationRankListDto() { Id = item.Id, PowerName = item.PowerName, OrganizationName = item.OrganizationName, StationName = item.StationName, NarryName = item.StationBranchName, RealValue = item.RealValue, TimeStamp = time.RealValue };
            //        stationRankListDtoList.Add(stationRankListDto);
            //    }
            //}
            //if (ordertype == "asc" || ordertype == "ASC")
            //{
            //    stationRankListDtoList = stationRankListDtoList.OrderBy(s => s.RealValue).ToList();
            //}
            //else
            //{
            //    stationRankListDtoList = stationRankListDtoList.OrderByDescending(s => s.RealValue).ToList();
            //}
            return stationRankList;
        }

        /// <summary>
        /// 换热站基础信息（生产调度）
        /// </summary>
        /// <param name="vpnuserId"></param>
        /// <param name="organizationid"></param>
        /// <returns></returns>
        public object SelStationMessage(int vpnuserId, string organizationid)
        {
            var orgids = Db.Queryable<Organization>().Where(s=>organizationid.Contains(s.Id.ToString())).Select(s=>s.Id).ToList();
             //var organ = orgList.Where(s => s.Id.ToString().Contains(organizationid)).ToList();
            //List<int> orgids = new List<int>();
            //if (organ!=null)
            //{
            //    if (organ.DepLevel == 3)
            //    {
            //        orgids = orgList.Where(s => s.Id == organizationid).Select(s => s.Id).ToList<int>();
            //    }
            //    else
            //    {
            //        orgids = orgList.Where(s => s.ParentDepID == organizationid).Select(s => s.Id).ToList<int>();
            //    }
            //}
            var stationRankList = Db.Queryable<VpnUser, StationBranch>((v, b) => new object[] {
          JoinType.Left,v.Id == b.VpnUser_id
          }).WhereIF(organizationid != "-1", (v, b) => SqlFunc.ContainsArray(orgids, v.Organization_id))
                .WhereIF(vpnuserId != -1, (v, b) => v.Id == vpnuserId)
                .Where((v, b) =>  v.IsValid == true && v.StationStandard<98)
                .Select((v, b) => new { v.Id,v.StationHotArea, v.StationName, b.StationBranchName, b.StationBranchHeatArea }).ToList();
            int narrayCount = stationRankList.Count;
            var resultStation = stationRankList.GroupBy(s => s.Id).ToList();
            int stationCount = resultStation.Count;
            var stationArea = Db.Queryable<VpnUser>().WhereIF(organizationid != "-1", v => SqlFunc.ContainsArray(orgids, v.Organization_id))
                .WhereIF(vpnuserId != -1, v => v.Id == vpnuserId)
                .Where(v => v.IsValid == true && v.StationStandard < 98)
                .Sum(v => v.StationHotArea);
            return new { stationArea, stationCount, narrayCount };
        }

        /// <summary>
        /// 站点通讯状态及站点类型饼图（生产调度）
        /// </summary>
        /// <param name="vpnuserId">换热站Id</param>
        /// <param name="powerid">热源id</param>
        /// <param name="organizationid">公司id串</param>
        /// <returns></returns>
        public object SelSatationTypeState(int vpnuserId, int powerid, string organizationid)
        {
            var stationTypeList = Db.Queryable<VpnUser, ValueDesc, Station, Organization>((v, d, s, o) => new object[] { JoinType.Left, v.Id == d.VpnUser_id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, v.Organization_id == o.Id })
                .WhereIF(organizationid != "-1", (v, d, s, o) => organizationid.Contains(v.Organization_id.ToString()))
                .WhereIF(powerid != -1, (v, d, s, o) => s.PowerInfo_id == powerid)
                .WhereIF(vpnuserId != -1, (v, d, s, o) => v.Id == vpnuserId)
                .Where((v, d) => v.StationStandard < 98 && v.IsValid == true && d.TagName == "TIMESTAMP").Select((v, d) => new { v.StationType, d.RealValue, d.TagName }).ToList();
            var stationStateList = stationTypeList.Where(s => Convert.ToDateTime(s.RealValue).AddMinutes(30) > DateTime.Now).ToList();
            int stationStateOnline = stationStateList.Count;
            int stationStateOffline = stationTypeList.Count - stationStateOnline;
            var typeList = stationTypeList.GroupBy(s => s.StationType).ToList();
            Dictionary<string, object> keys = new Dictionary<string, object>();
            foreach (var type in typeList)
            {
                int typeNumber = 0;
                string typeName = "";
                switch (type.Key)
                {
                    case 1:
                        typeNumber = type.Count();
                        typeName = "普通住宅";
                        break;
                    case 10:
                        typeNumber = type.Count();
                        typeName = "工业区";
                        break;
                    case 2:
                        typeNumber = type.Count();
                        typeName = "节能建筑";
                        break;
                    case 3:
                        typeNumber = type.Count();
                        typeName = "办公楼";
                        break;
                    case 4:
                        typeNumber = type.Count();
                        typeName = "商店";
                        break;
                    case 5:
                        typeNumber = type.Count();
                        typeName = "旅馆";
                        break;
                    case 6:
                        typeNumber = type.Count();
                        typeName = "影剧院";
                        break;
                    default:
                        typeName = "无";
                        break;
                }
                if (typeName != "无")
                {
                    keys.Add(typeName, typeNumber);
                }
            }
            return new { stationStateOnline, stationStateOffline, stationType = keys };

        }

        /// <summary>
        /// 室温温度区间分布
        /// </summary>
        /// <param name="serchType"></param>
        /// <param name="serchName"></param>
        /// <returns></returns>
        public dynamic RoomAnalysis(int serchType, string serchName, int xlinkId)
        {
            int min = 0;
            int mux = 0;
            int max = 0;
            decimal ProportionOne = 0.00M;
            decimal ProportionTwo = 0.00M;
            decimal ProportionThree = 0.00M;
            decimal IndoorOne = 0.00M;
            decimal IndoorTwoUp = 0.00M;
            decimal IndoorTwoDo = 0.00M;
            decimal IndoorThree = 0.00M;

            try
            {
                List<RealPieDataDto> da_s = new List<RealPieDataDto>();
                //公司信息
                if (serchType == 1)
                {
                    da_s =new DbContextMySql().DbMysqlIndoor.Queryable<da_realvalue, eq_devicebind, hu_detailedbind, or_structure>((dar, eqd, hud, org) => new object[] { JoinType.Inner, dar.DeviceNum == eqd.DeviceNum, JoinType.Inner, eqd.UserCode == hud.UserCode,JoinType.Inner, hud.CompanyId == org.Id })
                      .Where((dar, eqd, hud, org) => eqd.CheckState == 1 && org.xLinkId == xlinkId)
                      .Select((dar, eqd, hud, org) => new RealPieDataDto { DeviceNum = dar.DeviceNum, TValue = dar.TValue, Humidity = dar.Humidity, RecordTime = dar.RecordTime, UserCode = eqd.UserCode })
                      .OrderBy(dar => dar.TValue, OrderByType.Desc).ToList();
                }
                //热源信息
                else if (serchType == 2)
                {
                    da_s = new DbContextMySql().DbMysqlIndoor.Queryable<da_realvalue, eq_devicebind, hu_detailedbind, or_structure>((dar, eqd, hud, org) => new object[] { JoinType.Inner, dar.DeviceNum == eqd.DeviceNum, JoinType.Inner, eqd.UserCode == hud.UserCode, JoinType.Inner, hud.SourceId == org.Id })
                      .Where((dar, eqd, hud, org) => eqd.CheckState == 1 && org.PanyName == serchName)
                      .Select((dar, eqd, hud, org) => new RealPieDataDto { DeviceNum = dar.DeviceNum, TValue = dar.TValue, Humidity = dar.Humidity, RecordTime = dar.RecordTime, UserCode = eqd.UserCode })
                      .OrderBy(dar => dar.TValue, OrderByType.Desc).ToList();
                }
                //换热站信息
                else if (serchType == 3)
                {
                    da_s = new DbContextMySql().DbMysqlIndoor.Queryable<da_realvalue, eq_devicebind, hu_detailedbind, or_station>((dar, eqd, hud, org) => new object[] { JoinType.Inner, dar.DeviceNum == eqd.DeviceNum, JoinType.Inner, eqd.UserCode == hud.UserCode, JoinType.Inner, hud.StationId == org.Id })
                      .Where((dar, eqd, hud, org) => eqd.CheckState == 1 && org.StationName == serchName)
                      .Select((dar, eqd, hud, org) => new RealPieDataDto { DeviceNum = dar.DeviceNum, TValue = dar.TValue, Humidity = dar.Humidity, RecordTime = dar.RecordTime, UserCode = eqd.UserCode })
                      .OrderBy(dar => dar.TValue, OrderByType.Desc).ToList();
                }
                else
                {
                    da_s = new DbContextMySql().DbMysqlIndoor.Queryable<da_realvalue, eq_devicebind, hu_detailedbind>((dar, eqd, hud) => new object[] { JoinType.Inner, dar.DeviceNum == eqd.DeviceNum, JoinType.Inner, eqd.UserCode == hud.UserCode })
                      .Where((dar, eqd, hud) => eqd.CheckState == 1)
                      .Select((dar, eqd, hud) => new RealPieDataDto { DeviceNum = dar.DeviceNum, TValue = dar.TValue, Humidity = dar.Humidity, RecordTime = dar.RecordTime, UserCode = eqd.UserCode })
                      .OrderBy(dar => dar.TValue, OrderByType.Desc).ToList();
                }


                List<co_threshold> co_t = new DbContextMySql().DbMysqlIndoor.Queryable<co_threshold>().ToList();
                IndoorOne = decimal.Parse(co_t.Where(t => t.ValueEName == "IndoorOne").Select(m => m.ValueData).FirstOrDefault().ToString());
                IndoorTwoUp = decimal.Parse(co_t.Where(t => t.ValueEName == "IndoorTwoUp").Select(m => m.ValueData).FirstOrDefault().ToString());
                IndoorTwoDo = decimal.Parse(co_t.Where(t => t.ValueEName == "IndoorTwoDo").Select(m => m.ValueData).FirstOrDefault().ToString());
                IndoorThree = decimal.Parse(co_t.Where(t => t.ValueEName == "IndoorThree").Select(m => m.ValueData).FirstOrDefault().ToString());
                var resultList = getmake();
                foreach (var da_stm in da_s)
                {
                    var plusAdd = resultList.Where(m => m.UserCode == da_stm.UserCode).Select(m => new eq_makeupdetal { CategoryMake = m.CategoryMake, SeparateMake = m.SeparateMake }).ToList();
                    decimal? make = 0.00M;
                    if (plusAdd.Count() != 0)
                    {
                        //补偿温度
                        make = plusAdd.FirstOrDefault().CategoryMake + plusAdd.FirstOrDefault().SeparateMake;
                    }
                    if ((da_stm.TValue + make) < IndoorOne)
                    {
                        min++;
                    }
                    if ((da_stm.TValue + make) <= IndoorTwoUp && (da_stm.TValue + make) >= IndoorTwoDo)
                    {
                        mux++;
                    }
                    if ((da_stm.TValue + make) > IndoorThree)
                    {
                        max++;
                    }
                }
                decimal totalcount = da_s.Count();
                ProportionOne = Math.Round((min / totalcount), 4) * 100;
                ProportionTwo = Math.Round((mux / totalcount), 4) * 100;
                ProportionThree = Math.Round((max / totalcount), 4) * 100;

            }
            catch (Exception e)
            {
                return new
                {
                    temp = new object[] { },
                    data = new object[] { }
                };
            }

            object[] temp = new object[] {
                "<" + IndoorOne,
                IndoorTwoDo + "-" + IndoorTwoUp,
                 ">" + IndoorThree
            };

            object[] dataRoom = new object[] {
                min,
                mux,
                max
            };
            object[] dataPropo = new object[] {
                ProportionOne ,
                ProportionTwo ,
                ProportionThree
            };
            return new
            {
                //温度区间
                tempData = new object[] { temp },
                //室温分析
                RoomAnalysis = new object[] { dataRoom },
                //室温占比图
                Proportion = new object[] { dataPropo },

            };
        }

        public List<eq_makeupdetal> getmake()
        {
            return new DbContextMySql().DbMysqlIndoor.Queryable<eq_makeupdetal>().ToList();
        }

        /// <summary>
        /// 热网能耗数据展示
        /// </summary>
        /// <param name="energyType">参数1为水，2为电，3为热</param>
        /// <param name="stationType">1：普通住宅 2：节能建筑 10：工业区</param>
        /// <returns></returns>
        public object SelEnergyProductionList(int energyType,int stationType)
        {
            var list = Db.Queryable<VpnUser, MeterDataHour, Organization>((v, d, o) => new object[] { JoinType.Left, d.Vpnuser_id == v.Id, JoinType.Left, v.Organization_id == o.Id })
                .WhereIF(energyType == 1, (v, d, o) => d.MeterType == 1)
                .WhereIF(energyType == 2, (v, d, o) => d.MeterType == 2)
                .WhereIF(energyType == 3, (v, d, o) => d.MeterType == 3)
                .WhereIF(stationType!=-1,(v,d,o)=>v.StationType==stationType)
                .Where((v, d, o) => v.IsValid == true  && d.MeterDate == Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00")) && o.DepLevel!=1)
                .Select((v,d,o)=>new { d.Total,d.MeterType,o.Id,o.OrganizationName,v.StationHotArea})
                .ToList();
            var resultEnergyList = list.GroupBy(s => s.Id).ToList();
            List<object> xData = new List<object>();
            List<object> data = new List<object>();
            if (energyType == 3)
            {
                foreach (var item in resultEnergyList)
                {
                    var result = item.First();
                    xData.Add(result.OrganizationName);
                    decimal energySum = item.Sum(s => s.Total * 1000000M / 3.6M / s.StationHotArea);
                    data.Add(Math.Round(energySum,2));
                }
            }
            else
            { 
                foreach (var item in resultEnergyList)
                {
                    var result = item.First();
                    xData.Add(result.OrganizationName);
                   decimal energySum = item.Sum(s =>s.Total);
                    data.Add(energySum);
                }
            }
            return new { xData,data};
        }


        /// <summary>
        /// 室温历史数据检修
        /// </summary>
        /// <param name="serchName"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public object SelIndoorHistoryList(string serchName, DateTime startTime, DateTime endTime, int pageSize, int pageIndex)
        {
            int total = 0;
            var historyList = new DbContextMySql().DbMysqlIndoor.Queryable<da_historyday, eq_devicebind, hu_detailedbind, or_structure>((dar, eqd, hud, org) => new object[] { JoinType.Inner, dar.DeviceNum == eqd.DeviceNum, JoinType.Inner, eqd.UserCode == hud.UserCode, JoinType.Inner, hud.StationId == org.Id })
                .OrderBy((dar, eqd, hud, org) => dar.RecordTime)
                .WhereIF(!SqlFunc.IsNullOrEmpty(serchName), (dar, eqd, hud, org) => org.PanyName == serchName)
                .Where((dar, eqd, hud, org) => eqd.CheckState == 1 && SqlFunc.Between(dar.RecordTime, startTime, endTime))
                .Select((dar, eqd, hud, org) => new { DeviceNum = dar.DeviceNum, TValue = dar.TValue, Humidity = dar.Humidity, RecordTime = dar.RecordTime, UserCode = eqd.UserCode, hud.Village, hud.Address, org.PanyName }).ToPageList(pageIndex, pageSize, ref total);
            return new { historyList, total };
        }

        /// <summary>
        /// 保修与测温信息展示
        /// </summary>
        /// <param name="serchName">公司名称</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <param name="pageIndex">页标</param>
        /// <param name="type">参数1为保修信息、2为测温信息、-1为全部信息</param>
        /// <returns></returns>
        public object SelRepairinforList(string serchName, DateTime startTime, DateTime endTime, int pageSize, int pageIndex,int type)
        {
            int total = 0;
            var historyList = new DbContextMySql().DbMysqlIndoor.Queryable<eq_repairinfor, hu_detailedbind, or_structure>((dar, hud, org) => new object[] { JoinType.Inner,  dar.UserCode == hud.UserCode, JoinType.Inner, hud.CompanyId == org.Id })
                .OrderByIF(type==1,(dar,hud, org) => dar.RepairCarryoutTime)
                .OrderByIF(type==2, (dar, hud, org) => dar.TemCarryoutTime)
                .WhereIF(!SqlFunc.IsNullOrEmpty(serchName), (dar,hud, org) => org.PanyName == serchName)
                .WhereIF(type==1,(dar, hud, org) => SqlFunc.Between(dar.RepairCarryoutTime, startTime, endTime))
                .WhereIF(type==2,(dar, hud, org) => SqlFunc.Between(dar.TemCarryoutTime, startTime, endTime))
                .WhereIF(type==-1,(dar, hud, org) => SqlFunc.Between(dar.TemCarryoutTime, startTime, endTime) && SqlFunc.Between(dar.RepairCarryoutTime, startTime, endTime))
                .Select((dar, hud, org) => new { dar.FailureDescription,dar.RepairCarryoutTime,dar.TemCarryoutTime,hud.Village,hud.Unit,hud.House,hud.Address,org.PanyName,dar.IsNoRepairCarryout,dar.IsNoTemCarryout}).ToPageList(pageIndex, pageSize, ref total);
            return new { historyList, total };
        }

        public object SelEnergyHistoryThreeList(DateTime startTime,DateTime endTime,int type,int organId)
        {
            var energyList = Db.Queryable<MeterDataHour,VpnUser,Organization>((m,v,o)=>new object[] { JoinType.Left,m.Vpnuser_id==v.Id,JoinType.Left,v.Organization_id==o.Id})
                .WhereIF(type==1,(m,v,o)=>m.MeterType==1||m.MeterType==101)
                .WhereIF(type==2,(m,v,o)=>m.MeterType==2||m.MeterType==102)
                .WhereIF(type==3,(m,v,o)=>m.MeterType==3||m.MeterType==103)
                .Where((m,v,o) => SqlFunc.Between(m.MeterDate, startTime, endTime) && o.Id==organId).ToList();
            return energyList;
        }

        /// <summary>
        ///  公司失调度与控制目标柱状图展示
        /// </summary>
        /// <returns></returns>
        public object SelOrganScheduOrControlList()
        {
            //var resultList = Db.Queryable<OrganSchedu, Organization>((s, o) => new object[] {JoinType.Left, s.OrganizationCode == o.OrganizationCode }).Where((s, o) => o.DepLevel != 1).Select((s,o)=>new { s.OrganizationCode,s.StationNumber,s.ControNumber,s.Schedu,o.OrganizationName}).ToList();
            var resultList = Db.Queryable<VpnUser, PvssSetting, OrganSchedu>((v, p, o) => new object[] { JoinType.Left, v.Id.ToString() == p.VpnUser_id, JoinType.Left, p.Pvss_id == o.OrganizationCode.ToString() }).Where((v, p, o) => v.StationStandard == 98 && v.IsValid == true).Select((v, p, o) => new { o.OrganizationCode, o.StationNumber, o.ControNumber, o.Schedu, v.StationName }).ToList();
            var scheduXdate = resultList.Select(s => s.StationName).ToList();
            var controlXdata = resultList.Select(s => s.StationName).ToList();
            var scheduData = resultList.Select(s => s.Schedu / 100).ToList();
            var controlData = resultList.Select(s => s.ControNumber).ToList();
            return new { scheduXdate, controlXdata, scheduData, controlData };
        }
        /// <summary>
        /// 综合评价雷达图
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public object SelSynthesize(int orgId)
        {
            var resultList = Db.Queryable<MeterDataHour, VpnUser, Organization>((d, v, o) => new object[] { JoinType.Inner, d.Vpnuser_id == v.Id, JoinType.Inner, v.Organization_id == o.Id }).WhereIF(orgId != -1, (d, v, o) => o.Id == orgId).Where((d, v, o) => d.MeterType < 100 && d.MeterDate == Convert.ToDateTime(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00"))).Select((d, v, o) => new { d.MeterType, d.Total }).ToList();
          var flow=  resultList.Where(s => s.MeterType == 1).Sum(s =>Convert.ToDecimal(s.Total));
          var fower=   resultList.Where(s => s.MeterType == 2).Sum(s =>Convert.ToDecimal(s.Total));
          var heat =   resultList.Where(s => s.MeterType == 3).Sum(s =>Convert.ToDecimal(s.Total));
            var scheduList = Db.Queryable<Organization, OrganSchedu>((o, s) => new object[] { JoinType.Left, o.OrganizationCode == s.OrganizationCode }).Select((o,s)=>new {o.Id,s.Schedu }).ToList();
            double schedu = 0.00;
            if (orgId==-1)
            {
                schedu = scheduList.Average(s => s.Schedu);
            }
            else
            {
                schedu = scheduList.Where(s => s.Id == orgId).First().Schedu;
            }
            List<object> xData = new List<object>();
            string[] names = { "投诉量", "热耗", "水耗", "电耗", "收费率", "失调度" };
            xData.AddRange(names);
            List<object> data = new List<object>();
            data.Add(20);
            data.Add(heat);
            data.Add(flow);
            data.Add(fower);
            data.Add(30);
            data.Add(schedu.ToString("0.00"));
            return new { xData, data };
        }

        /// <summary>
        /// 三天水电热对比图
        /// </summary>
        /// <returns></returns>
        public object queryThreeDayWaterPowerHeat(string id,int type,int stationType)
        {
            try
            {
               var ids =id.Split(",");
                string sql = $@"select v.Id , v.StationName ,MeterType, Total ,CONVERT(varchar(100),MeterDate,23) as MeterDate
                                from MeterData c left join VpnUser v on c.VpnUser_id = v.Id left join Organization o on v.Organization_id = o.Id 
                                where o.Id in ({id}) and MeterDate between DATEADD(DAY,-4,GETDATE()) and GETDATE() and c.MeterType = " + type + " order by MeterDate , v.Id";
                var resultlist = Db.Ado.SqlQuery<EnergyThreeDayResponse>(sql).ToList();
                var list = Db.Queryable<VpnUser>().WhereIF(stationType!=-1,s=>s.StationType==stationType)
                    .Where(s => s.IsValid == true && ids.Contains(s.Organization_id.ToString())).Select(s => new { s.Id, s.StationName }).OrderBy(s=>s.Id).ToList();
                List<string> stationName = list.Select(s => s.StationName).ToList<string>();
                var groupDate = resultlist.GroupBy(s => s.MeterDate).OrderBy(s=>s.Key).ToList();
                List<string> date = groupDate.Select(s => s.Key).ToList();
                return new { stationName, date, groupDate };
            }
            catch (Exception ex)
            {
                return null;
            }
        
        }

        /// <summary>
        ///  公司失调度与控制目标柱状图展示
        /// </summary>
        /// <returns></returns>
        public object queryThreeDayScheduDay(int id)
        {
            string SQL = $@"select s.OrganizationCode, s.StationNumber, s.ControNumber, s.Schedu, o.OrganizationName , s.Createtime 
                            from OrganScheduHour s inner join Organization o on s.OrganizationCode = o.OrganizationCode 
                            where s.Createtime between DATEADD(DAY,-3,GETDATE()) and GETDATE() and o.Id = " + id + "";
            var resultList = Db.Ado.SqlQuery<dynamic>(SQL).ToList();
            //var scheduXdate = resultList.Select(s => s.OrganizationName).ToList();
            //var controlXdata = resultList.Select(s => s.OrganizationName).ToList();
            var scheduData = resultList.Select(s => s.Schedu).ToList();
            var controlData = resultList.Select(s => s.ControNumber).ToList();
            var createtime = resultList.Select(s => s.Createtime).ToList();
            return new { scheduData, controlData, createtime };
        }

        /// <summary>
        ///  模糊查询换热站名称
        /// </summary>
        /// <returns></returns>
        public object queryStationName(string name)
        {
            string SQL = $@"select indexCode,name from RadioViewByParams where name like '%" + name + "%'";
            var resultList = Db.Ado.SqlQuery<dynamic>(SQL).ToList();
            //var scheduData = resultList.Select(s => s.Schedu).ToList();
            //var controlData = resultList.Select(s => s.ControNumber).ToList();
            //var createtime = resultList.Select(s => s.Createtime).ToList();
            return new { resultList };
        }

        /// <summary>
        ///  查询站下视频监控点编码
        /// </summary>
        /// <returns></returns>
        public object queryCameraCode(int id)
        {
            string SQL = $@"select c.cameraName,c.cameraIndexCode from RadioViewByParams b inner join RadioCameraData c on b.indexCode = c.regionIndexCode 
                            where b.available = 'True' and b.VpnUser_id = " + id;
            var resultList = Db.Ado.SqlQuery<dynamic>(SQL).ToList();
            //var scheduData = resultList.Select(s => s.Schedu).ToList();
            //var controlData = resultList.Select(s => s.ControNumber).ToList();
            //var createtime = resultList.Select(s => s.Createtime).ToList();
            return new { resultList };
        }

        /// <summary>
        ///  匹配站id和站编码
        /// </summary>
        /// <returns></returns>
        public int updateParamsId(int id,string code)
        {
            var result = Db.Updateable<RadioViewByParams>().UpdateColumns(it => new RadioViewByParams() { VpnUser_id = id }).Where(it => it.indexCode == code).ExecuteCommand();
            //var scheduData = resultList.Select(s => s.Schedu).ToList();
            //var controlData = resultList.Select(s => s.ControNumber).ToList();
            //var createtime = resultList.Select(s => s.Createtime).ToList();
            return result > 0 ? ResultCode.Success : ResultCode.Error;
        }

        /// <summary>
        /// 查询热源总数量及分类数量
        /// </summary>
        /// <returns></returns>
        public SourceTypeNumberResponse sourceTypeNumberResponse()
        {
            try
            {
                List<VpnUser> vpnUserList = Db.Queryable<VpnUser>().Where(s => s.IsValid == true && s.StationStandard==99).ToList();
                SourceTypeNumberResponse source = new SourceTypeNumberResponse();
                source.TotalQuantity = vpnUserList.Count;
                source.TotalCentralizedHeat = vpnUserList.Where(s => s.HeatNetType == 0).Count();
                source.TotalRegionalHeating = vpnUserList.Where(s => s.HeatNetType == 1).Count();
                return source;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询热源瞬时热量饼图信息
        /// </summary>
        /// <param name="heatType">参数0为集中供热，1为区域供热</param>
        /// <param name="paramType">参数1为获取瞬时热量，2为瞬时流量</param>
        /// <returns></returns>
        public string sourceHeatType(int heatType,int paramType)
        {
            var result = Db.Queryable<VpnUser, ValueDesc>((s, p) => new object[] { JoinType.Left, s.Id == p.VpnUser_id }).WhereIF(paramType==1,(s,p)=> p.TagName == "HEAT").WhereIF(paramType == 2, (s, p) => p.TagName == "PRI_FLOW_S").Where((s,p) => s.HeatNetType == heatType && s.StationStandard == 99 && s.IsValid == true).Select((s,p) => new { name = s.StationName, sourceId = s.Id ,p.TagName,p.RealValue}).ToList();
            List<Dictionary<string, Object>> keyValuePairList = new List<Dictionary<string, object>>();
            foreach (var item in result)
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("sourceId", item.sourceId);
                keyValuePairs.Add("name", item.name);
                keyValuePairs.Add(item.TagName, item.RealValue);
                keyValuePairList.Add(keyValuePairs);
            }
            return JsonConvert.SerializeObject(keyValuePairList);
        }


        /// <summary>
        /// 热网管径信息
        /// </summary>
        /// <param name="id">热网Id</param>
        /// <returns></returns>
        public PipeNetWorkInfoResponse pipeNetWorkInfo(int id)
        {
            try
            {
                PipeNetWorkInfoResponse pipeNetWorkInfoResponse = new PipeNetWorkInfoResponse();
                var resultPrimaryPipe = primaryPipeNetWorkInfo(pipeNetWorkInfoResponse, id);
                var resultSecondaryPipe = secondaryPipeNetWorkInfo(resultPrimaryPipe, id);
                return resultSecondaryPipe;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// 一级管网数据信息展示
        /// </summary>
        /// <param name="pipeNetWorkInfoResponse">热网管径信息响应类</param>
        /// <param name="id">热网id</param>
        /// <returns></returns>
        public PipeNetWorkInfoResponse primaryPipeNetWorkInfo(PipeNetWorkInfoResponse pipeNetWorkInfoResponse, int id)
        {
            try
            {
                var pipeTgResponseList = Db.Queryable<PrimaryPipeNetwork>().Where(s => s.KHid == id && s.L_TYPE_ID==1).GroupBy(s => new { s.D_S_ID, s.D_S}).Select(s => new PrimaryPipeTgResponse { DcId = s.D_S_ID,DcName = s.D_S, DcNumber = SqlFunc.AggregateCount(s.D_S_ID), DcLength = SqlFunc.AggregateSum(SqlFunc.ToDecimal(s.SHAPE_LEN)) }).OrderBy(s=>s.DcId).ToList();
                var pipeThResponseList = Db.Queryable<PrimaryPipeNetwork>().Where(s => s.KHid == id && s.L_TYPE_ID == 2).GroupBy(s => new { s.D_S_ID, s.D_S }).Select(s => new PrimaryPipeThResponse { DcId = s.D_S_ID,DcName = s.D_S, DcNumber = SqlFunc.AggregateCount(s.D_S_ID), DcLength = SqlFunc.AggregateSum(SqlFunc.ToDecimal(s.SHAPE_LEN)) }).OrderBy(s => s.DcId).ToList();
                pipeNetWorkInfoResponse.PrimaryPipeTgResponseList = pipeTgResponseList;
                pipeNetWorkInfoResponse.PrimaryPipeThResponseList = pipeThResponseList;
                return pipeNetWorkInfoResponse;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        ///  二级管网数据信息展示
        /// </summary>
        /// <param name="pipeNetWorkInfoResponse">热网管径信息响应类</param>
        /// <param name="id">热网Id</param>
        /// <returns></returns>
        public PipeNetWorkInfoResponse secondaryPipeNetWorkInfo(PipeNetWorkInfoResponse pipeNetWorkInfoResponse, int id)
        {
            try
            {
                var pipeTgResponseList = Db.Queryable<SecondaryPipeNetwork>().Where(s => s.KHid == id && s.L_TYPE_ID == 1).GroupBy(s => new { s.D_S_ID, s.D_S }).Select(s => new SecondaryPipeTgResponse { DcId = s.D_S_ID, DcName = s.D_S, DcNumber = SqlFunc.AggregateCount(s.D_S_ID), DcLength = SqlFunc.AggregateSum(SqlFunc.ToDecimal(s.SHAPE_LEN)) }).OrderBy(s => s.DcId).ToList();
                var pipeThResponseList = Db.Queryable<SecondaryPipeNetwork>().Where(s => s.KHid == id && s.L_TYPE_ID == 2).GroupBy(s => new { s.D_S_ID, s.D_S }).Select(s => new SecondaryPipeThResponse { DcId = s.D_S_ID, DcName = s.D_S, DcNumber = SqlFunc.AggregateCount(s.D_S_ID), DcLength = SqlFunc.AggregateSum(SqlFunc.ToDecimal(s.SHAPE_LEN)) }).OrderBy(s => s.DcId).ToList();
                pipeNetWorkInfoResponse.SecondaryPipeTgResponseList = pipeTgResponseList;
                pipeNetWorkInfoResponse.SecondaryPipeThResponseList = pipeThResponseList;
                return pipeNetWorkInfoResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        
        }

        /// <summary>
        /// 全部热网管径信息柱状图展示
        /// </summary>
        /// <returns></returns>
        public object pipeNetWorkCharts(string startTime,string endTime,int type)
        {
            try
            {
                //var result = Db.Queryable<VpnUser>().Where(s => s.IsValid == true && s.StationStandard == 98).Select(s => new { s.Id, s.StationName }).ToList();
                //List<string> xData = new List<string>();
                //List<object> yPrimaryData = new List<object>();
                //List<object> ySecondaryData = new List<object>();
                string sql = "";
                if (type==-1)
                {
                    sql = "1=1";
                }
                else
                {
                    sql = $"p.P_T BETWEEN '{startTime}' AND '{endTime}'";
                }
                var pipeDiameterInfo = Db.Ado.SqlQuery<dynamic>($@"SELECT a.KHid AS Khid,a.StationName AS StationName,a.sumInfo AS FirstSumInfo,b.sumInfo AS SecondSumInfo,a.totalCount AS FirstTotalCount,b.totalCount AS SecondTotalCount FROM (
SELECT KHid,
       v.StationName AS StationName,
       SUM(CAST(SHAPE_LEN AS DECIMAL(19, 2))) AS sumInfo,
       COUNT(1) AS totalCount
FROM dbo.PrimaryPipeNetwork p
    LEFT JOIN dbo.VpnUser v
        ON p.KHid = v.Id WHERE {sql}
GROUP BY KHid,
         v.StationName
		 ) a LEFT JOIN 
		( SELECT KHid,
       v.StationName AS StationName,
       SUM(CAST(SHAPE_LEN AS DECIMAL(19, 2))) AS sumInfo,
       COUNT(1) AS totalCount
FROM dbo.SecondaryPipeNetwork p
    LEFT JOIN dbo.VpnUser v
        ON p.KHid = v.Id WHERE {sql}
GROUP BY KHid,
         v.StationName
		 ) b ON a.KHid = b.KHid").ToList();
//                var countPrimary = Db.Ado.SqlQuery<PipeNetWorkChartsResponse>($@"SELECT KHid,
//       v.StationName AS StationName,
//       SUM(CAST(SHAPE_LEN AS DECIMAL(19, 2))) AS sumInfo,
//       COUNT(1) AS totalCount
//FROM dbo.PrimaryPipeNetwork p
//    LEFT JOIN dbo.VpnUser v
//        ON p.KHid = v.Id WHERE p.P_T BETWEEN '{startTime}' AND '{endTime}'
//GROUP BY KHid,
//         v.StationName").ToList();
//                var countSecondary = Db.Ado.SqlQuery<PipeNetWorkChartsResponse>($@"SELECT KHid,
//       v.StationName AS StationName,
//       SUM(CAST(SHAPE_LEN AS DECIMAL(19, 2))) AS sumInfo,
//       COUNT(1) AS totalCount
//FROM dbo.SecondaryPipeNetwork p
//    LEFT JOIN dbo.VpnUser v
//        ON p.KHid = v.Id WHERE p.P_T BETWEEN '2013-06-01' AND '2013-07-10'
//GROUP BY KHid,
//         v.StationName").ToList();

                return new{ pipeDiameterInfo };
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 全部热网信息
        /// </summary>
        /// <returns></returns>
        public object heatNetInfo()
        {
            try
            {
               var result = Db.Queryable<VpnUser>().Where(s => s.IsValid == true && s.StationStandard == 98).Select(s => new { s.Id, s.StationName }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }

}
