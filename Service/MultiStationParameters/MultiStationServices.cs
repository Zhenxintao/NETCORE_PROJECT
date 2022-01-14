using ApiModel;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.MultiStationParameters
{
    /// <summary>
    /// 多站参数列表
    /// </summary>
    public class MultiStationServices : DbContextSqlSugar
    {

        /// <summary>
        /// 多站参数
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="OranId"></param>
        /// <param name="sortName"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        public dynamic GetMultiStationTable(int pageIndex, int pageSize, string OranId, string sortName = "TIMESTAMP", string sortType = "DESC")
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            var TagName = configuration.GetSection("ParameterData:TagName").Value;
            List<MultiStationOriginalData> databasdelist = new List<MultiStationOriginalData>();
            List<Dictionary<string, object>> listDictTableHeader = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> listDictTableContent = new List<Dictionary<string, object>>();
            string[] strArray = new string[] { "-1" };
            string[] TagNames = new string[] { "TIMESTAMP" };
            if (TagName != null)
            {
                TagNames = TagName.Split(',');

            }
            if (OranId != null && OranId != "-1")
            {
                strArray = OranId.Split(',');
            }
            try
            {
                databasdelist = Db.Queryable<ValueDesc, VpnUser, StationBranch, StandardParameter,organization,organization>((va, vp, sb, sd,org,orgg) => new object[] {JoinType.Inner ,vp.Id == va.VpnUser_id,
                                                                                                                  JoinType.Inner, va.NarrayNo == sb.StationBranchArrayNumber && sb.VpnUser_id == va.VpnUser_id,
                                                                                                                  JoinType.Inner,va.TagName == sd.TagName,JoinType.Inner,org.Id == vp.Organization_id, JoinType.Inner,orgg.Id == org.ParentDepID })
                                                                                                                 .Where((va, vp, sb, sd, org, orgg) => vp.StationStandard == 3 && vp.IsValid == true && sb.ParentId == 0)
                                                                                                                 .WhereIF(OranId != "-1" && OranId != null, (va, vp, sb, sd, org, orgg) => SqlFunc.ContainsArray(strArray, vp.Organization_id))
                                                                                                                .Select((va, vp, sb, sd, org, orgg) => new MultiStationOriginalData
                                                                                                                {
                                                                                                                    OrganizationName = orgg.OrganizationName,
                                                                                                                    VpnUser_Id = vp.Id,
                                                                                                                    StationName = vp.StationName,
                                                                                                                    StationBranchArrayNumber = sb.StationBranchArrayNumber,
                                                                                                                    AiDesc = va.AiDesc,
                                                                                                                    TagName = va.TagName,
                                                                                                                    RealValue = va.RealValue
                                                                                                                }).ToList();
                List<VpnUser> vpnuser = Db.Queryable<VpnUser,organization,organization>((vp, org, orgg)=> new object[] { JoinType.Inner, org.Id == vp.Organization_id ,JoinType.Inner, orgg.Id == org.ParentDepID }).Where((vp, org, orgg) => vp.StationStandard == 3 && vp.IsValid == true).WhereIF(OranId != "-1" && OranId != null, vp => SqlFunc.ContainsArray(strArray, vp.Organization_id)).OrderBy("  orgg.OrganizationName COLLATE Chinese_PRC_Stroke_CS_AS_KS_WS, vp.StationName COLLATE Chinese_PRC_Stroke_CS_AS_KS_WS; ").ToList();
                List<StationBranch> sbt = Db.Queryable<StationBranch>().ToList();

                List<StandardParameter> stp = Db.Queryable<StandardParameter>().ToList();
                var dictSta = new Dictionary<string, object>
                {
                    { "prop", "StationName" },
                    { "label", "站名" },
                     { "sortable", "true" }
                };
                var dictStaBra = new Dictionary<string, object>
                {
                    { "prop", "StationBranchName" },
                    { "label", "系统" }
                };
                var dictStaTime = new Dictionary<string, object>
                {
                    { "prop", "TIMESTAMP" },
                    { "label", "采集时间" },
                    { "width", "180" },
                    { "sortable", "true" }
                };
                listDictTableHeader.Add(dictSta);
                listDictTableHeader.Add(dictStaBra);
                listDictTableHeader.Add(dictStaTime);
                foreach (var itemTag in TagNames)
                {
                    Dictionary<string, object> dictHeader = new Dictionary<string, object>();
                    var tag = stp.Where(m => m.TagName == itemTag).FirstOrDefault(); ;
                    if (tag != null && tag.TagName != "TIMESTAMP")
                    {
                        dictHeader.Add("prop", tag.TagName);
                        dictHeader.Add("label", tag.AiDesc + "(" + tag.Unit + ")");
                        dictHeader.Add("sortable", "true");
                        listDictTableHeader.Add(dictHeader);

                    }

                }
                //换热站
                foreach (var itemvp in vpnuser)
                {
                    //机组
                    foreach (var itemSb in sbt.Where(m => m.VpnUser_id == itemvp.Id && m.ParentId == 0 && m.StationBranchArrayNumber != 0))
                    {
                        Dictionary<string, object> dictContent = new Dictionary<string, object>
                        {
                            { "StationName", itemvp.StationName },
                            { "StationBranchName", itemSb.StationBranchName },
                            { "VpnUser_id", itemSb.VpnUser_id }
                        };
                        //参量

                        foreach (var item in TagNames)
                        {

                            var body = databasdelist.Where(s => s.VpnUser_Id == itemvp.Id && s.TagName == item && s.StationBranchArrayNumber == 0).FirstOrDefault();
                            if (body != null)
                            {
                                dictContent.Add(body.TagName, body.RealValue);
                            }
                            else
                            {
                                var itemDuo = item.Remove(item.Length - 1);
                                var body1 = databasdelist.Where(s => s.VpnUser_Id == itemvp.Id && s.TagName == itemDuo + itemSb.StationBranchArrayNumber && s.StationBranchArrayNumber == itemSb.StationBranchArrayNumber).FirstOrDefault();
                                if (body1 != null)
                                    dictContent.Add(item, body1.RealValue);
                            }

                        }
                        listDictTableContent.Add(dictContent);
                    }
                }
            }
            catch (Exception e)
            {
                return new
                {
                    Header = e.Message,
                    Content = "出错啦",
                    Total = 0

                };
            }
            var total = listDictTableContent.Count();

            var infoList = listDictTableContent.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new
            {
                Header = listDictTableHeader,
                Content = infoList,
                Total = total

            };

        }
    }


    /// <summary>
    /// 原始数据
    /// </summary>
    public class MultiStationOriginalData
    {
        public string OrganizationName { get; set; }
        public int VpnUser_Id { get; set; }
        public string StationName { get; set; }
        public int StationBranchArrayNumber { get; set; }
        public string AiDesc { get; set; }
        public string TagName { get; set; }
        public string RealValue { get; set; }
    }
}
