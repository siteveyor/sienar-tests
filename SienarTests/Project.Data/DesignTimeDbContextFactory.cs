using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Project.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	/// <inheritdoc />
	public AppDbContext CreateDbContext(string[] args)
	{
		var builder = new DbContextOptionsBuilder<AppDbContext>();
		var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new NullReferenceException("Database connection string cannot be null.");
		}

		builder.UseMySql(
			connectionString,
			ServerVersion.AutoDetect(connectionString));

		return new AppDbContext(builder.Options);
	}
}