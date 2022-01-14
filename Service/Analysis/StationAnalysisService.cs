using ApiModel;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Analysis;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.Monitor;
using THMS.Core.API.Service.Common;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.Monitor;

namespace THMS.Core.API.Service.Analysis
{
    /// <summary>
    /// 监控分析模块
    /// </summary>
    public class StationAnalysisService : Controller
    {
        StationParameterService _stationParameterService = new StationParameterService();
        StationTimeCorrectService _stationTimeCorrectService = new StationTimeCorrectService();
        CommonService _commonService = new CommonService();

        /// <summary>
        /// 数据库连接串
        /// </summary>
        DbContextSqlSugar DbContext = new DbContextSqlSugar();

        #region 全网平衡分析
        /// <summary>
        /// 虚拟网列表
        /// </summary>
        /// <param name="status">启用状态 0:未启用，1:启用</param>
        /// <returns></returns>
        public List<VirtualNetConfigExt> GetVirtualNetConfigs(int status = -1)
        {
            var list = new List<VirtualNetConfigExt>();

            string sql = @"SELECT
                            	v.*,
                            	h.ControlName 
                            FROM
                            	VirtualNetConfig v
                            	LEFT JOIN HeatNetControlType h ON v.ControlType= h.ControlType where 1=1 ";

            if (status > -1)
            {
                sql = string.Format(sql + " and v.IsEnabled=@status");
            }

            list = DbContext.Db.Ado.SqlQuery<VirtualNetConfigExt>(sql, new SugarParameter[]{
                    new SugarParameter("@status", status)});

            return list;
        }

        /// <summary>
        /// 运行质量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxNarrayNo"></param>
        /// <param name="secTempSTagName"></param>
        /// <param name="secTempRTagName"></param>
        /// <param name="valveOpenTagName"></param>
        /// <param name="valveSetTagName"></param>
        /// <returns></returns>
        public List<VirtualNetStationM> GetStationHeatNetRunList(int id, int maxNarrayNo = 6,
            string secTempSTagName = "SEC_TEMP_S",
            string secTempRTagName = "SEC_TEMP_R",
            string valveOpenTagName = "SEC_VALVE_OPENING",
            string valveSetTagName = "SEC_VALVE_SETTING")
        {

            //当前时段
            var hour = DateTime.Now.Hour;
            if (hour % 2 == 1)//两个小时一个时段
            {
                hour = hour - 1;
            }

            #region sql
            List<string> tempS = new List<string>();
            List<string> tempR = new List<string>();

            //阀门反馈
            List<string> valveOpen = new List<string>();
            //阀门设定
            List<string> valveSet = new List<string>();

            StringBuilder sbTempS = new StringBuilder();
            StringBuilder sbTempR = new StringBuilder();
            StringBuilder sbValveOpen = new StringBuilder();
            StringBuilder sbValveSet = new StringBuilder();

            //生成机组tagname
            for (int i = 1; i <= maxNarrayNo; i++)
            {
                tempS.Add(secTempSTagName + i.ToString());
                tempR.Add(secTempRTagName + i.ToString());
                valveOpen.Add(valveOpenTagName + i.ToString());
                valveSet.Add(valveSetTagName + i.ToString());

                sbTempS.Append(" WHEN '" + secTempSTagName + i.ToString() + "' THEN RealValue ");
                sbTempR.Append(" WHEN '" + secTempRTagName + i.ToString() + "' THEN RealValue ");
                sbValveOpen.Append(" WHEN '" + valveOpenTagName + i.ToString() + "' THEN RealValue ");
                sbValveSet.Append(" WHEN '" + valveSetTagName + i.ToString() + "' THEN RealValue ");
            }

            string timeVCol = "isnull(stct.Time" + hour + ", 0) as TimeV,";

            string sql = @"SELECT
                                	v.Id AS VpnUserId ,
                                    v.StationName ,
                                    v.StaitionValveType AS ValveType ,
                                    v.ValveSigeValve ,
                                    cs.FameViewIp ,
                                    cs.FameViewPort ,
                                    s.HeatType ,
                                    sb.StationBranchName ,
                                    sb.StationBranchArrayNumber as NarrayNo ,
                                    CONVERT(DECIMAL(18, 2), sb.StationBranchHeatArea) AS StationBranchHeatArea,
                                    vnc.TargetTemp ,
                                    vnc.ControlType ,
                                    hct.ControlName ,
                                    ISNULL(stc.SecTempS, 0) SecTempSV ,
                                    ISNULL(stc.SecTempR, 0) SecTempRV ,
                                    ISNULL(stc.SecTempAvg, 0) SecTempAvgV ,
	                                {0}
                                    convert(decimal(18,2),vd.SecTempS) AS SecTempS,
                                    convert(decimal(18,2),vd.SecTempR) AS SecTempR,
                                	Round((convert(decimal(18,2),vd.SecTempS)+convert(decimal(18,2),vd.SecTempR))/2,2) as SecTempRS,
                                	convert(decimal(18,2),vd.Opening) AS Opening,
                                	convert(decimal(18,2),vd.Setting) AS Setting,
                                	CASE WHEN DATEDIFF(mi,vdr.RealValue,GETDATE())> 5 THEN 0 ELSE 1 END AS IsOnline
                FROM    VirtualNetRecord vnr
                        JOIN VirtualNetConfig vnc ON vnr.VirtualNetId = vnc.Id
                        LEFT JOIN HeatNetControlType hct ON vnc.ControlType = hct.ControlType
                        JOIN VpnUser v ON vnr.VpnUserId = v.id
                        JOIN Station s ON v.id = s.VpnUser_Id
                        JOIN StationBranch sb ON v.id = sb.VpnUser_Id
                        LEFT JOIN StationTimeCorrect AS stct ON v.Id = stct.VpnUserId and stct.NarrayNo=sb.StationBranchArrayNumber
                        LEFT JOIN CommunicationServer cs ON cs.Id = v.ServerId
                        LEFT JOIN StationTypeCorrect stc ON stc.DicTypeId = s.HeatType                                	
                        LEFT JOIN (SELECT VpnUser_Id,TagName,RealValue from ValueDesc where TagName = 'timestamp') vdr on v.Id=vdr.VpnUser_Id
                                	LEFT JOIN (
                                	SELECT
                                		VpnUser_Id,
                                		NarrayNo,
                                		MAX ( CASE TagName  " + sbTempS.ToString() + @" ELSE '0.00' END ) AS SecTempS,
                                		MAX ( CASE TagName" + sbTempR.ToString() + @" ELSE '0.00' END ) AS SecTempR,
                                		MAX ( CASE TagName" + sbValveOpen.ToString() + @" ELSE '0.00' END ) AS Opening,
                                		MAX ( CASE TagName" + sbValveSet.ToString() + @" ELSE '0.00' END ) AS Setting
                                    FROM
                                        ValueDesc

                                    WHERE 1=1
                                    GROUP BY
                                        VpnUser_Id,
                                		NarrayNo
                                	) vd ON vd.VpnUser_Id = v.Id
                                    AND sb.StationBranchArrayNumber = vd.narrayNo
                                WHERE
                                    sb.StationBranchArrayNumber > 0
                                    AND v.IsValid = 1  and vnr.VirtualNetId = @VirtualNetId";
            #endregion
            var list = new List<VirtualNetStationM>();

            var tagNames = new List<string>();
            tagNames.AddRange(tempS);
            tagNames.AddRange(tempR);
            tagNames.AddRange(valveOpen);
            tagNames.AddRange(valveSet);

            list = DbContext.Db.Ado.SqlQuery<VirtualNetStationM>(string.Format(sql, timeVCol), new SugarParameter[]{
                    new SugarParameter("@VirtualNetId", id)});

            //时段补偿
            var timeCorrect = _stationTimeCorrectService.GetStationTimeCorrectByIds(list.Select(m => m.VpnUserId).ToList());

            foreach (var vpnUser in list)
            {

                double correct = 0;
                var timec = timeCorrect.Where(m => m.VpnUserId == vpnUser.VpnUserId && m.NarrayNo == vpnUser.NarrayNo);
                if (timec.Any())
                    correct = GetCurrentTimeCorrect(timec.First());

                switch (vpnUser.ControlType)
                {
                    //均温
                    case 1:
                    case 4:
                        correct = vpnUser.SecTempAvgV + correct;
                        vpnUser.Complete = vpnUser.DesTemp == 0 ? 0 : Math.Round((vpnUser.SecTempRs - correct) / vpnUser.DesTemp, 2) * 100;
                        break;
                    //回温
                    case 2:
                    case 5:
                        correct = vpnUser.SecTempRV + correct;
                        vpnUser.Complete = vpnUser.DesTemp == 0 ? 0 : Math.Round((vpnUser.SecTempR - correct) / vpnUser.DesTemp, 2) * 100;
                        break;
                    //供温
                    case 3:
                    case 6:
                        correct = vpnUser.SecTempSV + correct;
                        vpnUser.Complete = vpnUser.DesTemp == 0 ? 0 : Math.Round((vpnUser.SecTempS - correct) / vpnUser.DesTemp, 2) * 100;
                        break;
                }

                if (vpnUser.Complete < 0)
                    vpnUser.Complete = 0;
            }

            return list;
        }

        /// <summary>
        /// TimeCorrect
        /// </summary>
        /// <param name="correct"></param>
        /// <returns></returns>
        double GetCurrentTimeCorrect(StationTimeCorrect correct)
        {
            var h = DateTime.Now.ToString("HH");
            int hh = Convert.ToInt32(h);
            #region Hour


            switch (hh)
            {
                case 0:
                case 1:
                    hh = 0;
                    break;
                case 2:
                case 3:
                    hh = 2;
                    break;
                case 4:
                case 5:
                    hh = 4;
                    break;
                case 6:
                case 7:
                    hh = 6;
                    break;
                case 8:
                case 9:
                    hh = 8;
                    break;
                case 10:
                case 11:
                    hh = 10;
                    break;
                case 12:
                case 13:
                    hh = 12;
                    break;
                case 14:
                case 15:
                    hh = 14;
                    break;
                case 16:
                case 17:
                    hh = 16;
                    break;
                case 18:
                case 19:
                    hh = 18;
                    break;
                case 20:
                case 21:
                    hh = 20;
                    break;
                case 22:
                case 23:
                    hh = 22;
                    break;
            }
            #endregion
            try
            {
                var column = "Time" + hh;
                Type type = correct.GetType(); //获取类型
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty(column); //获取指定名称的属性
                double value = (double)propertyInfo.GetValue(correct, null); //获取属性值
                return value;
            }
            catch (Exception)
            {

            }

            return 0;
        }
        #endregion

        #region 多站对比曲线分析
        /// <summary>
        /// 获取多站对比曲线数据
        /// </summary>
        /// <param name="userconfig_id">用户配置id</param>
        /// <param name="chartDateType">0:分钟、1:小时、2:天</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns></returns>
        public List<dynamic> GetMultistationChartList(int userconfig_id, ChartDateType chartDateType, DateTime startDate, DateTime endDate)
        {
            //01首先查询温度压力所对应的Aivalue值
            var paraList = _stationParameterService.GetStandardParameterList(userconfig_id);

            //站点列表
            var userconfig = DbContext.Db.Queryable<UserConfig>().Where(s => s.Id == userconfig_id).First();

            var VpnUser_id = userconfig.IncludeSta.Split(",").ToList();

            const string format =
            @"SELECT A.VpnUser_id VpnUserId,
                     V.StationName,
                     SB.StationBranchName,
                     CONVERT(DECIMAL(18, 2), SB.StationBranchHeatArea) AS StationBranchHeatArea,
                     {0}
              FROM dbo.{1} A
                  LEFT JOIN dbo.{2} B
                      ON A.VpnUser_id = B.VpnUser_id
                         AND A.CreateTime = B.CreateTime
                  LEFT JOIN dbo.VpnUser V
                      ON A.VpnUser_id = V.Id
                  JOIN dbo.StationBranch SB
                      ON A.VpnUser_id = SB.VpnUser_id
                         AND B.NarrayNo = SB.StationBranchArrayNumber
              WHERE A.VpnUser_id IN ( @vpnUserId )
                    AND A.CreateTime >= @begin
                    AND A.CreateTime <= @end;";


            string tableName0 = "";
            string tableName1 = "";

            var cols0 = new List<string>();
            var cols1 = new List<string>();

            switch (chartDateType)
            {
                case ChartDateType.Minutes:
                    tableName0 = "HistoryFirstData";
                    tableName1 = "HistorySecondData";
                    break;
                case ChartDateType.Hours:

                    tableName0 = "HistoryFirstDataHour";
                    tableName1 = "HistorySecondDataHour";
                    break;
                case ChartDateType.Days:
                    tableName0 = "HistoryFirstDayAvg";
                    tableName1 = "HistorySecondDayAvg";
                    break;
            }
            cols0.Add("a.CreateTime AS CreateTime,b.NarrayNo AS 'NarrayNo'");

            foreach (var desc in paraList)
            {
                cols1.Add(string.Format(" b.{0} AS {1} ", desc.AiValue, desc.TagName.ToUpper()));
            }

            var sql = string.Format(format, string.Join(",", cols0) + "," + string.Join(",", cols1), tableName0,
                tableName1, " ORDER BY A.CreateTime DESC ");

            var ViewList = DbContext.Db.Ado.SqlQuery<dynamic>(sql, new SugarParameter[]{
                    new SugarParameter("@vpnUserId", VpnUser_id),
                    new SugarParameter("@begin", startDate),
                    new SugarParameter("@end", endDate)});

            return ViewList;
        }

        #endregion

        #region 站点排行

        /// <summary>
        /// 站点排行，列表(历史部分)
        /// </summary>
        /// <param name="powerid">热源id,全部：-1,多选热源：1,2,3</param>
        /// <param name="organizationid">组织机构id,全部：-1,多选组织机构：1,2,3</param>
        /// <param name="tagname">tagname</param>
        /// <param name="ordertype">排序字段,asc\desc</param>
        /// <param name="chartDateType">0:分钟、1:小时、2:天</param>
        /// <param name="createtime">查询日期</param>
        /// <returns></returns>
        public List<dynamic> GetStationRankingList(string powerid, string organizationid, string tagname, string ordertype, ChartDateType chartDateType, DateTime createtime)
        {
            //首先查询温度压力所对应的Aivalue值
            var paraList = _stationParameterService.GetStandardParameterList(tagname).First();

            const string format =
            @"SELECT A.VpnUser_id VpnUserId,
                     O.OrganizationName,
                     P.StationName AS PowerName,
                     V.StationName,
                     SB.StationBranchName,
                     A.CreateTime,
                     {0}
              FROM dbo.{1} A
                  LEFT JOIN dbo.VpnUser V
                      ON A.VpnUser_id = V.Id
                  LEFT JOIN dbo.StationBranch SB
                      ON A.VpnUser_id = SB.VpnUser_id
                         AND SB.StationBranchArrayNumber = A.NarrayNo
                  LEFT JOIN dbo.Organization O
                      ON V.Organization_id = O.Id
                  LEFT JOIN dbo.Station S
                      ON V.Id = S.VpnUser_id
                  LEFT JOIN dbo.VpnUser P
                      ON S.PowerInfo_id = P.Id
              WHERE 1 = 1
                    AND A.CreateTime = @createtime
                    AND
                          (
                              V.Organization_id IN (@oids) OR -1 in (@oids)
                          )
                          AND
                          (
                              S.PowerInfo_id IN (@powerid) OR -1 in (@powerid)
                          )
              ORDER BY A.{2} {3};";

            string tableName0 = "";
            string tableName1 = "";

            var cols0 = new List<string>();
            var cols1 = new List<string>();

            switch (chartDateType)
            {
                case ChartDateType.Minutes:
                    tableName0 = "HistoryFirstData";
                    tableName1 = "HistorySecondData";
                    break;
                case ChartDateType.Hours:

                    tableName0 = "HistoryFirstDataHour";
                    tableName1 = "HistorySecondDataHour";
                    break;
                case ChartDateType.Days:
                    tableName0 = "HistoryFirstDayAvg";
                    tableName1 = "HistorySecondDayAvg";
                    break;
            }
            if (paraList.NarrayNo == 0)
            {
                cols0.Add(tableName0);
            }
            else
            {
                cols0.Add(tableName1);
            }

            var sql = string.Format(format, string.Join(",", string.Format(" A.{0} AS {1} ", paraList.AiValue, paraList.TagName.ToUpper())), string.Join(",", cols0), paraList.AiValue, ordertype);

            var ViewList = DbContext.Db.Ado.SqlQuery<dynamic>(sql, new SugarParameter[]{
                new SugarParameter("@createtime", createtime),
                new SugarParameter("@powerid", powerid),
                new SugarParameter("@oids", organizationid)});

            return ViewList;
        }

        /// <summary>
        /// 站点排行数据表(实时部分)
        /// </summary>
        /// <param name="powerid"></param>
        /// <param name="organizationid"></param>
        /// <param name="tagname"></param>
        /// <param name="ordertype"></param>
        /// <returns></returns>
        public dynamic GetStationRank(string powerid, string organizationid, string tagname, string ordertype)
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
            var stationRankList = DbContext.Db.Queryable<VpnUser, StationBranch, Station, VpnUser, Organization, ValueDesc>((v, b, s, t, o, d) => new object[] {
          JoinType.Left,v.Id == b.VpnUser_id,JoinType.Left,v.Id ==s.VpnUser_id,JoinType.Left,s.PowerInfo_id==t.Id,JoinType.Left,v.Organization_id==o.Id,JoinType.Left,v.Id==d.VpnUser_id && b.StationBranchArrayNumber==d.NarrayNo
          }) .WhereIF(!SqlFunc.IsNullOrEmpty(organizationid), (v, b, s, t, o, d)=>organizationid.Contains(v.Organization_id.ToString()))
                .WhereIF(!SqlFunc.IsNullOrEmpty(powerid), (v, b, s, t, o, d) =>powerid.Contains(s.PowerInfo_id.ToString()))
                .WhereIF(narrayNo == 0, (v, b, s, t, o, d) => d.TagName == tagname)
                .WhereIF(narrayNo != 0, $"d.TagName LIKE '{tagname.Substring(0, tagname.Length - 1) + "_"}'")
                .Select((v, b, s, t, o, d) => new { v.Id, PowerName = t.StationName, o.OrganizationName, v.StationName, b.StationBranchName, d.RealValue }).ToList();

            var timestamp = DbContext.Db.Queryable<ValueDesc>().Where(s => s.TagName == "TIMESTAMP").Select(s => new { s.VpnUser_id, s.RealValue }).ToList();
            List<StationRankListDto> stationRankListDtoList = new List<StationRankListDto>();
            foreach (var item in stationRankList)
            {
                foreach (var time in timestamp.Where(s => s.VpnUser_id == item.Id))
                {
                    StationRankListDto stationRankListDto = new StationRankListDto() { Id = item.Id, PowerName = item.PowerName, OrganizationName = item.OrganizationName, StationName = item.StationName, NarryName = item.StationBranchName, RealValue = item.RealValue, TimeStamp = time.RealValue };
                    stationRankListDtoList.Add(stationRankListDto);
                }
            }
            if (ordertype == "asc" || ordertype == "ASC")
            {
                stationRankListDtoList = stationRankListDtoList.OrderBy(s => s.RealValue).ToList();
            }
            else
            {
                stationRankListDtoList = stationRankListDtoList.OrderByDescending(s => s.RealValue).ToList();
            }
            return stationRankListDtoList;
        }
        #endregion

        #region 水压图
        /// <summary>
        /// 水压图
        /// </summary>
        /// <param name="pid">热源id</param>
        /// <returns></returns>
        public List<CustomParamChartModel> GetStationTempPressData4Stations(int pid)
        {
            var list = new List<CustomParamChartModel>();
            var lookUp = new Dictionary<int, CustomParamChartModel>();

            const string sql = @"SELECT
                    v.Id VpnUserId,
                	v.StationName,
                   	s.TerrainValue,
	                s.PipelineDistance,
                    vd.Id,
                    vd.VpnUser_id VpnUserId,
                	vd.TagName,
                	vd.AiDesc,
                	vd.Unit,
                	vd.RealValue
                FROM
                	VpnUser v
                JOIN Station s ON v.id = s.VpnUser_id
                JOIN ValueDesc vd ON v.id = vd.VpnUser_id
                AND vd.NarrayNo = 0
                AND AiType = 'ai'
                AND Unit <> ''
                WHERE
                    s.PowerInfo_id=@pid and
                	v.IsValid = 1
                AND v.StationStandard <= 3
                AND vd.TagName IN (
                	'pri_press_s',
                	'pri_press_r',
                	'pri_temp_r',
                	'pri_temp_s'
                        ) order by s.PipelineDistance";

            return DbContext.Db.Ado.SqlQuery<CustomParamChartModel>(sql, new SugarParameter[]{
                new SugarParameter("@pid", pid)});

        }

        #endregion

    }

    /// <summary>
    /// 全网平衡
    /// </summary>
    public class VirtualNetConfigExt : VirtualNetConfig
    {
        /// <summary>
        /// 控制策略名称
        /// </summary>
        public string ControlName
        {
            get; set;
        }
    }

    /// <summary>
    /// 水压图
    /// </summary>
    public class CustomParamChartModel
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public int VpnUserId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 地势
        /// </summary>
        public double TerrainValue { get; set; }

        /// <summary>
        /// 管道距离
        /// </summary>
        public double PipelineDistance { get; set; }

        /// <summary>
        /// TagName
        /// </summary>  
        public string TagName { get; set; }

        /// <summary>
        /// 采集量描述
        /// </summary>
        public string AiDesc { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public string RealValue { get; set; }

    }
}
