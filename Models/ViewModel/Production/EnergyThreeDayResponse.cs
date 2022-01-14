using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel.Production
{
    public class EnergyThreeDayResponse
    {
        public int Id { get; set; }
        public string StationName { get; set; }
        public int MeterType { get; set; }
        public decimal Total { get; set; }
        public string MeterDate { get; set; }
    }
}
