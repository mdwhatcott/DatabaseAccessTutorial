namespace DatabaseAccess
{
	using System.Data;

	public class PersonQuery
	{
		private const string SelectNameCommand = @"SELECT Name FROM People WHERE PersonId = @p0";
		private const string FirstParameter = "@p0";
		private readonly IDbConnection connection;
		private readonly int personId;

		public PersonQuery(IDbConnection connection, int personId)
		{
			this.connection = connection;
			this.personId = personId;
		}

		public string Execute()
		{
			using (var command = this.connection.CreateCommand())
				return ExtractName(command);
		}

		private string ExtractName(IDbCommand command)
		{
			this.ConfigureQueryWithIdParameter(command);

			// ExecuteScalar returns the very first result from the Database.
			// ExecuteReader returns an object that can be looped over to
			//  retrieve all matching objects from the query (if you are expecting multiple results).

			return command.ExecuteScalar().ToString(); 
		}

		private void ConfigureQueryWithIdParameter(IDbCommand command)
		{
			command.CommandText = SelectNameCommand;
			var idParameter = command.CreateParameter();
			idParameter.ParameterName = FirstParameter;
			idParameter.Value = this.personId;
			command.Parameters.Add(idParameter);
		}
	}
}
