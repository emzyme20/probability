namespace Probability.Core.Calculations
{
    public interface ICalculator
    {
        double Calculate(double left, double right);
    }
    
    public interface ICombineCalculator : ICalculator
    {
    }

    public interface IEitherCalculator : ICalculator
    {
    }
}
