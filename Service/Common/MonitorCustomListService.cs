using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Common;

namespace THMS.Core.API.Service.Monitor
{
    /// <summary>
    /// 
    /// </summary>
    public class MonitorCustomListService : DbContext.DbContextSqlSugar
    {
        /// <summary>
        /// 自定义列表用户配置信息
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public async Task<List<UserConfig>> SelMonitorCustomList(int listType)
        {
            //自定义列表配置项数据
            List<UserConfig> stationDispositionList = await Db.Queryable<UserConfig>().Where(s => s.ListType == listType).ToListAsync();
            return stationDispositionList;
        }

        /// <summary>
        /// 自定义列表参数配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<StationPra> SelStationPra()
        {
            //获取自定义列表参数
            var stationPraList = await Db.Queryable<StandardParameter>().Where(s => s.NarrayNo == 0 || s.NarrayNo == 1).OrderBy(s => s.AiType).OrderBy(s => s.ValueSequence).ToListAsync();
            ////定义公共参数集合
            //List<StandardParameter> stationPublicPra = new List<StandardParameter>();
            ////定义机组参数集合
            //List<StandardParameter> stationBranchPra = new List<StandardParameter>();
            //stationPublicPra = stationPra.Where(s => s.NarrayNo == 0).ToList();
            //stationBranchPra = stationPra.Where(s => s.NarrayNo == 1).ToList();

            //绑定公共参数、机组参数数据
            StationPra stationPra = new StationPra() { stationPublicPra = stationPraList.Where(s => s.NarrayNo == 0).ToList(), stationBranchPra = stationPraList.Where(s => s.NarrayNo == 1).ToList() };
            return stationPra;
        }

        /// <summary>
        /// 自定义列表换热站信息
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns></returns>
        public async Task<List<StationInfo>> SelStationInfo(string searchName)
        {
            var monitorCustomlist = await Db.Queryable<VpnUser, Organization, Station, DicDetail, Dic, VpnUser>((v, o, s, de, d, p) => new object[] { JoinType.Left, v.Organization_id == o.Id, JoinType.Left, v.Id == s.VpnUser_id, JoinType.Left, s.HeatType == de.Value, JoinType.Left, de.Dic_id == d.Id, JoinType.Left, p.Id == s.PowerInfo_id }).Where((v, o, s, de, d, p) => d.DicName == "供暖类型").WhereIF(!string.IsNullOrEmpty(searchName), (v, o, s, de, d, p) => SqlFunc.Contains(v.StationSabb, searchName)).Select((v, o, s, de, d, p) => new StationInfo { VpnUserId = v.Id, StationName = v.StationName, OrganizationName = o.OrganizationName, SavePowerType = s.SavePowerType, ItemName = de.ItemName, PowerInfoName = p.StationName }).ToListAsync();
            return monitorCustomlist;
        }

        /// <summary>
        /// 自定义列表信息修改
        /// </summary>
        /// <param name="userConfigs"></param>
        /// <returns></returns>
        public bool AddCustomList(UserConfig userConfigs)
        {
            UserConfig result = Db.Saveable(userConfigs).ExecuteReturnEntity();
            return result.Id > 0 ? true : false;
        }

        /// <summary>
        /// 自定义列表信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveCustomList(int id)
        {
            int result = Db.Deleteable<UserConfig>().Where(new UserConfig() { Id = id }).ExecuteCommand();
            return result > 0 ? true : false; 
        }

        /// <summary>
        /// 机组信息查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<StationBranch>> SelStationBranchList(int id)
        {
            List<StationBranch> stationBranches = await Db.Queryable<StationBranch>().Where(s => s.VpnUser_id == id && s.StationBranchArrayNumber != 0).ToListAsync();
            return stationBranches;
        }
    }
}
