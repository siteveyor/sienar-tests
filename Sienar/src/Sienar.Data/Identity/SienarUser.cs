#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sienar.Infrastructure;

namespace Sienar.Identity;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class SienarUser : EntityBase
{
#region Security

	/// <summary>
    /// Gets or sets the username
    /// </summary>
    [PersonalData]
    [MaxLength(32)]
    [Required]
    public string Username { get; set; }

	/// <summary>
	/// Gets or sets a salted and hashed representation of the password
	/// </summary>
	[MaxLength(100)]
	public string PasswordHash { get; set; }

	/// <summary>
	/// Gets or sets the number of failed login attempts
	/// </summary>
	public int LoginFailedCount { get; set; }

	/// <summary>
	/// Gets or sets the end date of the lockout period
	/// </summary>
	public DateTime? LockoutEnd { get; set; }

	/// <summary>
	/// Gets or sets whether the user has 2FA enabled
	/// </summary>
	public bool TwoFactorEnabled { get; set; }

	/// <summary>
	/// Gets or sets a list of verification codes
	/// </summary>
	public List<VerificationCode> VerificationCodes { get; set; }

	public List<SienarRole> Roles { get; set; }

#endregion

#region Contact information

    /// <summary>
    /// Gets or sets the email address
    /// </summary>
    [PersonalData]
    [MaxLength(100)]
    [Required]
    public string Email { get; set; }

	/// <summary>
	/// Gets or sets whether the email address for the user has been confirmed
	/// </summary>
	public bool EmailConfirmed { get; set; }

	/// <summary>
	/// Gets or sets the pending email address
	/// </summary>
	[PersonalData]
	[MaxLength(100)]
	public string PendingEmail { get; set; }

    /// <summary>
    /// Gets or sets the telephone number
    /// </summary>
    [PersonalData]
    [MaxLength(20)]
    public string PhoneNumber { get; set; }

	/// <summary>
	/// Gets or sets whether the phone number for the user has been confirmed
	/// </summary>
	public bool PhoneNumberConfirmed { get; set; }

#endregion

	public List<Medium> Media { get; set; }

	/// <inheritdoc/>
	public override string ToString() => Username;
}
