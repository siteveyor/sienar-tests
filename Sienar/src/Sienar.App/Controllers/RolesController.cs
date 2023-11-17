using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sienar.Identity;

namespace Sienar.Controllers;

[ApiController]
[Route("sienar/roles")]
[Authorize]
public class RolesController
	: SienarControllerBase<RolesController, IRoleService>
{
	/// <inheritdoc />
	public RolesController(
		ILogger<RolesController> logger,
		IRoleService service)
		: base(logger, service) {}

	[HttpGet]
	public Task<IActionResult> GetAll()
		=> ProcessServiceCall(() => Service.Get());
}