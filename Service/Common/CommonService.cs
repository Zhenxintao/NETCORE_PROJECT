using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;

namespace THMS.Core.API.Service.Common
{
    /// <summary>
    /// 共用方法
    /// </summary>
    public class CommonService : DbContext.DbContextSqlSugar
    {
        /// <summary>
        /// 返回Dynamic Value
        /// </summary>
        /// <param name="item">数据集合</param>
        /// <param name="tagName">tagName</param>
        /// <returns></returns>
        public object GetDynamicValue(dynamic item, string tagName)
        {
            foreach (dynamic pi in item)
            {
                object value1 = pi.Value;
                string name = pi.Key;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                //进行你想要的操作
                if (name == tagName) return value1;
            }
            return 0;
        }

        #region 热源树
        /// <summary>
        /// 热源树
        /// </summary>
        /// <returns></returns>
        public List<PowerInfoTree> GetPowerTrees()
        {
            var list = GetPowers();

            var root = new PowerInfoTree() { label = "所有热源", ParentId = -1, value = 0 };

            var result = new List<PowerInfoTree>();

            GeneralPowerTree(list, root, result);
            root.children = result;
            return new List<PowerInfoTree>() { root };
        }

        void GeneralPowerTree(IEnumerable<PowerInfoTree> source, PowerInfoTree root, List<PowerInfoTree> result)
        {
            var children = source.Where(m => m.ParentId == root.value).ToList();

            foreach (var item in children)
            {
                var child = new PowerInfoTree()
                {
                    value = item.value,

                    label = item.label,
                    children = new List<PowerInfoTree>()
                };

                if (item.ParentId == 0)
                {
                    result.Add(child);
                }
                else
                {
                    if (child.children.Count == 0)
                    {
                        child.children = null;
                    }
                    root.children.Add(child);
                }

                GeneralPowerTree(source, child, result);
            }
        }

        /// <summary>
        /// 获取热源树
        /// </summary>
        /// <returns></returns>
        public List<PowerInfoTree> GetPowers()
        {
            var list = new List<PowerInfoTree>() { };
            list = Db.Ado.SqlQuery<PowerInfoTree>(@"SELECT P.Id AS value,
                                                           V.StationName label,
                                                           P.ParentID
                                                    FROM dbo.VpnUser V
                                                        JOIN dbo.PowerInfo P
                                                            ON V.Id = P.VpnUser_id
                                                    WHERE 1 = 1
                                                          AND V.IsValid = 1
                                                          AND V.StationStandard IN ( 98, 99 );").ToList();
            return list;
        }

        #endregion
        #region 换热站树
        /// <summary>
        /// 换热站树
        /// </summary>
        /// <returns></returns>
        public List<object> GetStationTrees()
        {
            List<VpnUser> vpnUsers = Db.Queryable<VpnUser>().Where(s => s.StationStandard < 98).ToList();
            var stationBranches = Db.Queryable<StationBranch>().Where(s => s.StationBranchArrayNumber != 0).ToList();
            List<object> stationTreesList = new List<object>();
            foreach (var vpnuser in vpnUsers)
            {
                StationTree stationTree = new StationTree();
                stationTree.Name = vpnuser.StationName;
                stationTree.Id = vpnuser.Id;
                stationTree.children = stationBranches.Where(s => s.VpnUser_id == vpnuser.Id).Select(s => new StationTree { Id = s.StationBranchArrayNumber, Name = s.StationBranchName }).ToList();
                stationTreesList.Add(stationTree);
            }
            return stationTreesList;
        }



        /// <summary>
        /// 获取热源树
        /// </summary>
        /// <returns></returns>
        public List<StationTree> GetStation()
        {
            var list = new List<StationTree>() { };
            list = Db.Ado.SqlQuery<StationTree>(@"SELECT v.Id,
       s.VpnUser_id,
       s.StationBranchArrayNumber,
       v.StationName,
       s.StationBranchName FROM dbo.VpnUser v LEFT	JOIN dbo.StationBranch s ON v.Id = s.VpnUser_id WHERE s.StationBranchArrayNumber!=0 AND	v.StationStandard <98;").ToList();
            return list;
        }

        #endregion
        #region 组织结构树
        /// <summary>
        /// 得到组织结构列表
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetOrganizations()
        {
            var list = new List<Organization>();
            list = Db.Ado.SqlQuery<Organization>(@"SELECT * from Organization where IsValid = 1").ToList();
            return list;
        }

        /// <summary>
        ///得到组织结构树
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetOrganizationTree()
        {
            List<Organization> list = GetOrganizations();
            var rootNodes = list.Where(m => m.DepLevel == 1);

            var resultroot = new List<Organization>();

            foreach (var item in rootNodes)
            {
                var result = new List<Organization>();

                var root = new Organization()
                {
                    Id = item.Id,
                    OrganizationCode = item.OrganizationCode,
                    OrganizationName = item.OrganizationName,
                    OrganizationDesc = item.OrganizationDesc,
                    CreateTime = item.CreateTime,
                    IsValid = item.IsValid,
                    ParentDepID = item.ParentDepID,
                    CreateUser = item.CreateUser,
                    ClassList = item.ClassList,
                };

                GeneralOrganizationTree(list, root, result);

                root.children.AddRange(result);

                resultroot.Add(root);
            }
            return resultroot;

        }

        void GeneralOrganizationTree(List<Organization> allList, Organization node, List<Organization> result)
        {
            var children = allList.Where(m => m.ParentDepID == node.Id).ToList();

            if (children.Count > 0)
            {
                foreach (var item in children)
                {
                    var child = new Organization()
                    {
                        Id = item.Id,
                        OrganizationCode = item.OrganizationCode,
                        OrganizationName = item.OrganizationName,
                        OrganizationDesc = item.OrganizationDesc,
                        CreateTime = item.CreateTime,
                        IsValid = item.IsValid,
                        ParentDepID = item.ParentDepID,
                        CreateUser = item.CreateUser,
                        ClassList = item.ClassList,
                        children = new List<Organization>()
                    };
                    //if (child.children.Count == 0)
                    //{
                    //    child.children = null;
                    //}
                    node.children.Add(child);
                    GeneralOrganizationTree(allList, child, result);
                }
            }
        }
        #endregion

        /// <summary>
        /// 热源基础信息
        /// </summary>
        /// <returns></returns>
        public dynamic GetpowerInfos()
        {
            var list = Db.Queryable<VpnUser, PowerInfo>((v, p) => new object[] { JoinType.Right, v.Id == p.VpnUser_id }).Where((v, p) => v.StationStandard == 99 && v.IsValid == true).Select((v, p) => new { vpnuserid = v.Id, v.StationName }).ToList();
            return list;
        }

        /// <summary>
        /// 返回Di点含义
        /// </summary>
        /// <param name="RealValue"></param>
        /// <param name="ZeroMean"></param>
        /// <param name="OneMean"></param>
        /// <returns></returns>
        public string GetDIMean(string RealValue, string ZeroMean, string OneMean)
        {
            var msg = "未定义";

            switch (RealValue)
            {
                case "0":
                    msg = ZeroMean;
                    break;
                case "1":
                    msg = OneMean;
                    break;
                default:
                    break;
            }
            return msg;
        }

        /// <summary>
        /// 获取换热站基础信息
        /// </summary>
        /// <param name="typeid">类型id</param>
        /// <returns></returns>
        public List<VpnUser> GetStationList(int typeid)
        {
            List<VpnUser> vpnUsers = Db.Queryable<VpnUser>().WhereIF(typeid == -1, "1=1").WhereIF(typeid == 3, s => !new int[] { 98, 99 }.Contains(s.StationStandard)).WhereIF(typeid == 98, s => s.StationStandard == 98).WhereIF(typeid == 99, s => s.StationStandard == 99).Where(s => s.IsValid == true).ToList();
            return vpnUsers;
        }
    }
}
