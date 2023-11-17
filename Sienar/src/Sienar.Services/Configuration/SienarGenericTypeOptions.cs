#nullable disable

using System;

namespace Sienar.Configuration;

public class SienarGenericTypeOptions
{
	/// <summary>
	/// The <see cref="Type"/> of the user
	/// </summary>
	public Type UserType { get; set; }

	/// <summary>
	/// The <see cref="Type"/> of the DbContext
	/// </summary>
	public Type DbContextType { get; set; }
}