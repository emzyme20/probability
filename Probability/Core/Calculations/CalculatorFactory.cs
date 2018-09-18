using System;
using System.Collections.Generic;

namespace Probability.Core.Calculations
{
    public class CalculatorFactory : ICalculatorFactory
    {
        private readonly Dictionary<CalculatorType, ICalculator> _calculators;

        public CalculatorFactory(IServiceProvider serviceProvider)
        {
            _calculators = new Dictionary<CalculatorType, ICalculator>
            {
                { CalculatorType.Combine, (ICombineCalculator)serviceProvider.GetService(typeof(ICombineCalculator)) },
                { CalculatorType.Either, (IEitherCalculator)serviceProvider.GetService(typeof(IEitherCalculator)) }
            };
        }

        public ICalculator CreateCalculator(CalculatorType type)
        {
            _calculators.TryGetValue(type, out var calculator);
            
            return calculator ?? throw new Exception($"Calculator: {type} not found");
        }
    }
}