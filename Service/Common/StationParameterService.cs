using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Models.Common;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Common
{
    /// <summary>
    /// 标准参量信息表
    /// </summary>
    public class StationParameterService
    {
        /// <summary>
        /// 数据库连接串
        /// </summary>
        DbContextSqlSugar DbContext = new DbContextSqlSugar();

        /// <summary>
        /// 获取标准参量表(TagName列表，List)
        /// </summary>
        /// <param name="args">TagName</param>
        /// <returns></returns>
        public List<StandardParameter> GetStandardParameterList(List<string> args)
        {
            var list = new List<StandardParameter>();

            try
            {
                string sql = "select * from StandardParameter ";

                if (args.Count == 0)
                {
                    var ViewList = DbContext.Db.Ado.SqlQuery<StandardParameter>(sql);
                }
                else
                {
                    sql = sql + " where TagName IN ( @items ) ";

                    list = DbContext.Db.Ado.SqlQuery<StandardParameter>(sql, new SugarParameter[]{
                new SugarParameter("@items", args)});
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        /// <summary>
        /// 获取标准参量表(UserConfig数据表id)
        /// </summary>
        /// <param name="userconfig_id">UserConfig数据表id</param>
        /// <returns></returns>
        public List<StandardParameter> GetStandardParameterList(int userconfig_id)
        {
            var paraList = new List<StandardParameter>();

            try
            {
                //获取配置列表参数信息
                var userConfigData = DbContext.Db.Queryable<UserConfig>().Where(s => s.Id == userconfig_id).First();

                //includeParaMDLS 参数集合
                var includeParaMDLS = userConfigData.IncludeParaMDL.Split(",").ToList();

                //01首先查询温度压力所对应的Aivalue值
                paraList = GetStandardParameterList(includeParaMDLS);
            }
            catch (Exception ex)
            {

            }
            return paraList;
        }

        /// <summary>
        /// 获取标准参量表(TagName列表，String)
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public List<StandardParameter> GetStandardParameterList(string TagName)
        {
            var list = new List<StandardParameter>();

            try
            {
                string sql = "select * from StandardParameter ";

                sql = sql + " where TagName = @items ";

                list = DbContext.Db.Ado.SqlQuery<StandardParameter>(sql, new SugarParameter[]{
                new SugarParameter("@items", TagName)});
            }
            catch (Exception ex)
            {

            }
            return list;
        }
    }
}
