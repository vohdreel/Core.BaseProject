using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.API.ViewModel
{
    public class ExperienciaProfissional
    {
        public int IdExperiencia { get; set; }
        public int IdCandidato { get; set; }
        public string Empresa { get; set; }        
        public string Cargo { get; set; }        
        public DateTime? DataAdmissao { get; set; }        
        public DateTime? DataDesligamento { get; set; }

        public ExperienciaProfissional(Global.DAO.Model.ExperienciaProfissional model)
        {
            IdExperiencia = model.Id;
            IdCandidato = model.IdCandidato;
            Empresa = model.Empresa;
            Cargo = model.Cargo;
            DataAdmissao = model.DataAdmissao;
            DataDesligamento = model.DataDesligamento;      
        
        }
    }
}
