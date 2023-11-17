using System;

namespace Sienar;

public class UnitTestWithDbContextBase<TSut> : UnitTestBase<TSut>, IDisposable
	where TSut : class
{
	protected readonly TestDbContext Db = TestDbContext.Create();

	public void Dispose()
	{
		Db.Dispose();
		GC.SuppressFinalize(this);
	}
}

public class UnitTestWithDbContextBase<TSut, TMocks>
	: UnitTestBase<TSut, TMocks>, IDisposable
	where TSut : class
	where TMocks : new()
{
	protected readonly TestDbContext Db = TestDbContext.Create();

	public void Dispose()
	{
		Db.Dispose();
		GC.SuppressFinalize(this);
	}
}