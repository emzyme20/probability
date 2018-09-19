using System.Linq;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using FluentAssertions;

using Probability.Core.Validation;
using Probability.Models;

using Xunit;

namespace Test.Probability.Core.Validation
{
    public class CalculateModelValidatorTests
    {
        private readonly IFixture _fixture;

        public CalculateModelValidatorTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public void CalculateModel_WithNoCalculatorType()
        {
            var model = _fixture.Build<CalculatorModel>()
                                .Without(m => m.Calculator)
                                .With(m => m.Left, 0.8)
                                .With(m => m.Right, 0.1)
                                .Create();

            var sut = _fixture.Create<CalculateModelValidator>();

            var result = sut.Validate(model);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.FirstOrDefault()?.ErrorMessage.Should().Be("You need to choose the calculator type.");
        }

        [Fact]
        public void CalculateModel_WithInvalidCalculatorType()
        {
            var model = _fixture.Build<CalculatorModel>()
                                .With(m => m.Calculator, "Medium")
                                .With(m => m.Left, 0.99)
                                .With(m => m.Right, 1)
                                .Create();

            var sut = _fixture.Create<CalculateModelValidator>();

            var result = sut.Validate(model);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.FirstOrDefault()?.ErrorMessage.Should().Be("You need to choose the calculator type.");
        }

        [Fact]
        public void CalculateModel_WithLeftOutOfRange()
        {
            var model = _fixture.Build<CalculatorModel>()
                                .With(m => m.Calculator, "Either")
                                .With(m => m.Left, -0.8)
                                .With(m => m.Right, 1)
                                .Create();

            var sut = _fixture.Create<CalculateModelValidator>();

            var result = sut.Validate(model);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.FirstOrDefault()?.ErrorMessage.Should().Be("Your left input must be within the range 0-1 (inclusive)");
        }

        [Fact]
        public void CalculateModel_WithRightOutOfRange()
        {
            var model = _fixture.Build<CalculatorModel>()
                                .With(m => m.Calculator, "Combine")
                                .With(m => m.Left, 0.25)
                                .With(m => m.Right, 1.5)
                                .Create();

            var sut = _fixture.Create<CalculateModelValidator>();

            var result = sut.Validate(model);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.FirstOrDefault()?.ErrorMessage.Should().Be("Your right input must be within the range 0-1 (inclusive)");
        }
    }
}
