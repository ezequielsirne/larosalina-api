namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IReservaRepository ReservaRepository { get; }

        public IComprobanteRepository ComprobanteRepository { get; }

        public bool Save();

    }
}
