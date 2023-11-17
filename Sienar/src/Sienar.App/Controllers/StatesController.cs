using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sienar.Infrastructure;
using Sienar.Infrastructure.States;

namespace Sienar.Controllers;

[ApiController]
[Route("sienar/states")]
[Authorize(Roles = Roles.Admin)]
public class StatesController
	: SienarControllerBase<StatesController, IStateService>
{
	public StatesController(
		ILogger<StatesController> logger,
		IStateService service)
		: base(logger, service) {}

	[HttpGet]
	[AllowAnonymous]
	public virtual Task<IActionResult> GetAll([FromQuery] Filter filter)
		=> ProcessServiceCall(() => Service.Get(filter));

	[HttpGet("{id:guid}")]
	[AllowAnonymous]
	public virtual Task<IActionResult> GetById(Guid id)
		=> ProcessServiceCall(() => Service.Get(id));

	[HttpPost]
	public virtual Task<IActionResult> Create(StateDto entity)
		=> ProcessServiceCall(() => Service.Add(entity));

	[HttpPut]
	public virtual Task<IActionResult> Update(Guid id, StateDto entity)
	{
		return ProcessServiceCall(() => Service.Edit(entity));
	}

	[HttpDelete("{id:guid}")]
	public virtual Task<IActionResult> Delete(Guid id) 
		=> ProcessServiceCall(() => Service.Delete(id));
}