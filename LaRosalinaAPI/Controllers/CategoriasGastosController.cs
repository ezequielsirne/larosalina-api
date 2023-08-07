using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/categorias_gastos")]
    public class CategoriasGastosController : ControllerBase
    {
        private readonly IGenericRepository<categorias_gasto> _Repository;
        private readonly IMapper _mapper;

        public CategoriasGastosController(IGenericRepository<categorias_gasto> Repository, IMapper mapper)
        {
            _Repository = Repository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public  IActionResult GetCategorias() {
            var listado = _Repository.Get(e => e.descripcion);
            var listadoDto = new List<CategoriaGastosDto>();
            foreach (var item in listado)
            {
                listadoDto.Add(_mapper.Map<CategoriaGastosDto>(item));
            }
            return Ok(listadoDto);
        }

    }
}
