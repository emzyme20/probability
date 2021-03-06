﻿using System;

using Microsoft.Extensions.Logging;

using Serilog.Context;

namespace Probability.Core.Calculations
{
    public class EitherCalculator : IEitherCalculator
    {
        private readonly ILogger<EitherCalculator> _logger;

        public EitherCalculator(ILogger<EitherCalculator> logger)
        {
            _logger = logger;
        }

        public string Formula { get; } = "P(left) + P(right) - P(left) P(right)";

        public CalculatorType Calculator { get; } = CalculatorType.Either;

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

            var auditLogMessage =
                $"Type: {Calculator} left: {left} - right: {right} - result: {result}";

            using (LogContext.PushProperty("AuditLogEntry", auditLogMessage))
            {
                _logger.LogInformation(auditLogMessage);
            }

            return Math.Round(result, 3);
        }
    }
}
