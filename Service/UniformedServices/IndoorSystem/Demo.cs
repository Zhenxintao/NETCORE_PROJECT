using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.UniformedServices.IndoorSystem
{
    public class Demo: DbContextMySql
    {
        /// <summary>
        /// 室温测试
        /// </summary>
        /// <returns></returns>
        public List<object> GetListDevice()
        {
            var resultList = DbMysqlIndoor.Ado.SqlQuery<object>("SELECT * FROM eq_device").ToList();
            return resultList;
        }
    }
}
