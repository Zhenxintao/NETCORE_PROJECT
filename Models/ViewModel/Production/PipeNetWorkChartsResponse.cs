using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel.Production
{
    public class PipeNetWorkChartsResponse
    {
        public int KHid { get; set; }
        public string StationName { get; set; }

        public decimal SumInfo { get; set; }

        public decimal TotalCount { get; set; }

    }
}
