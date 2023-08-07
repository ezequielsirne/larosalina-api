using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository;
using LaRosalinaAPI.Repository.IRepository;
using LaRosalinaAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Security.Claims;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/comprobantes")]
    public class ComprobantesController : ControllerBase
    {
        private readonly IComprobanteRepository _Repository;
        private readonly IMovimientoRepository _MovimientoRepository;
        private readonly IMapper _mapper;
        private readonly string _comprobantesFolderPath;

        public ComprobantesController(IComprobanteRepository Repository, IMovimientoRepository MovimientoRepository, IConfiguration configuration, IMapper mapper)
        {
            _Repository = Repository;
            _MovimientoRepository = MovimientoRepository;
            _comprobantesFolderPath = configuration.GetValue<string>("ApiSettings:FilesPath");
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetComprobantes() {
            var listado = _Repository.Get();
            return Ok(listado);
        }

        [Authorize]
        [HttpGet("{RandomId:Guid}", Name = "GetByRandomId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetByRandomId(Guid RandomId)
        {
            var item = _Repository.GetByRandomId(RandomId.ToString());

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [Authorize]
        [HttpGet("reserva/{idReserva:int}", Name = "GetByReserva")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetByReserva(int idReserva)
        {
            var comprobantes = _Repository.GetByReserva(idReserva);

            if (comprobantes == null)
            {
                return NotFound();
            }

            var comprobantesDto = _mapper.Map<ICollection<ComprobanteDto>>(comprobantes);

            foreach (var comprobante in comprobantesDto)
            {
                string filePath = Path.Combine(_comprobantesFolderPath, comprobante.archivo_corto);

                // Verifica si el archivo existe en la ruta especificada
                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    // Convierte los bytes del archivo a una cadena base64 para enviarlo en la respuesta
                    comprobante.file = Convert.ToBase64String(fileBytes);
                }
            }

            return Ok(comprobantesDto);
        }

        [Authorize]
        [HttpGet("movimiento/{idMovimiento:int}", Name = "GetByMovimiento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetByMovimiento(int idMovimiento)
        {
            var comprobantes = _Repository.GetByMovimiento(idMovimiento);

            if (comprobantes == null)
            {
                return NotFound();
            }

            var comprobantesDto = _mapper.Map<ICollection<ComprobanteDto>>(comprobantes);

            foreach (var comprobante in comprobantesDto)
            {
                string filePath = Path.Combine(_comprobantesFolderPath, comprobante.archivo_corto);

                // Verifica si el archivo existe en la ruta especificada
                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    // Convierte los bytes del archivo a una cadena base64 para enviarlo en la respuesta
                    comprobante.file = Convert.ToBase64String(fileBytes);
                }
            }

            return Ok(comprobantesDto);
        }

        [Authorize]
        [HttpGet("gasto/{idGasto:int}", Name = "GetByGasto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetByGasto(int idGasto)
        {
            var comprobantes = _Repository.GetByGasto(idGasto);

            if (comprobantes == null)
            {
                return NotFound();
            }

            var comprobantesDto = _mapper.Map<ICollection<ComprobanteDto>>(comprobantes);

            foreach (var comprobante in comprobantesDto)
            {
                string filePath = Path.Combine(_comprobantesFolderPath, comprobante.archivo_corto);

                // Verifica si el archivo existe en la ruta especificada
                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    // Convierte los bytes del archivo a una cadena base64 para enviarlo en la respuesta
                    comprobante.file = Convert.ToBase64String(fileBytes);
                }
            }

            return Ok(comprobantesDto);
        }

        [Authorize]
        [HttpPost("{idMovimiento:int}", Name = "AddComprobante")]
        [ProducesResponseType(201, Type = typeof(comprobante))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddComprobante([FromForm] ICollection<IFormFile> comprobantes, int idMovimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (comprobantes == null)
            {
                return BadRequest(ModelState);
            }

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Obtenemos el RandomId 
            MovimientoDto movimiento = _MovimientoRepository.Get(idMovimiento);

            string RandomId = movimiento.id_random;

            foreach (var file in comprobantes)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(_comprobantesFolderPath, $"{RandomId}_{file.FileName}");

                // Se guarda el archivo
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Se crea el objeto comprobante para guardar en la DB
                comprobante newComprobante = new comprobante
                {
                    id_random = RandomId,
                    archivo = filePath,
                    archivo_corto = $"{RandomId}_{file.FileName}",
                    extension = fileExtension
                };

                _Repository.Add(newComprobante, idUser);

            }

            if (!_Repository.Save())
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el comprobante");
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
        public IActionResult DeleteComprobante([FromBody] ComprobanteDto comprobanteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comprobante = _mapper.Map<comprobante>(comprobanteDto);

            // Obtener el ID del usuario actual
            int idUser = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Obtener la ruta del archivo desde la propiedad comprobanteDto.archivo
            string filePath = comprobanteDto.archivo;

            // Eliminar el archivo del sistema de archivos
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            if (!_Repository.Delete(comprobante, idUser))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el comprobante: {comprobanteDto.archivo_corto}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


    }
}
