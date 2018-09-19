using MediatR;

using Probability.ViewModels;

namespace Probability.Models
{
    public class CalculatorModel : IRequest<CalculatorResultViewModel>
    {
        public string Calculator { get; set; }

        public double Left { get; set; }

        public double Right { get; set; }
    }
}
