using LaRosalinaAPI.Models;

namespace LaRosalinaAPI.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        usuario Get(int id);

        Task<UsuarioLoginReplyDto> Login(UsuarioLoginDto usuarioLoginDto);

    }
}
