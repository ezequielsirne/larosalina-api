using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientoRepository _Repository;
        private readonly IComprobanteRepository _ComprobanteRepository;
        private readonly IMapper _mapper;

        public MovimientosController(IMovimientoRepository Repository, IMapper mapper, IComprobanteRepository comprobanteRepository)
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
        public IActionResult GetMovimientos()
        {
            var listado = _Repository.Get();
            return Ok(listado);
        }

        [Authorize]
        [HttpGet("{idMovimiento:int}", Name = "GetMovimiento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovimiento(int idMovimiento)
        {
            var listado = _Repository.Get(idMovimiento);
            return Ok(listado);
        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovimiento([FromBody] MovimientoDto MovimientoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movimiento = _mapper.Map<movimiento>(MovimientoDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (!_Repository.Update(movimiento, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro del movimiento: {movimiento.descripcion}");
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
        public IActionResult DeleteMovimiento([FromBody] MovimientoDto MovimientoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movimiento = _mapper.Map<movimiento>(MovimientoDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Obtenemos los comprobantes
            var comprobantes = _ComprobanteRepository.GetByRandomId(movimiento.id_random);

            if (comprobantes.Count() > 0)
            {
                foreach ( var comprobante in comprobantes) {
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

            if (!_Repository.Delete(movimiento, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro del movimiento: {movimiento.descripcion}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
