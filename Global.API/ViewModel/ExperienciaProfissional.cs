using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class ExperienciaProfissional
    {
        public int Id { get; set; }
        public int IdCandidato { get; set; }
        public string Empresa { get; set; }        
        public string Cargo { get; set; }        
        public string StringDataAdmissao { get; set; }        
        public string StringDataDesligamento { get; set; }

        public DateTime? DataAdmissao { get; set; }
        public DateTime? DataDesligamento { get; set; }

        public ExperienciaProfissional(Global.DAO.Model.ExperienciaProfissional model)
        {
            Id = model.Id;
            IdCandidato = model.IdCandidato;
            Empresa = model.Empresa;
            Cargo = model.Cargo;
            StringDataAdmissao = model.DataAdmissao?.ToString("dd/MM/yyyy");
            StringDataDesligamento = model.DataDesligamento?.ToString("dd/MM/yyyy");
            DataAdmissao = model.DataAdmissao;
            DataDesligamento = model.DataDesligamento;

        }
    }
}
