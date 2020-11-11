using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Controllers
{
    public class ErrorHandleController : Controller
    {
        [HttpGet]
        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception
            //var code = 500; // Internal Server Error by default

            //if (exception is MyNotFoundException) code = 404; // Not Found
            //else if (exception is MyUnauthException) code = 401; // Unauthorized
            //else if (exception is MyException) code = 400; // Bad Request

            //Response.StatusCode = code; // You can use HttpStatusCode enum instead

            return new ErrorResponse(exception); // Your error model
        }

        [HttpGet]
        [Route("HttpError/{code:int}")]
        public JsonResult HandleError(int code)
        {
            ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
            return Json(ViewData["ErrorMessage"]);
        }

        //[Route("HttpError/{code:int}")]
        //public IActionResult HandleError(int code)
        //{
        //    ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
        //    return View("~/Views/Shared/Error.cshtml");
        //}
    }

    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
            StackTrace = ex.ToString();
        }
    }
}