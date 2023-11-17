using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Sienar.Infrastructure.Plugins;

public interface IPlugin
{
	static virtual void SetupDependencies(WebApplicationBuilder builder) {}

	static virtual void SetupApp(WebApplication app) {}

	PluginData PluginData { get; }

	/// <summary>
	/// Performs any setup needed on each request
	/// </summary>
	Task Setup() => Task.CompletedTask;
}