using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using LaRosalinaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/pagos")]
    public class PagosController : ControllerBase
    {
        private readonly IMovimientoRepository _Repository;
        private readonly IMapper _mapper;

        public PagosController(IMovimientoRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("{IdReserva:int}", Name = "GetPagos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPagos(int IdReserva)
        {
            var listado = _Repository.GetByReserva(IdReserva);
            return Ok(listado);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovimientoDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddPago([FromBody] MovimientoDto movimientoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (movimientoDto == null)
            {
                return BadRequest(ModelState);
            }

            var movimiento = _mapper.Map<movimiento>(movimientoDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            //Creamos el random_id
            string _random_id = Tools.RandomString(20);

            movimiento.descripcion = "Pago de: " + movimiento.descripcion;
            movimiento.tipo = 1;
            movimiento.id_random = _random_id;

            if (!_Repository.Add(movimiento, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el pego: {movimiento.descripcion}");
                return StatusCode(500, ModelState);
            }

            return Ok(new { idMovimiento = movimiento?.id ?? 0 });
        }

    }
}
