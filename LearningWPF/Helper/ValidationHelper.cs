using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace LearningWPF.Helper
{
    // Visit: https://stackoverflow.com/questions/127477/detecting-wpf-validation-errors#answer-128346
    //        https://stackoverflow.com/questions/338522/getlocalvalueenumerator-not-returning-all-properties#4645998  
    internal static class ValidationHelper
    {
        private static readonly Type[] ValidableFrameworkElements =
            { typeof(CheckBox), typeof(ComboBox), typeof(TextBox), typeof(ToggleButton) };

        /// <summary>
        /// When the user clicks the Save button, this method ensures that there are no data loading errors by checking
        /// the validation rules one by one, before continuing with the save.
        /// </summary>
        /// <param name="parent">Parent control from which to begin validation.</param>
        /// <param name="feUsesDataTemplate">Flag indicating whether the FrameworkElement to validate uses DataTemplate in its definition.</param>
        /// <returns>True is there is no error in any validation rule, otherwise False.</returns>
        public static bool IsValid(DependencyObject parent, bool feUsesDataTemplate = true)
        {
            return feUsesDataTemplate ? IsValidSlow(parent) : IsValidFast(parent);
        }

        /// <summary>
        /// When the user clicks the Save button, this method ensures that there are no data loading errors by checking
        /// the validation rules one by one, before continuing with the save.
        /// </summary>
        /// <param name="parent">Parent control from which to begin validation.</param>
        /// <returns>True is there is no error in any validation rule, otherwise False.</returns>
        private static bool IsValidSlow(DependencyObject parent)
        {
            bool valid = true;
            if (parent is UIElement element && element.Visibility != Visibility.Visible) return valid;

            // Enable for debugging
            // Type parentType = parent.GetType();

            // For better performance restrict validation to only validable controls
            if (ValidableFrameworkElements.Contains(parent.GetType()))
            {
                // Validate all the bindings on the parent        
                // Alternative flags: BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static
                IEnumerable<FieldInfo> fieldInfos = parent.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(p => p.FieldType == typeof(DependencyProperty));

                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    //Debug.WriteLine($"fieldInfo.Name: {parent.GetType()}*{fieldInfo.Name}");

                    if (fieldInfo.GetValue(null) is not DependencyProperty dp
                        || !BindingOperations.IsDataBound(parent, dp)) continue;

                    Binding? binding = BindingOperations.GetBinding(parent, dp);
                    if (binding == null || binding.ValidationRules.Count == 0) continue;

                    foreach (ValidationRule rule in binding.ValidationRules)
                    {
                        ValidationResult result = rule.Validate(parent.GetValue(dp), null);
                        BindingExpression expression = BindingOperations.GetBindingExpression(parent, dp)!;
                        if (result.IsValid) continue;       // Alternatively: Validation.ClearInvalid(expression);

                        Validation.MarkInvalid(expression,
                            new ValidationError(rule, expression, result.ErrorContent, null));
                        valid = false;
                    }
                }
            }

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsValidSlow(child)) { valid = false; }
            }

            return valid;
        }

        /// <summary>
        /// When the user clicks the Save button, this method ensures that there are no data loading errors by checking
        /// the validation rules one by one, before continuing with the save.
        /// BUT it doesn't work because GetLocalValueEnumerator doesn't get the DependencyProperties for controls inside a DataTemplate.
        /// </summary>
        /// <param name="parent">Parent control from which to begin validation.</param>
        /// <returns>True is there is no error in any validation rule, otherwise False.</returns>
        private static bool IsValidFast(DependencyObject parent)
        {
            bool valid = true;
            if (parent is UIElement element && element.Visibility != Visibility.Visible) return valid;

            // Validate all the bindings on the parent
            LocalValueEnumerator localValues = parent.GetLocalValueEnumerator();
            while (localValues.MoveNext())
            {
                LocalValueEntry entry = localValues.Current;

                Debug.WriteLine($"entry.Property.Name: {parent.DependencyObjectType}*{entry.Property.Name}");

                if (!BindingOperations.IsDataBound(parent, entry.Property)) continue;

                Binding binding = BindingOperations.GetBinding(parent, entry.Property)!;

                if (binding.ValidationRules.Count == 0) continue;

                foreach (ValidationRule rule in binding.ValidationRules)
                {
                    ValidationResult result = rule.Validate(parent.GetValue(entry.Property), null);
                    if (result.IsValid) continue;           // Alternatively: Validation.ClearInvalid(expression);

                    BindingExpression expression = BindingOperations.GetBindingExpression(parent, entry.Property)!;
                    Validation.MarkInvalid(expression,
                        new ValidationError(rule, expression, result.ErrorContent, null));
                    valid = false;
                }
            }

            // Validate all the bindings on the children
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsValidFast(child)) { valid = false; }
            }

            return valid;
        }
    }
}
