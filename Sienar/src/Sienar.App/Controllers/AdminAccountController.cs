using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sienar.Identity;
using Sienar.Infrastructure;

namespace Sienar.Controllers;

[ApiController]
[Route("sienar/admin-account")]
[Authorize(Roles = Roles.Admin)]
public class AdminAccountController
	: SienarControllerBase<AdminAccountController, IAdminAccountService>
{
	/// <inheritdoc />
	public AdminAccountController(
		ILogger<AdminAccountController> logger,
		IAdminAccountService service)
		: base(logger, service) {}

	[HttpGet]
	public Task<IActionResult> GetAll([FromQuery] Filter filter)
		=> ProcessServiceCall(() => Service.Get(filter));

	[HttpGet("{userId:guid}")]
	public Task<IActionResult> GetById(Guid userId)
		=> ProcessServiceCall(() => Service.Get(userId));

	[HttpPost]
	public virtual Task<IActionResult> Create(SienarUserDto entity)
		=> ProcessServiceCall(() => Service.Add(entity));

	[HttpPut]
	public virtual Task<IActionResult> Update(Guid id, SienarUserDto entity)
	{
		return ProcessServiceCall(() => Service.Edit(entity));
	}

	[HttpDelete("{id:guid}")]
	public virtual Task<IActionResult> Delete(Guid id) 
		=> ProcessServiceCall(() => Service.Delete(id));

	[HttpPost("roles")]
	public Task<IActionResult> AddUserToRole(UserUpdateRolesDto dto)
		=> ProcessServiceCall(() => Service.AddUserToRole(dto));

	[HttpDelete("roles")]
	public Task<IActionResult> RemoveUserFromRole(UserUpdateRolesDto dto)
		=> ProcessServiceCall(() => Service.RemoveUserFromRole(dto));
}