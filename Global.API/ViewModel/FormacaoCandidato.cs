using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class FormacaoCandidato
    {
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public int? TipoFormacao { get; set; }
        public int? Modalidade { get; set; }
        public string Instituicao { get; set; }
        public string Curso { get; set; }
        public int? Situacao { get; set; }
        public string StringDataConclusao { get; set; }
        public string StringDataInicio { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataConclusao { get; set; }


        public FormacaoCandidato(Global.DAO.Model.FormacaoCandidato model)
        {
            Id = model.Id;
            IdCandidato = model.IdCandidato;
            TipoFormacao = model.TipoFormacao;
            Modalidade = model.Modalidade;
            Curso = model.Curso;
            Situacao = model.Situacao;
            Instituicao = model.Instituicao;
            StringDataConclusao = model.DataConclusao?.ToString("MM/yyyy");
            StringDataInicio = model.DataInicio?.ToString("MM/yyyy");
            DataConclusao = model.DataConclusao;
            DataInicio = model.DataInicio;


        }

    }
}
