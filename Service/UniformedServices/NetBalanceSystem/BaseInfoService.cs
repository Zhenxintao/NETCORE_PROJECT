using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem;
using THMS.Core.API.Models.UniformedServices.NetBalanceSystem.SearchDto;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.UniformedServices.NetBalanceSystem
{
    /// <summary>
    /// 基础信息实现
    /// </summary>
    public class BaseInfoService : DbContextMySqlEw
    {
        /// <summary>
        /// 查询小区信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryCommunityList(CommunitySearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<Sys_Community>()
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (sys) => sys.Community_id == search.Community_id)
                .WhereIF(search.Id > 0, (sys) => sys.Id == search.Id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (sys) => sys.CommunityName.Contains(search.CommunityName) || sys.CommunitySabb.Contains(search.CommunityName))
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
        /// 查询楼栋信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryBuildingList(BuildingSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<Sys_Building, Sys_Community>((b, c) => new object[] { JoinType.Left, b.Community_id == c.Community_id })
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (b, c) => c.Community_id == search.Community_id)
                .WhereIF(search.Id > 0, (b, c) => b.Id == search.Id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (b, c) => c.CommunityName.Contains(search.CommunityName) || c.CommunitySabb.Contains(search.CommunityName))
                .WhereIF(!string.IsNullOrEmpty(search.Building_id) && search.Building_id != "string", (b, c) => b.Building_id == search.Building_id)
                .WhereIF(search.BuildingNum > 0, (b, c) => b.BuildingNum == search.BuildingNum)
                .WhereIF(!string.IsNullOrEmpty(search.BuildingName) && search.BuildingName != "string", (b, c) => b.BuildingName.Contains(search.BuildingName))
                .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "id asc" : search.SortColumn + " " + search.SortType)
                .Select((b, c) => new { Id = b.Id, Building_id = b.Building_id, Community_id = b.Community_id, CommunityName = c.CommunityName, BuildingNum = b.BuildingNum, BuildingName = b.BuildingName, BuiltArea = b.BuiltArea, ChargeArea = b.ChargeArea })
            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);

            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return JsonConvert.SerializeObject(resultList);
        }

        /// <summary>
        /// 查询单元信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryUnitNoList(UnitNoSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<Sys_UnitNo, Sys_Building, Sys_Community>((u, b, c) => new object[] {
                JoinType.Left, u.Building_id==b.Building_id,
                JoinType.Left, b.Community_id == c.Community_id
            })
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (u, b, c) => c.Community_id == search.Community_id)
                .WhereIF(search.Id > 0, (u, b, c) => u.Id == search.Id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (u, b, c) => c.CommunityName.Contains(search.CommunityName) || c.CommunitySabb.Contains(search.CommunityName))
                .WhereIF(!string.IsNullOrEmpty(search.Building_id) && search.Building_id != "string", (u, b, c) => b.Building_id == search.Building_id)
                .WhereIF(search.BuildingNum > 0, (u, b, c) => b.BuildingNum == search.BuildingNum)
                .WhereIF(!string.IsNullOrEmpty(search.BuildingName) && search.BuildingName != "string", (u, b, c) => b.BuildingName.Contains(search.BuildingName))
                .WhereIF(!string.IsNullOrEmpty(search.UnitNo_id) && search.UnitNo_id != "string", (u, b, c) => u.UnitNo_id == search.UnitNo_id)
                .WhereIF(search.UnitNoNum > 0, (u, b, c) => u.UnitNoNum == search.UnitNoNum)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNoName) && search.UnitNoName != "string", (u, b, c) => u.UnitNoName.Contains(search.UnitNoName))
                .Select((u, b, c) => new { Id = u.Id, UnitNo_id = u.UnitNo_id, Building_id = b.Building_id, BuildingName = b.BuildingName, Community_id = c.Community_id, CommunityName = c.CommunityName, UnitNoNum = u.UnitNoNum, UnitNoName = u.UnitNoName, BuiltArea = u.BuiltArea, ChargeArea = u.ChargeArea })
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
        /// 查询住户信息
        /// </summary>
        /// <param name="search">查询类</param>
        /// <returns></returns>
        public string queryResidentUserList(ResidentUserSearch search)
        {
            RefAsync<int> total = 0;
            var list = DbMysql.Queryable<Sys_ResidentUser, Sys_UnitNo, Sys_Building, Sys_Community, DicDetail, DicDetail, DicDetail, DicDetail, DicDetail>((r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => new object[] {
                JoinType.Left,r.UnitNo_id==u.UnitNo_id,
                JoinType.Left, u.Building_id==b.Building_id,
                JoinType.Left, b.Community_id == c.Community_id,
                JoinType.Left,r.BuildType==BuildType.Id,
                JoinType.Left,r.PaymentType==PaymentType.Id,
                JoinType.Left,r.HeatingType==HeatingType.Id,
                JoinType.Left,r.IsHeatPreservationType==IsHeatPreservationType.Id,
                JoinType.Left,r.HouseType==HouseType.Id
            })
                .WhereIF(!string.IsNullOrEmpty(search.Community_id) && search.Community_id != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => c.Community_id == search.Community_id)
                .WhereIF(search.Id > 0, (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.Id == search.Id)
                .WhereIF(!string.IsNullOrEmpty(search.CommunityName) && search.CommunityName != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => c.CommunityName.Contains(search.CommunityName) || c.CommunitySabb.Contains(search.CommunityName))
                .WhereIF(!string.IsNullOrEmpty(search.Building_id) && search.Building_id != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => b.Building_id == search.Building_id)
                .WhereIF(search.BuildingNum > 0, (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => b.BuildingNum == search.BuildingNum)
                .WhereIF(!string.IsNullOrEmpty(search.BuildingName) && search.BuildingName != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => b.BuildingName.Contains(search.BuildingName))
                .WhereIF(!string.IsNullOrEmpty(search.UnitNo_id) && search.UnitNo_id != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseTypec) => u.UnitNo_id == search.UnitNo_id)
                .WhereIF(search.UnitNoNum > 0, (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => u.UnitNoNum == search.UnitNoNum)
                .WhereIF(!string.IsNullOrEmpty(search.UnitNoName) && search.UnitNoName != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => u.UnitNoName.Contains(search.UnitNoName))
                .WhereIF(!string.IsNullOrEmpty(search.ResidentUser_id) && search.ResidentUser_id != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.ResidentUser_id == search.ResidentUser_id)
                .WhereIF(!string.IsNullOrEmpty(search.UserCard) && search.UserCard != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.UserCard == search.UserCard)
                .WhereIF(!string.IsNullOrEmpty(search.UserName) && search.UserName != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.UserName.Contains(search.UserName))
                .WhereIF(!string.IsNullOrEmpty(search.NetworkUserName) && search.NetworkUserName != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.NetworkUserName.Contains(search.NetworkUserName))
                .WhereIF(!string.IsNullOrEmpty(search.RoomNum) && search.RoomNum != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.RoomNum == search.RoomNum)
                .WhereIF(search.LayerNum > 0, (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.LayerNum == search.LayerNum)
                .WhereIF(!string.IsNullOrEmpty(search.UserPhone) && search.UserPhone != "string", (r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => r.UserPhone == search.UserPhone)
                .Select((r, u, b, c, BuildType, PaymentType, HeatingType, IsHeatPreservationType, HouseType) => new { Id = r.Id, UnitNo_id = u.UnitNo_id, Building_id = b.Building_id, BuildingName = b.BuildingName, Community_id = c.Community_id, CommunityName = c.CommunityName, UnitNoName = u.UnitNoName, ResidentUser_id = r.ResidentUser_id, UserCard = r.UserCard, UserName = r.UserName, NetworkUserName = r.NetworkUserName, RoomNum = r.RoomNum, LayerNum = r.LayerNum, UserPhone = r.UserPhone, BuiltArea = r.BuiltArea, ChargeArea = r.ChargeArea, HeatingState = r.HeatingState, IsDelete = r.IsDelete, ChargeState = r.ChargeState, BuildType = BuildType.ItemName, PaymentType = PaymentType.ItemName, HeatingType = HeatingType.ItemName, IsHeatPreservationType = IsHeatPreservationType.ItemName, HouseType = HouseType.ItemName, Remarks = r.Remarks })
                .OrderBy(string.IsNullOrEmpty(search.SortColumn) || string.IsNullOrEmpty(search.SortType) || search.SortColumn == "string" || search.SortType == "string" ? "id asc" : search.SortColumn + " " + search.SortType)
            .ToPageListAsync(search.PageIndex == 0 ? 1 : search.PageIndex, search.PageSize == 0 ? 30 : search.PageSize, total);

            var resultList = new
            {
                Total = total.Value,
                Data = list.Result
            };
            return JsonConvert.SerializeObject(resultList);
        }
    }
}
