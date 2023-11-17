#nullable disable

using Microsoft.EntityFrameworkCore;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Infrastructure.States;

namespace Sienar;

public class SienarDbContext<TUser>
	: DbContext, ISienarDbContext<TUser>
	where TUser : SienarUser
{
	/// <inheritdoc />
	public SienarDbContext(DbContextOptions options) : base(options) {}

	/// <inheritdoc />
	public virtual DbSet<TUser> Users { get; set; }

	/// <inheritdoc />
	public virtual DbSet<SienarRole> Roles { get; set; }

	/// <inheritdoc />
	public virtual DbSet<State> States { get; set; }

	/// <inheritdoc />
	public virtual DbSet<Medium> Files { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new SienarUserEntityTypeConfiguration<TUser>());
	}
}