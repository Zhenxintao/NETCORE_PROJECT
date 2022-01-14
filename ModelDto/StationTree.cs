using ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    public class StationTree
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<StationTree> children { get; set; }
    }
}
