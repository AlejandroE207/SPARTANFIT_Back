using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Services;
using SPARTANFIT.Utilitys;
namespace SPARTANFIT.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<UsuarioDto> RegistroUsuarioAsync(UsuarioDto usuario)
        {
            UsuarioDto usu = new UsuarioDto();
            BinarioUtility binarioUtility = new BinarioUtility();
            HashUtility hashUtility = new HashUtility();
            SintetizarFormulariosUtility sintetizarFormulariosUtility = new SintetizarFormulariosUtility();
            usuario.persona.nombres = sintetizarFormulariosUtility.Sintetizar(usuario.persona.nombres);
            usuario.persona.apellidos = sintetizarFormulariosUtility.Sintetizar(usuario.persona.apellidos);
            usuario.persona.correo = sintetizarFormulariosUtility.Sintetizar(usuario.persona.correo);
            usuario.persona.contrasena = binarioUtility.ConvertirBinarioAString(usuario.persona.contrasena);
            usuario.persona.contrasena = hashUtility.HashPassword(usuario.persona.contrasena);

            usu = await _usuarioRepository.RegistroUsuarioAsync(usuario);
            return usuario;
        }
        public async Task<bool> BuscarPersona(string correo)
        {
            return await _usuarioRepository.BuscarUsuario(correo);
        }

    }
}
