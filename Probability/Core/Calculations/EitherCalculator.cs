using System;

using Microsoft.Extensions.Logging;

using Probability.Core.Audit;

namespace Probability.Core.Calculations
{
    public class EitherCalculator : IEitherCalculator
    {
        private readonly ILogger<EitherCalculator> _logger;

        public EitherCalculator(ILogger<EitherCalculator> logger, IAuditor auditor)
        {
            _logger = logger;
        }

        //P(A) + P(B) - P(A)P(B) e.g. 0.5 + 0.5 – 0.5 * 0.5 = 0.75
        public double Calculate(double left, double right)
        {
            if (left < 0 || left > 1)
            {
                _logger.LogError($"Argument out of range {left}");
                throw new ArgumentOutOfRangeException(nameof(left), "Only values in the range of 0 and 1 are allowed.");
            }

            if (right < 0 || right > 1)
            {
                _logger.LogError($"Argument out of range {right}");
                throw new ArgumentOutOfRangeException(nameof(right), "Only values in the range of 0 and 1 are allowed.");
            }
            
            var result = left + right - left * right;

            _logger.LogInformation($"{DateTime.UtcNow:u} - {CalculatorType.Combine} left: {left:0.00#} - right: {right:0.00#} - result: {result:0.00#}");

            return result;
        }
    }
}