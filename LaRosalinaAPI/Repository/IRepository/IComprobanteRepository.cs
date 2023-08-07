using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IComprobanteRepository
    {
        ICollection<comprobante> Get();

        ICollection<comprobante> GetByRandomId(string randomId);

        ICollection<comprobante> GetByGasto(int idGasto);

        ICollection<comprobante> GetByMovimiento(int idMovimiento);

        ICollection<comprobante> GetByReserva(int idReserva);

        comprobante Get(int id);

        void Add(comprobante comprobante, int idUser);

        bool Delete(comprobante comprobante, int idUser);

        bool Save();

    }
}
