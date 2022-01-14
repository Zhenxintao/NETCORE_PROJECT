using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.UniformedServices.XlinkSystem
{
    public class ProductionTableParmas
    {
        public string sortName { get; set; }
        public string sortType { get; set; }
        public string[] tagNames { get; set; }
        public int type { get; set; }
        public string organIds { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
