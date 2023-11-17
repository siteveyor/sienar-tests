using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Sienar.Configuration;

public static class SienarServiceCollectionExtensions
{
	public static void AddSienarCrudService<
		TEntity,
		TDto,
		TMapperImplementation,
		TService,
		TServiceImplementation>(this IServiceCollection services)
		where TDto : EntityBase
		where TMapperImplementation : class, IDtoAdapter<TEntity, TDto>
		where TService : class
		where TServiceImplementation : class, TService
	{
		services.TryAddScoped<IDtoAdapter<TEntity, TDto>, TMapperImplementation>();
		services.TryAddScoped<TService, TServiceImplementation>();
		if (typeof(TService).IsAssignableTo(typeof(ICrudService<TDto>)))
		{
			services.TryAddScoped<ICrudService<TDto>>(
				sp => (ICrudService<TDto>)sp.GetRequiredService<TService>());
		}
	}

	public static void AddSienarCrudService(
		this IServiceCollection services,
		Type entityType,
		Type dtoType,
		Type mapperImplementationType,
		Type serviceType,
		Type serviceImplementationType)
	{
		typeof(SienarServiceCollectionExtensions)
			.GetMethod(
				nameof(AddSienarCrudService),
				5,
				BindingFlags.Public | BindingFlags.Static,
				null,
				new[] { typeof(IServiceCollection) },
				Array.Empty<ParameterModifier>())!
			.MakeGenericMethod(
				entityType,
				dtoType,
				mapperImplementationType,
				serviceType,
				serviceImplementationType)
			.Invoke(null, new object[] {services});
	}

	public static SienarGenericTypeOptions GetSienarGenerictypeOptions(this IServiceCollection self)
	{
		var typeOptionsDescriptor = self
			.First(s => s.ServiceType == typeof(SienarGenericTypeOptions));
		return (SienarGenericTypeOptions)typeOptionsDescriptor.ImplementationInstance!;
	}

	public static TService? GetAndRemoveService<TService>(this IServiceCollection self)
	{
		var service = self.FirstOrDefault(
			s => s.ServiceType == typeof(TService));
		if (service is not null)
		{
			self.Remove(service);
		}

		return (TService?)service?.ImplementationInstance;
	}

	public static void RemoveService<TService>(this IServiceCollection self)
	{
		var service = self.FirstOrDefault(
			s => s.ServiceType == typeof(TService));
		if (service is not null)
		{
			self.Remove(service);
		}
	}
}