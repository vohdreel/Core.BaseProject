using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.DAO.Interface.Service;
using BaseProject.DAO.Model;
using BaseProject.DAO.Procedure.Models;
using BaseProject.DAO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {

        private readonly IServiceMachine _machineService;

        public HomeController(
            IServiceMachine machineService
            )
        {
            _machineService = machineService;

        }


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

        [HttpGet("UnitProcedure")]
        [Authorize]
        public BattleUnit[] Procedure()
        {

            return _machineService.ListarBattleUnits();

        }

    }



}