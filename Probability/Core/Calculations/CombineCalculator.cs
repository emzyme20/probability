using System;

using Microsoft.Extensions.Logging;

using Serilog.Context;

namespace Probability.Core.Calculations
{
    public class CombineCalculator : ICombineCalculator
    {
        private readonly ILogger<CombineCalculator> _logger;

        public CombineCalculator(ILogger<CombineCalculator> logger)
        {
            _logger = logger;
        }

        public string Formula { get; } = "P(left) P(right)";

        private CalculatorType Calculator { get; } = CalculatorType.Combine;

        //P(A)P(B) e.g. 0.5 * 0.5 = 0.25
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

            var result = left * right;

            var auditLogMessage =
                $"Type: {Calculator} left: {left:0.00#} - right: {right:0.00#} - result: {result:0.00#}";

            using (LogContext.PushProperty("AuditLogEntry", auditLogMessage))
                using (LogContext.PushProperty("CalculatorType", Calculator))
                {
                    _logger.LogInformation(auditLogMessage);
                }

            return result;
        }
    }
}
