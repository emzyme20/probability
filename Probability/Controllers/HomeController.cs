using System.Diagnostics;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Probability.Models;
using Probability.ViewModels;

namespace Probability.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HomeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(new CalculatorViewModel());
        }

        [Route("calculate")]
        public async Task<IActionResult> Calculate(CalculatorModel model)
        {
            if (ModelState.IsValid)
            {
                var resultViewModel = await _mediator.Send(model);
                return View("Result", resultViewModel);
            }

            return View("Index", _mapper.Map<CalculatorViewModel>(model));
        }
        
        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
