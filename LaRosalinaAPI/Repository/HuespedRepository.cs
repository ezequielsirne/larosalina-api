using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;

namespace LaRosalinaAPI.Repository
{
    public class HuespedRepository : IHuespedRepository
    {
        private readonly LaRosalinaDbContext _context;
        private readonly IMapper _mapper;

        public HuespedRepository(LaRosalinaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<HuespedDto> Get()
        {
            var huespedes = _context.huespedes
                .OrderBy(h => h.checkin)
                .Select(h => new HuespedDto
                {
                    id = h.id,
                    nombre_apellido = h.nombre_apellido,
                    dni = h.dni,
                    domicilio = h.domicilio,
                    patente = h.patente,
                    checkin = h.checkin,
                    checkout = h.checkout,
                    id_reserva = h.id_reserva,
                    eliminado = h.eliminado,
                    usuario_alta = h.usuario_alta,
                    f_alta = h.f_alta,
                    usuario_mod = h.usuario_mod,
                    f_mod = h.f_mod,
                    usuario_baja = h.usuario_baja,
                    f_baja = h.f_baja,
                    reserva = _mapper.Map<ReservaDto>(_context.reservas.FirstOrDefault(r => r.id == h.id_reserva))
                })
                .Where(h => h.eliminado == 0).ToList();
            return huespedes;
        }

        public HuespedDto GetHuesped(int idHuesped)
        {
            var huespedes = _context.huespedes
                .Select(h => new HuespedDto
                {
                    id = h.id,
                    nombre_apellido = h.nombre_apellido,
                    dni = h.dni,
                    domicilio = h.domicilio,
                    patente = h.patente,
                    checkin = h.checkin,
                    checkout = h.checkout,
                    id_reserva = h.id_reserva,
                    eliminado = h.eliminado,
                    usuario_alta = h.usuario_alta,
                    f_alta = h.f_alta,
                    usuario_mod = h.usuario_mod,
                    f_mod = h.f_mod,
                    usuario_baja = h.usuario_baja,
                    f_baja = h.f_baja,
                    reserva = _mapper.Map<ReservaDto>(_context.reservas.FirstOrDefault(r => r.id == h.id_reserva))
                })
                .FirstOrDefault(h => h.id == idHuesped);
            return huespedes;
        }

        public bool Add(huespede huesped, int idUser)
        {
            var reserva = _context.reservas.FirstOrDefault(r => r.id == huesped.id_reserva);
            huesped.nombre_apellido = huesped.nombre_apellido.ToUpper();
            huesped.checkin = reserva.checkin;
            huesped.checkout = reserva.checkout;
            huesped.patente = huesped.patente.ToUpper();
            huesped.f_alta = DateTime.Now;
            huesped.usuario_alta = idUser;
            huesped.eliminado = 0;
            _context.huespedes.Add(huesped);
            return Save();
        }

        public bool Delete(huespede huesped, int idUser)
        {
            huesped.f_baja = DateTime.Now;
            huesped.usuario_baja = idUser;
            huesped.eliminado = 1;
            _context.huespedes.Update(huesped);
            return Save();
        }


        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool Update(huespede huesped, int idUser)
        {
            huesped.nombre_apellido = huesped.nombre_apellido.ToUpper();
            huesped.patente = huesped.patente.ToUpper();
            huesped.f_mod = DateTime.Now;
            huesped.usuario_mod = idUser;
            _context.huespedes.Update(huesped);
            return Save();
        }

        ICollection<HuespedDto> IHuespedRepository.Get(int IdReserva)
        {
            var huespedes = _context.huespedes.Where(h => h.id_reserva == IdReserva && h.eliminado == 0).ToList();
            return _mapper.Map<ICollection<HuespedDto>>(huespedes);
        }
    }
}
