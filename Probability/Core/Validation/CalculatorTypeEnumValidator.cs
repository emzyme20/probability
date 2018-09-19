using System;

using FluentValidation.Validators;

using Probability.Core.Calculations;

namespace Probability.Core.Validation
{
    public class CalculatorTypeEnumValidator : PropertyValidator
    {
        public CalculatorTypeEnumValidator()
            : base("Invalid CalculatorType")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue != null && Enum.TryParse(context.PropertyValue.ToString(), out CalculatorType calculatorType);
        }
    }
}
