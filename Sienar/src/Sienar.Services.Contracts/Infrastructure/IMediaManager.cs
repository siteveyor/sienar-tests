﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sienar.Infrastructure;

public interface IMediaManager
{
	/// <summary>
	/// Writes a file to the file storage medium
	/// </summary>
	/// <param name="path">The path to the file</param>
	/// <param name="fileContents">The contents of the file</param>
	/// <returns>whether the write was successful</returns>
	Task<bool> WriteFile(string path, IFormFile fileContents);

	/// <summary>
	/// Deletes a file from the file storage medium
	/// </summary>
	/// <param name="path">The path to the file</param>
	/// <returns>whether the deletion was successful</returns>
	Task<bool> DeleteFile(string path);

	/// <summary>
	/// Creates a filename based on the provided data
	/// </summary>
	/// <param name="fileTitle">The user-supplied title of the file</param>
	/// <param name="fileExtension">The extension of the file</param>
	/// <param name="type">The <see cref="MediaType"/> of the file</param>
	/// <returns>the full filename</returns>
	string GetFilename(
		string fileTitle,
		string? fileExtension,
		MediaType type);
}