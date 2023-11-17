using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sienar.Infrastructure.States;
using Sienar.Identity;

namespace Sienar;

public class TestDbContext
	: SienarDbContext<TestUser, TestRole, State>
{
	private readonly SqliteConnection _cnx;

	/// <inheritdoc />
	public TestDbContext(
		DbContextOptions options,
		SqliteConnection cnx) : base(options)
	{
		_cnx = cnx;
	}

	public static TestDbContext Create()
	{
		var cnx = new SqliteConnection("Filename=:memory:");
		cnx.Open();

		var options = new DbContextOptionsBuilder()
			.UseSqlite(cnx)
			.Options;

		var db = new TestDbContext(options, cnx);
		db.Database.EnsureCreated();
		return db;
	}

	/// <inheritdoc />
	public override void Dispose()
	{
		_cnx.Dispose();
		base.Dispose();
		GC.SuppressFinalize(this);
	}

#region Setup

	public void AddUser(TestUser user)
	{
		user.Username ??= "username";
		user.Email ??= "test@mail.com";
		Users.Add(user);
		SaveChanges();
	}

#endregion
}