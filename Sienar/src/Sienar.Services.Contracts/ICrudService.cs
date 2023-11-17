using System;
using System.Threading.Tasks;
using Sienar.Infrastructure;

namespace Sienar;

public interface ICrudService<TDto>
{
	/// <summary>
	/// Creates a new entry in the backend
	/// </summary>
	/// <param name="model">The DTO representing the entity to create</param>
	/// <returns><c>Task</c></returns>
	Task<ServiceResult<Guid>> Add(TDto model);

	/// <summary>
	/// Updates an existing entity in the backend
	/// </summary>
	/// <param name="model">The DTO representing the entity to update</param>
	/// <returns>whether the edit operation was successful</returns>
	Task<ServiceResult<bool>> Edit(TDto model);

	/// <summary>
	/// Gets an entity by primary key
	/// </summary>
	/// <param name="id">The primary key of the entity to retrieve</param>
	/// <param name="filter">A <see cref="Filter"/> to specify included results</param>
	/// <returns>the requested <c>TModel</c></returns>
	Task<ServiceResult<TDto>> Get(
		Guid id,
		Filter? filter = null);

	/// <summary>
	/// Gets a list of all entities in the backend
	/// </summary>
	/// &lt;param name="filter"&gt;A &lt;see cref="Filter"/&gt; to specify included results&lt;/param&gt;
	/// <returns>a list of all entities in the database</returns>
	Task<ServiceResult<PagedDto<TDto>>> Get(Filter? filter = null);

	/// <summary>
	/// Deletes an entity by primary key
	/// </summary>
	/// <param name="id">The primary key of the entity to delete</param>
	/// <returns><c>Task</c></returns>
	Task<ServiceResult<bool>> Delete(Guid id);
}