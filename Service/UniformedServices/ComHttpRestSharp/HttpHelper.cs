using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace THMS.Core.API.Service.UniformedServices.ComHttpRestSharp
{
    public class HttpHelper
    {
        public static string HttpClientPost(string url, object datajson)
        {
            var httpClient = new HttpClient(); //http对象
            //表头参数
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //转为链接需要的格式
            var httpContent = new JsonContent(datajson);
            //请求
            var res = httpClient.PostAsync(url, httpContent);
            var response = httpClient.PostAsync(url, httpContent).Result;

            //if (!response.IsSuccessStatusCode)
            //{
            //    //var str = response.Content.ReadAsStringAsync();
            //    //Task<string> t=
            //}
            Task<string> t = response.Content.ReadAsStringAsync();
            return t.Result;
        }
    }
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        {
        }

    }
}
