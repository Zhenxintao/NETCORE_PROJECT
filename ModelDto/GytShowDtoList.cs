using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using THMS.Core.API.Service.Common;

namespace THMS.Core.API.ModelDto
{
    public class GytShowDtoList
    {
        public int Id { get; set; }
        public int NarrayNo { get; set; }
        public string StationName { get; set; }
        public  string StationBranchName { get; set; }

        public string AiDesc { get; set; }
        public string TagName { get; set; }
        public string RealValue { get; set; }
        public string Unit { get; set; }
        public int ValueSeq { get; set; }
        public int GYTShow { get; set; }
        public int GYTTagShow { get; set; }
        public int GGLShow { get; set; }
        public int StationStandard { get; set; }
        public bool IsOnline { get; set; }

        public string FlowChart { get; set; }
        public double Xval { get; set; }
        public double Yval { get; set; }

        public string AiType { get; set; }

        public string ZeroMean { get; set; }
        public string OneMean { get; set; }

        public string GetDIMean { get {
                var msg = "未定义";

                switch (RealValue)
                {
                    case "0":
                        msg = ZeroMean;
                        break;
                    case "1":
                        msg = OneMean;
                        break;
                    default:
                        break;
                }
                return msg;
            } } 
        public DateTime TimeSjk { get; set; }
        public string StationType
        {
            get
            {
                var type = "未知";
                switch (StationStandard)
                {
                    case 0:
                        type = "人工站";
                        break;
                    case 1:
                        type = "监测站";
                        break;
                    case 2:
                        type = "管线监测点";
                        break;
                    case 3:
                        type = "监控站";
                        break;
                    case 4:
                        type = "能源站";
                        break;
                    case 98:
                        type = "热网";
                        break;
                    case 99:
                        type = "热源";
                        break;
                }
                return type;
            }
        }


    }
}
