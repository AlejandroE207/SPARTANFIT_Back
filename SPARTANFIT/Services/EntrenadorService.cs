using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Utilitys;

namespace SPARTANFIT.Services
{
    public class EntrenadorService
    {
        private readonly EjercicioRepository _ejercicioRepository;
        public EntrenadorService (EjercicioRepository _ejericioRepository)
        {
            _ejercicioRepository = _ejericioRepository;
        }


        public async Task<int>RegistrarEjercicio(EjercicioDto ejercicio)
        {
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            int resultado = 0;
            ejercicio.nombre_ejercicio = sintetizar.Sintetizar(ejercicio.nombre_ejercicio);
            ejercicio.apoyo_visual = sintetizar.Sintetizar(ejercicio.apoyo_visual);

            if(await _ejercicioRepository.BuscarEjercicio(ejercicio.nombre_ejercicio))
            {
                return resultado = 0;
            }
            else
            {
                resultado = await _ejercicioRepository.RegistrarEjercicio(ejercicio);
            }
            return resultado;
        }

        public async Task<int>EliminarEjercicio(int id_ejercicio)
        {
            int resultado = 0;
            resultado = await _ejercicioRepository.EliminarEjercicio(id_ejercicio);
            return resultado;
        }

        public async Task<int> ActualizarEjercicio(EjercicioDto ejercicio)
        {
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            int resultado = 0;
            ejercicio.nombre_ejercicio = sintetizar.Sintetizar(ejercicio.nombre_ejercicio);
            ejercicio.apoyo_visual = sintetizar.Sintetizar(ejercicio.apoyo_visual);

            resultado = await _ejercicioRepository.ActualizarEjercicio(ejercicio);
            return resultado;
        }

        public async Task<List<EjercicioDto>> MostrarEjercicios()
        {
            List<EjercicioDto> listEjercicios = new List<EjercicioDto>();
            listEjercicios = await _ejercicioRepository.MostrarEjercicios();
            return listEjercicios;
        }
        
    }
}
