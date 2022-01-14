using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Monitor;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Monitor
{
    /// <summary>
    /// 时段补偿
    /// </summary>
    public class StationTimeCorrectService
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        DbContextSqlSugar DbContext = new DbContextSqlSugar();

        /// <summary>
        /// 时段补偿
        /// </summary>
        StationTimeCorrect stationtimecorrect = new StationTimeCorrect();

        /// <summary>
        /// 获取时段补偿
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<StationTimeCorrect> GetStationTimeCorrectByIds(List<int> ids = null)
        {
            var list = new List<StationTimeCorrect>();

            try
            {

                string sql = @"SELECT * from StationTimeCorrect where 1=1  ";

                if (ids != null)
                {
                    sql += " and VpnUserId in @ids";
                }

                list = DbContext.Db.Ado.SqlQuery<StationTimeCorrect>(sql, new SugarParameter[]{
                    new SugarParameter("@ids", ids)});

            }
            catch (Exception)
            {

            }

            return list;
        }

    }
}
