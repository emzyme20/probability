using System;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Probability.Core.Calculations;

using Xunit;

namespace Test.Probability.Core.Calculations
{
    public class CalculatorFactoryTests
    {
        private readonly IFixture _fixture;

        public CalculatorFactoryTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

            _fixture.Freeze<IServiceProvider>();
            
            _fixture.Register<ICombineCalculator>(() => new CombineCalculator(_fixture.Create<ILogger<CombineCalculator>>()));
            
            _fixture
                .Create<IServiceProvider>()
                .GetService(typeof(ICombineCalculator))
                .Returns(new CombineCalculator(_fixture.Create<ILogger<CombineCalculator>>()));

            _fixture.Create<IServiceProvider>().GetService(typeof(IEitherCalculator)).Returns(null);
            
            _fixture.Register<ICalculatorFactory>(() => new CalculatorFactory(_fixture.Create<IServiceProvider>()));
        }

        [Fact]
        public void CreateCalculator_WithRegisteredCalculator()
        {
            var sut = _fixture.Create<ICalculatorFactory>();

            var calculator = sut.CreateCalculator(CalculatorType.Combine);

            calculator.Should().BeAssignableTo<ICalculator>();
            calculator.Should().BeOfType<CombineCalculator>();

            _fixture.Create<IServiceProvider>().Received(1).GetService(typeof(ICombineCalculator));
            _fixture.Create<IServiceProvider>().Received(1).GetService(typeof(IEitherCalculator));
        }

        [Fact]
        public void CreateCalculator_WithUnRegisteredCalculator_WillThrow()
        {
            var sut = _fixture.Create<ICalculatorFactory>();

            Action act = () => sut.CreateCalculator(CalculatorType.Either);

            act.Should().Throw<Exception>();
            
            _fixture.Create<IServiceProvider>().Received(1).GetService(typeof(ICombineCalculator));
            _fixture.Create<IServiceProvider>().Received(1).GetService(typeof(IEitherCalculator));
        }
    }
}