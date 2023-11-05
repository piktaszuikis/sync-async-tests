using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspPerformanceTest.Controllers
{
	[Route("async")]
	public class AsyncController
	{
		[HttpGet("get")]
		public async Task<string> Get()
		{
			return "ok";
		}

		[HttpGet("primes")]
		public async Task<IList<long>> Primes(long num)
		{
			return ComplexWork.GetPrimes(num);
		}

		[HttpGet("db-fast")]
		public async Task<IList<long>> QueryFast(long num)
		{
			num = await DbWork.QueryDBAsync(num);
			return ComplexWork.GetPrimes(num);
		}

		[HttpGet("db-slow")]
		public async Task<IList<long>> QuerySlow(long num)
		{
			num = await DbWork.QueryDBSlowAsync(num);
			return ComplexWork.GetPrimes(num);
		}
	}
}
