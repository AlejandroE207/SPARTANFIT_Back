using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Utilitys;

namespace SPARTANFIT.Services
{
    public class PersonaService
    {
        private readonly PersonaRepository _personaRepository;
        private readonly Recuperacion_ContrasenaRepository _recuperacion_ContrasenaRepository;

        public PersonaService(PersonaRepository personaRepository, Recuperacion_ContrasenaRepository recuperacion_ContrasenaRepository)
        {
            _personaRepository = personaRepository;
            _recuperacion_ContrasenaRepository = recuperacion_ContrasenaRepository ?? throw new ArgumentNullException(nameof(recuperacion_ContrasenaRepository));
        }
        public async Task<bool> IniciarSesion(string correo, string contrasena)
        {
            BinarioUtility binarioUtility = new BinarioUtility();
            SintetizarFormulariosUtility sintetizarFormulariosUtility = new SintetizarFormulariosUtility();
            correo = sintetizarFormulariosUtility.Sintetizar(correo);
            contrasena = sintetizarFormulariosUtility.Sintetizar(contrasena);
            string contrasenaConvertida = binarioUtility.ConvertirBinarioAString(contrasena);


            var persona = await _personaRepository.IniciarSesion(correo, contrasenaConvertida);

            return persona != null;
        }
        public async Task<PersonaDto> enviarCodigo(string correo)
        {
            PersonaDto persona = new PersonaDto();
            CorreoUtility correoUtility = new CorreoUtility();
            SintetizarFormulariosUtility sintetizarFormulariosUtility=new SintetizarFormulariosUtility();
            GeneradorCodigoUtility generadorCodigoUtility = new GeneradorCodigoUtility();
            correo=sintetizarFormulariosUtility.Sintetizar(correo);

            if (await _personaRepository.BuscarPersonaAsync(correo))
            {
                persona = await _personaRepository.SeleccionarPersonaAsync(correo);
                await _recuperacion_ContrasenaRepository.EliminarCodigo(persona.id_usuario);
                string codigo = generadorCodigoUtility.NumeroAleatorio().ToString();
                await _recuperacion_ContrasenaRepository.RegistroRecuperacion(persona.id_usuario, codigo);
                correoUtility.enviarCorreoContrasena(correo, codigo);
                return persona;
            }
            else
            {
                return persona;
            }
        }
        public async Task<int> ActualizarContrasena(string contrasena, string codigo)
        {
            int filasAfectadas = 0;
            BinarioUtility binarioUtility = new BinarioUtility();
            SintetizarFormulariosUtility sintetizarFormulariosUtility = new SintetizarFormulariosUtility();
            codigo = sintetizarFormulariosUtility.Sintetizar(codigo);
            contrasena = sintetizarFormulariosUtility.Sintetizar(contrasena);

            int? id = await _recuperacion_ContrasenaRepository.BuscarIDPersona(codigo);

            if (id.HasValue)
            {

                PersonaDto persona = await _personaRepository.SeleccionarPersona(id.Value);

                if (persona != null)
                {

                    Recuperacion_ContrasenaDto recuperacion = await _recuperacion_ContrasenaRepository.SeleccionarCodigo(persona.id_usuario);

                    if (codigo == recuperacion.codigo)
                    {

                        HashUtility hashUtility = new HashUtility();
                        contrasena = binarioUtility.ConvertirBinarioAString(contrasena);
                        string contrasenaNueva = hashUtility.HashPassword(contrasena);


                        filasAfectadas = await _personaRepository.ActualizarContrasenaAsync(persona.correo, contrasenaNueva);


                        await _recuperacion_ContrasenaRepository.EliminarCodigo(persona.id_usuario);
                    }
                }
            }

            return filasAfectadas;
        }
        public async Task<PersonaDto> EnviarPersonas(string correo)
        {
            PersonaDto persona = await _personaRepository.SeleccionarPersonaAsync( correo);
            return persona; 
        } 
    }
}
