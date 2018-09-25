namespace Probability.Core.Calculations
{
    public interface ICalculator
    {
        string Formula { get; }

        CalculatorType Calculator { get; }

        double Calculate(double left, double right);
    }

    // Marker interfaces to allow factory to return the correct calculators registered
    public interface ICombineCalculator : ICalculator
    {
    }

    public interface IEitherCalculator : ICalculator
    {
    }
}
