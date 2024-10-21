
using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Services;
using SPARTANFIT.Utilitys;
namespace SPARTANFIT.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly RutinaRepository _rutinaRepository;
        private readonly PlanAlimenticioRepository _planAlimenticioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository, RutinaRepository rutinaRepository, PlanAlimenticioRepository planAlimenticioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _rutinaRepository = rutinaRepository;
            _planAlimenticioRepository = planAlimenticioRepository;
        }
        public async Task<UsuarioDto> RegistroUsuarioAsync(UsuarioDto usuario)
        {
            UsuarioDto usu = new UsuarioDto();
            UsuarioDto usuarioResp = new UsuarioDto();
            usuarioResp.persona = new PersonaDto();
            BinarioUtility binarioUtility = new BinarioUtility();
            HashUtility hashUtility = new HashUtility();
            SintetizarFormulariosUtility sintetizarFormulariosUtility = new SintetizarFormulariosUtility();
            usuario.persona.nombres = sintetizarFormulariosUtility.Sintetizar(usuario.persona.nombres);
            usuario.persona.apellidos = sintetizarFormulariosUtility.Sintetizar(usuario.persona.apellidos);
            usuario.persona.correo = sintetizarFormulariosUtility.Sintetizar(usuario.persona.correo);
            usuario.persona.contrasena = binarioUtility.ConvertirBinarioAString(usuario.persona.contrasena);
            usuario.persona.contrasena = hashUtility.HashPassword(usuario.persona.contrasena);

            usu = await _usuarioRepository.RegistroUsuarioAsync(usuario);
            usuarioResp = await _usuarioRepository.ObtenerUsuario(usuario.persona.correo);
            int resultado = await _rutinaRepository.AsignarRutina(usuarioResp);
            int resultadoAlimento = await _planAlimenticioRepository.AsignarPlanAlimenticio(usuarioResp);

            return usuario;
        }
        public async Task<UsuarioDto>SeleccionarUsuarioAsync(int id_usuario)
        {
            UsuarioDto usuario = new UsuarioDto();
            usuario = await _usuarioRepository.SeleccionarUsuarioAsync(id_usuario);

            return usuario;
        }   
        public async Task<bool> BuscarPersona(string correo)
        {
            return await _usuarioRepository.BuscarUsuario(correo);
        }

        public async Task<UsuarioDto> EnviarDatosUsu(int id_usuario)
        {
            UsuarioDto usuario = await _usuarioRepository.SeleccionarUsuarioAsync(id_usuario);
            return usuario;
        }

        public async Task<int>ActualizarObjetivo(UsuarioDto usuario)
        {
            int resultado = 0;
            resultado = await _usuarioRepository.ActualizarObjetivo(usuario);
            if(resultado != 0)
            {
                int resultadoAux = await _rutinaRepository.AsignarRutina(usuario);
                int resultadoAlimento = await _planAlimenticioRepository.AsignarPlanAlimenticio(usuario);
                if(resultadoAux == 1 && resultadoAlimento == 1) 
                {
                    resultado = 1;
                }
            }
            return resultado;
        }

        public async Task<int>ActualizarDatos(UsuarioDto usuario)
        {
            int resultado = 0;
            resultado = await _usuarioRepository.ActualizarDatosUsuario(usuario);
            return resultado;
        }

        public async Task<int> EliminarUsuario(int id_usuario)
        {
            int resultado = 0;
            resultado = await _usuarioRepository.EliminarUsuario(id_usuario);
            return resultado;
        }


        public async Task<(RutinaDto, List<EjercicioDto>)> MostrarRutinaDia(UsuarioDto usuario)
        {
            List<EjercicioDto> listEjerciciosDia = new List<EjercicioDto>();
            RutinaDto rutinaResp = new RutinaDto();
            IdentificadorDiaUtility identDia = new IdentificadorDiaUtility();

            string dia = identDia.DiaActual();
            rutinaResp = await _rutinaRepository.BuscarRutinaIdUsuario(usuario.persona.id_usuario, dia);
            listEjerciciosDia = await _rutinaRepository.ObtenerEjerciciosDia(rutinaResp.id_rutina);
            return(rutinaResp, listEjerciciosDia);
        }

        public async Task<(PlanAlimenticioDto, List<AlimentoDto>)>MostrarPlanALimenticioDia(UsuarioDto usuario)
        {
            List<AlimentoDto> listAlimentosDia = new List<AlimentoDto>();
            PlanAlimenticioDto planAlimenticioDia = new PlanAlimenticioDto();
            IdentificadorDiaUtility identDia = new IdentificadorDiaUtility();

            string dia = identDia.DiaActual();
            planAlimenticioDia = await _planAlimenticioRepository.BuscarPlanIdUsuario(usuario.persona.id_usuario, dia);
            listAlimentosDia = await _planAlimenticioRepository.ObtenerAlimentosDia(planAlimenticioDia.id_plan_alimenticio);
            return(planAlimenticioDia, listAlimentosDia);
        }
    }
}
