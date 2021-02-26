using Global.DAO.Model;
using Global.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Global.DAO.Interface.Service
{
    public interface IServiceCandidato : IService<Candidato>
    {
        public Candidato BuscarPorIdAspNetUser(string IdAspNetUsers);
        public bool ExisteCpfUsuario(string cpf);
        public void AlternarMaterConectado(string IdAspNetUsers, bool value);
        public bool VerificarManterConectado(int IdCandidato);
        public Coordinates BuscarCoordenadasCandidato(int idCandidato);
        public string MontarVagaEndereco(Candidato candidato);
        public Candidato PreencherCoordenadas(Candidato candidato);

    }
}
