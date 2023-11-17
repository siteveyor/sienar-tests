using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Constants;
using Sienar.Identity;
using Sienar.Infrastructure;
using Sienar.Pages.Base;

namespace Sienar.Pages.Dashboard.Users;

public class Roles : ActionPageModelBase<IAdminAccountService>
{
	protected readonly IRoleService RoleService;

	/// <inheritdoc />
	public Roles(
		IAdminAccountService service,
		IToastService toastService,
		IRoleService roleService)
		: base(service, toastService)
	{
		RoleService = roleService;
		SuccessUrl = Urls.Dashboard.Users.UsersIndex;
	}

	[BindProperty(SupportsGet = true)]
	public Guid Id { get; set; }

	[BindProperty]
	public List<Guid> RoleIds { get; set; } = new();

	public SienarUserDto? Dto { get; set; }

	public IEnumerable<SienarRoleDto> AvailableRoles { get; set; } = Array.Empty<SienarRoleDto>();

	public async Task<IActionResult> OnGet()
	{
		Dto = (await Service.Get(Id)).Result;
		AvailableRoles = (await RoleService.Get()).Result ?? Array.Empty<SienarRoleDto>();
		if (Dto is not null)
		{
			RoleIds = Dto.Roles
				.Select(r => r.Id)
				.ToList();
		}

		return Page();
	}

	/// <inheritdoc />
	protected override async Task<ServiceResult<bool>> DoAction()
	{
		var original = (await Service.Get(Id)).Result;
		if (original is null)
		{
			return ServiceResult<bool>.NotFound();
		}

		// We need to copy the enumerable here to make sure that if roles are removed, the original enumerable is not modified
		foreach (var role in original.Roles.ToArray())
		{
			if (!RoleIds.Contains(role.Id))
			{
				await Service.RemoveUserFromRole(
					new()
					{
						RoleId = role.Id,
						UserId = original.Id
					});
			}
		}

		foreach (var roleId in RoleIds)
		{
			if (!original.Roles.Any(r => r.Id == roleId))
			{
				await Service.AddUserToRole(
					new()
					{
						RoleId = roleId,
						UserId = original.Id
					});
			}
		}

		return ServiceResult<bool>.Ok();
	}
}