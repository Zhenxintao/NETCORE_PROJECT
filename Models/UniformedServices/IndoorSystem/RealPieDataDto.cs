using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.IndoorSystem
{
    public class RealPieDataDto
    {
        public string DeviceNum { get; set; }
        public decimal TValue { get; set; }
        public decimal Humidity { get; set; }
        public DateTime RecordTime { get; set; }
        public string UserCode { get; set; }
    }
}
