using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Models.ViewModel;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;
using THMS.Core.API.Logs;

namespace THMS.Core.API.Service.UniformedServices.ServiceProcessing
{
    /// <summary>
    /// 服务数据处理
    /// </summary>
    public class ServiceData : DbContextSqlSugar
    {

        public void RequestInfo(string path,string startTime,string endTime)
        {
            try
            {
                ServiceInfo info = ServiceName(path);
                List<UniformedServicesInfo> list = UniformedServiceInfoList(info, startTime,endTime);
                if (list != null)
                {
                    var addList = list.Where(s => s.ServiceId != 999).ToList();
                    Db.Insertable<UniformedServicesInfo>(addList).ExecuteCommand();
                    UniformedServiceResponse uniformed = sendAll(info);
                    uniformed.ListInfo = list;
                    string url = "/api/Uniformed/SendAllInfo";
                    HttpClientSignalrCommon.HttpPostSharp(uniformed, url);
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                throw;
            }

        }

        public ServiceInfo ServiceName(string con)
        {
            string content = con.Substring(5);
            int index = content.IndexOf('/');
            string info = content.Substring(0, index);
            ServiceInfo serviceInfo = new ServiceInfo();
            switch (info)
            {
                case "PvssDS":
                    serviceInfo.ServiceName = "SCADA系统";
                    serviceInfo.ServiceId = 1;
                    break;
                case "MonitorSourceInfo":
                    serviceInfo.ServiceName = "智慧管控系统";
                    serviceInfo.ServiceId = 2;
                    break;
                case "MonitorStationInfo":
                    serviceInfo.ServiceName = "智慧管控系统";
                    serviceInfo.ServiceId = 2;
                    break;
                case "XlinkProduction":
                    serviceInfo.ServiceName = "智慧管控系统";
                    serviceInfo.ServiceId = 2;
                    break;
                case "RealTemp":
                    serviceInfo.ServiceName = "室内测温系统";
                    serviceInfo.ServiceId = 3;
                    break;
                case "BaseInfo":
                    serviceInfo.ServiceName = "二网平衡系统";
                    serviceInfo.ServiceId = 4;
                    break;
                case "BaseInfoDataUpload":
                    serviceInfo.ServiceName = "二网平衡系统";
                    serviceInfo.ServiceId = 4;
                    break;
                case "HvInfo":
                    serviceInfo.ServiceName = "二网平衡系统";
                    serviceInfo.ServiceId = 4;
                    break;
                case "UvInfo":
                    serviceInfo.ServiceName = "二网平衡系统";
                    serviceInfo.ServiceId = 4;
                    break;
                case "PvssReal":
                    serviceInfo.ServiceName = "实时数据同步";
                    serviceInfo.ServiceId = 5;
                    break;
                default:
                    serviceInfo.ServiceName = "Data Wrong";
                    serviceInfo.ServiceId = -1;
                    break;
            }
            return serviceInfo;
        }

        public List<UniformedServicesInfo> UniformedServiceInfoList(ServiceInfo info, string startTime,string endTime)
        {
            try
            {
                if (info.ServiceId==-1)
                {
                    return null;
                }
                else
                {
                    List<UniformedServicesInfo> list = new List<UniformedServicesInfo>();
                    if (info != null && startTime != "" && endTime!="")
                    {
                         TimeSpan span = Convert.ToDateTime(endTime).Subtract(Convert.ToDateTime(startTime));
                        double elaspedTime = span.TotalMilliseconds;
                        string content;
                        UniformedServicesInfo uniformedServicesInfo = new UniformedServicesInfo();
                        uniformedServicesInfo.RequestStartDate = startTime;
                        uniformedServicesInfo.RequestEndDate = endTime;
                        if (info.ServiceId == 1)
                        {
                            content = $"{info.ServiceName}同步基础数据至数据服务中心";
                            list.Add(new UniformedServicesInfo() { RequestStartDate = DateTime.Now.ToString(), ServiceName = "数据中台", CorrespondingTime = 15, ServiceContent = "数据服务中心开始处理各系统数据接口及交换处理信息", ServiceId = 999, ShowStatus = false });
                        }
                        else if (info.ServiceId == 5)
                        {
                            content = $"{info.ServiceName}同步实时数据至数据服务中心";
                            list.Add(new UniformedServicesInfo() { RequestStartDate = DateTime.Now.ToString(), ServiceName = "数据中台", CorrespondingTime = 15, ServiceContent = "数据服务中心清洗实时数据、分发控制", ServiceId = 999, ShowStatus = false });
                        }
                        else if (info.ServiceId == 2)
                        {
                            content = $"{info.ServiceName}请求数据服务中心一网信息";
                        }
                        else if (info.ServiceId == 3)
                        {
                            content = $"{info.ServiceName}请求数据服务中心室温信息";
                        }
                        else
                        {
                            content = $"{info.ServiceName}请求数据服务中心二网及末端温度采集信息";
                        }
                        uniformedServicesInfo.ServiceContent = content;
                        uniformedServicesInfo.ServiceId = info.ServiceId;
                        uniformedServicesInfo.CorrespondingTime = Convert.ToInt32(elaspedTime);
                        uniformedServicesInfo.ServiceName = info.ServiceName;
                        uniformedServicesInfo.ShowStatus = false;
                        list.Add(uniformedServicesInfo);
                        return list;
                    }
                    list.Add(new UniformedServicesInfo() { RequestStartDate = DateTime.Now.ToString(), ServiceName = "数据中台", CorrespondingTime = 15, ServiceContent = "数据服务中心开始处理各系统数据接口及交换处理信息", ServiceId = 999 });
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<UniformedServicesInfo> serachList()
        {
            try
            {
                List<UniformedServicesInfo> list = Db.Queryable<UniformedServicesInfo>().Where(s => s.ShowStatus == false).ToList();
                if (list.Count!=0)
                {
                    foreach (var item in list)
                    {
                        item.ShowStatus = true;
                    }
                    Db.Updateable(list).UpdateColumns(it => new { it.ShowStatus }).ExecuteCommand();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
      
        }

        public UniformedServiceResponse UniformedServiceResponse()
        {
            try
            {
                var list = Db.Queryable<UniformedServicesInfo>().Where(s =>  s.ServiceId!=999 && s.ServiceId!=-1).Select(s=>new { s.ServiceId }).ToList();
                UniformedServiceResponse response = new UniformedServiceResponse();
                response.WebSystem = list.Where(s => s.ServiceId == 2).Count();
                response.WisdomControlSystem = response.WebSystem;
                response.IndoorSystem = list.Where(s => s.ServiceId == 3).Count();
                response.NetBalanceSystem = list.Where(s => s.ServiceId == 4).Count();
                response.ScadaSystem = list.Count - response.WisdomControlSystem - response.IndoorSystem - response.NetBalanceSystem;
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public UniformedServiceResponse sendAll(ServiceInfo info)
        {
            UniformedServiceResponse response = new UniformedServiceResponse();
            switch (info.ServiceId)
            {
                case 1:
                    response.ScadaSystem = 1;
                    break;
                case 2:
                    response.WisdomControlSystem = 1;
                    response.WebSystem = 1;
                    break;
                case 3:
                    response.IndoorSystem = 1;
                    break;
                case 4:
                    response.NetBalanceSystem = 1;
                    break;
                case 5:
                    response.ScadaSystem = 1;
                    break;
                default:
                    break;
            }
            return response;
        }

    }
}
