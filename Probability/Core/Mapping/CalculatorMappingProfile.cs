using AutoMapper;

using Probability.Models;
using Probability.ViewModels;

namespace Probability.Core.Mapping
{
    public class CalculatorMappingProfile : Profile
    {
        public CalculatorMappingProfile()
        {
            CreateMap<CalculatorModel, CalculatorViewModel>();
        }
    }
}
