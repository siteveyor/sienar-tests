namespace Sienar.Identity;

public class RoleDtoAdapter : IDtoAdapter<SienarRole, SienarRoleDto>
{
	/// <inheritdoc />
	public void MapToEntity(SienarRoleDto dto, SienarRole entity)
	{
		entity.Name = dto.Name;
	}

	/// <inheritdoc />
	public SienarRoleDto MapToDto(SienarRole entity)
	{
		return new SienarRoleDto
		{
			Id = entity.Id,
			ConcurrencyStamp = entity.ConcurrencyStamp,
			Name = entity.Name
		};
	}
}