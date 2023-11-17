#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Identity;

public class SienarUserDto : EntityBase
{
	public string Username { get; set; }
	public string Email { get; set; }
	public string PendingEmail { get; set; }
	public string PhoneNumber { get; set; }

	[DataType(DataType.Password)]
	public string Password { get; set; }
	public bool EmailConfirmed { get; set; }
	public bool PhoneConfirmed { get; set; }
	public IEnumerable<SienarRoleDto> Roles { get; set; }
		= Array.Empty<SienarRoleDto>();
}