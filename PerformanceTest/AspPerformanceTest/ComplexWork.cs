using System.Collections.Generic;

namespace AspPerformanceTest
{
	public class ComplexWork
	{
		public static IList<long> GetPrimes(long num)
		{
			IList<long> list = new List<long>();
			bool isPrime = true;
			for (long i = 0; i <= num; i++)
			{
				for (long j = 2; j <= num; j++)
				{
					if (i != j && i % j == 0)
					{
						isPrime = false;
						break;
					}
				}
				if (isPrime)
				{
					list.Add(i);
				}
				isPrime = true;
			}
			return list;
		}
	}
}
