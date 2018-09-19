using System.Collections.Generic;

using Probability.Core.Calculations;

namespace Probability.ViewModels
{
    public class CalculatorViewModel
    {
        public IEnumerable<string> CalculatorTypes { get; } = new[] { CalculatorType.Combine.ToString(), CalculatorType.Either.ToString() };

        public string Formula { get; set; }

        public string Calculator { get; set; }

        public double Left { get; set; }

        public double Right { get; set; }
    }
}
