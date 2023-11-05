using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading;

namespace HttpListenerTest
{
	internal class Program
	{
		private static readonly byte[] RESPONSE_BYTES = Encoding.UTF8.GetBytes("OK");

		private static void WriteResponse(HttpListener listener)
		{
			while (true)
			{
				var context = listener.GetContext();

				context.Response.SendChunked = false;
				context.Response.ContentLength64 = RESPONSE_BYTES.Length;
				context.Response.OutputStream.Write(RESPONSE_BYTES, 0, RESPONSE_BYTES.Length);
				context.Response.Close();
			}
		}

		static void Main(string[] args)
		{
			var port = int.Parse(args[Array.IndexOf(args, "-port") + 1]);

			HttpListener listener = new HttpListener();
			listener.Prefixes.Add($"http://127.0.0.1:{port}/");
			listener.Start();

			if (args.Contains("-threaded"))
			{
				Console.WriteLine($"{System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}: Listening on port {port}...  Using threaded verion with {Environment.ProcessorCount} threads.");

				for (var i = 0; i < Environment.ProcessorCount; i++)
				{
					new Thread(() => WriteResponse(listener)).Start();
				}
			}
			else
			{
				Console.WriteLine($"{System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}: Listening on port {port}...  Using async verion.");

				while (true)
				{
					var x = listener.BeginGetContext(x =>
					{
						HttpListenerContext context = listener.EndGetContext(x);

						context.Response.SendChunked = false;
						context.Response.ContentLength64 = RESPONSE_BYTES.Length;
						context.Response.OutputStream.Write(RESPONSE_BYTES, 0, RESPONSE_BYTES.Length);
						context.Response.Close();
					}, null);

					x.AsyncWaitHandle.WaitOne();
				}
			}

		}
	}
}
