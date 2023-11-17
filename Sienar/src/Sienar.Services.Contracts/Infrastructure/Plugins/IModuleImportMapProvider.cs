using System.Collections.Generic;

namespace Sienar.Infrastructure.Plugins;

public interface IModuleImportMapProvider
{
	/// <summary>
	/// Adds a key to the Sienar import map rendered on each page
	/// </summary>
	/// <param name="importName">The name of the module imported from in a JS module</param>
	/// <param name="importPath">The path to which to map the named module</param>
	/// 
	/// <example>
	/// <para>
	/// To generate an import map with the following structure:
	/// </para>
	/// <code>
	/// &lt;script type="importmap"&gt;
	///     {
	///         "imports": {
	///             "react": "https://esm.sh/react@18.2.0/"
	///         }
	///     }
	/// &lt;/script&gt;
	/// </code>
	/// <para>
	/// You should create a mapping like so:
	/// </para>
	/// <code>
	///	importMapProvider.Add("react", "https://esm.sh/react@18.2.0/");
	/// </code>
	/// </example>
	IModuleImportMapProvider Add(string importName, string importPath);

	/// <summary>
	/// Retrieves a dictionary of all registered import mappings
	/// </summary>
	/// <returns>the import map dictionary</returns>
	Dictionary<string, string> GetImportMap();
}