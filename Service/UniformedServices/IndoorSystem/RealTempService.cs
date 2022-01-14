using Newtonsoft.Json;
using SqlSugar;
using Sugar.Enties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;

namespace THMS.Core.API.Service.UniformedServices.IndoorSystem
{
    /// <summary>
    /// 给华夏实时室温的接口
    /// </summary>
    public class RealTempService : DbContextMySql
    {
        /// <summary>
        /// 生成实时室内温度txt
        /// </summary>
        /// <returns></returns>
        public string GetRealTemp()
        {
            string fileName = DqHxConfigSetting.HxFileNameTemperature + DateTime.Now.ToString("yyyyMMddHH");
           
            try
            {
                List<RealTempTohx> te =  DbMysqlIndoor.Queryable<da_realvalue, eq_devicebind, eq_makeupdetal>((dar, eqd, eqm) =>
                                                                                               new object[] { JoinType.Inner,dar.DeviceNum == eqd.DeviceNum,
                                                                                                              JoinType.Inner,eqd.UserCode == eqm.UserCode})
                                                                                             .Select((dar, eqd, eqm) => new RealTempTohx
                                                                                             {
                                                                                                 userId = eqd.UserCode,
                                                                                                 record_time = dar.RecordTime,
                                                                                                 temperature = dar.TValue,
                                                                                                 humidity = dar.Humidity,
                                                                                                 compensateValue = Convert.ToDecimal(eqm.CategoryMake + eqm.SeparateMake),
                                                                                                 //isonline = SqlFunc.IF((SqlFunc.GetDate() - dar.RecordTime).TotalMinutes < 180).Return(true).End(false)
                                                                                                 isonline =SqlFunc.IF(dar.RecordTime.AddMinutes(180) > SqlFunc.GetDate() )
                                                                                                                     .Return(true).End(false)
                                                                                             }).ToList();
                FileDataCreateCommon.CreateEnergyFile(fileName, JsonConvert.SerializeObject(te));
            }
            catch (Exception e)
            {
                fileName = "生成失败";
            }
            return fileName;
        }

        /// <summary>
        /// 给华夏室温数据的实体类
        /// </summary>
        public class RealTempTohx
        {

            /// <summary>
            /// 用户唯一编码(用户收费卡号)
            /// </summary>
            public string userId { get; set; }
            /// <summary>
            /// 采集时间
            /// </summary>
            public DateTime record_time { get; set; }
            /// <summary>
            /// 温度
            /// </summary>
            public decimal temperature { get; set; }
            /// <summary>
            /// 湿度
            /// </summary>
            public decimal humidity { get; set; }
            /// <summary>
            /// 补偿值
            /// </summary>
            public decimal compensateValue { get; set; } = 0;

            /// <summary>
            /// 是否在线
            /// </summary>
            public bool isonline { get; set; } = true;
        }
        #region 室温用户基础数据
        #endregion

    }
}
