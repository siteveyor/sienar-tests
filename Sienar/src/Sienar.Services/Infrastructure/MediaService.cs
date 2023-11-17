using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sienar.Errors;

namespace Sienar.Infrastructure;

public class MediaService<TContext>
	: CrudService<Medium, MediumDto, TContext>, IMediaService
	where TContext : DbContext
{
	protected readonly IMediaManager MediaManager;
	protected readonly IUserAccessor UserAccessor;

	/// <inheritdoc />
	public MediaService(
		TContext context,
		IDtoAdapter<Medium, MediumDto> adapter,
		IFilterProcessor<Medium> filterProcessor,
		IBotDetector botDetector,
		IMediaManager mediaManager,
		IUserAccessor userAccessor)
		: base(context, adapter, filterProcessor, botDetector)
	{
		MediaManager = mediaManager;
		UserAccessor = userAccessor;

		EntityName = "Media";
	}

	/// <inheritdoc />
	public override async Task<ServiceResult<Guid>> Add(MediumDto model)
	{
		if (!(UserAccessor.UserInRole(Roles.Admin) && model.IsUnassigned))
		{
			model.UserId = UserAccessor.GetUserId();
		}
		model.UploadedAt = DateTime.Now;

		if (model.Contents is not null)
		{
			model.Path = MediaManager.GetFilename(
            			model.Title,
            			model.Contents?.FileName,
            			model.MediaType);

			await MediaManager.WriteFile(model.Path, model.Contents!);
		}

		return await base.Add(model);
	}

	/// <inheritdoc />
	public override async Task<ServiceResult<bool>> Edit(MediumDto model)
	{
		var file = await EntitySet.FindAsync(model.Id);
		if (file is null)
		{
			return ServiceResult<bool>.NotFound();
		}

		VerifyCanEditFile(file.UserId);

		return await base.Edit(model);
	}

	/// <inheritdoc />
	public override async Task<ServiceResult<MediumDto>> Get(Guid id, Filter? filter = null)
	{
		var fileResult = await base.Get(id, filter);
		if (!fileResult.WasSuccessful)
		{
			return fileResult;
		}

		VerifyCanReadFile(fileResult.Result!.UserId, fileResult.Result!.IsPrivate);

		return fileResult;
	}

	// <inheritdoc />
	// TODO: when Filter is made generic, add extra filter to only return this user's files
	// public override Task<ServiceResult<PagedDto<MediumDto>>> Get(Filter? filter = null) => base.Get(filter);

	/// <inheritdoc />
	public override async Task<ServiceResult<bool>> Delete(Guid id)
	{
		var file = await Get(id);
		if (!file.WasSuccessful)
		{
			return ServiceResult<bool>.Ok();
		}
	
		VerifyCanEditFile(file.Result!.UserId);
	
		return await base.Delete(id);
	}

	protected virtual bool CanReadFile(Guid? ownerId, bool isPrivate)
	{
		if (UserAccessor.UserInRole(Roles.Admin))
		{
			return true;
		}

		if (ownerId is null)
		{
			return true;
		}

		if (UserAccessor.GetUserId() == ownerId)
		{
			return true;
		}

		return !isPrivate;
	}

	protected virtual bool CanEditFile(Guid? ownerId)
	{
		if (UserAccessor.UserInRole(Roles.Admin))
		{
			return true;
		}

		return UserAccessor.GetUserId() == ownerId;
	}

	protected virtual void VerifyCanReadFile(Guid? ownerId, bool isPrivate)
	{
		if (!CanReadFile(ownerId, isPrivate))
		{
			throw new SienarForbiddenException("You do not have permission to access the specified file.");
		}
	}

	protected virtual void VerifyCanEditFile(Guid? ownerId)
	{
		if (!CanEditFile(ownerId))
		{
			throw new SienarForbiddenException("You do not have permission to modify the specified file.");
		}
	}
}