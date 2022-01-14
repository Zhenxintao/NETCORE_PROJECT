using ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.DbContext;

namespace THMS.Core.API.Service.Forecast
{
    public  class StationForecastBasicService:DbContextSqlSugar
    {

        /// <summary>
        /// 换热站负荷预测基础数据表新增信息
        /// </summary>
        /// <param name="stationForecastBasic"></param>
        /// <returns></returns>
        public bool AddStationForecastBasic(StationForecastBasic stationForecastBasic)
        {
            int con = Db.Insertable(stationForecastBasic).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 换热站负荷预测基础数据表删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DelStationForecastBasic(int id)
        {
            
            int con = Db.Deleteable<StationForecastBasic>().Where(it => it.Id == id).ExecuteCommand();
            return con > 0 ? true : false;
        }

        /// <summary>
        /// 换热站负荷预测基础数据表更新修改信息
        /// </summary>
        /// <param name="stationForecastBasic"></param>
        /// <returns></returns>
        public bool UpdStationForecastBasic(StationForecastBasic stationForecastBasic)
        {

            int con = Db.Updateable(stationForecastBasic).ExecuteCommand();
            return con > 0 ? true : false;
        }



        /// <summary>
        /// 换热站负荷预测基础数据表查询信息
        /// </summary>
        /// <returns></returns>
        public Tuple<List<StationForecastBasic>,int> SelStationForecastBasic(int pageSize, int pageIndex)
        {
            var totalCount = 0;
            List<StationForecastBasic> list = Db.Queryable<StationForecastBasic>().ToPageList(pageIndex, pageSize, ref totalCount);
            return new Tuple<List<StationForecastBasic>, int>(list,totalCount);
        }
    }
}
