using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.API.Models;
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
        [Route("HttpError/{id:length(3,3)}")]
        public IActionResult HandleError(int id)
        {

            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte.";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para acessar essa página.";
                modelErro.Titulo = "Acesso Negado!";
                modelErro.ErroCode = id;
            }
            else if (id == 401)
            {
                modelErro.Mensagem = "Você não tem autorização para acessar essa página.";
                modelErro.Titulo = "Não autorizado!";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);

        }


        [HttpGet]
        [Route("TokenExpired")]
        public IActionResult HandleExpiredToken()
        {
            var modelErro = new ErrorViewModel();
                        
            modelErro.Mensagem = "Esse token já foi utilizado ou está expirado.";
            modelErro.Titulo = "Token expirado!";
            modelErro.ErroCode = 401;

            return View("Error", modelErro);
        }


        /*
        [HttpGet]
        [Route("HttpError/{code:int}")]
        public JsonResult HandleError(int code)
        {
            ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
            var json = Json(new {ok = false, message = ViewData["ErrorMessage"] });
            json.StatusCode = code;
            return json;
        }
        

        [HttpGet]
        [Route("TokenExpired")]
        public JsonResult HandleExpiredToken(int code)
        {
            ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
            var json = Json(new { ok = false, message = ViewData["ErrorMessage"] });
            json.StatusCode = code;
            return json;
        }
        */
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