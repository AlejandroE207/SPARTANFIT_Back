using Microsoft.AspNetCore.Identity;
using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Utilitys;

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

        public async Task<int> Registrar_Entrenadores(PersonaDto entrenador)
        {
            int resultado = 0;
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            HashUtility hashUtility = new HashUtility();
            entrenador.nombres = sintetizar.Sintetizar(entrenador.nombres);
            entrenador.apellidos = sintetizar.Sintetizar(entrenador.apellidos);
            entrenador.correo = sintetizar.Sintetizar(entrenador.correo);
            entrenador.contrasena = hashUtility.HashPassword(entrenador.contrasena);

            resultado =await  _entrenadorRepository.Registrar_Entrenador(entrenador);

            return resultado;
        }

        public async Task<int> Actualizar_Entrenador(PersonaDto entrenador)
        {
            int resultado = 0;
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            HashUtility hashUtility = new HashUtility();
            entrenador.nombres = sintetizar.Sintetizar(entrenador.nombres);
            entrenador.apellidos = sintetizar.Sintetizar(entrenador.apellidos);
            entrenador.correo = sintetizar.Sintetizar(entrenador.correo);

            resultado = await _entrenadorRepository.Actualizar_Entrenador(entrenador);
            return resultado;
        }

        public async Task<int> Eliminar_Entrenador(int id_entrenador)
        {
            int resultado = 0;
            resultado = await _entrenadorRepository.Eliminar_Entrenador(id_entrenador);
            return resultado;
        }

        public async Task<string> CrearPdfUsuarios()
        {
            ReporteUtility reporte = new ReporteUtility();
            var lista = await Mostrar_Usuario();
            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Usuarios.pdf");
            reporte.CrearPdfUsuarios(lista, tempFilePath);
            return tempFilePath;
        }

        public async Task<string> CrearPdfEntrenadores()
        {
            ReporteUtility reporte = new ReporteUtility();
            var lista = await Mostrar_Entrenadores();

            string tempFilePath = Path.Combine(Path.GetTempPath(), "Lista_Entrenadores.pdf");
            reporte.CrearPdfDeEntrenadores(lista, tempFilePath);

            return tempFilePath;
        }
    }
}
