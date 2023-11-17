using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sienar.Validation;

public class RequireUpperAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var displayName = validationContext.DisplayName;
		var memberName = new [] { validationContext.MemberName! };

		if (value == null)
		{
			return new ValidationResult($"{displayName} cannot be null", memberName);
		}

		var stringVal = value.ToString();
		return Regex.IsMatch(stringVal!, "[A-Z]") ? ValidationResult.Success : new ValidationResult(ErrorMessage, memberName);
	}
}