using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class FormacaoCandidato
    {
        public int IdFormacao { get; set; }
        public int IdCandidato { get; set; }
        public int? TipoFormacao { get; set; }
        public int? Modalidade { get; set; }
        public string Instituicao { get; set; }
        public string Curso { get; set; }
        public int? Situacao { get; set; }
        public string DataConclusao { get; set; }
        public string DataInicio { get; set; }


        public FormacaoCandidato(Global.DAO.Model.FormacaoCandidato model)
        {
            IdFormacao = model.Id;
            IdCandidato = model.IdCandidato;
            TipoFormacao = model.TipoFormacao;
            Modalidade = model.Modalidade;
            Curso = model.Curso;
            Situacao = model.Situacao;
            DataConclusao = model.DataConclusao?.ToString("dd/MM/yyyy");
            DataInicio = model.DataInicio?.ToString("dd/MM/yyyy");


        }

    }
}
