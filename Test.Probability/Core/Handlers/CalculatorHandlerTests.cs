using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using FluentAssertions;

using NSubstitute;

using Probability.Core.Calculations;
using Probability.Core.Handlers;
using Probability.Models;

using Xunit;

namespace Test.Probability.Core.Handlers
{
    public class CalculatorHandlerTests
    {
        private readonly IFixture _fixture;

        public CalculatorHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Freeze<ICalculatorFactory>();
            _fixture.Register<ICombineCalculator>(() => _fixture.Create<CombineCalculator>());
        }

        [Fact]
        public async Task Handle_WithValidModel_WillCreateCalculatorToUse()
        {
            // Arrange
            var model = new CalculatorModel
            {
                Calculator = CalculatorType.Combine.ToString(),
                Left = 0.87,
                Right = 0.43
            };

            _fixture.Create<ICalculatorFactory>()
                    .CreateCalculator(
                        Arg.Is<CalculatorType>(calculatorType => calculatorType == CalculatorType.Combine))
                    .Returns(_fixture.Create<ICombineCalculator>());
            
            // Act
            var sut = _fixture.Create<CalculatorHandler>();

            var viewModel = await sut.Handle(model, CancellationToken.None);

            // Assert
            viewModel.Result.Should().Be(0.3741);
            viewModel.Formula.Should().NotBeNullOrWhiteSpace();

            _fixture.Create<ICalculatorFactory>()
                    .Received(1)
                    .CreateCalculator(
                        Arg.Is<CalculatorType>(calculatorType => calculatorType == CalculatorType.Combine));
        }
    }
}
