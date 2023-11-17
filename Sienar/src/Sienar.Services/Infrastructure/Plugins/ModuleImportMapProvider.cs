using System.Collections.Generic;

namespace Sienar.Infrastructure.Plugins;

public class ModuleImportMapProvider : IModuleImportMapProvider
{
	protected readonly Dictionary<string, string> Mapping = new();

	/// <inheritdoc />
	public IModuleImportMapProvider Add(string importName, string importPath)
	{
		Mapping.Add(importName, importPath);
		return this;
	}

	/// <inheritdoc />
	public Dictionary<string, string> GetImportMap() => Mapping;
}