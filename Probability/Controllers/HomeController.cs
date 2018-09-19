﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
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

            return View("Index", Mapper.Map<CalculatorViewModel>(model));
        }

        [Route("about")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
