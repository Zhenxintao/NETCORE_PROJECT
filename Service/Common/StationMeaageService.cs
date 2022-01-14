using ApiModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class StationMessageService : DbContextSqlSugar
    {
        /// <summary>
        /// 机组下拉框
        /// </summary>
        /// <returns></returns>
        public object SelStationBranch()
        {
            var result = Db.Queryable<StationBranch>().GroupBy(s => s.StationBranchArrayNumber).GroupBy(s => s.StationBranchName).Select(s => new { s.StationBranchArrayNumber, s.StationBranchName }).ToList();
            return result;
        }

        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object SelVpnuser(string value)
        {
            var result = Db.Queryable<VpnUser>().Where(s => SqlFunc.Contains(s.StationSabb, value)).Select(s => new { id = s.Id, value = s.StationName, StationSabb = s.StationSabb, StationStandard = s.StationStandard }).ToList();
            return result;
        }

        //热源信息返回



    }
}
