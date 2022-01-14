using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.Common
{
    public class CommonTagItem
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get;
            set;
        }

        public string TagName
        {
            get ;
            set;
        }

        public string AiDesc
        {
            get ;
            set;
        }
        public string Unit
        {
            get ;
            set;
        }


        public string AiValue
        {
            get ;
            set;
        }

        public int NarrayNo
        {
            get ;
            set;
        }

        public bool IsImage
        {
            get ;
            set;
        }

        public string AiType
        {
            get ;
            set;
        }

        public int ValueSeq
        {
            get;
            set;
        }
    }
}
