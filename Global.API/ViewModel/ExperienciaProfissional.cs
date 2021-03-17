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
        public string DataAdmissao { get; set; }        
        public string DataDesligamento { get; set; }

        public ExperienciaProfissional(Global.DAO.Model.ExperienciaProfissional model)
        {
            Id = model.Id;
            IdCandidato = model.IdCandidato;
            Empresa = model.Empresa;
            Cargo = model.Cargo;
            DataAdmissao = model.DataAdmissao?.ToString("dd/MM/yyyy");
            DataDesligamento = model.DataDesligamento?.ToString("dd/MM/yyyy");


        }
    }
}
