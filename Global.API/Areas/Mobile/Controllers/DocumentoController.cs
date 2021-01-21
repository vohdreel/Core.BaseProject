using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    public class DocumentoController : ControllerBase
    {
        [HttpPost("SalvarArquivo")]
        public object SalvarArquivo([FromBody] ViewModel.Documento anexo)
        {
            using (var service = new DocumentoService())
            {

                Documento _anexo = new Documento()
                {

                    ByteArray = Encoding.ASCII.GetBytes(anexo.ByteArray),
                    IdCandidato = anexo.IdCandidato,
                    IdEnumTipoDocumento = anexo.IdEnumTipoDocumento,
                    NomeArquivo = anexo.NomeArquivo,
                    Extensao = anexo.Extensao

                };

                bool salvar = service.Salvar(_anexo);
                return new { ok = salvar };            
            
            }      
        
        
        }
        
    }
}
