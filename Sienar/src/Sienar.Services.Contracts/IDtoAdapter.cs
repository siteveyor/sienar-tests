namespace Sienar;

public interface IDtoAdapter<TEntity, TDto>
{
	/// <summary>
	/// Maps a DTO to a <c>TEntity</c>
	/// </summary>
	/// <param name="dto">The DTO to map to a <c>TEntity</c></param>
	/// <param name="entity">The <c>TEntity</c> to map the DTO to</param>
	public void MapToEntity(TDto dto, TEntity entity);

	/// <summary>
	/// Maps a <c>TEntity</c> to a new DTO
	/// </summary>
	/// <param name="entity">The <c>TEntity</c> to map to a DTO</param>
	/// <returns>the mapped DTO</returns>
	public TDto MapToDto(TEntity entity);
}