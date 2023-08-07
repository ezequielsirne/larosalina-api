using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IHuespedRepository
    {
        ICollection<HuespedDto> Get();

        ICollection<HuespedDto> Get(int IdReserva);

        HuespedDto GetHuesped(int id);

        bool Add(huespede huesped, int idUser);

        bool Delete(huespede huesped, int idUser);

        bool Update(huespede huesped, int idUser);

        bool Save();


    }
}
