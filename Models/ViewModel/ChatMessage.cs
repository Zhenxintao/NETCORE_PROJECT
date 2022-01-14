using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace THMS.Core.API.Models.ViewModel
{
    public class ChatMessage
    {

        public int Type { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
    }
}
