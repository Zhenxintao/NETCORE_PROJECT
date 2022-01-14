﻿using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;

namespace THMS.Core.API.Service.DbContext
{
    /// <summary>
    /// Mysql数据库连接
    /// </summary>
    public class DbContextMySqlEw: ConfigAppsetting
    {

        /// <summary>
        /// Mysql数据库连接
        /// </summary>
        public DbContextMySqlEw()
            {
            string connecation = MySqlConfigEw;
                DbMysql = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = connecation,
                    DbType = DbType.MySql,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,

                });
                DbMysql.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql + "\r\n" +
                        DbMysql.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine();
                };
            }
        /// <summary>
        /// Mysql ORM 操作数据库属性
        /// </summary>
            public SqlSugarClient DbMysql;
        } 
}
