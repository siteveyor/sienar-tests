using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sienar.Infrastructure;

public class MediumFilterProcessor : IFilterProcessor<Medium>
{
	/// <inheritdoc />
	public IQueryable<Medium> Search(IQueryable<Medium> dataset, Filter filter)
	{
		if (string.IsNullOrEmpty(filter.SearchTerm))
		{
			return dataset;
		}

		return dataset.Where(
			m => m.Title.Contains(filter.SearchTerm)
			|| m.Path.Contains(filter.SearchTerm)
			|| m.Description.Contains(filter.SearchTerm));
	}

	/// <inheritdoc />
	public Expression<Func<Medium, object>> GetSortPredicate(string? sortName) => sortName switch
	{
		nameof(MediumDto.UploadedAt) => m => m.UploadedAt,
		nameof(MediumDto.MediaType) => m => m.MediaType,
		_ => m => m.Title
	};
}