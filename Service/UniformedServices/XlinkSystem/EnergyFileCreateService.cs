using ApiModel;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;
using THMS.Core.API.Models.UniformedServices.XlinkSystem;
using THMS.Core.API.Service.DbContext;
using THMS.Core.API.Service.UniformedServices.ComHttpRestSharp;
using THMS.Core.API.Logs;
using Hangfire;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Models.Energy;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using THMS.Core.API.Service.UniformedServices.IndoorSystem;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace THMS.Core.API.Service.UniformedServices.XlinkSystem
{
    /// <summary>
    /// 创建能耗文件应用
    /// </summary>
    public class EnergyFileCreateService : DbContextSqlSugar
    {
        
        /// <summary>
        /// 热源、换热站及室温能耗数据生成
        /// </summary>
        public void FilesCreate()
        {
            //换热站能耗文件上传
            StationFileFtp();
            //热源能耗文件上传
            PowerFileFtp();
            //室温数据数据上传
            TempFileFtp();
            //// 获取视频监控数据
            //GetViewData();
        }

        /// <summary>
        /// 实时温度txt文件上传Ftp服务器
        /// </summary>
        public async void TempFileFtp() {
            try
            {
                RealTempService realTemp = new RealTempService();
                //获取室温实时数据
                string TempResult =  realTemp.GetRealTemp();
                string TempPath = $@"FileData\{TempResult}.txt";
                var TempFtpResult = await new CoreFtpOpFiel().Upload(TempResult + ".txt", TempPath);
                if (TempFtpResult == ResultMessageInfo.SuccessMessage)
                {
                    Logger.Info($"室温实时数据文件上传成功！时间为：{DateTime.Now}");
                }
                else
                {
                    Logger.Info($"室温实时数据文件上传失败！时间为：{DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 换热站能耗txt文件上传Ftp服务器
        /// </summary>
        public async void StationFileFtp()
        {
            try
            {
                //执行换热站能耗数据文件生成方法
                string energyStationResult = EnergyStationFileCreate();
                string stationPath = $@"FileData\{energyStationResult}.txt";
                var stationFtpResult = await new CoreFtpOpFiel().Upload(energyStationResult + ".txt", stationPath);
                if (stationFtpResult == ResultMessageInfo.SuccessMessage)
                {
                    Logger.Info($"换热站能耗文件上传成功！时间为：{DateTime.Now}");
                }
                else
                {
                    Logger.Info($"换热站能耗文件上传失败！时间为：{DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                throw ex;
            }  
        }

        /// <summary>
        /// 热源能耗txt文件上传Ftp服务器
        /// </summary>
        public async void PowerFileFtp()
        {
            try
            {
                //执行热源能耗数据生成文件方法
                string energyPowerResult = EnergyPowerFileCreate();
                string powerPath = $@"FileData\{energyPowerResult}.txt";
                var powerFtpResult = await new CoreFtpOpFiel().Upload(energyPowerResult + ".txt", powerPath);
                if (powerFtpResult == ResultMessageInfo.SuccessMessage)
                {
                    Logger.Info($"热源能耗文件上传成功！时间为：{DateTime.Now}");
                }
                else
                {
                    Logger.Info($"热源能耗文件上传失败！时间为：{DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 生成换热站能耗数据文件.txt
        /// </summary>
        /// <returns></returns>
        public  string EnergyStationFileCreate()
        {
            try
            {
                //获取MeterData表中水、电、热各能耗数据信息
                var stationEnergyList = Db.Queryable<MeterDataHour, VpnUser>((s, v) => new object[] { JoinType.Left, s.Vpnuser_id == v.Id }).Where((s, v) => s.MeterDate.ToString("yyyy-MM-dd hh:00:00") == SqlFunc.DateAdd(DateTime.Now,-1,DateType.Hour).ToString("yyyy-MM-dd hh:00:00") && v.StationStandard < 98).OrderBy((s, v) => s.Vpnuser_id).OrderBy((s, v) => s.NarrayNo).ToList();
                List<HeatStationFileDto> HeatStationFileList = new List<HeatStationFileDto>();
                foreach (var item in stationEnergyList)
                {
                    int flag = HeatStationFileList.Where(s => s.stationId == item.Vpnuser_id && s.narray_no == item.NarrayNo).ToList().Count;
                    if (flag < 1)
                    {
                        HeatStationFileDto heatStationFileDto = new HeatStationFileDto();
                        heatStationFileDto.stationId = item.Vpnuser_id;
                        heatStationFileDto.narray_no = item.NarrayNo;
                        var stationResult = stationEnergyList.Where(s => s.Vpnuser_id == item.Vpnuser_id && s.NarrayNo == item.NarrayNo).ToList();
                        foreach (var narray in stationResult)
                        {

                            switch (narray.MeterType)
                            {
                                case 1:
                                    heatStationFileDto.water_consume = narray.Total;
                                    break;
                                case 2:
                                    heatStationFileDto.electricity_consume = narray.Total;
                                    break;
                                case 3:
                                    heatStationFileDto.heat_consume = narray.Total;
                                    break;
                                default:
                                    break;
                            }
                        }
                        heatStationFileDto.record_time = item.MeterDate;
                        HeatStationFileList.Add(heatStationFileDto);
                    }
                }
                string content = JsonConvert.SerializeObject(HeatStationFileList);
                string fileName = DqHxConfigSetting.HxFileNameHeatStation + DateTime.Now.ToString("yyyyMMddHH");
                FileDataCreateCommon.CreateEnergyFile(fileName, content);
                return fileName;
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                throw;
            }
          
        }

        /// <summary>
        /// 生成热源能耗数据文件.txt
        /// </summary>
        /// <returns></returns>
        public string EnergyPowerFileCreate()
        {
            //获取MeterData表中水、电、热各能耗数据信息
            var stationEnergyList = Db.Queryable<MeterDataHour, VpnUser>((s, v) => new object[] { JoinType.Left, s.Vpnuser_id == v.Id }).Where((s, v) => s.MeterDate.ToString("yyyy-MM-dd hh:00:00") == SqlFunc.DateAdd(DateTime.Now, -1, DateType.Hour).ToString("yyyy-MM-dd hh:00:00") && v.StationStandard ==99).OrderBy((s, v) => s.Vpnuser_id).OrderBy((s, v) => s.NarrayNo).ToList();
            List<HeatSourceFileDto> HeatSourceFileList = new List<HeatSourceFileDto>();
            foreach (var item in stationEnergyList)
            {
                int flag = HeatSourceFileList.Where(s => s.SourceId == item.Vpnuser_id && s.narray_no == item.NarrayNo).ToList().Count;
                if (flag < 1)
                {
                    HeatSourceFileDto heatSourceFileDto = new HeatSourceFileDto();
                    heatSourceFileDto.SourceId = item.Vpnuser_id;
                    heatSourceFileDto.narray_no = item.NarrayNo;
                    var stationResult = stationEnergyList.Where(s => s.Vpnuser_id == item.Vpnuser_id && s.NarrayNo == item.NarrayNo).ToList();
                    foreach (var narray in stationResult)
                    {

                        switch (narray.MeterType)
                        {
                            case 101:
                                heatSourceFileDto.water_consume = narray.Total;
                                break;
                            case 102:
                                heatSourceFileDto.electricity_consume = narray.Total;
                                break;
                            case 103:
                                heatSourceFileDto.heat_consume = narray.Total;
                                break;
                            default:
                                break;
                        }
                    }
                    heatSourceFileDto.record_time = item.MeterDate;
                    HeatSourceFileList.Add(heatSourceFileDto);
                }
            }
            string content = JsonConvert.SerializeObject(HeatSourceFileList);
            string fileName = DqHxConfigSetting.HxFileNameHeatSource + DateTime.Now.ToString("yyyyMMddHH");
            FileDataCreateCommon.CreateEnergyFile(fileName, content);
            return fileName;
        }
        public void HangfireCeshi()
        {
            Logger.Info($"Hangfire测试；时间为：{DateTime.Now}");
        }

        #region 视频监控代码，暂废弃

        /// <summary>
        /// 获取视频监控列表
        /// </summary>
        //public void GetViewData()
        //{
        //    string body = "{\"resourceType\": \"region\",\"pageNo\": 1,\"pageSize\": 200}";
        //    HttpPost("irds/v2/region/nodesByParams", body, 15);
        //    body = "{\"pageNo\": 1,\"pageSize\": 1000}";
        //    HttpPost("resource/v1/camera/advance/cameraList", body, 15);
        //}
        /// <summary>
        /// 视频监控POST请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="body"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        //public byte[] HttpPost(string uri, string body, int timeout)
        //{
        //    Dictionary<string, string> header = new Dictionary<string, string>();
        //    // 初始化请求：组装请求头，设置远程证书自动验证通过
        //    initRequest(header, uri, body, true);
        //    // build web request object
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(ConfigAppsetting.ViewAPI).Append(uri);
        //    // 创建POST请求
        //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
        //    req.KeepAlive = false;
        //    req.ProtocolVersion = HttpVersion.Version11;
        //    req.AllowAutoRedirect = false;   // 不允许自动重定向
        //    req.Method = "POST";
        //    req.Timeout = timeout * 1000;    // 传入是秒，需要转换成毫秒
        //    req.Accept = header["Accept"];
        //    req.ContentType = header["Content-Type"];
        //    foreach (string headerKey in header.Keys)
        //    {
        //        if (headerKey.Contains("x-ca-"))
        //        {
        //            req.Headers.Add(headerKey + ":" + header[headerKey]);
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(body))
        //    {
        //        byte[] postBytes = Encoding.UTF8.GetBytes(body);
        //        req.ContentLength = postBytes.Length;
        //        Stream reqStream = null;
        //        try
        //        {
        //            reqStream = req.GetRequestStream();
        //            reqStream.Write(postBytes, 0, postBytes.Length);
        //            reqStream.Close();
        //        }
        //        catch (WebException e)
        //        {
        //            if (reqStream != null)
        //            {
        //                reqStream.Close();
        //            }
        //            return null;
        //        }
        //    }
        //    HttpWebResponse rsp = null;
        //    try
        //    {
        //        rsp = (HttpWebResponse)req.GetResponse();
        //        if (HttpStatusCode.OK == rsp.StatusCode)
        //        {
        //            Stream rspStream = rsp.GetResponseStream();
        //            StreamReader sr = new StreamReader(rspStream);
        //            string strStream = sr.ReadToEnd();
        //            long streamLength = strStream.Length;
        //            byte[] response = System.Text.Encoding.UTF8.GetBytes(strStream);
        //            rsp.Close();
        //            if (uri.Contains("nodesByParams"))
        //                new MonitorStationInfoServices().SaveLocalViewData(response);
        //            else if (uri.Contains("cameraList"))
        //                new MonitorStationInfoServices().SavecameraData(response);
        //            return response;
        //        }
        //        else if (HttpStatusCode.Found == rsp.StatusCode || HttpStatusCode.Moved == rsp.StatusCode)  // 302/301 redirect
        //        {
        //            try
        //            {
        //                string reqUrl = rsp.Headers["Location"].ToString();    // 如需要重定向URL，请自行修改接口返回此参数
        //                rsp.Close();
        //                WebRequest wreq = WebRequest.Create(reqUrl);
        //                rsp = (HttpWebResponse)wreq.GetResponse();
        //                Stream rspStream = rsp.GetResponseStream();
        //                long streamLength = rsp.ContentLength;
        //                int offset = 0;
        //                byte[] response = new byte[streamLength];
        //                while (streamLength > 0)
        //                {
        //                    int n = rspStream.Read(response, offset, (int)streamLength);
        //                    if (0 == n)
        //                    {
        //                        break;
        //                    }
        //                    offset += n;
        //                    streamLength -= n;
        //                }
        //                if (uri.Contains("nodesByParams"))
        //                    new MonitorStationInfoServices().SaveLocalViewData(response);
        //                else if (uri.Contains("cameraList"))
        //                    new MonitorStationInfoServices().SavecameraData(response);
        //                return response;
        //            }
        //            catch (Exception e)
        //            {
        //                rsp.Close();
        //                return null;
        //            }
        //        }
        //        rsp.Close();
        //    }
        //    catch (WebException e)
        //    {
        //        if (rsp != null)
        //        {
        //            rsp.Close();
        //        }
        //    }
        //    return null;
        //}

        //private static void initRequest(Dictionary<string, string> header, string url, string body, bool isPost)
        //{
        //    // Accept                
        //    string accept = "application/json";// "*/*";
        //    header.Add("Accept", accept);

        //    // ContentType  
        //    string contentType = "application/json";
        //    header.Add("Content-Type", contentType);

        //    if (isPost)
        //    {
        //        // content-md5，be careful it must be lower case.
        //        string contentMd5 = computeContentMd5(body);
        //        header.Add("content-md5", contentMd5);
        //    }

        //    // x-ca-timestamp
        //    string timestamp = ((DateTime.Now.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 1000).ToString();
        //    header.Add("x-ca-timestamp", timestamp);

        //    // x-ca-nonce
        //    string nonce = System.Guid.NewGuid().ToString();
        //    header.Add("x-ca-nonce", nonce);

        //    // x-ca-key
        //    header.Add("x-ca-key", ConfigAppsetting.appkey);

        //    // build string to sign
        //    string strToSign = buildSignString(isPost ? "POST" : "GET", url, header);
        //    string signedStr = computeForHMACSHA256(strToSign, ConfigAppsetting.secret);

        //    // x-ca-signature
        //    header.Add("x-ca-signature", signedStr);

        //    if (true)
        //    {
        //        // set remote certificate Validation auto pass
        //        ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(remoteCertificateValidate);
        //        // FIX：修复不同.Net版对一些SecurityProtocolType枚举支持情况不一致导致编译失败等问题，这里统一使用数值
        //        //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)3072 | (SecurityProtocolType)768 | (SecurityProtocolType)192;
        //        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //    }
        //}

        //private static string computeContentMd5(string body)
        //{
        //    MD5 md5 = new MD5CryptoServiceProvider();
        //    byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(body));
        //    return Convert.ToBase64String(result);
        //}

        //private static string buildSignString(string method, string url, Dictionary<string, string> header)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(method.ToUpper()).Append("\n");
        //    if (null != header)
        //    {
        //        if (null != header["Accept"])
        //        {
        //            sb.Append((string)header["Accept"]);
        //            sb.Append("\n");
        //        }
        //        if (header.Keys.Contains("Content-MD5") && null != header["Content-MD5"])
        //        {
        //            sb.Append((string)header["Content-MD5"]);
        //            sb.Append("\n");
        //        }
        //        if (null != header["Content-Type"])
        //        {
        //            sb.Append((string)header["Content-Type"]);
        //            sb.Append("\n");
        //        }
        //        if (header.Keys.Contains("Date") && null != header["Date"])
        //        {
        //            sb.Append((string)header["Date"]);
        //            sb.Append("\n");
        //        }
        //    }
        //    string signHeader = buildSignHeader(header);
        //    sb.Append(signHeader);
        //    sb.Append(url);
        //    return sb.ToString();
        //}

        //private static string buildSignHeader(Dictionary<string, string> header)
        //{
        //    Dictionary<string, string> sortedDicHeader = new Dictionary<string, string>();
        //    sortedDicHeader = header;
        //    var dic = from objDic in sortedDicHeader orderby objDic.Key ascending select objDic;
        //    StringBuilder sbSignHeader = new StringBuilder();
        //    StringBuilder sb = new StringBuilder();
        //    foreach (KeyValuePair<string, string> kvp in dic)
        //    {
        //        if (kvp.Key.Replace(" ", "").Contains("x-ca-"))
        //        {
        //            sb.Append(kvp.Key + ":");
        //            if (!string.IsNullOrWhiteSpace(kvp.Value))
        //            {
        //                sb.Append(kvp.Value);
        //            }
        //            sb.Append("\n");
        //            if (sbSignHeader.Length > 0)
        //            {
        //                sbSignHeader.Append(",");
        //            }
        //            sbSignHeader.Append(kvp.Key);
        //        }
        //    }
        //    header.Add("x-ca-signature-headers", sbSignHeader.ToString());
        //    return sb.ToString();
        //}

        //private static string computeForHMACSHA256(string str, string secret)
        //{
        //    var encoder = new System.Text.UTF8Encoding();
        //    byte[] secretBytes = encoder.GetBytes(secret);
        //    byte[] strBytes = encoder.GetBytes(str);
        //    var opertor = new HMACSHA256(secretBytes);
        //    byte[] hashbytes = opertor.ComputeHash(strBytes);
        //    return Convert.ToBase64String(hashbytes);
        //}

        //private static bool remoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        //{
        //    return true;
        //}
        #endregion

    }
}
