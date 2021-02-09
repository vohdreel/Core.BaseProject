using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    public class CandidatoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
