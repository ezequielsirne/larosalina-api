using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly IGenericRepository<cuenta> _Repository;
        private readonly IMapper _mapper;

        public CuentasController(IGenericRepository<cuenta> Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetCuentas() {
            var listado = _Repository.Get(e => e.descripcion);
            var listadoDto = new List<CuentaDto>();
            foreach (var item in listado)
            {
                listadoDto.Add(_mapper.Map<CuentaDto>(item));
            }
            return Ok(listadoDto);
        }

    }
}
