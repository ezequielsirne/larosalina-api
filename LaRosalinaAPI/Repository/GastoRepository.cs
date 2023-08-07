using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using LaRosalinaAPI.Utils;
using System.Globalization;

namespace LaRosalinaAPI.Repository
{
    public class GastoRepository : IGastoRepository
    {
        private readonly LaRosalinaDbContext _context;
        private readonly IMapper _mapper;

        public GastoRepository(LaRosalinaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<GastoDto> Get()
        {
            var gastos = _context.gastos
                .OrderBy(g => g.fecha)
                .Select(g => new GastoDto
                {
                    id = g.id,
                    fecha = g.fecha,
                    descripcion = g.descripcion,
                    categoria = g.categoria,
                    cuenta = g.cuenta,
                    importe = g.importe,
                    observaciones = g.observaciones,
                    id_random = g.id_random,
                    eliminado = g.eliminado,
                    usuario_alta = g.usuario_alta,
                    f_alta = g.f_alta,
                    usuario_mod = g.usuario_mod,
                    f_mod = g.f_mod,
                    usuario_baja = g.usuario_baja,
                    f_baja = g.f_baja,
                    categoriaDto = _mapper.Map<CategoriaGastosDto>(_context.categorias_gastos.FirstOrDefault(c => c.id == g.categoria)),
                    cuentaDto = _mapper.Map<CuentaDto>(_context.cuentas.FirstOrDefault(c => c.id == g.cuenta)),
                    movimientos = _mapper.Map<List<MovimientoDto>>(_context.movimientos.Where(m => m.id_gasto == g.id)),
                    responsable = _mapper.Map<UsuarioDto>(_context.usuarios.FirstOrDefault(u => u.id == g.usuario_alta))
                })
                .Where(g => g.eliminado == 0)
                .ToList();

            return gastos;
        }

        public GastoDto Get(int id)
        {
            var gasto = _context.gastos
                .OrderBy(g => g.fecha)
                .Select(g => new GastoDto
                {
                    id = g.id,
                    fecha = g.fecha,
                    descripcion = g.descripcion,
                    categoria = g.categoria,
                    cuenta = g.cuenta,
                    importe = g.importe,
                    observaciones = g.observaciones,
                    id_random = g.id_random,
                    eliminado = g.eliminado,
                    usuario_alta = g.usuario_alta,
                    f_alta = g.f_alta,
                    usuario_mod = g.usuario_mod,
                    f_mod = g.f_mod,
                    usuario_baja = g.usuario_baja,
                    f_baja = g.f_baja,
                    categoriaDto = _mapper.Map<CategoriaGastosDto>(_context.categorias_gastos.FirstOrDefault(c => c.id == g.categoria)),
                    cuentaDto = _mapper.Map<CuentaDto>(_context.cuentas.FirstOrDefault(c => c.id == g.cuenta)),
                    movimientos = _mapper.Map<List<MovimientoDto>>(_context.movimientos.Where(m => m.id_gasto == g.id)),
                    responsable = _mapper.Map<UsuarioDto>(_context.usuarios.FirstOrDefault(u => u.id == g.usuario_alta))
                })
                .FirstOrDefault(g => g.id == id);

            return gasto;
        }

        public bool Add(gasto gasto, int idUser)
        {
            //Creamos el random_id
            string _random_id = Tools.RandomString(20);

            gasto.id_random = _random_id;
            gasto.usuario_alta = idUser;
            gasto.eliminado = 0;

            var movimiento = new movimiento
            {
                fecha = gasto.fecha,
                descripcion = "Gasto: " + gasto.descripcion,
                tipo = 2,
                cuenta = gasto.cuenta,
                movimiento1 = -gasto.importe,
                id_random = _random_id,
                f_alta = gasto.f_alta,
                usuario_alta = idUser,
                eliminado = 0
            };
            gasto.movimientos.Add(movimiento);
            _context.gastos.Add(gasto);
            return Save();
        }

        public bool Delete(gasto gasto, int idUser)
        {
            gasto.f_baja = DateTime.Now;
            gasto.usuario_baja = idUser;
            gasto.eliminado = 1;
            foreach (var movimiento in gasto.movimientos)
            {
                movimiento.f_baja = DateTime.Now;
                movimiento.usuario_baja = idUser;
                movimiento.eliminado = 1;
                _context.movimientos.Update(movimiento);
            }
            _context.gastos.Update(gasto);
            return Save();
        }

        public bool Update(gasto gasto, int idUser)
        {
            gasto.f_mod = DateTime.Now;
            gasto.usuario_mod = idUser;
            foreach (var movimiento in gasto.movimientos)
            {
                movimiento.fecha = gasto.fecha;
                movimiento.descripcion = "Gasto: " + gasto.descripcion;
                movimiento.cuenta = gasto.cuenta;
                movimiento.movimiento1 = -gasto.importe;
                movimiento.f_mod = DateTime.Now;
                movimiento.usuario_mod = idUser;
                _context.movimientos.Update(movimiento);
            }
            _context.gastos.Update(gasto);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public ICollection<InformesGasto> GetGastos(InformeRequest request)
        {
            var desde = request.Desde.Date;
            var hasta = request.Hasta.Date.AddDays(1).AddMinutes(-1);

            var total = (_context.gastos
                .Where(g => g.eliminado == 0 && g.fecha >= desde && g.fecha <= hasta)
                .Sum(g => g.importe) ?? 0);

            var resultados = (from g in _context.gastos
                              join c in _context.categorias_gastos on g.categoria equals c.id
                              where g.eliminado == 0 && g.fecha >= desde && g.fecha <= hasta
                              group g by c.descripcion into gastosGrupo
                              select new InformesGasto
                              {
                                  descripcion = gastosGrupo.Key,
                                  total = (decimal)gastosGrupo.Sum(g => g.importe),
                                  participacion = (decimal)(gastosGrupo.Sum(g => g.importe) / (total==0? 1 : total))
                              })
                              .OrderByDescending(r => r.participacion)
                              .ToList();

            return resultados;

        }
    }
}
