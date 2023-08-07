using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Data.Entity.SqlServer;
using AutoMapper;
using System.Globalization;
using System.Linq;

namespace LaRosalinaAPI.Repository
{
    public class ReservaRepository : IReservaRepository
    {

        private readonly LaRosalinaDbContext _context;
        private readonly IMapper _mapper;

        public ReservaRepository(LaRosalinaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //En este caso usamos el Dto para traer los datos de la entidad asocidad (departamento)
        //En Data first, cuando existen entidades asociadas por claves foraneas se crean propiedades virtualas en ambas entidades
        //Por lo tanto si usamos la clase original "reserva" se genera una referencia circular: reserva trae departamento y departamento trae todas las reservas asociadas
        //Usando Cade first esto no sucede

        public ICollection<ReservaDto> Get()
        {
            var reservas = from r in _context.reservas
                           join e in _context.estado_reservas on r.estado equals e.id into estadoReservas
                           from estadoReserva in estadoReservas.DefaultIfEmpty()
                           where r.eliminado == 0
                           orderby r.checkin
                           select new ReservaDto
                            {
                                id = r.id,
                                checkin = r.checkin,
                                checkout = r.checkout,
                                departamento = r.departamento,
                                late_checkout = r.late_checkout,
                                noches = r.noches,
                                nombre_apellido = r.nombre_apellido,
                                telefono = r.telefono,
                                mail = r.mail,
                                localidad = r.localidad,
                                adultos = r.adultos,
                                menores = r.menores,
                                mascotas = r.mascotas,
                                observaciones = r.observaciones,
                                total = r.total,
                                saldo = r.total - _context.movimientos.Where(m => r.id == m.id_reserva && m.eliminado == 0).Sum(m => m.movimiento1),
                                estado = r.estado,
                                estadoDescripcion = _context.reservas.Any(R1 =>
                                            R1.eliminado == 0 && R1.id != r.id && r.departamento == R1.departamento &&
                                            ((r.checkin >= R1.checkin && r.checkin <= R1.checkout || r.checkout >= R1.checkin && r.checkout <= R1.checkout) || 
                                            (R1.checkin >= r.checkin && R1.checkin <= r.checkout || R1.checkout >= r.checkin && R1.checkout <= r.checkout))) ?
                                            "En conflicto" : estadoReserva.descripcion,
                                eliminado = r.eliminado,
                                usuario_alta = r.usuario_alta,
                                f_alta = r.f_alta,
                                usuario_mod = r.usuario_mod,
                                f_mod = r.f_mod,
                                usuario_baja = r.usuario_baja,
                                f_baja = r.f_baja,
                                departamentoDto = _mapper.Map<DepartamentoDto>(_context.departamentos.FirstOrDefault(d => d.id == r.departamento)),
                                huespedes = _context.huespedes.Where(h => h.id_reserva == r.id).ToList(),
                                movimientos = _context.movimientos.Where(m => m.id_reserva == r.id).ToList()
                           };

            return reservas.ToList();
        }


        public ReservaDto Get(int idReserva)
        {
            var reserva = _context.reservas
                .OrderBy(r => r.checkin)
                .Select(r => new ReservaDto
                {
                    id = r.id,
                    checkin = r.checkin,
                    checkout = r.checkout,
                    departamento = r.departamento,
                    late_checkout = r.late_checkout,
                    noches = r.noches,
                    nombre_apellido = r.nombre_apellido,
                    telefono = r.telefono,
                    mail = r.mail,
                    localidad = r.localidad,
                    adultos = r.adultos,
                    menores = r.menores,
                    mascotas = r.mascotas,
                    observaciones = r.observaciones,
                    total = r.total,
                    saldo = r.total - _context.movimientos.Where(m => r.id == m.id_reserva && m.eliminado == 0).Sum(m => m.movimiento1),
                    estado = r.estado,
                    eliminado = r.eliminado,
                    usuario_alta = r.usuario_alta,
                    f_alta = r.f_alta,
                    usuario_mod = r.usuario_mod,
                    f_mod = r.f_mod,
                    usuario_baja = r.usuario_baja,
                    f_baja = r.f_baja
                }).FirstOrDefault(r => r.id == idReserva);

            return reserva;
        }

        public bool Add(reserva reserva, int idUser)
        {
            reserva.f_alta = DateTime.Now;
            reserva.usuario_alta = idUser;
            reserva.eliminado = 0;
            _context.reservas.Add(reserva);
            return Save();
        }

        public bool Update(reserva reserva, int idUser)
        {
            reserva.f_mod = DateTime.Now;
            reserva.usuario_mod = idUser;
            _context.reservas.Update(reserva);
            return Save();
        }       

        public ICollection<ReservaDto> ReservasEnConflicto()
        {
            var reservas  = (from R1 in _context.reservas
                  join E in _context.estado_reservas on R1.estado equals E.id
                  join R2 in _context.reservas on R1.departamento equals R2.departamento
                  where R1.eliminado == 0 && R2.eliminado == 0 && R1.id != R2.id
                  && (R1.checkin >= R2.checkin && R1.checkin <= R2.checkout || R1.checkout >= R2.checkin && R1.checkout <= R2.checkout)
                  select new ReservaDto
                  {
                      id = R1.id,
                      checkin = R1.checkin,
                      checkout = R1.checkout,
                      departamento = R1.departamento,
                      late_checkout = R1.late_checkout,
                      noches = R1.noches,
                      nombre_apellido = R1.nombre_apellido,
                      telefono = R1.telefono,
                      mail = R1.mail,
                      localidad = R1.localidad,
                      adultos = R1.adultos,
                      menores = R1.menores,
                      mascotas = R1.mascotas,
                      total = R1.total,
                      saldo = R1.total - _context.movimientos.Where(m => R1.id == m.id_reserva && m.eliminado == 0).Sum(m => m.movimiento1),
                      estado = R1.estado,
                      eliminado = R1.eliminado,
                      usuario_alta = R1.usuario_alta,
                      f_alta = R1.f_alta,
                      usuario_mod = R1.usuario_mod,
                      f_mod = R1.f_mod,
                      usuario_baja = R1.usuario_baja,
                      f_baja = R1.f_baja,
                      departamentoDto = _mapper.Map<DepartamentoDto>(_context.departamentos.FirstOrDefault(d => d.id == R1.departamento))
                  }).Distinct();

            var reservasList = reservas.ToList();

            foreach (var reserva in reservasList)
            {
                reserva.huespedes = _context.huespedes.Where(h => h.id_reserva == reserva.id).ToList();
                reserva.movimientos = _context.movimientos.Where(m => m.id_reserva == reserva.id).ToList();
            }

            return reservasList;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool Delete(reserva reserva, int idUser)
        {
            reserva.f_baja = DateTime.Now;
            reserva.usuario_baja = idUser;
            reserva.eliminado = 1;
            foreach (var huesped in reserva.huespedes)
            {
                huesped.f_baja = DateTime.Now;
                huesped.usuario_baja = idUser;
                huesped.eliminado = 1;
                _context.huespedes.Update(huesped);
            }
            foreach (var movimiento in reserva.movimientos)
            {
                movimiento.f_baja = DateTime.Now;
                movimiento.usuario_baja = idUser;
                movimiento.eliminado = 1;
                _context.movimientos.Update(movimiento);
            }
            _context.reservas.Update(reserva);
            return Save();
        }

        public DashboardIndicadores GetIndicadores()
        {
            //DateTime hoy = DateTime.UtcNow.AddHours(-3);
            DateTime hoy = new DateTime(2023, 1, 15); //Demo
            DateTime desde = new DateTime(hoy.Year, hoy.Month, 1);
            DateTime hasta = desde.AddMonths(1).AddDays(-1);

            var noches = _context.reservas
                .Where(r => (r.checkin >= desde && r.checkin <= hasta) || (r.checkout >= desde && r.checkout <= hasta))
                .Where(r => r.eliminado == 0)
                .OrderBy(r => r.checkin)
                .Select(r => new noche
                {
                    checkin = r.checkin,
                    checkout = r.checkout,
                    noches = r.checkin >= desde && r.checkout <= hasta ? r.noches :
                             r.checkin >= desde && r.checkout > hasta ? (int?)(EF.Functions.DateDiffDay(r.checkin, hasta.AddDays(1)).Value) :
                             r.checkin < desde && r.checkout <= hasta ? (int?)(EF.Functions.DateDiffDay(desde, r.checkout).Value) :
                             0,
                    totalPPN = r.checkin >= desde && r.checkout <= hasta ? r.total :
                               r.checkin >= desde && r.checkout > hasta ? r.total / r.noches * (int?)(EF.Functions.DateDiffDay(r.checkin, hasta.AddDays(1)).Value) :
                               r.checkin < desde && r.checkout <= hasta ? r.total / r.noches * (int?)(EF.Functions.DateDiffDay(desde, r.checkout).Value) :
                               0,
                    total = r.checkin >= desde ? r.total : 0,
                    saldo = r.checkin >= desde ? r.total - (_context.movimientos
                                                            .Where(m => m.id_reserva == r.id && m.eliminado == 0)
                                                            .Sum(m => m.movimiento1) ?? 0) : 0
                })
                .ToList();

            if (noches.Count > 0)
            {
                var dashboardIndicadores = noches.Select(r => new DashboardIndicadores
                {
                    ocupacion = (decimal)noches.Sum(r => r.noches) / (decimal)((hasta - desde).TotalDays * 7),
                    ppn = (decimal)noches.Sum(r => r.totalPPN) / ((decimal)noches.Sum(r => r.noches) != 0 ? (decimal)noches.Sum(r => r.noches) : 1),
                    ingresos = (decimal)noches.Sum(r => r.total),
                    saldo = (decimal)noches.Sum(r => r.saldo),
                    efectivo = (decimal)(_context.movimientos
                    .Where(m => m.eliminado == 0 && m.cuenta == 1)
                    .Sum(m => m.movimiento1) ?? 0)
                }).FirstOrDefault();

                return dashboardIndicadores;
            }
            else
            {
                var dashboardIndicadores = new DashboardIndicadores
                {
                    ocupacion = 0,
                    ppn = 0,
                    ingresos = 0,
                    saldo = 0,
                    efectivo = (decimal)(_context.movimientos
                    .Where(m => m.eliminado == 0 && m.cuenta == 1)
                    .Sum(m => m.movimiento1) ?? 0)
                };

                return dashboardIndicadores;
            }


        }

        public InformesIndicadores GetIndicadores(InformeRequest request)
        {
            var hoy = DateTime.UtcNow.AddHours(-3);
            var desde = request.Desde.Date;
            var hasta = request.Hasta.Date.AddDays(1).AddMinutes(-1);

            var noches = _context.reservas
                .Where(r => (r.checkin >= desde && r.checkin <= hasta) || (r.checkout >= desde && r.checkout <= hasta))
                .Where(r => r.eliminado == 0)
                .OrderBy(r => r.checkin)
                .Select(r => new noche
                {
                    checkin = r.checkin,
                    checkout = r.checkout,
                    noches = r.checkin >= desde && r.checkout <= hasta ? r.noches :
                             r.checkin >= desde && r.checkout > hasta ? (int?)(EF.Functions.DateDiffDay(r.checkin, hasta.AddDays(1)).Value) :
                             r.checkin < desde && r.checkout <= hasta ? (int?)(EF.Functions.DateDiffDay(desde, r.checkout).Value) :
                             0,
                    totalPPN = r.checkin >= desde && r.checkout <= hasta ? r.total :
                               r.checkin >= desde && r.checkout > hasta ? r.total / r.noches * (int?)(EF.Functions.DateDiffDay(r.checkin, hasta.AddDays(1)).Value) :
                               r.checkin < desde && r.checkout <= hasta ? r.total / r.noches * (int?)(EF.Functions.DateDiffDay(desde, r.checkout).Value) :
                               0,
                    total = r.checkin >= desde ? r.total : 0,
                    saldo = r.checkin >= desde ? r.total - (_context.movimientos
                                                            .Where(m => m.id_reserva == r.id && m.eliminado == 0)
                                                            .Sum(m => m.movimiento1) ?? 0) : 0
                })
                .ToList();

            if (noches.Count > 0)
            {
                var ingresos = (decimal)noches.Sum(r => r.total);
                var gastos = (_context.gastos
                     .Where(g => g.eliminado == 0 && g.fecha >= desde && g.fecha <= hasta)
                     .Sum(g => g.importe) ?? 0);

                var informesIndicadores = noches.Select(r => new InformesIndicadores
                {
                    ocupacion = (decimal)noches.Sum(r => r.noches) / (decimal)((hasta - desde).TotalDays * 7),
                    ppn = (decimal)noches.Sum(r => r.totalPPN) / ((decimal)noches.Sum(r => r.noches) != 0 ? (decimal)noches.Sum(r => r.noches) : 1),
                    ingresos = ingresos,
                    cantidad = noches.Count(),
                    saldo = (decimal)noches.Sum(r => r.saldo),
                    efectivo = (_context.movimientos
                    .Where(m => m.eliminado == 0 && m.cuenta == 1)
                    .Sum(m => m.movimiento1) ?? 0),
                    gastos = gastos,
                    resultado = ingresos - gastos
            }).FirstOrDefault();

                return informesIndicadores;
            }
            else
            {
                var gastos = (_context.gastos
                     .Where(g => g.eliminado == 0 && g.fecha >= desde && g.fecha <= hasta)
                     .Sum(g => g.importe) ?? 0);

                var informesIndicadores = new InformesIndicadores
                {
                    ocupacion = 0,
                    ppn = 0,
                    ingresos = 0,
                    cantidad = 0,
                    saldo = 0,
                    efectivo = (decimal)(_context.movimientos
                    .Where(m => m.eliminado == 0 && m.cuenta == 1)
                    .Sum(m => m.movimiento1) ?? 0),
                    gastos = (decimal)(_context.gastos
                    .Where(g => g.eliminado == 0 && g.fecha >= desde && g.fecha <= hasta)
                    .Sum(g => g.importe) ?? 0),
                    resultado = - gastos
                };

                return informesIndicadores;
            }


        }

        public ICollection<InformeEconomico> GetEconomico(InformeRequest request)
        {
            var desde = request.Desde.Date;
            var hasta = request.Hasta.Date.AddDays(1).AddMinutes(-1);

            var noches = new List<InformeEconomicoNoche>();

            while (desde <= hasta)
            {
                //Para obtener el útimo día del mes actual creamos una fecha con el primer día del mes actual
                //Sumamos un mes y tenemos el primer día del mes siguiente, restamos un minuto y tenemos el último minuto del último día del mes actual
                var hastaMes = new DateTime(desde.Year, desde.Month, 1).AddMonths(1).AddMinutes(-1); 

                var nochesMes = _context.reservas
                    .Where(r => ((r.checkin >= desde && r.checkin <= hastaMes) || (r.checkout >= desde && r.checkout <= hastaMes))
                                && r.eliminado == 0)
                    .OrderBy(r => r.checkin)
                    .Select(r => new InformeEconomicoNoche
                    {
                        Mes = desde,
                        Noches = r.checkin >= desde && r.checkout <= hastaMes ? r.noches :
                                 r.checkin >= desde && r.checkout > hastaMes ? (int?)(EF.Functions.DateDiffDay(r.checkin, r.checkout).Value + 1) :
                                 r.checkin < desde && r.checkout <= hastaMes ? (int?)(EF.Functions.DateDiffDay(desde, r.checkout).Value) : 0,
                        TotalPPN = r.checkin >= desde && r.checkout <= hastaMes ? r.total :
                                   r.checkin >= desde && r.checkout > hastaMes ? r.total / r.noches * (int?)(EF.Functions.DateDiffDay(r.checkin, r.checkout).Value + 1) :
                                   r.checkin < desde && r.checkout <= hastaMes ? r.total / r.noches * (int?)(EF.Functions.DateDiffDay(desde, r.checkout).Value) : 0,
                        Total = r.checkin >= desde ? r.total : 0,
                        Saldo = r.checkin >= desde ? r.total - _context.movimientos
                                                                .Where(m => m.id_reserva == r.id && m.eliminado == 0)
                                                                .Sum(m => m.movimiento1) : 0
                    }).ToList();

                noches.AddRange(nochesMes);

                desde = new DateTime(desde.Year, desde.Month, 1).AddMonths(1);
            }

            var resultados = noches
                .GroupBy(n => n.Mes)
                .Select(n => new InformeEconomico
                {
                    mes = n.Key.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES")),
                    ocupacion = (decimal)n.Sum(n => n.Noches) / ((n.Key.AddMonths(1) - n.Key).Days * 7.0m),
                    ppn = (decimal)(n.Sum(n => n.TotalPPN) / n.Sum(n => n.Noches)),
                    ingresos = (decimal)n.Sum(n => n.Total),
                    gastos = (decimal)_context.gastos
                        .Where(g => g.eliminado == 0 && ((DateTime)(g.fecha)).Month == n.Key.Month && ((DateTime)(g.fecha)).Year == n.Key.Year)
                        .Sum(g => g.importe),
                    resultado = (decimal)(n.Sum(n => n.Total) - _context.gastos
                        .Where(g => g.eliminado == 0 && ((DateTime)(g.fecha)).Month == n.Key.Month && ((DateTime)(g.fecha)).Year == n.Key.Year)
                        .Sum(g => g.importe))
                })
                .ToList();

            return resultados;

        }
    }
}
