#nullable disable

using Microsoft.EntityFrameworkCore;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Infrastructure.States;

namespace Sienar;

public interface ISienarDbContext<TUser>
	where TUser : SienarUser
{
	DbSet<TUser> Users { get; set; }
	DbSet<SienarRole> Roles { get; set; }
	DbSet<State> States { get; set; }
	DbSet<Medium> Files { get; set; }
}