using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository;
using LaRosalinaAPI.Repository.IRepository;
using LaRosalinaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/gastos")]
    public class GastosController : ControllerBase
    {
        private readonly IGastoRepository _Repository;
        private readonly IComprobanteRepository _ComprobanteRepository;
        private readonly IMapper _mapper;

        public GastosController(IGastoRepository Repository, IMapper mapper, IComprobanteRepository comprobanteRepository)
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
        public IActionResult GetGastos()
        {
            var listado = _Repository.Get();
            return Ok(listado);
        }

        [Authorize]
        [HttpGet("{idGasto:int}", Name = "GetGasto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGasto(int idGasto)
        {
            var listado = _Repository.Get(idGasto);
            return Ok(listado);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(GastoDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddGasto([FromBody] GastoDto gastoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (gastoDto == null)
            {
                return BadRequest(ModelState);
            }

            var gasto = _mapper.Map<gasto>(gastoDto);

             // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));           

            if (!_Repository.Add(gasto, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el gasto: {gasto.descripcion}");
                return StatusCode(500, ModelState);
            }
            return Ok(new { idMovimiento = gasto.movimientos.FirstOrDefault()?.id ?? 0 });
        }

        [Authorize]
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateGasto([FromBody] GastoDto GastoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gasto = _mapper.Map<gasto>(GastoDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (!_Repository.Update(gasto, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro del gasto: {gasto.descripcion}");
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
        public IActionResult DeleteGasto([FromBody] GastoDto GastoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gasto = _mapper.Map<gasto>(GastoDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Obtenemos los comprobantes
            var comprobantes = _ComprobanteRepository.GetByRandomId(gasto.id_random);

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


            if (!_Repository.Delete(gasto, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro del gasto: {gasto.descripcion}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
