using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sienar.Infrastructure;

namespace Sienar.Identity;

public class RoleService<TContext> : IRoleService
	where TContext : DbContext
{
	protected readonly IDtoAdapter<SienarRole, SienarRoleDto> Adapter;
	protected readonly TContext Context;

	public RoleService(
		IDtoAdapter<SienarRole, SienarRoleDto> adapter, TContext context)
	{
		Adapter = adapter;
		Context = context;
	}

	/// <inheritdoc />
	public async Task<ServiceResult<IEnumerable<SienarRoleDto>>> Get()
	{
		var roles = await Context.Set<SienarRole>()
			.ToListAsync();
		var mapped = roles.Select(r => Adapter.MapToDto(r));
		return ServiceResult<IEnumerable<SienarRoleDto>>.Ok(mapped);
	}
}