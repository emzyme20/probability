using System;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Probability.Core.Calculations;

using Xunit;

namespace Test.Probability.Core.Calculations
{
    public class CombineCalculatorTests
    {
        private readonly IFixture _fixture;

        public CombineCalculatorTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

            _fixture.Register<ICombineCalculator>(() => new CombineCalculator(_fixture.Create<ILogger<CombineCalculator>>()));
        }

        [Theory]
        [InlineData(0.8, 0.8, 0.640)]
        [InlineData(0.5, 0.75, 0.375)]
        [InlineData(0, 1, 0)]
        public void CombinedCalculate_InRangeValues_ReturnsResult(double left, double right, double expected)
        {
            var sut = _fixture.Create<ICombineCalculator>();

            var result = sut.Calculate(left, right);

            Math.Round(result, 3).Should().Be(expected);
        }

        [Fact]
        public void CombinedCalculate_OneOutOfRangeValue_WillThrowArgumentOutOfRangeException()
        {
            var sut = _fixture.Create<ICombineCalculator>();

            Action act = () => sut.Calculate(1, 1.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CombinedCalculate_BothOutOfRangeValue_WillThrowArgumentOutOfRangeException()
        {
            var sut = _fixture.Create<ICombineCalculator>();

            Action act = () => sut.Calculate(1.1, 1.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CombinedCalculate_NegativeValueOutOfRange_WillThrowArgumentOutOfRangeException()
        {
            var sut = _fixture.Create<ICombineCalculator>();

            Action act = () => sut.Calculate(-0.1, 0.5);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
