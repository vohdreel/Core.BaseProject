using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ExcelDataReader;
using ExcelNumberFormat;
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

        [HttpPost("PostTest")]
        public object PostTest()
        {
            return "Post Test Ok!";

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

        [HttpPost("GenerateCandidatos")]
        public async Task<object> GenerateCandidatos()
        {

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            System.Globalization.CultureInfo cultureinfo =
        new System.Globalization.CultureInfo("pt-BR");

            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            bool first = true;
            string idLegado = "";
            var formFile = Request.Form.Files[0].OpenReadStream();
            LogCandidatoService log = new LogCandidatoService();
            using (var reader = ExcelReaderFactory.CreateReader(formFile))
            {

                do
                {
                    while (reader.Read()) //Each ROW
                    {
                        idLegado = reader.GetValue(1)?.ToString();
                        try
                        {

                            CandidatoService service = new CandidatoService();
                            if (first) { first = !first; continue; }
                            if (service.ExisteCpfUsuario(reader.GetValue(21).ToString())) { continue; }

                            Candidato candidato = new Candidato();

                            //return new
                            //{
                            //    format = GetFormattedValue(reader, 13, cultureinfo),
                            //    raw = reader.GetValue(13).ToString(),
                            //    dataConvertida = (DateTime?)DateTime.Parse(GetFormattedValue(reader, 13, cultureinfo), cultureinfo),

                            //};


                            #region Informações Pessoais

                            candidato.Nome = reader.GetValue(0)?.ToString() ?? "N/A";
                            candidato.Idlegado = reader.GetValue(1)?.ToString();


                            candidato.DataNascimento = reader.GetValue(2) != null ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 2, cultureinfo), cultureinfo) : null;
                            //idade 3
                            candidato.Sexo = reader.GetValue(4)?.ToString();

                            string raca = reader.GetValue(5)?.ToString();
                            if (string.IsNullOrEmpty(raca))
                            {
                                //candidato.Raca = (int)EnumRaca.Indisponivel;
                            }
                            else
                            {
                                if (raca.Contains("Preto"))
                                    raca = "Preto";

                                //candidato.Raca = (int)(Enum.Parse(typeof(EnumRaca), TextExtensions.GetRacaValue(raca)));
                            }

                            //candidato.EstadoCivil = (int)(Enum.Parse(typeof(EnumEstadoCIvil), reader.GetString(6)));
                            candidato.EstadoCivil = string.IsNullOrEmpty(reader.GetValue(6)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromShortName<EnumEstadoCivil>(reader.GetValue(6)?.ToString());

                            string deficiente = reader.GetValue(7)?.ToString();
                            if (!string.IsNullOrEmpty(deficiente))
                            {
                                candidato.Deficiente = deficiente == "Sim" ? true : false;

                            }

                            //telefones 8

                            string telefone = reader.GetString(8);

                            List<TelefoneCandidato> telefones = new List<TelefoneCandidato>();
                            if (!string.IsNullOrEmpty(telefone))
                            {
                                var arrayTelefones = telefone.Split(";");
                                foreach (var tel in arrayTelefones)
                                {
                                    telefones.Add(new TelefoneCandidato
                                    {
                                        IdTelefoneNavigation = new Telefone { Numero = tel }
                                    });
                                }
                            }

                            candidato.TelefoneCandidato = telefones;

                            candidato.Email = reader.GetValue(9)?.ToString() ?? "N/A";
                            candidato.EmailSecundario = reader.GetValue(10)?.ToString();

                            //Validar a partir do nome, por conta dos acentos
                            candidato.PerfilProfissional = string.IsNullOrEmpty(reader.GetValue(11)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<NivelProfissional>(reader.GetValue(11)?.ToString());

                            //datainsricao (12)
                            candidato.DataInscricao = reader.GetValue(12) != null ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 12, cultureinfo), cultureinfo) : null;

                            //datainsricao (13)

                            candidato.NomePai = reader.GetValue(14)?.ToString();
                            candidato.NomeMae = reader.GetValue(15)?.ToString();


                            candidato.Nacionalidade = reader.GetValue(16)?.ToString();
                            candidato.EstadoNascimento = reader.GetValue(17)?.ToString()?.ConverterEstados();
                            candidato.Agrupadores = reader.GetValue(18)?.ToString();

                            //candidato com foto (19)
                            //candidato com teste (20)
                            string cpf = reader.GetValue(21)?.ToString();

                            //if (cpf.Length > 11)
                            //    continue;

                            candidato.Cpf = reader.GetValue(21)?.ToString();
                            string possuiCnh = reader.GetValue(22)?.ToString();
                            if (!string.IsNullOrEmpty(possuiCnh))
                                candidato.PossuiCnh = possuiCnh == "Sim" ? true : false;
                            if (candidato.PossuiCnh.Value)
                                candidato.CategoriaCnh = reader.GetValue(23)?.ToString();
                            candidato.Identidade = reader.GetValue(24)?.ToString();
                            candidato.OrgaoEmissor = reader.GetValue(25)?.ToString();

                            //deficiencia 26
                            candidato.Cid = reader.GetValue(27)?.ToString();
                            candidato.Observacoes = reader.GetValue(28)?.ToString();

                            candidato.Cep = reader.GetValue(29)?.ToString();
                            candidato.Pais = reader.GetValue(30) != null ?
                                             (string?)(new RegionInfo(TextExtensions.GetISOCountryNameByCode(Convert.ToInt32(reader.GetValue(30)))).NativeName) :
                                             null;
                            try
                            {
                                candidato.Estado = reader.GetValue(31) != null ? (string?)((Estado)Convert.ToInt32(reader.GetValue(31))).GetEnumDisplayName().ConverterEstados() : null;
                            }
                            catch (Exception e)
                            {
                                candidato.Estado = null;
                            }

                            candidato.Cidade = reader.GetValue(32)?.ToString();
                            candidato.Bairro = reader.GetValue(33)?.ToString();
                            candidato.Endereco = reader.GetValue(34)?.ToString();
                            candidato.Complemento = reader.GetValue(35)?.ToString();
                            //data 36
                            //coordenda 37
                            candidato.NivelProfissionalVagaDesejada = string.IsNullOrEmpty(reader.GetValue(38)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<NivelProfissional>(reader.GetValue(38)?.ToString());
                            candidato.DisponibilidadeHorario = string.IsNullOrEmpty(reader.GetValue(39)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<DisponibilidadeHorario>(reader.GetValue(39)?.ToString());
                            candidato.DisponibilidadeViagem = string.IsNullOrEmpty(reader.GetValue(40)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<Disponibilidade>(reader.GetValue(40)?.ToString());
                            candidato.DisponibilidadeTransferencia = string.IsNullOrEmpty(reader.GetValue(41)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<Disponibilidade>(reader.GetValue(41)?.ToString());

                            candidato.PretensaoSalarial = string.IsNullOrEmpty(reader.GetValue(42)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<PretensaoSalarial>(reader.GetValue(42)?.ToString());
                            candidato.LocalPreferencia = reader.GetValue(43)?.ToString();
                            candidato.LocalPreferenciaSecundario = reader.GetValue(44)?.ToString();

                            candidato.CargoInteresse = reader.GetValue(45)?.ToString();
                            candidato.CargoInteresseSecundario = reader.GetValue(46)?.ToString();

                            #region Formaçaõ do Candidato

                            if (!string.IsNullOrEmpty(reader.GetValue(47)?.ToString()))
                            {
                                DateTime dataInicio = new DateTime();
                                DateTime dataConclusao = new DateTime();
                                if (!string.IsNullOrEmpty(reader.GetValue(52)?.ToString()))
                                {
                                    string[] intervaloConclusao = reader.GetValue(52)?.ToString().Split("até");

                                    if (!string.IsNullOrEmpty(intervaloConclusao[0]))
                                        DateTime.TryParse(intervaloConclusao[0].Trim(), cultureinfo, DateTimeStyles.None, out dataInicio);
                                    if (!string.IsNullOrEmpty(intervaloConclusao[1]))
                                        DateTime.TryParse(intervaloConclusao[0].Trim(), cultureinfo, DateTimeStyles.None, out dataInicio);

                                    //dataConclusao = DateTime.Parse(intervaloConclusao[1].Trim(), cultureinfo);
                                }


                                FormacaoCandidato formacao_1 = new FormacaoCandidato()
                                {
                                    TipoFormacao = string.IsNullOrEmpty(reader.GetValue(47)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<NivelFormacao>(reader.GetValue(47)?.ToString()),
                                    Modalidade = string.IsNullOrEmpty(reader.GetValue(48)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<ModalidadeFormacao>(reader.GetValue(48)?.ToString()),
                                    Instituicao = reader.GetValue(49)?.ToString(),
                                    Curso = reader.GetValue(50)?.ToString(),
                                    Situacao = string.IsNullOrEmpty(reader.GetValue(51)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<SituacaoFormacao>(reader.GetValue(51)?.ToString()),
                                    DataInicio = dataInicio,
                                    DataConclusao = dataConclusao
                                };
                                candidato.FormacaoCandidato.Add(formacao_1);
                            }

                            if (!string.IsNullOrEmpty(reader.GetValue(53)?.ToString()))
                            {
                                DateTime? dataInicio = null;
                                DateTime? dataConclusao = null;
                                if (!string.IsNullOrEmpty(reader.GetValue(58)?.ToString()))
                                {
                                    string[] intervaloConclusao = reader.GetValue(58)?.ToString().Split("até");

                                    if (!string.IsNullOrEmpty(intervaloConclusao[0]))
                                        dataInicio = DateTime.Parse(intervaloConclusao[0].Trim(), cultureinfo);
                                    if (!string.IsNullOrEmpty(intervaloConclusao[1]))
                                        dataConclusao = DateTime.Parse(intervaloConclusao[1].Trim(), cultureinfo);
                                }

                                FormacaoCandidato formacao_2 = new FormacaoCandidato()
                                {
                                    TipoFormacao = string.IsNullOrEmpty(reader.GetValue(53)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<NivelFormacao>(reader.GetValue(53)?.ToString()),
                                    Modalidade = string.IsNullOrEmpty(reader.GetValue(54)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<ModalidadeFormacao>(reader.GetValue(54)?.ToString()),
                                    Instituicao = reader.GetValue(55)?.ToString(),
                                    Curso = reader.GetValue(56)?.ToString(),
                                    Situacao = string.IsNullOrEmpty(reader.GetValue(57)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<SituacaoFormacao>(reader.GetValue(57)?.ToString()),
                                    DataInicio = dataInicio,
                                    DataConclusao = dataConclusao

                                };
                                candidato.FormacaoCandidato.Add(formacao_2);
                            }

                            if (!string.IsNullOrEmpty(reader.GetValue(59)?.ToString()))
                            {
                                DateTime? dataInicio = null;
                                DateTime? dataConclusao = null;
                                if (!string.IsNullOrEmpty(reader.GetValue(64)?.ToString()))
                                {
                                    string[] intervaloConclusao = reader.GetValue(64)?.ToString().Split("até");

                                    if (!string.IsNullOrEmpty(intervaloConclusao[0]))
                                        dataInicio = DateTime.Parse(intervaloConclusao[0].Trim(), cultureinfo);
                                    if (!string.IsNullOrEmpty(intervaloConclusao[1]))
                                        dataConclusao = DateTime.Parse(intervaloConclusao[1].Trim(), cultureinfo);
                                }

                                FormacaoCandidato formacao_3 = new FormacaoCandidato()
                                {
                                    TipoFormacao = string.IsNullOrEmpty(reader.GetValue(59)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<NivelFormacao>(reader.GetValue(59)?.ToString()),
                                    Modalidade = string.IsNullOrEmpty(reader.GetValue(60)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<ModalidadeFormacao>(reader.GetValue(60)?.ToString()),
                                    Instituicao = reader.GetValue(61)?.ToString(),
                                    Curso = reader.GetValue(62)?.ToString(),
                                    Situacao = string.IsNullOrEmpty(reader.GetValue(63)?.ToString()) ? 0 : (int)EnumExtensions.GetValueFromName<SituacaoFormacao>(reader.GetValue(63)?.ToString()),
                                    DataInicio = dataInicio,
                                    DataConclusao = dataConclusao

                                };
                                candidato.FormacaoCandidato.Add(formacao_3);
                            }

                            #endregion

                            candidato.NomeProcesso = reader.GetValue(65)?.ToString();
                            candidato.SituacaoPlanoSaude = reader.GetValue(66)?.ToString();

                            if (reader.GetValue(67) != null)
                                candidato.DataSituacaoPlanoSaude = DateTime.Parse(GetFormattedValue(reader, 67, cultureinfo), cultureinfo);

                            string primeiroEmprego = reader.GetValue(68)?.ToString();
                            if (!string.IsNullOrEmpty(primeiroEmprego))
                                candidato.PrimeiroEmprego = primeiroEmprego == "Sim" ? true : false;


                            #region Experiecia Profissional
                            if (!string.IsNullOrEmpty(reader.GetValue(69)?.ToString()))
                            {

                                ExperienciaProfissional experiencia_1 = new ExperienciaProfissional()
                                {
                                    Empresa = reader.GetValue(69)?.ToString(),
                                    Cargo = reader.GetValue(70)?.ToString(),
                                    Salario = !string.IsNullOrEmpty(reader.GetValue(71)?.ToString()) ? (decimal?)decimal.Parse(Regex.Replace(reader.GetValue(71)?.ToString(), @"[^\d,]", "")) : null,
                                    DataAdmissao = reader.GetValue(72) != null ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 72, cultureinfo), cultureinfo) : null,
                                    DataDesligamento = reader.GetValue(73) != null && reader.GetValue(73).ToString() != "emprego atual" ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 73, cultureinfo), cultureinfo) : null,
                                    ResumoAtividades = reader.GetValue(74)?.ToString()
                                };
                                candidato.ExperienciaProfissional.Add(experiencia_1);
                            }

                            if (!string.IsNullOrEmpty(reader.GetValue(75)?.ToString()))
                            {

                                ExperienciaProfissional experiencia_2 = new ExperienciaProfissional()
                                {
                                    Empresa = reader.GetValue(75)?.ToString(),
                                    Cargo = reader.GetValue(76)?.ToString(),
                                    Salario = !string.IsNullOrEmpty(reader.GetValue(77)?.ToString()) ? (decimal?)decimal.Parse(Regex.Replace(reader.GetValue(77)?.ToString(), @"[^\d,]", "")) : null,
                                    DataAdmissao = reader.GetValue(78) != null ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 78, cultureinfo), cultureinfo) : null,
                                    DataDesligamento = reader.GetValue(79) != null && reader.GetValue(79)?.ToString() != "emprego atual" ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 79, cultureinfo), cultureinfo) : null,
                                    ResumoAtividades = reader.GetValue(80)?.ToString()
                                };
                                candidato.ExperienciaProfissional.Add(experiencia_2);
                            }

                            if (!string.IsNullOrEmpty(reader.GetValue(81)?.ToString()))
                            {

                                ExperienciaProfissional experiencia_3 = new ExperienciaProfissional()
                                {
                                    Empresa = reader.GetValue(81)?.ToString(),
                                    Cargo = reader.GetValue(82)?.ToString(),
                                    Salario = !string.IsNullOrEmpty(reader.GetString(83)) ? (decimal?)decimal.Parse(Regex.Replace(reader.GetString(83), @"[^\d,]", "")) : null,
                                    DataAdmissao = reader.GetValue(84) != null ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 84, cultureinfo), cultureinfo) : null,
                                    DataDesligamento = reader.GetValue(85) != null && reader.GetValue(85).ToString() != "emprego atual" ? (DateTime?)DateTime.Parse(GetFormattedValue(reader, 85, cultureinfo), cultureinfo) : null,
                                    ResumoAtividades = reader.GetValue(86)?.ToString()
                                };
                                candidato.ExperienciaProfissional.Add(experiencia_3);
                            }



                            #endregion


                            #endregion

                            string exception;
                            //CandidatoService service = new CandidatoService();
                            bool sucesso = service.CadastrarCandidato(candidato, out exception);
                            if (sucesso)
                            {
                                var user = new IdentityUser();
                                user.UserName = candidato.Cpf;
                                user.Email = candidato.Email;

                                string userPWD = TextExtensions.RandomPassword(6);

                                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);
                                if (chkUser.Succeeded)
                                {
                                    candidato.IdAspNetUsers = user.Id;
                                    candidato.SenhaCriptografada = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userPWD));
                                    service.AtualizarCandidato(candidato);
                                }
                                else
                                {
                                    log.Salvar(new LogCandidato
                                    {

                                        IdLegado = idLegado,
                                        DataLog = DateTime.Now,
                                        StackTrace = string.Join(",", chkUser.Errors.Select(x => x.Description).ToArray()),

                                    });

                                }
                            }

                            if (!sucesso)
                            {
                                log.Salvar(new LogCandidato
                                {

                                    IdLegado = idLegado,
                                    DataLog = DateTime.Now,
                                    StackTrace = exception,

                                });
                            }
                        }
                        catch (Exception e)
                        {
                            log.Salvar(new LogCandidato
                            {

                                IdLegado = idLegado,
                                DataLog = DateTime.Now,
                                StackTrace = e.Message + " --------- " + e.StackTrace

                            });
                        }
                    }
                } while (reader.NextResult()); //Move to NEXT SHEET

            }


            return "Concluído com sucesso";

        }

        string GetFormattedValue(IExcelDataReader reader, int columnIndex, CultureInfo culture)
        {
            var value = reader.GetValue(columnIndex);
            var formatString = reader.GetNumberFormatString(columnIndex);
            if (formatString != null)
            {
                var format = new NumberFormat(formatString);
                return format.Format(value, culture);
            }
            return Convert.ToString(value, culture);
        }
    }
}
