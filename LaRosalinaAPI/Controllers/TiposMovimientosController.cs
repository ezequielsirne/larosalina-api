using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/movimientos_tipos")]
    public class TiposMovimientosController : ControllerBase
    {
        private readonly IGenericRepository<movimientos_tipo> _Repository;
        private readonly IMapper _mapper;

        public TiposMovimientosController(IGenericRepository<movimientos_tipo> Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetTipos() {
            var listado = _Repository.Get(e => e.descripcion);
            var listadoDto = new List<MovimientoTiposDto>();
            foreach (var item in listado)
            {
                listadoDto.Add(_mapper.Map<MovimientoTiposDto>(item));
            }
            return Ok(listadoDto);
        }

    }
}
