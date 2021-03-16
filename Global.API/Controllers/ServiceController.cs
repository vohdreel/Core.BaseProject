using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ExcelDataReader;
using Global.DAO.Model;
using Global.DAO.Service;
using Global.Util;
using Global.Util.SystemEnumerations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Global.API.Controllers
{
    [Route("[controller]")]

    public class ServiceController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ServiceController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment webHostEnvironment)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet("FeedTest")]
        public dynamic FeedTest()
        {
            var feedArray = HttpHelper
                .Get<JObject>(
                "https://jobs.i-hunter.com/",
                "globalempregos/feed/indeed",
                isXML: true);


            ProcessoSeletivoService processoService = new ProcessoSeletivoService();
            VagaService vagaService = new VagaService();

            JArray jobs = feedArray["source"]["jobs"]["job"] as JArray;

            foreach (JObject job in jobs)
            {
                int referenceNumber = job["referencenumber"]["#cdata-section"].Value<int>();

                if (!vagaService.VerificarVagaPorReferenceNumber(referenceNumber))
                {
                    int enumTipoContratacao = 0;
                    decimal valueSalario = 0;
                    ///primeiro verificar se a empresa existe
                    Vaga vaga = new Vaga();
                    Empresa empresa = new EmpresaService().BuscarPorNomeFantasia(job["company"].ToString());
                    if (empresa == null)
                    {
                        empresa = new Empresa()
                        {
                            NomeFantasia = job["company"]["#cdata-section"].ToString(),
                            EmailContato = job["email"]["#cdata-section"].ToString(),
                        };
                    }
                    string rawVagaTitle = job["title"]["#cdata-section"].ToString();

                    //Regex regex = new Regex(@".*?-\s*(?<cargo>.*?)\s*-");
                    Regex regex = new Regex(@".*?-\s+(?<cargo>.*?(?(?=\s*-)\s*-|((?!\s-).*-|$)))");
                    Match match = regex.Match(rawVagaTitle);
                    Cargo cargoVaga = new Cargo();
                    if (match.Success)
                    {
                        string nameVaga = match.Groups["cargo"].Value.Replace("-", "").Trim();
                        CargoService cargoService = new CargoService();
                        cargoVaga = cargoService.BuscarCargoFeed(nameVaga);
                        if (cargoVaga == null)
                        {
                            cargoVaga = new Cargo()
                            {
                                NomeCargo = nameVaga,
                                IdEnumAgrupamento = new EnumAgrupamentoService().BuscarPorNome("Prestação de Serviços").Id

                            };
                            cargoService.Salvar(cargoVaga);
                        }

                    }
                    string rawVagaDescription = job["description"]["#cdata-section"].ToString().Replace("\r\n", " ");

                    regex = new Regex(@"^.*?Tipo de contratação.*?\s+(?<tipoContratacao>.*?\s+)");
                    match = regex.Match(rawVagaDescription);
                    if (match.Success)
                        enumTipoContratacao = (int)(VagaModalidade)Enum.Parse(typeof(VagaModalidade), match.Groups["tipoContratacao"].Value.RemoveDiacritics());

                    regex = new Regex(@"^.*?Salário.*?\s+(?<salario>.*?\s+)");
                    match = regex.Match(rawVagaDescription);
                    if (match.Success)
                    {
                        try
                        {
                            valueSalario = Convert.ToDecimal(match.Groups["salario"].Value.Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });

                        }
                        catch (Exception e)
                        {
                            valueSalario = 0;
                        }


                    }
                    ProcessoSeletivo processoSeletivo = new ProcessoSeletivo()
                    {
                        DataInicioProcesso = Convert.ToDateTime(job["date"]["#cdata-section"].ToString()),
                        DataTerminoProcesso = Convert.ToDateTime(job["expiration_date"]["#cdata-section"].ToString()),
                        StatusProcesso = (int)StatusProcesso.EmAndamento,
                        NomeProcesso = job["title"]["#cdata-section"].ToString(),
                        IdEmpresaNavigation = empresa,
                        Vaga = new List<Vaga>()
                        {
                            new Vaga()
                            {
                                Cidade = job["city"]["#cdata-section"].ToString(),
                                Estado = job["state"]["#cdata-section"].ToString().ConverterEstados(),
                                ReferenceNumber =job["referencenumber"]["#cdata-section"].Value<int>(),
                                Requisitos= job["description"]["#cdata-section"].ToString(),
                                UrlVaga = job["url"]["#cdata-section"].ToString(),
                                DisponibilidadeTransferencia = (int)Disponibilidade.Negociavel,
                                DisponibilidadeViagem = (int)Disponibilidade.Negociavel,
                                Jornada = (int)DisponibilidadeHorario.Integral,
                                StatusVaga = (int)StatusVaga.Aberta,
                                Modalidade = enumTipoContratacao,
                                Salario = valueSalario,
                                IdCargo = cargoVaga.Id,


                            }
                        }
                    };

                    bool success = processoService.Salvar(processoSeletivo);
                }
            }




            return feedArray;


            //ver

        }

        [HttpGet("GenerateVagasByFeed")]
        public dynamic GenerateVagasByFeed()
        {
            var feedArray = HttpHelper
                .Get<JObject>(
                "https://jobs.i-hunter.com/",
                "globalempregos/feed/indeed",
                isXML: true);

            System.Globalization.CultureInfo cultureinfo =
       new System.Globalization.CultureInfo("pt-BR");


            ProcessoSeletivoService processoService = new ProcessoSeletivoService();
            VagaService vagaService = new VagaService();

            JArray rejetctedVagas = new JArray();

            JArray jobs = feedArray["source"]["jobs"]["job"] as JArray;

            foreach (JObject job in jobs)
            {
                try
                {
                    int referenceNumber = job["referencenumber"]["#cdata-section"].Value<int>();

                    if (!vagaService.VerificarVagaPorReferenceNumber(referenceNumber))
                    {
                        int enumTipoContratacao = 0;
                        decimal valueSalario = 0;
                        ///primeiro verificar se a empresa existe
                        Vaga vaga = new Vaga();
                        EmpresaService empresaService = new EmpresaService();
                        Empresa empresa = empresaService.BuscarPorNomeFantasia(job["company"]["#cdata-section"].ToString());
                        if (empresa == null)
                        {
                            empresa = new Empresa()
                            {
                                NomeFantasia = job["company"]["#cdata-section"].ToString(),
                                EmailContato = job["email"]["#cdata-section"].ToString(),
                            };
                            empresaService.Salvar(empresa);
                        }
                        string rawVagaTitle = job["title"]["#cdata-section"].ToString();

                        //Regex regex = new Regex(@".*?-\s*(?<cargo>.*?)\s*-");
                        Regex regex = new Regex(@".*?-\s+(?<cargo>.*?(?(?=\s*-)\s*-|((?!\s-).*-|$)))");
                        Match match = regex.Match(rawVagaTitle);
                        Cargo cargoVaga = new Cargo();
                        if (match.Success)
                        {
                            string nameCargoVaga = match.Groups["cargo"].Value.Replace("-", "").Trim();
                            CargoService cargoService = new CargoService();
                            cargoVaga = cargoService.BuscarCargoPorNome(nameCargoVaga);
                            if (cargoVaga == null)
                            {
                                cargoVaga = new Cargo()
                                {
                                    NomeCargo = nameCargoVaga,
                                    IdEnumAgrupamento = new EnumAgrupamentoService().BuscarPorNome("Prestação de Serviços").Id

                                };
                                cargoService.Salvar(cargoVaga);
                            }

                        }
                        string rawVagaDescription = job["description"]["#cdata-section"].ToString().Replace("\r\n", " ");

                        regex = new Regex(@"^.*?Tipo de contratação.*?\s+(?<tipoContratacao>.*?\s+)");
                        match = regex.Match(rawVagaDescription);
                        if (match.Success)
                            enumTipoContratacao = (int)(VagaModalidade)Enum.Parse(typeof(VagaModalidade), match.Groups["tipoContratacao"].Value.RemoveDiacritics());

                        regex = new Regex(@"^.*?Salário.*?\s+(?<salario>.*?\s+)");
                        match = regex.Match(rawVagaDescription);
                        if (match.Success)
                        {
                            try
                            {
                                valueSalario = Convert.ToDecimal(match.Groups["salario"].Value.Replace(".", ""), new NumberFormatInfo() { NumberDecimalSeparator = "," });

                            }
                            catch (Exception e)
                            {
                                valueSalario = 0;
                            }


                        }
                        ProcessoSeletivo processoSeletivo = new ProcessoSeletivo()
                        {
                            DataInicioProcesso = Convert.ToDateTime(job["date"]["#cdata-section"].ToString(), cultureinfo),
                            DataTerminoProcesso = Convert.ToDateTime(job["expiration_date"]["#cdata-section"].ToString(), cultureinfo),
                            StatusProcesso = (int)StatusProcesso.EmAndamento,
                            NomeProcesso = job["title"]["#cdata-section"].ToString(),
                            IdEmpresa = empresa.Id,
                            Vaga = new List<Vaga>()
                        {
                            new Vaga()
                            {
                                Cidade = job["city"]["#cdata-section"].ToString(),
                                Estado = job["state"]["#cdata-section"].ToString().Replace("State of ", "").ConverterEstados(),
                                ReferenceNumber =job["referencenumber"]["#cdata-section"].Value<int>(),
                                Requisitos= job["description"]["#cdata-section"].ToString(),
                                UrlVaga = job["url"]["#cdata-section"].ToString(),
                                DisponibilidadeTransferencia = (int)Disponibilidade.Negociavel,
                                DisponibilidadeViagem = (int)Disponibilidade.Negociavel,
                                Jornada = (int)DisponibilidadeHorario.Integral,
                                StatusVaga = (int)StatusVaga.Aberta,
                                Modalidade = enumTipoContratacao,
                                Salario = valueSalario,
                                IdCargo = cargoVaga.Id,


                            }
                        }
                        };

                        if (processoSeletivo.DataInicioProcesso.Value.Year <= 1753 || processoSeletivo.DataInicioProcesso.Value.Year >= 9999)
                        {
                            rejetctedVagas.Add(job); continue;
                        }
                        bool success = processoService.Salvar(processoSeletivo);
                        if (!success)
                            rejetctedVagas.Add(job);

                    }
                }
                catch (Exception e)
                {
                    return e;

                }
            }

            string ContentRootPath = _webHostEnvironment.ContentRootPath;

            string templatePath = @"\File\_InvalidVagas.json";

            var completePath = ContentRootPath + templatePath;

            JSONExtensions.Write<JArray>(rejetctedVagas, completePath);



            return rejetctedVagas;


            //ver

        }

        [HttpGet("GenerateAspNetUsers")]
        public async Task<object> GenerateAspNetUsers()
        {
            using (var service = new CandidatoService())
            {
                Candidato[] candidatos = service.VerificarCandidatoSemUsuario();
                foreach (Candidato candidato in candidatos)
                {

                    IdentityUser user = new IdentityUser();
                    user.UserName = candidato.Nome.Replace(" ", "_").RemoveDiacritics().ToLower();
                    user.Email = candidato.Email;

                    string password = TextExtensions.RandomPassword(6);

                    IdentityResult chkUser = await _userManager.CreateAsync(user, password);
                    if (chkUser.Succeeded)
                    {
                        candidato.IdAspNetUsers = user.Id;
                        bool success = service.AtualizarCandidato(candidato);

                        if (success)
                        {

                        }

                    }
                }

            }

            return new { ok = true };



        }

        [HttpGet("GetBanners")]
        public object GetBanner()
        {
            using (var service = new BannerService())
            {
                return service.BuscarTodos().Select(x => new ViewModel.Banner(x));
            }

        }

        [HttpGet("GenerateCargos")]
        public object GenerateCargos()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            string lastAgrupamento = "";

            string ContentRootPath = _webHostEnvironment.ContentRootPath;

            string templatePath = @"\File\Listagem_Cargos_Select.xlsx";

            var completePath = ContentRootPath + templatePath;

            using (var cargoService = new CargoService())
            using (var agrupamentoService = new EnumAgrupamentoService())
            using (var stream = System.IO.File.Open(completePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read()) //Each ROW
                        {
                            string stringAgrupamento = reader.GetString(2);

                            if (string.IsNullOrEmpty(stringAgrupamento))
                                stringAgrupamento = lastAgrupamento;
                            else
                                lastAgrupamento = stringAgrupamento;


                            string stringNomeCargo = reader.GetString(1);
                            int referenceNumber = 0;
                            string stringReferenceNumber = reader.GetValue(0)?.ToString() ?? "0";
                            Int32.TryParse(stringReferenceNumber, out referenceNumber);

                            if (string.IsNullOrEmpty(stringAgrupamento) || stringAgrupamento == "??" || stringAgrupamento == "Excluir" || referenceNumber == 0)
                                continue;

                            if (cargoService.BuscarCargoPorReferenceNumberENomeCargo(referenceNumber, stringNomeCargo) == null)
                            {


                                EnumAgrupamento agrupamento = agrupamentoService.BuscarPorNome(stringAgrupamento);
                                if (agrupamento == null)
                                {
                                    agrupamento = new EnumAgrupamento()
                                    {
                                        NomeAgrupamento = stringAgrupamento
                                    };
                                    agrupamentoService.Salvar(agrupamento);
                                };

                                Cargo cargo = new Cargo()
                                {
                                    NomeCargo = stringNomeCargo,
                                    DescricaoCargo = referenceNumber != 0 ?
                                                referenceNumber + " - " + stringNomeCargo :
                                                stringNomeCargo,
                                    IdEnumAgrupamento = agrupamento.Id,
                                    ReferenceNumber = referenceNumber



                                };

                                bool sucess = cargoService.Salvar(cargo);
                            }

                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET

                }
            }

            return 0;
        }

        [HttpGet("EnviorimentTest")]
        public object EnviorimentTest()
        {
            string ContentRootPath = _webHostEnvironment.ContentRootPath;

            string templatePath = @"\File\_InvalidVagas.json";

            var completePath = ContentRootPath + templatePath;

            return completePath;
        }

        [HttpGet("GetInvalidVagasLog")]
        public object GetInvalidVagasLog()
        {
            string ContentRootPath = _webHostEnvironment.ContentRootPath;

            string templatePath = @"\File\_InvalidVagas.json";

            var completePath = ContentRootPath + templatePath;

            JArray log = JSONExtensions.Read<JArray>(completePath);

            return log;
        }

        [HttpGet("GenerateCandidatos")]
        public async Task<object> GenerateCandidatos()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.Globalization.CultureInfo cultureinfo =
        new System.Globalization.CultureInfo("pt-BR");
            bool first = true;
            var formFile = Request.Form.Files[0].OpenReadStream();
            using (var service = new CandidatoService())
            using (var reader = ExcelReaderFactory.CreateReader(formFile))
            {
                do
                {
                    while (reader.Read()) //Each ROW
                    {
                        if (first) { first = !first; continue; }
                        if (service.ExisteCpfUsuario(reader.GetDouble(21).ToString())) { continue; }
                        Candidato candidato = new Candidato();

                        #region Informações Pessoais

                        candidato.Nome = reader.GetString(0);
                        candidato.Idlegado = reader.GetValue(1)?.ToString();

                        candidato.DataNascimento = reader.GetValue(2) != null ? (DateTime?)DateTime.ParseExact(reader.GetValue(2).ToString(), "dd/MM/yyyy", cultureinfo) : null;
                        //idade 3
                        candidato.Sexo = reader.GetString(4);

                        string raca = reader.GetString(5);
                        if (string.IsNullOrEmpty(raca))
                        {
                            candidato.Raca = (int)EnumRaca.Indisponivel;
                        }
                        else
                        {
                            if (raca.Contains("Preto"))
                                raca = "Preto";

                            candidato.Raca = (int)(Enum.Parse(typeof(EnumRaca), TextExtensions.GetRacaValue(raca)));
                        }

                        //candidato.EstadoCivil = (int)(Enum.Parse(typeof(EnumEstadoCIvil), reader.GetString(6)));
                        candidato.EstadoCivil = string.IsNullOrEmpty(reader.GetString(6)) ? 0 : (int)EnumExtensions.GetValueFromShortName<EnumEstadoCivil>(reader.GetString(6));

                        string deficiente = reader.GetString(7);
                        if (!string.IsNullOrEmpty(deficiente))
                        {
                            candidato.Deficiente = deficiente == "Sim" ? true : false;

                        }

                        //telefones 8

                        candidato.Email = reader.GetString(9);
                        candidato.EmailSecundario = reader.GetString(10);

                        //Validar a partir do nome, por conta dos acentos
                        candidato.PerfilProfissional = string.IsNullOrEmpty(reader.GetString(11)) ? 0 : (int)EnumExtensions.GetValueFromName<NivelProfissional>(reader.GetString(11));

                        //datainsricao (12)
                        //datainsricao (13)

                        candidato.NomeMae = reader.GetString(14);
                        candidato.NomePai = reader.GetString(15);

                        candidato.Nacionalidade = reader.GetString(16);
                        candidato.EstadoNascimento = reader.GetString(17)?.ConverterEstados();
                        candidato.Agrupadores = reader.GetString(18);

                        //candidato com foto (19)
                        //candidato com teste (20)

                        candidato.Cpf = ((double?)reader.GetDouble(21))?.ToString();
                        string possuiCnh = reader.GetString(22);
                        if (!string.IsNullOrEmpty(possuiCnh))
                            candidato.PossuiCnh = possuiCnh == "Sim" ? true : false;
                        if (candidato.PossuiCnh.Value)
                            candidato.CategoriaCnh = reader.GetString(23);
                        candidato.Identidade = reader.GetValue(24)?.ToString();
                        candidato.OrgaoEmissor = reader.GetValue(25)?.ToString();

                        //deficiencia 26
                        candidato.Cid = reader.GetString(27);
                        candidato.Observacoes = reader.GetString(28);

                        candidato.Cep = reader.GetValue(29)?.ToString();
                        candidato.Pais = reader.GetValue(30) != null ?
                                         (string?)(new RegionInfo(TextExtensions.GetISOCountryNameByCode(Convert.ToInt32(reader.GetValue(30)))).NativeName) :
                                         null;
                        candidato.Estado = reader.GetValue(31) != null ? (string?)((Estado)Convert.ToInt32(reader.GetValue(31))).GetEnumDisplayName().ConverterEstados() : null;
                        candidato.Cidade = reader.GetString(32);
                        candidato.Bairro = reader.GetString(33);
                        candidato.Endereco = reader.GetString(34);
                        candidato.Complemento = reader.GetValue(35)?.ToString();
                        //data 36
                        //coordenda 37
                        candidato.NivelProfissionalVagaDesejada = string.IsNullOrEmpty(reader.GetString(38)) ? 0 : (int)EnumExtensions.GetValueFromName<PretensaoSalarial>(reader.GetString(38));
                        candidato.DisponibilidadeHorario = string.IsNullOrEmpty(reader.GetString(39)) ? 0 : (int)EnumExtensions.GetValueFromName<DisponibilidadeHorario>(reader.GetString(39));
                        candidato.DisponibilidadeViagem = string.IsNullOrEmpty(reader.GetString(40)) ? 0 : (int)EnumExtensions.GetValueFromName<Disponibilidade>(reader.GetString(40));
                        candidato.DisponibilidadeTransferencia = string.IsNullOrEmpty(reader.GetString(41)) ? 0 : (int)EnumExtensions.GetValueFromName<Disponibilidade>(reader.GetString(41));

                        candidato.PretensaoSalarial = string.IsNullOrEmpty(reader.GetString(42)) ? 0 : (int)EnumExtensions.GetValueFromName<PretensaoSalarial>(reader.GetString(42));
                        candidato.LocalPreferencia = reader.GetString(43);
                        candidato.LocalPreferenciaSecundario = reader.GetString(44);

                        candidato.CargoInteresse = reader.GetString(45);
                        candidato.CargoInteresseSecundario = reader.GetString(46);

                        #region Formaçaõ do Candidato

                        if (!string.IsNullOrEmpty(reader.GetString(47)))
                        {
                            DateTime? dataInicio = null;
                            DateTime? dataConclusao = null;
                            if (!string.IsNullOrEmpty(reader.GetString(52)))
                            {
                                string[] intervaloConclusao = reader.GetString(52).Split("até");

                                if (!string.IsNullOrEmpty(intervaloConclusao[0]))
                                    dataInicio = DateTime.ParseExact(intervaloConclusao[0].Trim(), "yyyy-MM", cultureinfo);
                                if (!string.IsNullOrEmpty(intervaloConclusao[1]))
                                    dataConclusao = DateTime.ParseExact(intervaloConclusao[1].Trim(), "yyyy-MM", cultureinfo);
                            }


                            FormacaoCandidato formacao_1 = new FormacaoCandidato()
                            {
                                TipoFormacao = string.IsNullOrEmpty(reader.GetString(47)) ? 0 : (int)EnumExtensions.GetValueFromName<NivelFormacao>(reader.GetString(47)),
                                Modalidade = string.IsNullOrEmpty(reader.GetString(48)) ? 0 : (int)EnumExtensions.GetValueFromName<ModalidadeFormacao>(reader.GetString(48)),
                                Instituicao = reader.GetString(49),
                                Curso = reader.GetString(50),
                                Situacao = string.IsNullOrEmpty(reader.GetString(51)) ? 0 : (int)EnumExtensions.GetValueFromName<SituacaoFormacao>(reader.GetString(51)),
                                DataInicio = dataInicio,
                                DataConclusao = dataConclusao
                            };
                            candidato.FormacaoCandidato.Add(formacao_1);
                        }
                        if (!string.IsNullOrEmpty(reader.GetString(53)))
                        {
                            DateTime? dataInicio = null;
                            DateTime? dataConclusao = null;
                            if (!string.IsNullOrEmpty(reader.GetString(58)))
                            {
                                string[] intervaloConclusao = reader.GetString(58).Split("até");

                                if (!string.IsNullOrEmpty(intervaloConclusao[0]))
                                    dataInicio = DateTime.ParseExact(intervaloConclusao[0].Trim(), "yyyy-MM", cultureinfo);
                                if (!string.IsNullOrEmpty(intervaloConclusao[1]))
                                    dataConclusao = DateTime.ParseExact(intervaloConclusao[1].Trim(), "yyyy-MM", cultureinfo);
                            }

                            FormacaoCandidato formacao_2 = new FormacaoCandidato()
                            {
                                TipoFormacao = string.IsNullOrEmpty(reader.GetString(53)) ? 0 : (int)EnumExtensions.GetValueFromName<NivelFormacao>(reader.GetString(53)),
                                Modalidade = string.IsNullOrEmpty(reader.GetString(54)) ? 0 : (int)EnumExtensions.GetValueFromName<ModalidadeFormacao>(reader.GetString(54)),
                                Instituicao = reader.GetString(55),
                                Curso = reader.GetString(56),
                                Situacao = string.IsNullOrEmpty(reader.GetString(57)) ? 0 : (int)EnumExtensions.GetValueFromName<SituacaoFormacao>(reader.GetString(57)),
                                DataInicio = dataInicio,
                                DataConclusao = dataConclusao

                            };
                            candidato.FormacaoCandidato.Add(formacao_2);
                        }

                        if (!string.IsNullOrEmpty(reader.GetString(59)))
                        {
                            DateTime? dataInicio = null;
                            DateTime? dataConclusao = null;
                            if (!string.IsNullOrEmpty(reader.GetString(64)))
                            {
                                string[] intervaloConclusao = reader.GetString(64).Split("até");

                                if (!string.IsNullOrEmpty(intervaloConclusao[0]))
                                    dataInicio = DateTime.ParseExact(intervaloConclusao[0].Trim(), "yyyy-MM", cultureinfo);
                                if (!string.IsNullOrEmpty(intervaloConclusao[1]))
                                    dataConclusao = DateTime.ParseExact(intervaloConclusao[1].Trim(), "yyyy-MM", cultureinfo);
                            }

                            FormacaoCandidato formacao_3 = new FormacaoCandidato()
                            {
                                TipoFormacao = string.IsNullOrEmpty(reader.GetString(59)) ? 0 : (int)EnumExtensions.GetValueFromName<NivelFormacao>(reader.GetString(59)),
                                Modalidade = string.IsNullOrEmpty(reader.GetString(60)) ? 0 : (int)EnumExtensions.GetValueFromName<ModalidadeFormacao>(reader.GetString(60)),
                                Instituicao = reader.GetString(61),
                                Curso = reader.GetString(62),
                                Situacao = string.IsNullOrEmpty(reader.GetString(63)) ? 0 : (int)EnumExtensions.GetValueFromName<SituacaoFormacao>(reader.GetString(63)),
                                DataInicio = dataInicio,
                                DataConclusao = dataConclusao

                            };
                            candidato.FormacaoCandidato.Add(formacao_3);
                        }

                        #endregion

                        candidato.NomeProcesso = reader.GetString(65);
                        candidato.SituacaoPlanoSaude = reader.GetString(66);

                        if (reader.GetValue(67) != null)
                            candidato.DataSituacaoPlanoSaude = DateTime.ParseExact(reader.GetValue(67).ToString(), "dd/MM/yyyy", cultureinfo);

                        string primeiroEmprego = reader.GetString(68);
                        if (!string.IsNullOrEmpty(primeiroEmprego))
                            candidato.PrimeiroEmprego = primeiroEmprego == "Sim" ? true : false;


                        #region Experiecia Profissional
                        if (!string.IsNullOrEmpty(reader.GetString(69)))
                        {

                            ExperienciaProfissional experiencia_1 = new ExperienciaProfissional()
                            {
                                Empresa = reader.GetString(69),
                                Cargo = reader.GetString(70),
                                Salario = !string.IsNullOrEmpty(reader.GetString(71)) ? (decimal?)decimal.Parse(Regex.Replace(reader.GetString(71), @"[^\d.]", "")) : null,
                                DataAdmissao = reader.GetValue(72) != null ? (DateTime?)DateTime.ParseExact(reader.GetValue(72).ToString(), "dd/MM/yyyy", cultureinfo) : null,
                                DataDesligamento = reader.GetValue(73) != null && reader.GetValue(73).ToString() != "emprego atual" ? (DateTime?)DateTime.ParseExact(reader.GetValue(73).ToString(), "dd/MM/yyyy", cultureinfo) : null,
                                ResumoAtividades = reader.GetString(74)
                            };
                            candidato.ExperienciaProfissional.Add(experiencia_1);
                        }

                        if (!string.IsNullOrEmpty(reader.GetString(75)))
                        {

                            ExperienciaProfissional experiencia_2 = new ExperienciaProfissional()
                            {
                                Empresa = reader.GetString(75),
                                Cargo = reader.GetString(76),
                                Salario = !string.IsNullOrEmpty(reader.GetString(77)) ? (decimal?)decimal.Parse(Regex.Replace(reader.GetString(77), @"[^\d.]", "")) : null,
                                DataAdmissao = reader.GetValue(78) != null ? (DateTime?)DateTime.ParseExact(reader.GetValue(78).ToString(), "dd/MM/yyyy", cultureinfo) : null,
                                DataDesligamento = reader.GetValue(79) != null && reader.GetValue(79).ToString() != "emprego atual" ? (DateTime?)DateTime.ParseExact(reader.GetValue(79).ToString(), "dd/MM/yyyy", cultureinfo) : null,
                                ResumoAtividades = reader.GetString(80)
                            };
                            candidato.ExperienciaProfissional.Add(experiencia_2);
                        }

                        if (!string.IsNullOrEmpty(reader.GetString(81)))
                        {

                            ExperienciaProfissional experiencia_3 = new ExperienciaProfissional()
                            {
                                Empresa = reader.GetString(81),
                                Cargo = reader.GetString(82),
                                Salario = !string.IsNullOrEmpty(reader.GetString(83)) ? (decimal?)decimal.Parse(Regex.Replace(reader.GetString(83), @"[^\d.]", "")) : null,
                                DataAdmissao = reader.GetValue(84) != null ? (DateTime?)DateTime.ParseExact(reader.GetValue(84).ToString(), "dd/MM/yyyy", cultureinfo) : null,
                                DataDesligamento = reader.GetValue(85) != null && reader.GetValue(85).ToString() != "emprego atual" ? (DateTime?)DateTime.ParseExact(reader.GetValue(85).ToString(), "dd/MM/yyyy", cultureinfo) : null,
                                ResumoAtividades = reader.GetString(86)
                            };
                            candidato.ExperienciaProfissional.Add(experiencia_3);
                        }



                        #endregion


                        #endregion

                        //CandidatoService service = new CandidatoService();
                        bool sucesso = service.CadastrarCandidato(candidato);



                    }


                } while (reader.NextResult()); //Move to NEXT SHEET

            }


            return "Concluído com sucesso";
        }
    }
}
