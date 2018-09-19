using FluentValidation;

using Probability.Models;

namespace Probability.Core.Validation
{
    public class CalculateModelValidator : AbstractValidator<CalculatorModel>
    {
        public CalculateModelValidator()
        {
            RuleFor(x => x.Calculator)
                .SetValidator(new CalculatorTypeEnumValidator())
                .WithMessage("You need to choose the calculator type.");

            RuleFor(model => model.Left)
                .InclusiveBetween(0, 1)
                .WithMessage("Your left input must be within the range 0-1 (inclusive)");

            RuleFor(model => model.Right)
                .InclusiveBetween(0, 1)
                .WithMessage("Your right input must be within the range 0-1 (inclusive)");
        }
    }
}
