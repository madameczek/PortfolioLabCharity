using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Charity.Mvc
{
    public static class Program
	{
		public static void Main(string[] args)
		{
            try
            {
                //if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"Sqlite.db")) File.Create(AppDomain.CurrentDomain.BaseDirectory + @"Sqlite.db");
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

        private static IHostBuilder CreateHostBuilder(string[] args) =>
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
