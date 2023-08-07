using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IReservaRepository
    {
        ICollection<ReservaDto> Get();

        ReservaDto Get(int id);

        bool Add(reserva reserva, int idUser);

        bool Update(reserva reserva, int idUser);

        bool Delete(reserva reserva, int idUser);

        DashboardIndicadores GetIndicadores();

        InformesIndicadores GetIndicadores(InformeRequest request);

        ICollection<InformeEconomico> GetEconomico(InformeRequest request);

        ICollection<ReservaDto> ReservasEnConflicto();

        bool Save();

    }
}
