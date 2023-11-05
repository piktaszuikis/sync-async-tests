using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspPerformanceTest.Controllers
{
	[Route("sync")]
	public class SyncController
	{
		[HttpGet("/")]
		[HttpGet("get")]
		public string Get()
		{
			return "ok";
		}

		[HttpGet("primes")]
		public IList<long> Primes(long num)
		{
			return ComplexWork.GetPrimes(num);
		}

		[HttpGet("db-fast")]
		public IList<long> QueryFast(long num)
		{
			num = DbWork.QueryDB(num);
			return ComplexWork.GetPrimes(num);
		}

		[HttpGet("db-slow")]
		public IList<long> QuerySlow(long num)
		{
			num = DbWork.QueryDBSlow(num);
			return ComplexWork.GetPrimes(num);
		}
	}
}
