using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using System.Drawing.Drawing2D;

namespace LaRosalinaAPI.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private LaRosalinaDbContext _context;
        private readonly IMapper _mapper;
        private IComprobanteRepository _ComprobanteRepository;
        private IReservaRepository _ReservaRepository;

        public UnitOfWork(LaRosalinaDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public IReservaRepository ReservaRepository
        {
            get
            {
                return _ReservaRepository == null ?
                    _ReservaRepository = new ReservaRepository(_context, _mapper) :
                    _ReservaRepository;
            }
        }

        public IComprobanteRepository ComprobanteRepository
        {
            get
            {
                return _ComprobanteRepository == null ?
                    _ComprobanteRepository = new ComprobanteRepository(_context) :
                    _ComprobanteRepository;
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}
