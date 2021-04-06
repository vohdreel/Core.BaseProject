using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost("AtualizarInformacoesPessoais")]
        public object AtualizarInformacoesPessoais([FromBody] Candidato informacoesPessoais, [FromQuery] bool IsInformacaoPessoal, [FromQuery] int[] idsCargosSelecionados = null, [FromQuery] int[] idsEnumAgrupamentoSelecionados = null)
        {
            using (var cargoInteresseService = new CargoInteresseService())
            using (var areaInteresseService = new AreaInteresseService())
            using (var service = new CandidatoService())
            {

                Candidato candidato = service.BuscarCandidato(informacoesPessoais.Id);

                if (candidato.TelefoneCandidato.ToArray().Count() == 0)
                {
                    return new
                    {
                        ok = false,
                        message = "Você precisa cadastrar ao menos um telefone de contato!"
                    };

                }

                
                //informacoesPessoais.Id = candidato.Id;
                //informacoesPessoais.Cpf = candidato.Cpf;
                //informacoesPessoais.Email = candidato.Email;
                //informacoesPessoais.Nome = candidato.Nome;
                candidato.Bairro = !string.IsNullOrEmpty(informacoesPessoais.Bairro) ? informacoesPessoais.Bairro : candidato.Bairro;
                candidato.Nacionalidade = !string.IsNullOrEmpty(informacoesPessoais.Nacionalidade) ? informacoesPessoais.Nacionalidade : candidato.Nacionalidade;
                candidato.EstadoNascimento = !string.IsNullOrEmpty(informacoesPessoais.EstadoNascimento) ? informacoesPessoais.EstadoNascimento : candidato.EstadoNascimento;
                candidato.DataNascimento = informacoesPessoais.DataNascimento != null ? informacoesPessoais.DataNascimento : candidato.DataNascimento;
                candidato.Sexo = !string.IsNullOrEmpty(informacoesPessoais.Sexo) ? informacoesPessoais.Sexo : candidato.Sexo;
                candidato.Raca = informacoesPessoais.Raca != null ? informacoesPessoais.Raca : candidato.Raca;
                candidato.EstadoCivil = informacoesPessoais.EstadoCivil != null ? informacoesPessoais.EstadoCivil : candidato.EstadoCivil;
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

                if (IsInformacaoPessoal)
                    candidato.InformacoesPessoaisConcluido = true;
                else
                    candidato.ObjetivosConcluido = true;



                

                bool sucesso = service.AtualizarCandidato(candidato);

                

                if (sucesso)
                {
                    if (idsCargosSelecionados != null && idsCargosSelecionados.Count() > 0 || idsEnumAgrupamentoSelecionados != null && idsEnumAgrupamentoSelecionados.Count() > 0)
                    {
                        sucesso = cargoInteresseService.AtualizarListaDeCargosInteressePorCandidato(candidato.Id, idsCargosSelecionados);
                        if (sucesso) areaInteresseService.AtualizarListaDeAreasInteressePorCandidato(candidato.Id, idsEnumAgrupamentoSelecionados);


                    }

                    if (candidato.InformacoesPessoaisConcluido.Value && candidato.ObjetivosConcluido.Value)
                    { 
                    
                        // metodo que atualiza 
                    
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

            using (var service = new CandidatoService())
            {
                Candidato candidato = service.BuscarCandidato(idCandidato);

                if (!isObjetivo)
                {
                    return candidato;
                }


                Cargo[] cargos = new CargoService().BuscarTodos();

                EnumAgrupamento[] areas = new EnumAgrupamentoService().BuscarTodos();

                Cargo[] idsCargosSelecionados = new CargoInteresseService().BuscarTodosPorCandidato(idCandidato)
                    .Select(x => x.IdCargoNavigation)
                    .ToArray();

                EnumAgrupamento[] idsEnumAgrupamentoSelecionados = new AreaInteresseService().BuscarTodosPorCandidato(idCandidato)
                    .Select(x => x.IdEnumAgrupamentoNavigation)
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

        #region Experiencias Profissionais

        [HttpGet("ObterExperienciasProfissionais")]
        public object ObterExperienciasProfissionais(int IdCandidato)
        {
            using (var service = new ExperienciaProfissionalService())
            {
                ViewModel.ExperienciaProfissional[] experienciaProfissional = service
                    .BuscarPorCandidato(IdCandidato)
                    .Select(x => new ViewModel.ExperienciaProfissional(x)).ToArray();

                return experienciaProfissional;
            }

        }

        [HttpPost("SalvarNovaExpericenciaProfissional")]
        public object SalvarNovaExpericenciaProfissional([FromBody] ExperienciaProfissional experienciaProfissional)
        {
            using (var service = new ExperienciaProfissionalService())
            {
                bool success = true; string message = "";
                if (experienciaProfissional.Id == 0)
                {
                    success = service.Salvar(experienciaProfissional); message = "Nova experiênica cadastrada com sucesso!";
                }
                else
                {
                    success = service.Editar(experienciaProfissional); message = "Experiênica atualizada com sucesso!";
                }

                return new { ok = success, message = success ? message : "Ocorreu um erro ao tentar salvar, tente novamente mais tarde!" };
            }

        }

        [HttpGet("DeletarExperienciaProfissional")]
        public object DeletarExperienciaProfissional(int IdExperiencia)
        {
            using (var service = new ExperienciaProfissionalService())
            {
                bool success = service.Excluir(IdExperiencia);
                return new { ok = success, message = success ? "Experiênica excluída com sucesso!" : "Ocorreu um erro ao tentar excluir, tente novamente mais tarde!" };
            }

        }

        #endregion

        [HttpGet("ObterFormacoesCandidato")]
        public object ObterFormacoesCandidato(int IdCandidato)
        {
            using (var service = new FormacaoCandidatoService())
            {
                ViewModel.FormacaoCandidato[] formacaoCandidatos = service
                    .BuscarPorCandidato(IdCandidato)
                    .Select(x => new ViewModel.FormacaoCandidato(x)).ToArray();

                return formacaoCandidatos;
            }

        }

        [HttpPost("SalvarNovaFormacaoCandidato")]
        public object SalvarNovaFormacaoCandidato([FromBody] FormacaoCandidato formacaoCandidato)
        {
            using (var service = new FormacaoCandidatoService())
            {
                bool success = true; string message = "";
                if (formacaoCandidato.Id == 0)
                {
                    success = service.Salvar(formacaoCandidato); message = "Nova formação cadastrada com sucesso!";
                }
                else
                {
                    success = service.Editar(formacaoCandidato); message = "Formação atualizada com sucesso!";
                }

                return new { ok = success, message = success ? message : "Ocorreu um erro ao tentar salvar, tente novamente mais tarde!" };
            }

        }

        [HttpGet("DeletarFormacaoCandidato")]
        public object DeletarFormacaoCandidato(int IdFormacao)
        {
            using (var service = new FormacaoCandidatoService())
            {
                bool success = service.Excluir(IdFormacao);
                return new { ok = success, message = success ? "Formação excluída com sucesso!" : "Ocorreu um erro ao tentar excluir, tente novamente mais tarde!" };
            }

        }


        #region Telefones de Contato

        [HttpGet("ObterTelefoneCandidato")]
        public object ObterTelefoneCandidato(int IdCandidato)
        {
            using (var service = new TelefoneCandidatoService())
            using (var telefoneService = new TelefoneService())
            {
                var _telefones = service
                    .BuscarTelefonesPorCandidato(IdCandidato).ToList();

                _telefones.ForEach(x =>
                 {
                     if (x.TipoTelefone == null)
                     {
                         x.Numero = x.Numero.Replace("(0", "(");
                         if (x.TipoTelefone == null)
                             x.TipoTelefone = 1;
                         if (x.Numero.Trim().Contains("Cel."))
                         {
                             x.TipoTelefone = 1;
                             x.Numero = x.Numero.Replace("Cel.", "").Trim();
                         }
                         if (x.Numero.Trim().Contains("Res."))
                         {
                             x.TipoTelefone = 2;
                             x.Numero = x.Numero.Replace("Res.", "").Trim();
                         }
                         if (x.Numero.Trim().Contains("Rec."))
                         {
                             x.TipoTelefone = 3;
                             x.Numero = x.Numero.Replace("Rec.", "").Trim();
                         }
                         if (x.Numero.Trim().Contains("(aceita whatsapp)"))
                         {
                             x.TipoTelefone = 4;
                             x.Numero = x.Numero.Replace("(aceita whatsapp)", "").Trim();
                         }
                         telefoneService.Editar(x);
                     }
                 });

                ViewModel.Telefone[] telefones = _telefones.Select(x => new ViewModel.Telefone(x)).ToArray();


                return telefones;
            }

        }

        [HttpPost("SalvarTelefoneCandidato")]
        public object SalvarTelefoneCandidato([FromBody] Telefone telefoneCanidato, [FromQuery] int idCandidato)
        {
            using (var telefoneService = new TelefoneService())
            using (var service = new TelefoneCandidatoService())
            {
                bool success = true; string message = "";
                if (telefoneCanidato.Id != 0)
                {
                    success = telefoneService.Editar(telefoneCanidato); message = "Telefone atualizado com sucesso!";
                }
                else
                {
                    var telefoneCandidato = new TelefoneCandidato()
                    {
                        IdCandidato = idCandidato,
                        IdTelefoneNavigation = new Telefone()
                        {

                            Numero = telefoneCanidato.Numero,
                            TipoTelefone = telefoneCanidato.TipoTelefone
                        }
                    };
                    success = service.Salvar(telefoneCandidato); message = "Telefone adicionado com sucesso!";
                }

                return new { ok = success, message = success ? message : "Ocorreu um erro ao tentar salvar, tente novamente mais tarde!" };
            }

        }

        [HttpGet("DeletarTelefoneCandidato")]
        public object DeletarTelefoneCandidato(int IdTelefone)
        {
            using (var service = new TelefoneCandidatoService())
            {


                bool success = service.RemoverTelefoneCandidatoPorIdTelefone(IdTelefone);
                return new { ok = success, message = success ? "Telefone de contato excluído com sucesso!" : "Ocorreu um erro ao tentar excluir, tente novamente mais tarde!" };
            }

        }
        #endregion
    }
}
