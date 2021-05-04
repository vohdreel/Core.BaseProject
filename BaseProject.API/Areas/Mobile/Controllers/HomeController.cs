using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public HomeController() { }


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

        //[HttpGet("ServiceTest")]
        //[Authorize]
        //public Machine ServiceTest()
        //{
        //    using (var service = new MachineService())
        //    {
        //        return service.GetMachines().First();

        //    }

        //}


        //[HttpGet("UnreachableMethod")]
        //[Authorize(Roles = "Overlord")]
        //public Machine Unreachable()
        //{
        //    using (var service = new MachineService())
        //    {
        //        return service.GetMachines().First();

        //    }

        //}


        //[HttpGet("ProcedureTest")]
        //[Authorize]
        //public MachineUser[] ProcedureTest()
        //{
        //    using (var service = new MachineService())
        //    {
        //        return service.GetMachinesByProcudeure();

        //    }

        //}

        //[HttpGet("UnitProcedure")]
        //[Authorize]
        //public EgressUnit Procedure()
        //{
        //    using (var service = new MachineService())
        //    {
        //        return service.GetEgressByProcudeure();

        //    }

        //}

    }



}