﻿using Bookify.Application.Abstractions.Data;
using Npgsql;
using System.Data;

namespace Bookify.Infrastructure.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
	private readonly string connectionString;

	public SqlConnectionFactory(string connectionString)
	{
		this.connectionString = connectionString;
	}

	public IDbConnection CreateConnection()
	{
		var connection = new NpgsqlConnection(connectionString);
		connection.Open();

		return connection;
	}
}
