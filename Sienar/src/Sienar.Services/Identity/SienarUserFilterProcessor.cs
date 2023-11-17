using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sienar.Infrastructure;

namespace Sienar.Identity;

public class SienarUserFilterProcessor<TUser> : IFilterProcessor<TUser>
	where TUser : SienarUser
{
	public IQueryable<TUser> Search(IQueryable<TUser> dataset, Filter filter)
	{
		if (string.IsNullOrEmpty(filter.SearchTerm))
		{
			return dataset;
		}

		return dataset.Where(
			s => s.Username.Contains(filter.SearchTerm)
			|| s.Email.Contains(filter.SearchTerm)
			|| s.PendingEmail.Contains(filter.SearchTerm));
	}

	/// <inheritdoc />
	public IQueryable<TUser> ProcessIncludes(IQueryable<TUser> dataset, Filter filter)
	{
		if (filter.Includes is null || !filter.Includes.Any())
		{
			return dataset;
		}

		if (filter.Includes.Contains(nameof(SienarUser.Roles)))
		{
			dataset = dataset.Include(u => u.Roles);
		}

		return dataset;
	}

	/// <inheritdoc />
	public Expression<Func<TUser, object>> GetSortPredicate(string? sortName) => sortName switch
	{
		nameof(SienarUser.Username) => u => u.Username,
		nameof(SienarUser.Email) => u => u.Email,
		nameof(SienarUser.PendingEmail) => u => u.PendingEmail,
		_ => u => u.Username
	};
}