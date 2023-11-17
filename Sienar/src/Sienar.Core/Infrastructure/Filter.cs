namespace Sienar.Infrastructure;

public class Filter
{
	public string? SearchTerm { get; set; }
	public string? SortName { get; set; }
	public bool? SortDescending { get; set; }
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 5;
	public string[]? Includes { get; set; }
}