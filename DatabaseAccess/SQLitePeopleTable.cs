namespace DatabaseAccess
{
	using System.Data;

	public class SQLitePeopleTable
	{
		private const string CreateTableCommand = "CREATE TABLE IF NOT EXISTS People (PersonId INTEGER, Name TEXT);";
		private const string InsertPersonCommand = "INSERT INTO People VALUES (@p0, @p1);";
		private const string FirstParameter = "@p0";
		private const string SecondParameter = "@p1";
		private readonly IDbConnection connection;

		public SQLitePeopleTable(IDbConnection connection)
		{
			this.connection = connection;
		}

		public void Create()
		{
			using (var command = this.connection.CreateCommand())
			{
				command.CommandText = CreateTableCommand;
				command.ExecuteNonQuery();
			}
		}

		public void InsertPerson(int id, string name)
		{
			using (var command = this.connection.CreateCommand())
			{
				command.CommandText = InsertPersonCommand;
				command.Parameters.Add(BuildIdParameter(command, id));
				command.Parameters.Add(BuildNameParameter(command, name));
				command.ExecuteNonQuery();
			}
		}
		private static IDataParameter BuildIdParameter(IDbCommand command, int id)
		{
			var parameter = command.CreateParameter();
			parameter.ParameterName = FirstParameter;
			parameter.Value = id;
			return parameter;
		}
		private static IDataParameter BuildNameParameter(IDbCommand command, string name)
		{
			var parameter = command.CreateParameter();
			parameter.ParameterName = SecondParameter;
			parameter.Value = name;
			return parameter;
		}
	}
}
