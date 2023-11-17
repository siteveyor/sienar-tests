namespace Sienar.Infrastructure;

public class MediaDtoAdapter : IDtoAdapter<Medium, MediumDto>
{
	/// <inheritdoc />
	public void MapToEntity(MediumDto dto, Medium entity) => throw new System.NotImplementedException();

	/// <inheritdoc />
	public MediumDto MapToDto(Medium entity) => throw new System.NotImplementedException();
}