using System;

using Microsoft.Extensions.Logging;

using Probability.Core.Audit;

namespace Probability.Core.Calculations
{
    public class CombineCalculator : ICombineCalculator
    {
        private readonly ILogger<CombineCalculator> _logger;
        private readonly IAuditor _auditor;

        public CombineCalculator(ILogger<CombineCalculator> logger, IAuditor auditor)
        {
            _logger = logger;
            _auditor = auditor;
        }

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

            _logger.LogInformation($"{DateTime.UtcNow.ToString("u")} - ");
            return left * right;
        }
    }
}