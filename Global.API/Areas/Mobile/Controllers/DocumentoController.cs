﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.Util;
using Global.Util.SystemEnumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class DocumentoController : ControllerBase
    {


        [HttpGet("ListarArquivos")]
        public ViewModel.Documento[] ListarArquivo(int IdCandidato)
        {

            using (var service = new DocumentoService())
            {

                ViewModel.Documento[] lista = service
                    .ListarPorCandidato(IdCandidato)
                    .Select(x => new ViewModel.Documento
                    {
                        IdArquivo = x.Id,
                        Extensao = x.Extensao,
                        IdCandidato = x.IdCandidato,
                        NomeArquivo = x.NomeArquivo,
                        TipoDocumento = EnumExtensions.GetEnumDisplayName((NomeTipoDocumento)x.IdEnumTipoDocumento)


                    }).ToArray();
                return lista;

            }



        }


        [HttpGet("BaixarArquivo")]
        public object BaixarArquivo(int idArquivo)
        {
            using (var service = new DocumentoService())
            {

                Documento anexo = service.Buscar(idArquivo);

                return new
                {
                    nomeArquivo = anexo.NomeArquivo,
                    tipoArquivo = anexo.Extensao,
                    fileStream = anexo.ByteArray
                };

            }


        }
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
