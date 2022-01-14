using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.IO;
using System.Linq;
using THMS.Core.API.Configuration;

namespace THMS.Core.API.Service.DbContext
{
    /// <summary>
    /// SQLServer数据库连接
    /// </summary>
    public class DbContextSqlSugar: ConfigAppsetting
    {
        /// <summary>
        /// SQLServer数据库连接
        /// </summary>
        public DbContextSqlSugar()
        {
            string connecation = SqlServerConfig;
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connecation,
                DbType = DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,

            });
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                    Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
        /// <summary>
        /// SQL Server ORM 操作数据库属性
        /// </summary>
        public SqlSugarClient Db;
    }
}
