using System;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Probability.Core.Calculations;

using Xunit;

namespace Test.Probability.Core.Calculations
{
    public class EitherCalculatorTests
    {
        private readonly IFixture _fixture;

        public EitherCalculatorTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

            _fixture.Register<ICombineCalculator>(() => new CombineCalculator(_fixture.Create<ILogger<CombineCalculator>>()));

            _fixture.Register<IEitherCalculator>(() => new EitherCalculator(_fixture.Create<ILogger<EitherCalculator>>()));
        }

        [Theory]
        [InlineData(0.8, 0.8, 0.96)]
        [InlineData(0.5, 0.75, 0.875)]
        [InlineData(0.5, 0.5, 0.75)]
        [InlineData(0, 1, 1)]
        public void EitherCalculate_InRangeValues_ReturnsResult(double left, double right, double expected)
        {
            var sut = _fixture.Create<IEitherCalculator>();

            var result = sut.Calculate(left, right);

            result.Should().Be(expected);
        }

        [Fact]
        public void EitherCalculate_OneOutOfRangeValue_WillThrowArgumentOutOfRangeException()
        {
            var sut = _fixture.Create<IEitherCalculator>();

            Action act = () => sut.Calculate(1, 1.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void EitherCalculate_BothOutOfRangeValue_WillThrowArgumentOutOfRangeException()
        {
            var sut = _fixture.Create<IEitherCalculator>();

            Action act = () => sut.Calculate(1.1, 1.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void EitherCalculate_NegativeValueOutOfRange_WillThrowArgumentOutOfRangeException()
        {
            var sut = _fixture.Create<IEitherCalculator>();

            Action act = () => sut.Calculate(-0.1, 0.5);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
