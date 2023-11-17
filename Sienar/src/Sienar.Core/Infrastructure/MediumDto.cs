using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Sienar.Infrastructure;

public class MediumDto : EntityBase
{
#nullable disable
	/// <summary>
	/// The title of the image to display
	/// </summary>
	[MaxLength(100)]
	public string Title { get; set; }

	/// <summary>
	/// The path to the file
	/// </summary>
	[MaxLength(300)]
	public string Path { get; set; }

	/// <summary>
	/// The description of the file
	/// </summary>
	[MaxLength(300)]
	public string Description { get; set; }
#nullable enable

	/// <summary>
	/// The type of the medium
	/// </summary>
	public MediaType MediaType { get; set; }

	/// <summary>
	/// The date and time the medium was created. Only used when reading the medium
	/// </summary>
	public DateTime UploadedAt { get; set; }

	/// <summary>
	/// Indicates whether the file should be considered private to the user who uploaded it
	/// </summary>
	public bool IsPrivate { get; set; }

	/// <summary>
	/// The GUID of the user who uploaded the file
	/// </summary>
	public Guid? UserId { get; set; }

	/// <summary>
	/// Indicates whether the file should be owned by the user who uploaded it. Ignored on non-admin accounts
	/// </summary>
	public bool IsUnassigned { get; set; }

	/// <summary>
	/// The contents of the file. Only used during initial upload
	/// </summary>
	public IFormFile? Contents { get; set; }
}