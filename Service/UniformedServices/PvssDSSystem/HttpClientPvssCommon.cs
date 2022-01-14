using ApiModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using THMS.Core.API.ModelDto;
using THMS.Core.API.Logs;
using THMS.Core.API.Configuration;

namespace THMS.Core.API.Service.UniformedServices.PvssDSSystem
{
    public class HttpClientPvssCommon: ConfigAppsetting
    {
        static public RestClient Pvssclient { get; set; } = new RestSharp.RestClient(PvssWebApi);

        /// <summary>
        /// Post请求接口WebApi方法
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="data">实例化数据</param>
        /// <param name="apiurl">接口路径</param>
        /// <returns></returns>
        static public int HttpPvssPostSharp<T>(T data, string apiurl)
        {

            var requestPost = new RestRequest(apiurl, Method.POST);
            string json = JsonConvert.SerializeObject(data);
            requestPost.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse responsePost = Pvssclient.Execute(requestPost);
            string contentPost = responsePost.Content;
            if (responsePost.StatusCode == HttpStatusCode.OK)
            {
                Logger.Info($"{apiurl}:Post请求接口成功:{contentPost};记录时间:{DateTime.Now};");
                return ResultCode.Success;
            }
            else
            {
                Logger.Info($"Post请求接口失败:{responsePost.ErrorMessage};记录时间:{DateTime.Now};");
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// Put请求接口WebApi方法
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="data">实例化数据</param>
        /// <param name="apiurl">接口路径</param>
        /// <returns></returns>
        static public int HttpPvssPutSharp<T>(T data, string apiurl)
        {

            var requestPut = new RestRequest(apiurl, Method.PUT);
            string json = JsonConvert.SerializeObject(data);
            requestPut.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse responsePut = Pvssclient.Execute(requestPut);
            string contentPut = responsePut.Content;
            if (responsePut.StatusCode == HttpStatusCode.OK)
            {
                Logger.Info($"{apiurl}:Put 请求接口成功:{contentPut };记录时间:{DateTime.Now};");
                return ResultCode.Success;
            }
            else
            {
                Logger.Info($"Put 请求接口失败:{responsePut.ErrorMessage};记录时间:{DateTime.Now};");
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// Get请求接口WebApi方法
        /// </summary>
        /// <param name="parmas"></param>
        /// <param name="apiurl"></param>
        /// <returns></returns>
        static public int HttpPvssGetSharp(Dictionary<string, object> parmas, string apiurl)
        {
            var requestGet = new RestRequest(apiurl, Method.GET);
            foreach (var item in parmas)
            {
                requestGet.AddParameter(item.Key, item.Value);
            }
            IRestResponse responseGet = Pvssclient.Execute(requestGet);
            string contentGet = responseGet.Content;
            if (responseGet.StatusCode == HttpStatusCode.OK)
            {
                Logger.Info($"Get请求接口成功:{contentGet};记录时间:{DateTime.Now};");
                return ResultCode.Success;
            }
            else
            {
                Logger.Info($"Get请求接口失败:{responseGet.ErrorMessage};记录时间:{DateTime.Now};");
                return ResultCode.Error;
            }
        }

        /// <summary>
        /// Get请求接口WebApi方法
        /// </summary>
        /// <param name="parmas"></param>
        /// <param name="apiurl"></param>
        /// <returns></returns>
        static public int HttpPvssDeleteSharp(Dictionary<string, object> parmas, string apiurl)
        {
            var requestGet = new RestRequest(apiurl, Method.DELETE);
            foreach (var item in parmas)
            {
                requestGet.AddParameter(item.Key, item.Value);
            }
            IRestResponse responseGet = Pvssclient.Execute(requestGet);
            string contentGet = responseGet.Content;
            if (responseGet.StatusCode == HttpStatusCode.OK)
            {
                Logger.Info($"Get请求接口成功:{contentGet};记录时间:{DateTime.Now};");
                return ResultCode.Success;
            }
            else
            {
                Logger.Info($"Get请求接口失败:{responseGet.ErrorMessage};记录时间:{DateTime.Now};");
                return ResultCode.Error;
            }
        }
    }
}
