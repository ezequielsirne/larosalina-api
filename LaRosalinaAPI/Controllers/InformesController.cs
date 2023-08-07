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
    [Route("api/informes")]
    public class InformesController : ControllerBase
    {

        private readonly IGastoRepository _GastoRepository;
        private readonly IMovimientoRepository _MovimientoRepository;
        private readonly IReservaRepository _ReservaRepository;
        private readonly IComprobanteRepository _ComprobanteRepository;
        private readonly IMapper _mapper;

        public InformesController(IGastoRepository GastoRepository, IMovimientoRepository MovimientoRepository, IReservaRepository ReservaRepository, IMapper mapper)
        {
            _GastoRepository = GastoRepository;
            _MovimientoRepository = MovimientoRepository;
            _ReservaRepository = ReservaRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("indicadores")]
        [ProducesResponseType(201, Type = typeof(InformeRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetIndicadores([FromBody] InformeRequest request)
        {
            var listado = _ReservaRepository.GetIndicadores(request);
            return Ok(listado);

        }

        [Authorize]
        [HttpPost("gastos")]
        [ProducesResponseType(201, Type = typeof(InformeRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetGastos([FromBody] InformeRequest request)
        {
            var listado = _GastoRepository.GetGastos(request);
            return Ok(listado);

        }

        [Authorize]
        [HttpPost("economico")]
        [ProducesResponseType(201, Type = typeof(InformeRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEconomico([FromBody] InformeRequest request)
        {
            var listado = _ReservaRepository.GetEconomico(request);
            return Ok(listado);

        }

        [Authorize]
        [HttpPost("financiero")]
        [ProducesResponseType(201, Type = typeof(InformeRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetFinanciero([FromBody] InformeRequest request)
        {
            var listado = _MovimientoRepository.GetFinanciero(request);
            return Ok(listado);

        }


    }
}
