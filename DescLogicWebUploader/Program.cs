using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace FirstASPNETCOREProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();


        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //options.Limits.MaxRequestBodySize = 1073741824;
                    webBuilder
                    .UseStartup<Startup>()
                    .UseKestrel()
                    .ConfigureKestrel((context, options) =>
                    {
                        options.Limits.MaxRequestBodySize = 1073741824;
                    });
                });
    }
}
