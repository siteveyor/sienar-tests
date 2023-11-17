namespace Sienar;

public abstract class UnitTestBase<TSut> where TSut : class
{
	protected TSut Sut = default!;
}

public abstract class UnitTestBase<TSut, TMocks> : UnitTestBase<TSut>
	where TSut : class
	where TMocks : new()
{
	protected TMocks Mocks = new();
}