namespace DatabaseAccess
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;

	public class Program
	{
		private const int JohnId = 1;
		private const string JohnName = "John Smith";
		private const int MaryId = 2;
		private const string MaryName = "Mary Jones";
		private const string ReportTemplate = "\nPerson with ID == {0}: {1}\n";
		private const string Success = "It worked! :)";
		private const string Failure = "It didn't work! :(";

		// This is how you grab the connection string from App.config:
		private static readonly ConnectionStringSettings ConnectionSettings = 
			ConfigurationManager.ConnectionStrings["TestDatabase"]; 

		public static void Main()
		{
			using (var connection = GetConnection())
			{
				PopulateDatabase(connection);
				var result = QueryDatabase(connection);
				ReportResult(result);
			}
			Console.ReadLine();
		}
		private static IDbConnection GetConnection()
		{
			var provider = DbProviderFactories.GetFactory(ConnectionSettings.ProviderName);
			var connection = provider.CreateConnection();
			connection.ConnectionString = ConnectionSettings.ConnectionString;
			connection.Open();
			return connection;
		}

		private static void PopulateDatabase(IDbConnection connection)
		{
			var table = new SQLitePeopleTable(connection);
			table.Create();
			table.InsertPerson(JohnId, JohnName);
			table.InsertPerson(MaryId, MaryName);
		}
		private static string QueryDatabase(IDbConnection connection)
		{
			var query = new PersonQuery(connection, JohnId);
			return query.Execute();
		}
		private static void ReportResult(string result)
		{
			Console.WriteLine(string.Format(ReportTemplate, JohnId, result));

			if (result == JohnName)
				Console.WriteLine(Success);
			else
				Console.WriteLine(Failure);
		}
	}
}
