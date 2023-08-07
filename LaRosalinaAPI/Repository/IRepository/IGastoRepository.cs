using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IGastoRepository
    {
        ICollection<GastoDto> Get();

        GastoDto Get(int id);

        ICollection<InformesGasto> GetGastos(InformeRequest request);

        bool Add(gasto gasto, int idUser);

        bool Delete(gasto gasto, int idUser);

        bool Update(gasto gasto, int idUser);

        bool Save();

    }
}
