using Microsoft.AspNetCore.Identity;
using SPARTANFIT.Dto;
using SPARTANFIT.Repository;

namespace SPARTANFIT.Services
{
    public class AdministradorService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly EntrenadorRepository _entrenadorRepository;

        public AdministradorService(UsuarioRepository usuarioRepository, EntrenadorRepository entrenadorRepository)
        {
            _usuarioRepository = usuarioRepository;
            _entrenadorRepository = entrenadorRepository;
        }

        public async Task<List<UsuarioDto>> Mostrar_Usuario()
        {
            List<UsuarioDto> listUsuarios = new List<UsuarioDto>();
            listUsuarios = await _usuarioRepository.Mostrar_Usuarios();
            return listUsuarios;
        }

        public async Task<List<PersonaDto>> Mostrar_Entrenadores()
        {
            List<PersonaDto> listEntrenadores = new List<PersonaDto>();
            listEntrenadores = await _entrenadorRepository.Mostrar_Entrenadores();
            return listEntrenadores;
        }
    }
}
