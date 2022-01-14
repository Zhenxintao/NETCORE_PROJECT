using ApiModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 基础数据同步，监听数据库表信息
    /// </summary>
    public class BasicsDataSyn : DbContextSqlSugar
    {
        /// <summary>
        /// 获取SQL Server字符串连接地址
        /// </summary>
        public static string connecation { get; set; } = ConfigAppsetting.SqlServerConfig;
        /// <summary>
        /// 开始监听数据库
        /// </summary>
        static public void GetDataChange()
        {
            SqlDependency.Start(connecation);
            SelectData();
            Console.WriteLine("开始监听了");
            //SqlDependency.Stop(connecation);
        }
        /// <summary>
        /// 数据库查询操作
        /// </summary>
        private static void SelectData()
        {
            using (SqlConnection connection = new SqlConnection(connecation))
            {
                //依赖是基于某一张表的,而且查询语句只能是简单查询语句,不能带top或*,同时必须指定所有者,即类似[dbo].[] 
                string cmdText = "SELECT [Id],[StationName] from dbo.VpnUser";
                using (SqlCommand command = new SqlCommand(cmdText, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    SqlDependency dependency = new SqlDependency(command);
                    // 事件注册，这是核心
                    dependency.OnChange += new OnChangeEventHandler(Dependency_OnChange);
                    SqlDataReader sdr = command.ExecuteReader();
                    Console.WriteLine();
                    while (sdr.Read())
                    {                 
                                string value = sdr["StationName"].ToString();
                                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  student表新增信息：" + value);
                                //CreateInterFace(sdr["Id"].ToString(), sdr["StationName"].ToString());                       
                    }
                    sdr.Close();
                }
            }
        }
        //static public void CreateInterFace(string messageID, string StationName)
        //{
        //    VpnUser result = new DbContextSqlSugar().Db.Queryable<VpnUser>().Where(s => s.Id == int.Parse(messageID)).First();
        //}

        /// <summary>
        /// 具体事件
        /// </summary>
        /// <param name="sender">update insert delete都会进入</param>
        /// <param name="e"></param>
        private static void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            // 只有数据发生变化时,才重新获取数据 
            if (e.Type == SqlNotificationType.Change)
            {
                SelectData();
            }
        }
    }
}
