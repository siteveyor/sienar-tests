using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sienar.Infrastructure;

namespace Sienar.Controllers;

[ApiController]
[Route("sienar/media")]
public class MediaController
	: SienarControllerBase<MediaController, IMediaService>
{
	public MediaController(
		ILogger<MediaController> logger,
		IMediaService service)
		: base(logger, service) {}

	[HttpGet]
	public virtual Task<IActionResult> GetAll([FromQuery] Filter filter)
		=> ProcessServiceCall(() => Service.Get(filter));

	[HttpGet("{id:guid}")]
	[AllowAnonymous]
	public virtual Task<IActionResult> GetById(Guid id)
		=> ProcessServiceCall(() => Service.Get(id));

	[HttpPost]
	public virtual Task<IActionResult> Create(MediumDto entity)
		=> ProcessServiceCall(() => Service.Add(entity));

	[HttpPut]
	public virtual Task<IActionResult> Update(MediumDto entity)
	{
		return ProcessServiceCall(() => Service.Edit(entity));
	}

	[HttpDelete("{id:guid}")]
	public virtual Task<IActionResult> Delete(Guid id) 
		=> ProcessServiceCall(() => Service.Delete(id));
}