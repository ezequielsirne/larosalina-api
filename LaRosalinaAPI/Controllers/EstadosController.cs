using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/estados")]
    public class EstadosController : ControllerBase
    {
        private readonly IGenericRepository<estado_reserva> _Repository;
        private readonly IMapper _mapper;

        public EstadosController(IGenericRepository<estado_reserva> Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetEstados() {
            var listado = _Repository.Get(e => e.descripcion);
            var listadoDto = new List<EstadoReservaDto>();
            foreach (var item in listado)
            {
                listadoDto.Add(_mapper.Map<EstadoReservaDto>(item));
            }
            return Ok(listadoDto);
        }

    }
}
