#nullable disable

using Microsoft.EntityFrameworkCore;
using Sienar;

namespace Project.Data;

public class AppDbContext : SienarDbContext<AppUser>
{
	/// <inheritdoc />
	public AppDbContext(DbContextOptions options)
		: base(options) {}
}