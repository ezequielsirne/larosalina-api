using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/departamentos")]
    public class DepartamentosController : ControllerBase
    {
        private readonly IGenericRepository<departamento> _Repository;
        private readonly IMapper _mapper;

        public DepartamentosController(IGenericRepository<departamento> Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetDepartamentos() {
            var listado = _Repository.Get(e => e.descripcion);
            var listadoDto = new List<DepartamentoDto>();
            foreach (var item in listado)
            {
                listadoDto.Add(_mapper.Map<DepartamentoDto>(item));
            }
            return Ok(listadoDto);
        }

    }
}
