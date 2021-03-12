using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Global.Util.SystemEnumerations
{
    public enum VagaModalidade
    {
        [Display(Name = "A Combinar", ShortName = "N/A")]
        NaoDisponivel = 0,
        [Display(Name = "CLT", ShortName = "CLT")]
        CLT = 1,
        [Display(Name = "PJ", ShortName = "PJ")]
        PJ = 2,
        [Display(Name = "Estágio", ShortName = "ESTAG")]
        Estagio = 3,
        [Display(Name = "Temporário", ShortName = "TEMP")]
        Temporario = 4,
        [Display(Name = "Terceirizado", ShortName = "TERC")]
        Terceirizado = 5

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
        PelaManha = 3,
        [Display(Name = "Pela Tarde")]
        PelaTarde = 4,
        [Display(Name = "Pela Noite")]
        PelaNoite = 5


    }

    public enum EnumRaca
    {

        [Display(Name = "Preta")]
        Preta = 1,
        [Display(Name = "Amarela")]
        Amarela = 2,
        [Display(Name = "Indigena")]
        Indigena = 3,
        [Display(Name = "Branca")]
        Branca = 4,
        [Display(Name = "Parda")]
        Parda = 5


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
        [Display(Name = "Profissional (nível superior)")]
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
        Separado = 5,
        [Display(Name = "União Estável")]
        UniaoEstavel = 6


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

    public enum NivelFormacao
    {
        [Display(Name = "Ensino Fundamental")]
        Fundamental = 1,
        [Display(Name = "Ensino Médio / Técnico")]
        MedioOuTecnico = 2,
        [Display(Name = "Graduação")]
        Graduacao = 3,
        [Display(Name = "Pós-graduação")]
        PosGraduacao = 4

    }


    public enum SituacaoFormacao
    {
        [Display(Name = "Em Curso")]
        EmCurso = 1,
        [Display(Name = "Incompleto")]
        Incompleto = 2,
        [Display(Name = "Concluído")]
        Concluido = 3

    }

    public enum ModalidadeFormacao
    {
        [Display(Name = "Médio")]
        Medio = 1,
        [Display(Name = "Técnico")]
        Tecnico = 2,
        [Display(Name = "Licenciatura")]
        Licenciatura = 3,
        [Display(Name = "Bacharelado")]
        Bacharelado = 4,

    }


    public enum Estado
    {
        [Display(Name = ("Acre"))]
        AC = 1,
        [Display(Name = ("Alagoas"))]
        AL = 2,
        [Display(Name = ("Amapá"))]
        AP = 3,
        [Display(Name = ("Amazonas"))]
        AM = 4,
        [Display(Name = ("Bahia"))]
        BA = 5,
        [Display(Name = ("Ceará"))]
        CE = 6,
        [Display(Name = ("Distrito Federal"))]
        DF = 7,
        [Display(Name = ("Espirito Santo"))]
        ES = 8,
        [Display(Name = ("Goiás"))]
        GO = 9,
        [Display(Name = ("Maranhão"))]
        MA = 10,
        [Display(Name = ("Mato Grosso"))]
        MT = 11,
        [Display(Name = ("Mato Grosso do Sul"))]
        MS = 12,
        [Display(Name = ("Minas Gerais"))]
        MG = 13,
        [Display(Name = ("Pará"))]
        PA = 14,
        [Display(Name = ("Paraíba"))]
        PB = 15,
        [Display(Name = ("Paraná"))]
        PR = 16,
        [Display(Name = ("Pernambuco"))]
        PE = 17,
        [Display(Name = ("Piauí"))]
        PI = 18,
        [Display(Name = ("Rio de Janeiro"))]
        RJ = 19,
        [Display(Name = ("Rio Grande do Norte"))]
        RN = 20,
        [Display(Name = ("Rio Grande do Sul"))]
        RS = 21,
        [Display(Name = ("Rondônia"))]
        RO = 22,
        [Display(Name = ("Roraima"))]
        RR = 23,
        [Display(Name = ("Santa Catarina"))]
        SC = 24,
        [Display(Name = ("São Paulo"))]
        SP = 25,
        [Display(Name = ("Sergipe"))]
        SE = 26,
        [Display(Name = ("Tocantins"))]
        TO = 27
    }


}
