using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Charity.Mvc
{
	public class Program
	{
		public static void Main(string[] args)
		{
            try
            {
                var host = CreateHostBuilder(args).Build();
                Log.Logger.Information("App is starting...");
                host.Run();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "Application is shutting down abnormally.");
                Log.CloseAndFlush();
            }
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
					config.AddJsonFile(@"appsecrets.json");
                })
				.ConfigureWebHostDefaults(webBuilder =>
				{
                    webBuilder.UseSerilog();
					webBuilder.UseStartup<Startup>();
				});
	}
}
