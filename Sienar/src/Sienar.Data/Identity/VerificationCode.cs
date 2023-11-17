using System;
using System.ComponentModel.DataAnnotations;

namespace Sienar.Identity;

public class VerificationCode : EntityBase
{
	/// <summary>
	/// The unique verification code
	/// </summary>
	public Guid Code { get; set; }

	/// <summary>
	/// The type of the verification code
	/// </summary>
	[MaxLength(25)]
	public string Type { get; set; } = default!;

	/// <summary>
	/// The verification code expiration date
	/// </summary>
	public DateTime ExpiresAt { get; set; }
}