using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Energy
{
    public class MeterDataEnergyService : DbContextSqlSugar
    {
        /// <summary>
        /// 查询耗热量曲线，柱状图，表格数据。
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<MeterDataListDto> ListmeterDataList(SearchDto dto)
        {
            List<MeterDataListDto> resultList = new List<MeterDataListDto>();
            var sb = new StringBuilder();
            var count=0;
            string EnergyName = "";
            if (dto.StartTime.HasValue && dto.EndTime.HasValue)
            {
                sb.Append($"MeterDate>='{dto.StartTime?.ToString("yyyy-MM-dd")}' AND MeterDate<'{dto.EndTime?.ToString("yyyy-MM-dd")}'");
            }
            if (dto.PowerInfoid.HasValue)
            {
                sb.Append($"AND st.PowerInfo_id='{dto.PowerInfoid}' ");
            }
            if (dto.Organizationid.HasValue)
            {
                sb.Append($"AND s.Organization_id='{dto.Organizationid}' ");
            }
            if (dto.VpnUserid.HasValue)
            {
                sb.Append($"AND m.Vpnuser_id='{dto.VpnUserid}' ");
            }
            //            var meterList = Db.SqlQueryable<dynamic>($@"SELECT CONVERT(VARCHAR(100), m.MeterDate, 23) AS 'Date',SUM([m].[Total]) AS [Total] , (SUM([m].[Total]) / AVG(m.Area) * 10000) AS [Target],AVG(m.Area) AS 'Area',DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(VARCHAR(100), m.MeterDate, 23))  AS 'TickDate'  FROM dbo.MeterData  m LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.Vpnuser_id
            //WHERE 1=1 AND {sb} AND m.MeterType='{dto.EnergyType}' 
            // GROUP BY CONVERT(VARCHAR(100), MeterDate, 23)  ").ToPageList(dto.Page,dto.PageSize,ref count);
            var meterList = Db.SqlQueryable<dynamic>($@"SELECT CONVERT(VARCHAR(100), m.MeterDate, 23) AS 'Date',SUM([m].[Total]) AS [Total] , (SUM([m].[Total]) / AVG(m.Area) * 10000) AS [Target],AVG(m.Area) AS 'Area',DATEDIFF(S,'1970-01-01 00:00:00', CONVERT(VARCHAR(100), m.MeterDate, 23))  AS 'TickDate'  FROM dbo.MeterData  m LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.Vpnuser_id
WHERE 1=1 AND {sb} AND m.MeterType='{dto.EnergyType}' 
 GROUP BY CONVERT(VARCHAR(100), MeterDate, 23) ").OrderBy("Date").ToList();
            if (dto.EnergyType == 1)
            {
                EnergyName = "水能耗";
           }
            else if (dto.EnergyType==2)
            {
                EnergyName = "电能耗";
            }
            else if (dto.EnergyType == 3)
            {
                EnergyName = "热能耗";
            }
            
            List<EnergyData> datalist = new List<EnergyData>();
            foreach (var item in meterList)
            {
              var en = new EnergyData  {
                    NcapTime = item.Date,
                    Tick=Convert.ToString(item.TickDate),
                    EnergyToTal = item.Total,
                    EnergyTarget = item.Target                  
                };
                datalist.Add(en);
            }

            var meter = new MeterDataListDto
            {
                EnergyName=EnergyName,
                Area= meterList[0].Area,
                EnergyDataList =datalist
            };
            resultList.Add(meter);

            return resultList;
        }

        /// <summary>
        /// 两日能耗对比，同比，环比柱状图
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<object> YtdaymeterList(SearchDto dto)
        {
            dto.StartTime = DateTime.Now;
            dto.EndTime = DateTime.Now.AddDays(1);
            var sb = new StringBuilder();
            if (dto.PowerInfoid.HasValue)
            {
                sb.Append($"AND st.PowerInfo_id='{dto.PowerInfoid}' ");
            }
            if (dto.Organizationid.HasValue)
            {
                sb.Append($"AND s.Organization_id='{dto.Organizationid}' ");
            }
            if (dto.VpnUserid.HasValue)
            {
                sb.Append($"AND m.Vpnuser_id='{dto.VpnUserid}' ");
            }
            string sql = $@"SELECT CONVERT
	( DECIMAL ( 18, 2 ), a.Total ) AS NTotal,
	Round( a.Target, 2 ) AS NTarget,
	a.Date AS NDate,
	CONVERT ( DECIMAL ( 18, 2 ), a.Fuhe ) AS NFuhe,
	CONVERT ( DECIMAL ( 18, 2 ), b.OldTotal ) AS OTotal,
	ROUND(b.OldTarget, 2 ) AS OTarget,
	b.OldDate AS ODate,
	CONVERT ( DECIMAL ( 18, 2 ), b.OldFuhe ) AS OFuhe,
	ROUND(b.OldTotalTong,2) AS OldTotalTong,
	ROUND(b.OldTargetTong,2)AS OldTargetTong,
	ROUND(b.OldFuheTong,2)AS OldFuheTong,
	ROUND(b.OldTotalHuan,2) AS OldTotalHuan,
	ROUND(b.OldTargetHuan,2)AS OldTargetHuan,
	ROUND(b.OldFuheHuan,2)AS OldFuheHuan  FROM
(
SELECT SUM(m.Total) AS Total,(SUM([m].[Total]) / AVG(m.Area) ) AS [Target],MAX(CONVERT(VARCHAR(100), m.MeterDate, 23))AS 'Date', 
(SUM([m].[Total])/24*1000000000/3600/AVG(m.Area)) AS 'Fuhe'
  FROM dbo.MeterData  m LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1  {sb} AND m.MeterType='{dto.EnergyType}'  AND  MeterDate>='{dto.StartTime?.ToString("yyyy-MM-dd").ToString()}' AND	MeterDate<'{dto.EndTime?.ToString("yyyy-MM-dd")}'
)a ,
(
SELECT SUM(m.Total) AS OldTotal,(SUM([m].[Total]) / AVG(m.Area) ) AS OldTarget,MAX(CONVERT(VARCHAR(100), m.MeterDate, 23))AS 'OldDate', 
(SUM([m].[Total])/24*1000000000/3600/AVG(m.Area)) AS 'OldFuhe',
((SUM(m.Total)-(SELECT SUM(m.Total) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND  
MeterDate>=DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}')) AND	MeterDate<DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))
/
(SELECT SUM(m.Total) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb}  AND m.MeterType='{dto.EnergyType}'  AND 
 MeterDate>=DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') ) AND	MeterDate<DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))AS 'OldTotalTong',

 ((SUM([m].[Total]) / AVG(m.Area)-(SELECT SUM([m].[Total]) / AVG(m.Area) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb}  AND m.MeterType='{dto.EnergyType}'  AND  
MeterDate>=DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}')) AND	MeterDate<DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))
/
(SELECT SUM([m].[Total]) / AVG(m.Area) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb}  AND m.MeterType='{dto.EnergyType}'  AND 
 MeterDate>=DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') ) AND	MeterDate<DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}')))) AS 'OldTargetTong',
  
 (((SUM([m].[Total])/24*1000000000/3600/AVG(m.Area))-(SELECT (SUM([m].[Total])/24*1000000000/3600/AVG(m.Area)) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND  
MeterDate>=DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}')) AND	MeterDate<DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))
/
(SELECT (SUM([m].[Total])/24*1000000000/3600/AVG(m.Area)) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND 
 MeterDate>=DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') ) AND	MeterDate<DATEADD(MONTH,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}')))) AS 'OldFuheTong',
 ((SUM(m.Total)-(SELECT SUM(m.Total) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND  
MeterDate>=DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}')) AND	MeterDate<DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))
/
(SELECT SUM(m.Total) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND 
 MeterDate>=DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') ) AND	MeterDate<DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))AS 'OldTotalHuan',

 ((SUM([m].[Total]) / AVG(m.Area)-(SELECT SUM([m].[Total]) / AVG(m.Area) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND  
MeterDate>=DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}')) AND	MeterDate<DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))
/
(SELECT SUM([m].[Total]) / AVG(m.Area) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND 
 MeterDate>=DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') ) AND	MeterDate<DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}')))) AS 'OldTargetHuan',
  
 (((SUM([m].[Total])/24*1000000000/3600/AVG(m.Area))-(SELECT (SUM([m].[Total])/24*1000000000/3600/AVG(m.Area)) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND  
MeterDate>=DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}')) AND	MeterDate<DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}'))))
/
(SELECT (SUM([m].[Total])/24*1000000000/3600/AVG(m.Area)) FROM dbo.MeterData  m 
LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND 
 MeterDate>=DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') ) AND	MeterDate<DATEADD(YEAR,-1,DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}')))) AS 'OldFuheHuan'
 	
  FROM dbo.MeterData  m LEFT JOIN dbo.VpnUser s ON	m.Vpnuser_id=s.Id LEFT JOIN dbo.Station st ON m.Vpnuser_id=st.VpnUser_id
WHERE 1=1 {sb} AND m.MeterType='{dto.EnergyType}'  AND  MeterDate>= DATEADD(DAY,-1,'{dto.StartTime?.ToString("yyyy-MM-dd")}') 
AND	MeterDate<DATEADD(DAY,-1,'{dto.EndTime?.ToString("yyyy-MM-dd")}') 
)b";
            var newList = Db.SqlQueryable<dynamic>(sql).ToList();
            return newList;

            
        }

        /// <summary>
        /// 今年、去年总能耗数据及增长值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public object SumEnergyList(SearchDto dto)
        {
            var heatCycleList = Db.Queryable<HeatCycle>().OrderBy(s=>s.Name,OrderByType.Desc).ToList();

            //今年累计总能耗
            var tnewList = Db.Queryable<MeterData,VpnUser,Station>((m,s,st)=>new object[] { JoinType.Left,m.Vpnuser_id==s.Id,JoinType.Left,m.Vpnuser_id==st.VpnUser_id})
                .WhereIF(SqlFunc.HasNumber(dto.EnergyType), (m, s, st) => m.MeterType == dto.EnergyType)
                .WhereIF(SqlFunc.HasNumber(dto.VpnUserid), (m, s, st) => m.Vpnuser_id == dto.VpnUserid)
                .WhereIF(SqlFunc.HasNumber(dto.PowerInfoid), (m, s, st) => st.PowerInfo_id == dto.PowerInfoid)
                .WhereIF(SqlFunc.HasNumber(dto.Organizationid), (m, s, st) => s.Organization_id == dto.Organizationid)
                .Where((m,s,st) => m.MeterDate >= heatCycleList[0].StartDate && m.MeterDate < heatCycleList[0].EndDate ).Select((m,s,st)=>new { NowTotal =SqlFunc.AggregateSum(m.Total)}).First();

            //去年累计总能耗
            var yoldList = Db.Queryable<MeterData, VpnUser, Station>((m, s, st) => new object[] { JoinType.Left, m.Vpnuser_id == s.Id, JoinType.Left, m.Vpnuser_id == st.VpnUser_id })
                    .WhereIF(SqlFunc.HasNumber(dto.EnergyType), (m, s, st) => m.MeterType == dto.EnergyType)
                .WhereIF(SqlFunc.HasNumber(dto.VpnUserid), (m, s, st) => m.Vpnuser_id == dto.VpnUserid)
                .WhereIF(SqlFunc.HasNumber(dto.PowerInfoid), (m, s, st) => st.PowerInfo_id == dto.PowerInfoid)
                .WhereIF(SqlFunc.HasNumber(dto.Organizationid), (m, s, st) => s.Organization_id == dto.Organizationid)
                .Where((m, s, st) => m.MeterDate >= heatCycleList[1].StartDate && m.MeterDate < heatCycleList[1].EndDate ).Select((m, s, st) => new { YoldTotal = SqlFunc.AggregateSum(m.Total) }).First();
           decimal increase= (tnewList.NowTotal - yoldList.YoldTotal) / yoldList.YoldTotal;

            return new { NowTotal = Math.Round(tnewList.NowTotal, 2), YoldTotal = Math.Round(yoldList.YoldTotal, 2), InCrease = Math.Round(increase) };
        }


        public PlanEnergyDto PlanEnergyList(SearchDto dto)
        {

            var heatCycleList = Db.Queryable<HeatCycle>().OrderBy(s => s.Name, OrderByType.Desc).First();
            var areHot = Db.Queryable<VpnUser>().WhereIF(SqlFunc.HasNumber(dto.VpnUserid), s => s.Id == dto.VpnUserid)
                .Where(s=>s.IsValid==true && s.StationStandard<10).Select(s=>new {StationHotArea= SqlFunc.AggregateSum(s.StationHotArea) }).First();
            //今年累计总能耗,总单耗 
            var tnewList = Db.Queryable<MeterData, VpnUser, Station>((m, s, st) => new object[] { JoinType.Left, m.Vpnuser_id == s.Id, JoinType.Left, m.Vpnuser_id == st.VpnUser_id })
                .WhereIF(SqlFunc.HasNumber(dto.VpnUserid), (m, s, st) => m.Vpnuser_id == dto.VpnUserid)
                .WhereIF(SqlFunc.HasNumber(dto.PowerInfoid), (m, s, st) => st.PowerInfo_id == dto.PowerInfoid)
                .WhereIF(SqlFunc.HasNumber(dto.Organizationid), (m, s, st) => s.Organization_id == dto.Organizationid)
                .Where((m, s, st) => m.MeterDate >= heatCycleList.StartDate && m.MeterDate < heatCycleList.EndDate && m.MeterType == dto.EnergyType && m.Total<1000)
                .Select((m, s, st) => new { NowTotal = SqlFunc.AggregateSum(m.Total), NowTarget = SqlFunc.AggregateSum(m.Target) }).First();
            if (dto.VpnUserid==-1 && dto.Organizationid==-1 && dto.PowerInfoid==-1)
            {

                var meterTargetConfig = Db.Queryable<MeterTargetConfig>().Where(s => s.HeatCycle_id == heatCycleList.Id).First();
                //计划耗热量GJ
                //var a = heatCycleList.EndDate.Subtract(heatCycleList.StartDate).Days;
                var planHeatTotal= meterTargetConfig.Heat_ComTarget * 3.6M / Convert.ToDecimal(Math.Pow(10, 6))*24* heatCycleList.EndDate.Subtract(heatCycleList.StartDate).Days* areHot.StationHotArea;
                //总项目完成比例
                var ratioTotal = (planHeatTotal - tnewList.NowTotal) / planHeatTotal;
                //总项目剩余热量GJ
                var residue = planHeatTotal - tnewList.NowTotal;
                //可用天数
                var b = DateTime.Now.Subtract(heatCycleList.StartDate).Days;
               var avgnumber= tnewList.NowTotal /DateTime.Now.Subtract(heatCycleList.StartDate).Days;
                var dayNumber = residue / avgnumber;

                //总项目热指标完成比例 W/平方米
               var c= tnewList.NowTarget /24 / 3.6M * Convert.ToDecimal(Math.Pow(10, 6))/ areHot.StationHotArea;

                var ratioTarget = (meterTargetConfig.Heat_ComTarget- (tnewList.NowTarget /24 / 3.6M * Convert.ToDecimal(Math.Pow(10, 6)))/ areHot.StationHotArea) / meterTargetConfig.Heat_ComTarget;

                return new PlanEnergyDto { NowTotal= Math.Round(tnewList.NowTotal,2),NowTarget= Math.Round(tnewList.NowTarget,2),PlanTotal= Math.Round(planHeatTotal,2), PlanTarget= Math.Round(meterTargetConfig.Heat_ComTarget,2),RatioTarget= Math.Round(ratioTarget,2), RatioTotal= Math.Round(ratioTotal,2), ResidueTotal= Math.Round(residue,2), UseDays= Math.Round(dayNumber,0) };
            }
            else
            {
                var meterTargetConfig = Db.Queryable<MeterTargetConfig,MeterTargetConfigDetail>((s,m)=>new object[] { JoinType.Left,s.Id==m.MeterTarget_id })
                    .WhereIF(SqlFunc.HasNumber(dto.VpnUserid),(s,m)=>m.VpnUser_id==dto.VpnUserid.ToString())
                    .Where((s,m) => s.HeatCycle_id == heatCycleList.Id).Select<MeterTargetConfigDetail>().First();
                //计划耗热量GJ
                var planHeatTotal = meterTargetConfig.Heat_ComTarget  * 3.6M / Convert.ToDecimal(Math.Pow(10, 6)) * 24 * heatCycleList.EndDate.Subtract(heatCycleList.StartDate).Days * areHot.StationHotArea;
                //总项目完成比例
                var ratioTotal = tnewList.NowTotal / planHeatTotal;
                //总项目剩余热量GJ
                var residue = planHeatTotal - tnewList.NowTotal;
                //可用天数
                //int i = Convert.ToInt32(DateTime.Now.Subtract(heatCycleList.StartDate));
                var avgnumber = tnewList.NowTotal / DateTime.Now.Subtract(heatCycleList.StartDate).Days;
                var dayNumber = residue / avgnumber;
                //总项目热指标完成比例 W/平方米
                var c = tnewList.NowTarget /24 / 3.6M * Convert.ToDecimal(Math.Pow(10, 6)) / areHot.StationHotArea;
                var ratioTarget = (meterTargetConfig.Heat_ComTarget - (tnewList.NowTarget/24 / 3.6M * Convert.ToDecimal(Math.Pow(10, 6))/ areHot.StationHotArea)) / meterTargetConfig.Heat_ComTarget;
                return new PlanEnergyDto { NowTotal = Math.Round(tnewList.NowTotal, 2), NowTarget = Math.Round(tnewList.NowTarget, 2), PlanTotal = Math.Round(planHeatTotal, 2), PlanTarget = Math.Round(meterTargetConfig.Heat_ComTarget, 2), RatioTarget = Math.Round(ratioTarget, 2), RatioTotal = Math.Round(ratioTotal, 2), ResidueTotal = Math.Round(residue, 2), UseDays = Math.Round(dayNumber, 0) };
            }
            
           
        }
    }
   
}
