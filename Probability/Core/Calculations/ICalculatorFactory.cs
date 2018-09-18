namespace Probability.Core.Calculations
{
    public interface ICalculatorFactory
    {
        ICalculator CreateCalculator(CalculatorType type);
    }
}