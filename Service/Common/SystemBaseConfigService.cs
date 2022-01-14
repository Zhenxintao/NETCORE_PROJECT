using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Common
{
    /// <summary>
    /// 数值颜色配置
    /// </summary>
    public class SystemBaseConfigService
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        DbContextSqlSugar DbContext = new DbContextSqlSugar();

        /// <summary>
        /// 获取数值颜色配置表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SystemBaseConfig>> GetSystemBaseConfigList()
        {
            List<SystemBaseConfig> SystemBaseConfig = await DbContext.Db.Queryable<SystemBaseConfig>().ToListAsync();

            return SystemBaseConfig;
        }

        /// <summary>
        /// 修改数值颜色配置表
        /// </summary>
        /// <param name="SystemBaseConfig">SystemBaseConfig Model</param>
        /// <returns></returns>
        public bool UpdateSystemBaseConfig(SystemBaseConfig SystemBaseConfig)
        {
            SystemBaseConfig result = DbContext.Db.Saveable(SystemBaseConfig).ExecuteReturnEntity();

            return result.Id > 0 ? true : false;
        }
    }
}
