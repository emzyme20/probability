using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoNSubstitute;

using AutoMapper;

using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using NSubstitute;

using Probability.Controllers;
using Probability.Core.Calculations;
using Probability.Models;
using Probability.ViewModels;

using Xunit;

namespace Test.Probability.Controllers
{
    public class HomeControllerTests
    {
        private readonly IFixture _fixture;

        public HomeControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            _fixture.Freeze<IMediator>();
            _fixture.Freeze<IMapper>();
        }

        [Fact]
        public void Index_ValidModel_ShowsCalculatorPage()
        {
            var sut = _fixture.Build<HomeController>()
                              .OmitAutoProperties()
                              .With(c => c.ControllerContext)
                              .With(c => c.Url, _fixture.Create<IUrlHelper>())
                              .With(c => c.TempData, _fixture.Create<ITempDataDictionary>())
                              .Create();

            var actionResult = sut.Index();

            actionResult
                .Should()
                .BeViewResult("Index");
        }

        [Fact]
        public async Task Calculate_ValidModel_PerformsCalculation()
        {
            // Arrange
            var mediator = _fixture.Create<IMediator>();

            mediator
                .Send(Arg.Any<CalculatorModel>())
                .Returns(
                    new CalculatorResultViewModel
                    {
                        Result = 0.693,
                        Calculator = CalculatorType.Either.ToString(),
                        Left = 0.65,
                        Right = 0.123
                    });

            var sut = _fixture.Build<HomeController>()
                              .OmitAutoProperties()
                              .With(c => c.ControllerContext)
                              .With(c => c.Url, _fixture.Create<IUrlHelper>())
                              .With(c => c.TempData, _fixture.Create<ITempDataDictionary>())
                              .Create();

            // Act
            var actionResult = await sut.Calculate(
                new CalculatorModel
                {
                    Calculator = CalculatorType.Either.ToString(),
                    Left = 0.65,
                    Right = 0.123
                });

            // Assert
            actionResult
                .Should()
                .BeViewResult("Result")
                .ModelAs<CalculatorResultViewModel>().Result.Should().Be(0.693);
        }

        [Fact]
        public async Task Calculate_WithInvalidModel_ReturnsUserToIndex()
        {
            // Arrange
            var sut = _fixture.Build<HomeController>()
                              .OmitAutoProperties()
                              .With(c => c.ControllerContext)
                              .With(c => c.Url, _fixture.Create<IUrlHelper>())
                              .With(c => c.TempData, _fixture.Create<ITempDataDictionary>())
                              .Create();

            _fixture.Create<IMapper>()
                    .Map<CalculatorViewModel>(Arg.Any<CalculatorModel>())
                    .Returns(
                        new CalculatorViewModel
                        {
                            Calculator = CalculatorType.Either.ToString(),
                            Left = -0.65,
                            Right = 1
                        });

            // ModelState is readonly, so we have to set it from the controller (after it has been constructed).
            sut.ModelState.AddModelError("Left", "Your left input must be within the range 0-1 (inclusive)");

            // Act
            var actionResult = await sut.Calculate(
                new CalculatorModel
                {
                    Calculator = CalculatorType.Either.ToString(),
                    Left = -0.65,
                    Right = 1
                });

            // Assert
            actionResult
                .Should()
                .BeViewResult("Index")
                .ModelAs<CalculatorViewModel>().Left.Should().Be(-0.65);
        }
    }
}
