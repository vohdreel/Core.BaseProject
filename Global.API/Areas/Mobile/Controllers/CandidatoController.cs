using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.DAO.Interface.Service;
using Global.DAO.Model;
using Global.DAO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Global.API.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class CandidatoController : ControllerBase
    {
        private readonly IServiceCandidato _serviceCandidato;

        public CandidatoController(IServiceCandidato serviceCandidato)
        {

            _serviceCandidato = serviceCandidato;


        }



        [HttpPost("AtualizarInformacoesPessoais")]
        public object AtualizarInformacoesPessoais([FromBody] Candidato informacoesPessoais, [FromQuery] int[] idsCargosSelecionados = null, [FromQuery] int[] idsEnumAgrupamentoSelecionados = null)
        {
            using (var cargoInteresseService = new CargoInteresseService())
            using (var areaInteresseService = new ServiceAreaInteresse())
            {

                Candidato candidato = _serviceCandidato.BuscarPorId(informacoesPessoais.Id);


                #region Montar Objeto de Atualização
                informacoesPessoais.Id = candidato.Id;
                informacoesPessoais.Cpf = candidato.Cpf;
                informacoesPessoais.Email = candidato.Email;
                informacoesPessoais.Nome = candidato.Nome;
                candidato.Bairro = !string.IsNullOrEmpty(informacoesPessoais.Bairro) ? informacoesPessoais.Bairro : candidato.Bairro;
                candidato.Nacionalidade = !string.IsNullOrEmpty(informacoesPessoais.Nacionalidade) ? informacoesPessoais.Nacionalidade : candidato.Nacionalidade;
                candidato.EstadoNascimento = !string.IsNullOrEmpty(informacoesPessoais.EstadoNascimento) ? informacoesPessoais.EstadoNascimento : candidato.EstadoNascimento;
                candidato.DataNascimento = informacoesPessoais.DataNascimento != null ? informacoesPessoais.DataNascimento : candidato.DataNascimento;
                candidato.Sexo = !string.IsNullOrEmpty(informacoesPessoais.Sexo) ? informacoesPessoais.Sexo : candidato.Sexo;
                candidato.Raca = !string.IsNullOrEmpty(informacoesPessoais.Raca) ? informacoesPessoais.Raca : candidato.Raca;
                candidato.PossuiDependentes = informacoesPessoais.PossuiDependentes != null ? informacoesPessoais.PossuiDependentes : candidato.PossuiDependentes;
                candidato.QuantidadeDependentes = informacoesPessoais.QuantidadeDependentes != null ? informacoesPessoais.QuantidadeDependentes : candidato.QuantidadeDependentes;
                candidato.PossuiCnh = informacoesPessoais.PossuiCnh != null ? informacoesPessoais.PossuiCnh : candidato.PossuiCnh;
                candidato.CategoriaCnh = !string.IsNullOrEmpty(informacoesPessoais.CategoriaCnh) ? informacoesPessoais.CategoriaCnh : candidato.CategoriaCnh;
                candidato.Cep = !string.IsNullOrEmpty(informacoesPessoais.Cep) ? informacoesPessoais.Cep : candidato.Cep;
                candidato.Pais = !string.IsNullOrEmpty(informacoesPessoais.Pais) ? informacoesPessoais.Pais : candidato.Pais;
                candidato.Estado = !string.IsNullOrEmpty(informacoesPessoais.Estado) ? informacoesPessoais.Estado : candidato.Estado;
                candidato.Cidade = !string.IsNullOrEmpty(informacoesPessoais.Cidade) ? informacoesPessoais.Cidade : candidato.Cidade;
                candidato.Endereco = !string.IsNullOrEmpty(informacoesPessoais.Endereco) ? informacoesPessoais.Endereco : candidato.Endereco;
                candidato.Numero = !string.IsNullOrEmpty(informacoesPessoais.Numero) ? informacoesPessoais.Numero : candidato.Numero;
                candidato.Complemento = !string.IsNullOrEmpty(informacoesPessoais.Complemento) ? informacoesPessoais.Complemento : candidato.Complemento;
                candidato.Bairro = !string.IsNullOrEmpty(informacoesPessoais.Bairro) ? informacoesPessoais.Bairro : candidato.Bairro;
                candidato.EmailSecundario = !string.IsNullOrEmpty(informacoesPessoais.EmailSecundario) ? informacoesPessoais.EmailSecundario : candidato.EmailSecundario;
                candidato.Identidade = !string.IsNullOrEmpty(informacoesPessoais.Identidade) ? informacoesPessoais.Identidade : candidato.Identidade;
                candidato.DataEmissao = informacoesPessoais.DataEmissao != null ? informacoesPessoais.DataEmissao : candidato.DataEmissao;
                candidato.OrgaoEmissor = !string.IsNullOrEmpty(informacoesPessoais.OrgaoEmissor) ? informacoesPessoais.OrgaoEmissor : candidato.OrgaoEmissor;
                candidato.IdentidadePais = !string.IsNullOrEmpty(informacoesPessoais.IdentidadePais) ? informacoesPessoais.IdentidadePais : candidato.IdentidadePais;
                candidato.IdentidadeEstado = !string.IsNullOrEmpty(informacoesPessoais.IdentidadeEstado) ? informacoesPessoais.IdentidadeEstado : candidato.IdentidadeEstado;
                candidato.NomeMae = !string.IsNullOrEmpty(informacoesPessoais.NomeMae) ? informacoesPessoais.NomeMae : candidato.NomeMae;
                candidato.NomePai = !string.IsNullOrEmpty(informacoesPessoais.NomePai) ? informacoesPessoais.NomePai : candidato.NomePai;
                candidato.Deficiente = informacoesPessoais.Deficiente != null ? informacoesPessoais.Deficiente : candidato.Deficiente;
                candidato.Cid = !string.IsNullOrEmpty(informacoesPessoais.Cid) ? informacoesPessoais.Cid : candidato.Cid;
                candidato.AmputacaoMembrosInferiores = !string.IsNullOrEmpty(informacoesPessoais.AmputacaoMembrosInferiores) ? informacoesPessoais.AmputacaoMembrosInferiores : candidato.AmputacaoMembrosInferiores;
                candidato.AmputacaoMembrosSuperiores = !string.IsNullOrEmpty(informacoesPessoais.AmputacaoMembrosSuperiores) ? informacoesPessoais.AmputacaoMembrosSuperiores : candidato.AmputacaoMembrosSuperiores;
                candidato.DeficienciaAudicao = !string.IsNullOrEmpty(informacoesPessoais.DeficienciaAudicao) ? informacoesPessoais.DeficienciaAudicao : candidato.DeficienciaAudicao;
                candidato.DeficienciasMentais = !string.IsNullOrEmpty(informacoesPessoais.DeficienciasMentais) ? informacoesPessoais.DeficienciasMentais : candidato.DeficienciasMentais;
                candidato.DeficienciaCrescimento = !string.IsNullOrEmpty(informacoesPessoais.DeficienciaCrescimento) ? informacoesPessoais.DeficienciaCrescimento : candidato.DeficienciaCrescimento;
                candidato.DeficienciaFala = !string.IsNullOrEmpty(informacoesPessoais.DeficienciaFala) ? informacoesPessoais.DeficienciaFala : candidato.DeficienciaFala;
                candidato.Observacoes = !string.IsNullOrEmpty(informacoesPessoais.Observacoes) ? informacoesPessoais.Observacoes : candidato.Observacoes;
                candidato.LocalPreferencia = !string.IsNullOrEmpty(informacoesPessoais.LocalPreferencia) ? informacoesPessoais.LocalPreferencia : candidato.LocalPreferencia;


                candidato.DisponibilidadeHorario = informacoesPessoais.DisponibilidadeHorario != null ? informacoesPessoais.DisponibilidadeHorario : candidato.DisponibilidadeHorario;
                candidato.DisponibilidadeViagem = informacoesPessoais.DisponibilidadeViagem != null ? informacoesPessoais.DisponibilidadeViagem : candidato.DisponibilidadeViagem;
                candidato.DisponibilidadeTransferencia = informacoesPessoais.DisponibilidadeTransferencia != null ? informacoesPessoais.DisponibilidadeTransferencia : candidato.DisponibilidadeTransferencia;
                candidato.PretensaoSalarial = informacoesPessoais.PretensaoSalarial != null ? informacoesPessoais.PretensaoSalarial : candidato.PretensaoSalarial;
                candidato.PerfilProfissional = informacoesPessoais.PerfilProfissional != null ? informacoesPessoais.PerfilProfissional : candidato.PerfilProfissional;

                #endregion



                bool sucesso = _serviceCandidato.Atualizar(candidato);

                if (sucesso)
                {
                    if (idsCargosSelecionados != null && idsCargosSelecionados.Count() > 0 || idsEnumAgrupamentoSelecionados != null && idsEnumAgrupamentoSelecionados.Count() > 0)
                    {
                        sucesso = cargoInteresseService.AtualizarListaDeCargosInteressePorCandidato(candidato.Id, idsCargosSelecionados);
                        if (sucesso) areaInteresseService.AtualizarListaDeAreasInteressePorCandidato(candidato.Id, idsEnumAgrupamentoSelecionados);


                    }
                }
                return new
                {
                    ok = sucesso,
                    message = sucesso ? "As suas informações foram atualizadas com sucesso!" : "Ocorreu um erro ao salvar suas informações, tente novamente mais tarde"
                };



            }
        }



        [HttpGet("ObterInformacoesPessoais")]
        public object ObterInformacoesPessoais(int idCandidato, bool isObjetivo = false)
        {

            Candidato candidato = _serviceCandidato.BuscarPorId(idCandidato);

            if (!isObjetivo)
            {
                return candidato;
            }

            Cargo[] cargos = new CargoService().BuscarTodos();

            EnumAgrupamento[] areas = new EnumAgrupamentoService().BuscarTodos();

            int[] idsCargosSelecionados = new CargoInteresseService().BuscarTodosPorCandidato(idCandidato)
                .Select(x => x.IdCargo)
                .ToArray();

            int[] idsEnumAgrupamentoSelecionados = new ServiceAreaInteresse().BuscarTodosPorCandidato(idCandidato)
                .Select(x => x.IdEnumAgrupamento)
                .ToArray();


            return new
            {
                candidato,
                cargos,
                areas,
                idsCargosSelecionados,
                idsEnumAgrupamentoSelecionados
            };

        }
    }
}
