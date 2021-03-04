using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Global.Util.SystemEnumerations
{
    public enum VagaModalidade
    {
        [Display(Name = "CLT")]
        CLT = 1,
        [Display(Name = "PJ")]
        PJ = 2,
        [Display(Name = "Estário")]
        Estagio = 3,
        [Display(Name = "Temporário")]
        Temporario = 4

    }

    public enum Disponibilidade
    {
        [Display(Name = "Sim")]
        Sim = 1,
        [Display(Name = "Não")]
        Nao = 2,
        [Display(Name = "Negociável")]
        Negociavel = 3

    }

    public enum NivelFluencia
    {
        [Display(Name = "Básico")]
        Basico = 1,
        [Display(Name = "Intermediário")]
        Intermediario = 2,
        [Display(Name = "Avançado")]
        Avancado = 3,
        [Display(Name = "Fluente")]
        Fluente = 4

    }

    public enum PretensaoSalarial
    {
        [Display(Name = "Até R$1000 mês")]
        Ate1000 = 1,
        [Display(Name = "R$1001 a R$2000 mês")]
        Entre1001a2000 = 2,
        [Display(Name = "R$2001 a R$3000 mês")]
        Entre2001a3000 = 3,
        [Display(Name = "R$3001 a R$4500 mês")]
        Entre3001a4500 = 4,
        [Display(Name = "R$6001 a R$8000 mês")]
        Entre6001a8000 = 5,
        [Display(Name = "Acima R$8000 mês")]
        Acima8000 = 6,
        [Display(Name = "A combinar")]
        ACombinar = 7


    }

    public enum NomeTipoDocumento
    {
        [Display(Name = "Curriculum Vitae")]
        CurriculumVitae = 1,
        [Display(Name = "Documento com Foto")]
        DocumentoComFoto = 2,
        [Display(Name = "Comprovante de Residência")]
        ComprovanteDeResidencia = 3

    }

    public enum DisponibilidadeHorario
    {

        [Display(Name = "Integral")]
        Integral = 1,
        [Display(Name = "Total")]
        Total = 2,
        [Display(Name = "Pela Manhã")]
        PelaManha= 3,
        [Display(Name = "Pela Tarde")]
        PelaTarde = 4,
        [Display(Name = "Pela Noite")]
        PelaNoite = 5


    }

    public enum NivelProfissional
    {

        [Display(Name = "Estagio (nível médio/técnico)")]
        Estagio = 1,
        [Display(Name = "Estagio (nível superior)")]
        EstagioNivelSuperior = 2,
        [Display(Name = "Profissional (nível fundamental)")]
        ProfissionalFundamental = 3,
        [Display(Name = "Profissional (nível médio/técnico)")]
        ProfissionalNvelTecnico = 4,
        [Display(Name = "Profissional  (nível superior)")]
        ProfissionaNivelSuperior = 5


    }

    public enum EnumEstadoCIvil
    {

        [Display(Name = "Solteiro(a)")]
        Solteiro = 1,
        [Display(Name = "Casado(a)")]
        Casado = 2,
        [Display(Name = "Divorciado(a)")]
        Divoricado = 3,
        [Display(Name = "Viúvo(a)")]
        Viuvo = 4,
        [Display(Name = "Separado(a)")]
        Separado = 5


    }

    public enum StatusCandidatura
    {
        [Display(Name = "Inscrito")]
        Inscrito = 1,
        [Display(Name = "Participando")]
        Participando = 2,
        [Display(Name = "Aprovado")]
        Aprovado = 3,
        [Display(Name = "Reprovado")]
        Reprovado = 4,
        [Display(Name = "Em Entrevista")]
        EmEntrevista = 5,
        [Display(Name = "Em Teste")]
        EmTeste = 6

    }

    public enum StatusProcesso
    {
        [Display(Name = "Em Andamento")]
        EmAndamento = 1,
        [Display(Name = "Encerrado")]
        Encerrado = 2,

    }


    public enum StatusVaga
    {
        [Display(Name = "Aberta")]
        Aberta = 1,
        [Display(Name = "Preenchida")]
        Preenchida = 2,
        [Display(Name = "Revogada")]
        Revogada = 3

    }


}
