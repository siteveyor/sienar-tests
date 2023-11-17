namespace Sienar.Infrastructure.States;

public class StateDtoAdapter : IDtoAdapter<State, StateDto>
{
	/// <inheritdoc />
	public void MapToEntity(StateDto dto, State entity)
	{
		entity.Name = dto.Name;
		entity.Abbreviation = dto.Abbreviation;
	}

	/// <inheritdoc />
	public StateDto MapToDto(State entity)
	{
		return new StateDto
		{
			Id = entity.Id,
			Name = entity.Name,
			Abbreviation = entity.Abbreviation,
			ConcurrencyStamp = entity.ConcurrencyStamp
		};
	}
}