using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
   public class InCreaseEnergyDto
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal NowTotal { get; set; }
        public decimal YoldTotal { get; set; }
        public decimal InCrease { get; set; }
    }
}
