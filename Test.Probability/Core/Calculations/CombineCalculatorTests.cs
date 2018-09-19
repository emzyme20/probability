﻿using System;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Probability.Core.Audit;
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

            _fixture.Register<ICombineCalculator>(() => new CombineCalculator(_fixture.Create<ILogger<CombineCalculator>>(), _fixture.Create<IAuditor>()));
        }

        [Theory]
        [InlineData(0.8, 0.8, 0.640)]
        [InlineData(0.5, 0.75, 0.375)]
        [InlineData(0, 1, 0)]
        public void CombinedWith_InRangeValues(double left, double right, double expected)
        {
            var sut = _fixture.Create<ICombineCalculator>();

            var result = sut.Calculate(left, right);

            Math.Round(result, 3).Should().Be(expected);
        }

        [Fact]
        public void CombinedWith_OneOutOfRangeValue()
        {
            var sut = _fixture.Create<ICombineCalculator>();

            Action act = () => sut.Calculate(1, 1.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CombinedWith_BothOutOfRangeValue()
        {
            var sut = _fixture.Create<ICombineCalculator>();

            Action act = () => sut.Calculate(1.1, 1.1);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void CombinedWith_NegativeValueOutOfRange()
        {
            var sut = _fixture.Create<ICombineCalculator>();

            Action act = () => sut.Calculate(-0.1, 0.5);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
