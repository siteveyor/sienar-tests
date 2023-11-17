using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sienar.Errors;

namespace Sienar.Infrastructure.States;

public class StateService<TContext>
	: CrudService<State, StateDto, TContext>, IStateService
	where TContext : DbContext
{
	public StateService(
		TContext context,
		IDtoAdapter<State, StateDto> adapter,
		IFilterProcessor<State> filterProcessor,
		IBotDetector botDetector)
		: base(context, adapter, filterProcessor, botDetector)
	{
		EntityName = "State";
	}

	protected override async Task ValidateState(StateDto model)
	{
		var existing = await EntitySet.FirstOrDefaultAsync(
			s => s.Id != model.Id && s.Name == model.Name);
		if (existing is not null)
		{
			throw new SienarConflictException(ErrorMessages.States.DuplicateName);
		}

		existing = await EntitySet.FirstOrDefaultAsync(s => s.Id != model.Id && s.Abbreviation == model.Abbreviation);
		if (existing is not null)
		{
			throw new SienarConflictException(ErrorMessages.States.DuplicateAbbreviation);
		}
	}
}