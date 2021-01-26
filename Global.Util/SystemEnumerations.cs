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
        Estagio = 3

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

    public enum StatusCandidatura
    {
        [Display(Name = "Inscrito")]
        Inscrito = 1,
        [Display(Name = "Aprovado")]
        Aprovado = 2,
        [Display(Name = "Reprovado")]
        Reprovado = 3,
        [Display(Name = "Em Entrevista")]
        EmEntrevista = 4,
        [Display(Name = "Em Teste")]
        EmTeste = 5
    }


}
