using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sienar.Infrastructure;

namespace Sienar;

public abstract class CrudService<TEntity, TDto, TContext>
	: ICrudService<TDto>
	where TEntity : EntityBase, new()
	where TDto : EntityBase
	where TContext : DbContext
{
	protected string? EntityName;
	protected readonly TContext Context;
	protected readonly IDtoAdapter<TEntity, TDto> Adapter;
	protected readonly IFilterProcessor<TEntity> FilterProcessor;
	protected readonly IBotDetector BotDetector;
	protected DbSet<TEntity> EntitySet => Context.Set<TEntity>();

	public CrudService(
		TContext context,
		IDtoAdapter<TEntity, TDto> adapter,
		IFilterProcessor<TEntity> filterProcessor,
		IBotDetector botDetector)
	{
		Context = context;
		Adapter = adapter;
		FilterProcessor = filterProcessor;
		BotDetector = botDetector;
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult<Guid>> Add(TDto model)
	{
		DetectHoneypot(model);
		await ValidateState(model);

		var entity = new TEntity();
		Adapter.MapToEntity(model, entity);
		await EntitySet.AddAsync(entity);
		await Context.SaveChangesAsync();
		return ServiceResult<Guid>.Ok(entity.Id, CreateSuccessMessage("added"));
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult<bool>> Edit(TDto model)
	{
		DetectHoneypot(model);
		await ValidateState(model);

		var entity = await EntitySet.FindAsync(model.Id);
		if (entity is null)
		{
			return ServiceResult<bool>.NotFound();
		}

		if (entity.ConcurrencyStamp != model.ConcurrencyStamp)
		{
			return ServiceResult<bool>.ConcurrencyConflict();
		}

		Adapter.MapToEntity(model, entity);
		entity.ConcurrencyStamp = Guid.NewGuid();

		EntitySet.Update(entity);
		await Context.SaveChangesAsync();

		return ServiceResult<bool>.Ok(true, CreateSuccessMessage("updated"));
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult<TDto>> Get(
		Guid id,
		Filter? filter = null)
	{
		var entity = filter == null
			? await EntitySet.FindAsync(id)
			: await FilterProcessor.ProcessIncludes(EntitySet, filter)
				.FirstOrDefaultAsync(u => u.Id == id);
		if (entity is null)
		{
			return ServiceResult<TDto>.NotFound();
		}

		var mapped = Adapter.MapToDto(entity);

		return ServiceResult<TDto>.Ok(mapped);
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult<PagedDto<TDto>>> Get(Filter? filter = null)
	{
		var entries = filter is null
			? EntitySet
			: ProcessFilter(filter);

		var items = new List<TDto>();

		foreach (var entry in entries)
		{
			items.Add(Adapter.MapToDto(entry));
		}

		IQueryable<TEntity> countEntries = EntitySet;
		if (filter is not null)
		{
			countEntries = FilterProcessor.Search(countEntries, filter);
		}

		var dto = new PagedDto<TDto>
		{
			Items = items,
			TotalCount = await countEntries.CountAsync()
		};

		return ServiceResult<PagedDto<TDto>>.Ok(dto);
	}

	/// <inheritdoc />
	public virtual async Task<ServiceResult<bool>> Delete(Guid id)
	{
		var entity = await EntitySet.FindAsync(id);
		if (entity is null)
		{
			return ServiceResult<bool>.Ok(true);
		}

		await PrepareEntityForDeletion(entity);

		EntitySet.Remove(entity);
		await Context.SaveChangesAsync();
		return ServiceResult<bool>.Ok(true, CreateSuccessMessage("deleted"));
	}

	/// <summary>
	/// Validates that a DTO does not violate logical rules of the app state (for example, checking fields for uniqueness against the database)
	/// </summary>
	/// <param name="model">The DTO to validate against the current app state</param>
	protected virtual Task ValidateState(TDto model) => Task.CompletedTask;

	/// <summary>
	/// Executes any logic necessary to clean up the database or other app state prior to deleting an entity
	/// </summary>
	/// <param name="entity">The entity to prepare to delete</param>
	protected virtual Task PrepareEntityForDeletion(TEntity entity) => Task.CompletedTask;

	protected void DetectHoneypot(TDto model)
	{
		if (model is not HoneypotDto honeypot)
		{
			return;
		}

		BotDetector.DetectSpambot(honeypot);
	}

	protected IQueryable<TEntity> ProcessFilter(
		Filter filter,
		Expression<Func<TEntity, bool>>? predicate = null)
	{
		var result = (IQueryable<TEntity>)EntitySet;
		if (predicate is not null)
		{
			result = result.Where(predicate);
		}

		result = FilterProcessor.Search(result, filter);
		result = FilterProcessor.ProcessIncludes(result, filter);
		var sortPredicate = FilterProcessor.GetSortPredicate(filter.SortName);
		result = filter.SortDescending ?? false
			         ? result.OrderByDescending(sortPredicate)
			         : result.OrderBy(sortPredicate);

		if (filter.Page > 1)
		{
			result = result.Skip((filter.Page - 1) * filter.PageSize);
		}

		// If filter.PageSize == 0, return all results
		if (filter.PageSize > 0)
		{
			result = result.Take(filter.PageSize);
		}

		return result;
	}

	protected string? CreateSuccessMessage(string operationVerb)
	{
		return string.IsNullOrEmpty(EntityName)
			? null
			: $"{EntityName} {operationVerb} successfully!";
	}
}