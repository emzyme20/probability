using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Probability.Core.Calculations;
using Probability.Models;
using Probability.ViewModels;

namespace Probability.Core.Handlers
{
    public class CalculatorHandler : IRequestHandler<CalculatorModel, CalculatorResultViewModel>
    {
        private readonly ICalculatorFactory _calculatorFactory;

        public CalculatorHandler(ICalculatorFactory calculatorFactory)
        {
            _calculatorFactory = calculatorFactory;
        }

        public Task<CalculatorResultViewModel> Handle(CalculatorModel request, CancellationToken cancellationToken)
        {
            Enum.TryParse<CalculatorType>(request.Calculator, out var calculatorType);

            var calculator = _calculatorFactory.CreateCalculator(calculatorType);

            var result = calculator.Calculate(request.Left, request.Right);

            return Task.FromResult(new CalculatorResultViewModel
            {
                Calculator = request.Calculator,
                Left = request.Left,
                Right = request.Right,
                Result = result,
                Formula = calculator.Formula
            });
        }
    }
}
