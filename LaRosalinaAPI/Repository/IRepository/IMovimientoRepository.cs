using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IMovimientoRepository
    {
        ICollection<MovimientoDto> Get();

        ICollection<MovimientoDto> GetByReserva(int idReserva); 

        MovimientoDto Get(int id);

        bool Add(movimiento movimiento, int idUser);

        bool Delete(movimiento movimiento, int idUser);

        bool Update(movimiento movimiento, int idUser);

        bool Save();

        ICollection<InformeFinanciero> GetFinanciero(InformeRequest request);

    }
}
