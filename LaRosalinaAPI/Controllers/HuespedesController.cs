using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/huespedes")]
    public class HuespedesController : ControllerBase
    {
        private readonly IHuespedRepository _Repository;
        private readonly IMapper _mapper;

        public HuespedesController(IHuespedRepository Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            var listado = _Repository.Get();
            return Ok(listado);
        }

        [Authorize]
        [HttpGet("{IdReserva:int}", Name = "GetHuespedes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHuespedes(int IdReserva)
        {
            var listado = _Repository.Get(IdReserva);
            return Ok(listado);
        }

        [Authorize]
        [HttpGet("huesped/{idHuesped:int}", Name = "GetHuesped")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetHuesped(int idHuesped)
        {
            var listado = _Repository.GetHuesped(idHuesped);
            return Ok(listado);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(HuespedDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddHuesped([FromBody] HuespedDto huespedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (huespedDto == null)
            {
                return BadRequest(ModelState);
            }

            var huesped = _mapper.Map<huespede>(huespedDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!_Repository.Add(huesped, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el huesped: {huesped.nombre_apellido}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateHuesped([FromBody] HuespedDto HuespedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var huesped = _mapper.Map<huespede>(HuespedDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (!_Repository.Update(huesped, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro del huesped: {huesped.nombre_apellido}");
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
        public IActionResult DeleteHuespedes([FromBody] HuespedDto HuespedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var huesped = _mapper.Map<huespede>(HuespedDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (!_Repository.Delete(huesped, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro del huesped: {huesped.nombre_apellido}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
