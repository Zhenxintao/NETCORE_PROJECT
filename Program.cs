using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using THMS.Core.API.Service.UniformedServices.XlinkSystem;

namespace THMS.Core.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BasicsDataSyn.GetDataChange();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            var config = new ConfigurationBuilder().AddCommandLine(args).Build();
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls("http://10.0.2.52:5001");
        }
           

    }
}
