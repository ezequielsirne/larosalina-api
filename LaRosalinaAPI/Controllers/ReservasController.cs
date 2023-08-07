using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository;
using LaRosalinaAPI.Repository.IRepository;
using LaRosalinaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Security.Claims;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaRepository _Repository;
        private readonly IComprobanteRepository _ComprobanteRepository;
        private readonly IMapper _mapper;

        public ReservasController(IReservaRepository Repository, IMapper mapper, IComprobanteRepository comprobanteRepository)
        {
            _Repository = Repository;
            _mapper = mapper;
            _ComprobanteRepository = comprobanteRepository;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetReservas() {
            var listado = _Repository.Get();
            return Ok(listado);
        }

        [Authorize]
        [HttpGet("{idReserva:int}", Name = "GetReserva")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetReserva(int idReserva)
        {
            var item = _Repository.Get(idReserva);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReservaAddDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddReserva([FromBody] ReservaAddDto reservaAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (reservaAddDto == null)
            {
                return BadRequest(ModelState);
            }

            var reserva = _mapper.Map<reserva>(reservaAddDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            reserva.nombre_apellido = reserva.nombre_apellido.ToUpper();
            reserva.late_checkout = Convert.ToSByte(reserva.checkout.Value.TimeOfDay > new TimeSpan(11, 0, 0));
            if (reserva.checkin.HasValue && reserva.checkout.HasValue && reserva.checkin.Value.Date.Equals(reserva.checkout.Value.Date))
            {
                // Ambas fechas son el mismo día
                reserva.checkin = reserva.checkin.Value.Date.AddHours(10);
                reserva.checkout = reserva.checkout.Value.Date.AddHours(18);
            }
            reserva.estado = 1;

            // Eliminar los objetos con nombre y apellido vacíos

            List<huespede> huespedes = reserva.huespedes.ToList();
            huespedes.RemoveAll(h => string.IsNullOrEmpty(h.nombre_apellido));
            reserva.huespedes = huespedes;
            foreach (var huesped in reserva.huespedes)
            {
                huesped.checkin = reserva.checkin;
                huesped.checkout = reserva.checkout;
                huesped.nombre_apellido = huesped.nombre_apellido.ToUpper();
                huesped.patente = huesped.patente.ToUpper();
                huesped.f_alta = DateTime.Now;
                huesped.usuario_alta = idUser;
                huesped.eliminado = 0;
            }

            List<movimiento> movimientos = reserva.movimientos.ToList();
            movimientos.RemoveAll(m => m.movimiento1 == 0);
            reserva.movimientos = movimientos;

            //Creamos el random_id
            string _random_id = Tools.RandomString(20);

            //Recorremos el array pero solo existe un movimiento
            foreach (var movimiento in reserva.movimientos)
            {
                movimiento.descripcion = "Reserva: " + reserva.nombre_apellido.ToUpper();
                movimiento.id_random = _random_id;
                movimiento.tipo = 1;
                movimiento.f_alta = DateTime.Now;
                movimiento.usuario_alta = idUser;
                movimiento.eliminado = 0;
            }

            if (!_Repository.Add(reserva, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando la reserva de: {reserva.nombre_apellido}");
                return StatusCode(500, ModelState);
            }


            return Ok(new { idMovimiento = reserva.movimientos.FirstOrDefault()?.id ?? 0 });

        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateReserva([FromBody] ReservaDto ReservaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reserva = _mapper.Map<reserva>(ReservaDto);

            reserva.nombre_apellido = reserva.nombre_apellido.ToUpper();

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            reserva.late_checkout = Convert.ToSByte(reserva.checkout.Value.TimeOfDay > new TimeSpan(11, 0, 0));
            if (reserva.checkin.HasValue && reserva.checkout.HasValue && reserva.checkin.Value.Date.Equals(reserva.checkout.Value.Date))
            {
                // Ambas fechas son el mismo día
                reserva.checkin = reserva.checkin.Value.Date.AddHours(10);
                reserva.checkout = reserva.checkout.Value.Date.AddHours(18);
            }

            if (!_Repository.Update(reserva, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro de la reserva para: {reserva.nombre_apellido}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteReserva([FromBody] ReservaDto reservaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reserva = _mapper.Map<reserva>(reservaDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Obtenemos los comprobantes
            foreach (var movimiento in reserva.movimientos)
            {
                var comprobantes = _ComprobanteRepository.GetByRandomId(movimiento.id_random);

                if (comprobantes.Count() > 0)
                {
                    foreach (var comprobante in comprobantes)
                    {
                        _ComprobanteRepository.Delete(comprobante, idUser);
                        // Obtener la ruta del archivo desde la propiedad comprobanteDto.archivo
                        string filePath = comprobante.archivo;

                        // Eliminar el archivo del sistema de archivos
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }
            }

            if (!_Repository.Delete(reserva, idUser))
            {
                ModelState.AddModelError("", $"algo salió mal eliminado la reserva de: {reserva.nombre_apellido}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [Authorize]
        [HttpGet("enconflicto")]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetReservasEnConflicto()
        {
            var listado = _Repository.ReservasEnConflicto();
            return Ok(listado);
        }


        [Authorize]
        [HttpGet("indicadores")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetIndicadores()
        {
            var listado = _Repository.GetIndicadores();
            return Ok(listado);
        }

    }
}
