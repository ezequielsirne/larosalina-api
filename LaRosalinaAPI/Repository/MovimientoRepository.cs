using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;

namespace LaRosalinaAPI.Repository
{
    public class MovimientoRepository : IMovimientoRepository
    {
        private readonly LaRosalinaDbContext _context;
        private readonly IMapper _mapper;

        public MovimientoRepository(LaRosalinaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<MovimientoDto> Get()
        {
            var movimientos = _context.movimientos
                .OrderBy(m => m.fecha)
                .Select(m => new MovimientoDto
                {
                    id = m.id,
                    fecha = m.fecha,
                    descripcion = m.descripcion,
                    id_reserva = m.id_reserva,
                    id_gasto = m.id_gasto,
                    id_transferencia = m.id_transferencia,
                    tipo = m.tipo,
                    cuenta = m.cuenta,
                    movimiento1 = m.movimiento1,
                    id_random = m.id_random,
                    eliminado = m.eliminado,
                    usuario_alta = m.usuario_alta,
                    f_alta = m.f_alta,
                    usuario_mod = m.usuario_mod,
                    f_mod = m.f_mod,
                    usuario_baja = m.usuario_baja,
                    f_baja = m.f_baja,
                    cuentaDto = _mapper.Map<CuentaDto>(_context.cuentas.FirstOrDefault(c => c.id == m.cuenta)),
                    id_gastoNavigation = _mapper.Map<GastoDto>(_context.gastos.FirstOrDefault(g => g.id == m.id_gasto)),
                    tipoDto = _mapper.Map<MovimientoTiposDto>(_context.movimientos_tipos.FirstOrDefault(t => t.id == m.tipo))
                })
                .Where(m => m.eliminado == 0)
                .ToList();

            return movimientos;
        }

        public ICollection<MovimientoDto> GetByReserva(int idReserva)
        {
            var movimientos = _context.movimientos
                .OrderBy(m => m.fecha)
                .Select(m => new MovimientoDto
                {
                    id = m.id,
                    fecha = m.fecha,
                    descripcion = m.descripcion,
                    id_reserva = m.id_reserva,
                    id_gasto = m.id_gasto,
                    id_transferencia = m.id_transferencia,
                    tipo = m.tipo,
                    cuenta = m.cuenta,
                    movimiento1 = m.movimiento1,
                    id_random = m.id_random,
                    eliminado = m.eliminado,
                    usuario_alta = m.usuario_alta,
                    f_alta = m.f_alta,
                    usuario_mod = m.usuario_mod,
                    f_mod = m.f_mod,
                    usuario_baja = m.usuario_baja,
                    f_baja = m.f_baja,
                    cuentaDto = _mapper.Map<CuentaDto>(_context.cuentas.FirstOrDefault(c => c.id == m.cuenta)),
                    tipoDto = _mapper.Map<MovimientoTiposDto>(_context.movimientos_tipos.FirstOrDefault(t => t.id == m.tipo))
                })
                .Where(m => m.eliminado == 0 && m.id_reserva == idReserva)
                .ToList();

            return movimientos;
        }

        public MovimientoDto Get(int id)
        {
            var movimiento = _context.movimientos
                .OrderBy(m => m.fecha)
                .Select(m => new MovimientoDto
                {
                    id = m.id,
                    fecha = m.fecha,
                    descripcion = m.descripcion,
                    id_reserva = m.id_reserva,
                    id_gasto = m.id_gasto,
                    id_transferencia = m.id_transferencia,
                    tipo = m.tipo,
                    cuenta = m.cuenta,
                    movimiento1 = m.movimiento1,
                    id_random = m.id_random,
                    eliminado = m.eliminado,
                    usuario_alta = m.usuario_alta,
                    f_alta = m.f_alta,
                    usuario_mod = m.usuario_mod,
                    f_mod = m.f_mod,
                    usuario_baja = m.usuario_baja,
                    f_baja = m.f_baja,
                    cuentaDto = _mapper.Map<CuentaDto>(_context.cuentas.FirstOrDefault(c => c.id == m.cuenta)),
                    id_gastoNavigation = _mapper.Map<GastoDto>(_context.gastos.FirstOrDefault(g => g.id == m.id_gasto)),
                    tipoDto = _mapper.Map<MovimientoTiposDto>(_context.movimientos_tipos.FirstOrDefault(t => t.id == m.tipo))
                })
                .FirstOrDefault(m => m.id == id);

            return movimiento;
        }

        public bool Add(movimiento movimiento, int idUser)
        {
            movimiento.f_alta = DateTime.Now;
            movimiento.usuario_alta = idUser;
            movimiento.eliminado = 0;
            _context.movimientos.Add(movimiento);
            return Save();
        }

        public bool Delete(movimiento movimiento, int idUser)
        {
            movimiento.f_baja = DateTime.Now;
            movimiento.usuario_baja = idUser;
            movimiento.eliminado = 1;
            if(movimiento.id_gasto != null)
            {
                movimiento.id_gastoNavigation.f_baja = DateTime.Now;
                movimiento.id_gastoNavigation.usuario_baja = idUser;
                movimiento.id_gastoNavigation.eliminado = 1;
            }            
            _context.movimientos.Update(movimiento);
            return Save();
        }

        public bool Update(movimiento movimiento, int idUser)
        {
            movimiento.f_mod = DateTime.Now;
            movimiento.usuario_mod = idUser;
            if(movimiento?.id_gastoNavigation != null)
            {
                movimiento.id_gastoNavigation.f_mod = DateTime.Now;
                movimiento.id_gastoNavigation.usuario_mod = idUser;
                movimiento.id_gastoNavigation.fecha = movimiento.fecha;
                movimiento.id_gastoNavigation.importe = -movimiento.movimiento1;
                movimiento.id_gastoNavigation.cuenta = movimiento.cuenta;
                movimiento.id_gastoNavigation.usuario_mod = idUser;
            }            
            _context.movimientos.Update(movimiento);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public ICollection<InformeFinanciero> GetFinanciero(InformeRequest request)
        {
            var desde = request.Desde.Date;
            var hasta = request.Hasta.Date.AddDays(1).AddMinutes(-1);

            var financiero = (from m in _context.movimientos
                              join c in _context.cuentas on m.cuenta equals c.id
                              where m.eliminado == 0 && c.activo == 1
                              group m by c.descripcion into g
                              select new InformeFinanciero
                              {
                                  cuenta = g.Key,
                                  inicial = g.Sum(m => m.fecha <= desde ? m.movimiento1 : 0),
                                  ingresos = g.Sum(m => m.fecha > desde && m.fecha < hasta && m.tipo == 1 ? m.movimiento1 : 0),
                                  gastos = g.Sum(m => m.fecha > desde && m.fecha < hasta && m.tipo == 2 ? m.movimiento1 : 0),
                                  salientes = g.Sum(m => m.fecha > desde && m.fecha < hasta && m.tipo == 3 && m.movimiento1 < 0 ? m.movimiento1 : 0),
                                  entrantes = g.Sum(m => m.fecha > desde && m.fecha < hasta && m.tipo == 3 && m.movimiento1 > 0 ? m.movimiento1 : 0),
                                  final = g.Sum(m => m.fecha <= hasta ? m.movimiento1 : 0)
                              }).ToList();

            var total = new InformeFinanciero
            {
                cuenta = "Total",
                inicial = financiero.Sum(f => f.inicial) ?? 0,
                ingresos = financiero.Sum(f => f.ingresos) ?? 0,
                gastos = financiero.Sum(f => f.gastos) ?? 0,
                salientes = financiero.Sum(f => f.salientes) ?? 0,
                entrantes = financiero.Sum(f => f.entrantes) ?? 0,
                final = financiero.Sum(f => f.final) ?? 0
            };

            financiero.Add(total);

            return financiero;
        }
    }
}

