using System.Globalization;
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
}
