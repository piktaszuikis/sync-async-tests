using Npgsql;
using System.Threading.Tasks;

namespace AspPerformanceTest
{
	public class DbWork
	{
		private const string CONNECTION_STRING = "Host=/run/postgresql/;Username=postgres;Maximum Pool Size=50;Timeout=160";
		private const string FAST_QUERY = "SELECT @num";
		private const string SLOW_QUERY = "SELECT CAST(pg_sleep(3) || '0' AS bigint) + @num";

		public static long QueryDB(long num) => RunSyncQuery(FAST_QUERY, num);
		public static long QueryDBSlow(long num) => RunSyncQuery(SLOW_QUERY, num);

		public static Task<long> QueryDBAsync(long num) => RunAsyncQuery(FAST_QUERY, num);
		public static Task<long> QueryDBSlowAsync(long num) => RunAsyncQuery(SLOW_QUERY, num);

		private static async Task<long> RunAsyncQuery(string queryString, long num)
		{
			await using var connection = new NpgsqlConnection(CONNECTION_STRING);
			await connection.OpenAsync();

			await using (var command = new NpgsqlCommand(queryString, connection))
			{
				command.Parameters.Add(new NpgsqlParameter(nameof(num), num));
				var result = await command.ExecuteScalarAsync();
				return (long?)result ?? -1L;
			}
		}

		private static long RunSyncQuery(string queryString, long num)
		{
			using var connection = new NpgsqlConnection(CONNECTION_STRING);
			connection.Open();

			using var query = new NpgsqlCommand(queryString, connection)
			{
				Parameters =
				{
					new NpgsqlParameter(nameof(num), num)
				}
			};

			return (long?)query.ExecuteScalar() ?? -1L;
		}
	}
}
