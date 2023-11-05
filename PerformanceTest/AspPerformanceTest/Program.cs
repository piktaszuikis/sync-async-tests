using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

#if NET5_0
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore;
#endif

namespace AspPerformanceTest
{
	internal class Program
	{
#if NET5_0
		private class Startup
		{
			public void Configure(IApplicationBuilder builder)
			{
				builder.UseRouting();
				builder.UseEndpoints(points =>
				{
					points.MapControllers();
				});

			}
		}
#endif

		static void Main(string[] args)
		{
#if NET5_0
			var builder = WebHost.CreateDefaultBuilder(args);
			builder.UseStartup<Startup>();
			builder.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
			});

			builder.ConfigureKestrel(o =>
			{
				//o.Limits.Http2.MaxStreamsPerConnection = 1000;
				o.AllowSynchronousIO = true;
			})
			.ConfigureLogging(logging =>
			{
				logging.SetMinimumLevel(LogLevel.Warning);
			})
			.ConfigureServices(services =>
			{
				services.AddControllers();
			});

			var app = builder.Build();
#endif

#if NET6_0_OR_GREATER

#if NET8_0
			var builder = WebApplication.CreateSlimBuilder(args);
#else
			var builder = WebApplication.CreateBuilder(args);
#endif
			builder.Logging.ClearProviders();
			builder.WebHost.ConfigureKestrel(o =>
			{
				//o.Limits.Http2.MaxStreamsPerConnection = 1000;
				o.AllowSynchronousIO = true;
			});

			builder.Logging.SetMinimumLevel(LogLevel.Warning);
			builder.Services.AddControllers();

			var app = builder.Build();
			app.MapControllers();
#endif

			{
				var index = Array.IndexOf(args, "-threads");
				if(index >= 0)
				{
					var threadCount = int.Parse(args[Array.IndexOf(args, "-threads") + 1]);
					Console.WriteLine($"Using min thread count {threadCount}");
					ThreadPool.SetMinThreads(threadCount, 1);
				}
			}
			
			app.Start();

			Console.WriteLine("Running on environment " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
			Console.WriteLine("Listening on:");
#if NET5_0
			var addresses = ((IWebHost)app).ServerFeatures.Get<IServerAddressesFeature>().Addresses;
#else
			var addresses = ((IApplicationBuilder)app).ServerFeatures.Get<IServerAddressesFeature>().Addresses;
#endif
			foreach (var address in addresses)
				System.Console.WriteLine($"\t* {address}");

			app.WaitForShutdown();
		}
	}
}
