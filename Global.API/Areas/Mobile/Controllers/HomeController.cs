using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet("MobileIndex")]
        public string Index()
        {
            return "Ok";
        }
        [HttpGet("Exception")]
        public JsonResult Exception()
        {
            throw new Exception();
        }

        [HttpGet("AuthorizedTest")]
        [Authorize(Roles = "Manager")]
        public int AuthorizedTest()
        {
            return 0;

        }

        [HttpGet("ServiceTest")]
        [Authorize]
        public int ServiceTest()
        {
            using (var service = new UnidadeService())
            {




            }

            return 0;
        }

    }



}