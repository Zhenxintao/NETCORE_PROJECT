using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Configuration;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    public class HeatStationFileDto
    {
        public int stationId { get; set; }
        public int narray_no { get; set; }
        [JsonConverter(typeof(NewtonsoftConverter))]
        public DateTime record_time { get; set; }
        public decimal heat_consume { get; set; }
        public decimal water_consume { get; set; }
        public decimal electricity_consume { get; set; }
    }

    public class HeatSourceFileDto
    {
        public int SourceId { get; set; }
        public int narray_no { get; set; }
        [JsonConverter(typeof(NewtonsoftConverter))]
        public DateTime record_time { get; set; }
        public decimal heat_consume { get; set; }
        public decimal water_consume { get; set; }
        public decimal electricity_consume { get; set; }
    }
}
