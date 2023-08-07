using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using System;

namespace LaRosalinaAPI.Repository
{
    public class ComprobanteRepository : IComprobanteRepository
    {
        private readonly LaRosalinaDbContext _context;

        public ComprobanteRepository(LaRosalinaDbContext context)
        {
            _context = context;
        }

        public ICollection<comprobante> Get()
        {
            var comprobantes = _context.comprobantes
                .Where(c => c.activo == 1)
                .ToList();

            return comprobantes;
        }

        public ICollection<comprobante> GetByRandomId(string randomId)
        {
            var comprobantes = _context.comprobantes
                .Where(c => c.id_random == randomId && c.activo == 1)
                .ToList();

            return comprobantes;
        }

        public ICollection<comprobante> GetByMovimiento(int idMovimiento)
        {
            var comprobantes = _context.comprobantes
                .Where(c => c.id_random == _context.movimientos
                    .Where(m => m.id == idMovimiento)
                    .Select(m => m.id_random)
                    .FirstOrDefault() && c.activo == 1)
                .ToList();

            return comprobantes;
        }

        public ICollection<comprobante> GetByGasto(int idGasto)
        {
            var comprobantes = _context.comprobantes
                .Where(c => c.id_random == _context.movimientos
                    .Where(m => m.id_gasto == idGasto)
                    .Select(m => m.id_random)
                    .FirstOrDefault() && c.activo == 1)
                .ToList();

            return comprobantes;
        }

        public ICollection<comprobante> GetByReserva(int idReserva)
        {
            var comprobantes = (from movimiento in _context.movimientos
                                join comprobante in _context.comprobantes on movimiento.id_random equals comprobante.id_random
                                where movimiento.id_reserva == idReserva && movimiento.eliminado == 0 && comprobante.activo == 1
                                select comprobante)
                     .ToList();

            return comprobantes;
        }

        public comprobante Get(int id)
        {
            var comprobante = _context.comprobantes
               .FirstOrDefault(c => c.id == id);

            return comprobante;
        }

        public void Add(comprobante comprobante, int idUser)
        {
            comprobante.f_alta = DateTime.Now;
            comprobante.usuario_alta = idUser;
            comprobante.activo = 1;
            _context.comprobantes.Add(comprobante);
        }

        public bool Delete(comprobante comprobante, int idUser)
        {
            comprobante.f_baja = DateTime.Now;
            comprobante.usuario_baja = idUser;
            comprobante.activo = 0;
            _context.comprobantes.Update(comprobante);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}

