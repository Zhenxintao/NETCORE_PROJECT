using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Configuration
{
    /// <summary>
    /// 重写Newtonsoft序列化时间格式自定义
    /// </summary>
    public class NewtonsoftConverter : DateTimeConverterBase
    {
        //[return: NullableAttribute(2)]
        //public override object ReadJson(JsonReader reader, Type objectType, [NullableAttribute(2)] object existingValue, JsonSerializer serializer)
        //{
        //    throw new NotImplementedException();
        //}

        //public override void WriteJson(JsonWriter writer, [NullableAttribute(2)] object value, JsonSerializer serializer)
        //{
        //    throw new NotImplementedException();
        //}
        private static IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
}
