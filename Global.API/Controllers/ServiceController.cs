using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
                    rejetctedVagas.Add(job);

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


    }
}
