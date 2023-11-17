#nullable disable
using System;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Infrastructure;

public class Medium : EntityBase
{
	/// <summary>
	/// The title of the image
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

	/// <summary>
	/// The type of the medium
	/// </summary>
	public MediaType MediaType { get; set; }

	/// <summary>
	/// The date and time the medium was created
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
}