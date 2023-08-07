using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using LaRosalinaAPI.Utils;

namespace LaRosalinaAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUsuarioRepository _Repository;
        private readonly IMapper _mapper;
        protected LoginReply _respuestaApi;

        public UsersController(IUsuarioRepository Repository, IMapper mapper)
        {
            _Repository = Repository;   
            _mapper = mapper;
            this._respuestaApi = new();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuestaLogin = await _Repository.Login(usuarioLoginDto);

            if (respuestaLogin.User == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("El usuario o la contraseña son incorrectos");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuestaLogin;
            return Ok(_respuestaApi);
        }

        [HttpGet("getbytoken")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Get()
        {
            int userId = Tools.GetUserByToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            var Usuario = _Repository.Get(userId);

            if (Usuario == null)
            {
                return NotFound();
            }

            var UsuarioDto = _mapper.Map<UsuarioDto>(Usuario);
            return Ok(UsuarioDto);

        }

    }
}
