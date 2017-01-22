using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaxiCab.Models;

namespace TaxiCab.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Calc(DateTime o, decimal s, int f){ //Takes the input parameters and creates an object in the model layer which performs the calculation.
             return new JsonResult(new Journey{Origin = o,Slow = s,Fast = f}); //The object is returned in JSON format for the angular UI layer.
        }

        public IActionResult Index(){ //Returns the view page for the application.
             return View();
        }

    }
}
