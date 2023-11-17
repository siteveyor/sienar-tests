using System;

namespace Sienar;

/// <summary>
/// Represents a DTO that has honeypot capabilities
/// </summary>
public class HoneypotDto : EntityBase
{
	/// <summary>
	/// Used to detect spambot submissions
	/// </summary>
	public string? SecretKeyField { get; set; }

	/// <summary>
	/// Used to determine how long a form took an agent to complete
	/// </summary>
	public TimeSpan TimeToComplete { get; set; }

	/// <summary>
	/// Used to determine whether the honeypot captured a spambot or not
	/// </summary>
	public bool IsSpambot => !string.IsNullOrEmpty(SecretKeyField);
}