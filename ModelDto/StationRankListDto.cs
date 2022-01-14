using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.ModelDto
{
    public class StationRankListDto
    {
        public int Id { get; set; }
        public string PowerName { get; set; }
        public string OrganizationName { get; set; }

        public string StationName { get; set; }

        public string NarryName { get; set; }

        public string RealValue { get; set; }

        public string TimeStamp { get; set; }

    }
}
