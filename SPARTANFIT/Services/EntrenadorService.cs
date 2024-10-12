using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Utilitys;

namespace SPARTANFIT.Services
{
    public class EntrenadorService
    {
        private readonly EjercicioRepository _ejercicioRepository;
        private readonly AlimentoRepository _alimentoRepository;
        public EntrenadorService (EjercicioRepository _ejericioRepository, AlimentoRepository alimentoRepository)
        {
            _ejercicioRepository = _ejericioRepository;
            _alimentoRepository = alimentoRepository;
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

        //--------------------ALIMENTOS------------------------------
        public async Task<List<AlimentoDto>> MostrarAlimentos()
        {
            List<AlimentoDto> listAlimentos = new List<AlimentoDto>();
            listAlimentos = await _alimentoRepository.MostrarAlimentos();
            return listAlimentos; 
        }

        public async Task<int> RegistrarAlimento(AlimentoDto alimento)
        {
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            int resultado = 0;
            alimento.nombre = sintetizar.Sintetizar(alimento.nombre);
            if (await _alimentoRepository.BuscarAlimento(alimento.nombre)){
                return resultado = 0;
            }
            else
            {
                resultado = await _alimentoRepository.RegistrarAlimento(alimento);
            }
            return resultado;
        }

        public async Task<int>ActualizarAlimento(AlimentoDto alimento)
        {
            int resultado = 0;
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            alimento.nombre = sintetizar.Sintetizar(alimento.nombre);
            resultado = await _alimentoRepository.ActualizarAlimento(alimento);
            return resultado;
        }
        
        public async Task<int>EliminarAlimento(int id_alimento)
        {
            int resultado = 0;
            resultado = await _alimentoRepository.EliminarAlimento(id_alimento);
            return resultado;
        }
    }
}
