using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace LearningWPF.ViewModels
{
    internal class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace(value?.ToString())
                ? new ValidationResult(false, "Required")
                : ValidationResult.ValidResult;
        }
    }

    internal class RegexValidationRule : ValidationRule
    {
        public string? Pattern { get; set; } = "";

        public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(Pattern)) return ValidationResult.ValidResult;

            Regex regex = new(Pattern);
            Match match = regex.Match(value?.ToString() ?? "");
            return match == Match.Empty
                ? new ValidationResult(false, "Invalid format.")
                : ValidationResult.ValidResult;
        }
    }
}
