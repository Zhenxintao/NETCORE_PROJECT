using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    public class PowerInfoTree
    {
        public int value { get; set; }
        public int ParentId { get; set; }
        public string label { get; set; }
        public List<PowerInfoTree> children { get; set; }
    }
}
