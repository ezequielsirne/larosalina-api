using AutoMapper;
using LaRosalinaAPI.Models;
using LaRosalinaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LaRosalinaAPI.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly LaRosalinaDbContext _context;
        private readonly IMapper _mapper;
        private string _key;

        public UsuarioRepository(LaRosalinaDbContext context, IMapper mapper, string key)
        {
            _context = context;
            _mapper = mapper;
            _key = key;
        }

        public usuario Get(int id)
        {
            return _context.usuarios.FirstOrDefault(u => u.id == id);
        }

        public async Task<UsuarioLoginReplyDto> Login(UsuarioLoginDto usuarioLoginDto)
        {

            var encriptedPass = ComputeHash(usuarioLoginDto.password, new MD5CryptoServiceProvider()).Replace("-", "").ToUpper();

            var usuario = _context.usuarios.FirstOrDefault(
                u => u.nick.ToLower() == usuarioLoginDto.nick.ToLower() && u.password.ToUpper() == encriptedPass);

            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null)
            {
                return new UsuarioLoginReplyDto()
                {
                    Token = "",
                    User = null
                };
            }

            //Aquí existe el usuario entonces podemos procesar el login
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.nombre_apellido.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginReplyDto usuarioLoginRespuestaDto = new UsuarioLoginReplyDto()
            {
                Token = manejadorToken.WriteToken(token),
                User = _mapper.Map<UsuarioDto>(usuario)
            };

            return usuarioLoginRespuestaDto;
        }

        public string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes);
        }

        
    }
}
