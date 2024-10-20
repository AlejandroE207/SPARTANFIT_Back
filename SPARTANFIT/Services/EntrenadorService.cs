using SPARTANFIT.Dto;
using SPARTANFIT.Repository;
using SPARTANFIT.Utilitys;
using System.Numerics;

namespace SPARTANFIT.Services
{
    public class EntrenadorService
    {
        private readonly EjercicioRepository _ejercicioRepository;
        private readonly AlimentoRepository _alimentoRepository;
        private readonly RutinaRepository _rutinaRepository;
        private readonly PlanAlimenticioRepository _planAlimenticioRepository;
        public EntrenadorService (EjercicioRepository _ejericioRepository, AlimentoRepository alimentoRepository, RutinaRepository rutinaRepository, PlanAlimenticioRepository planAlimenticioRepository)
        {
            _ejercicioRepository = _ejericioRepository;
            _alimentoRepository = alimentoRepository;
            _rutinaRepository = rutinaRepository;
            _planAlimenticioRepository = planAlimenticioRepository;
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
        //-------------------------RUTINA-------------------------------
        public async Task<int>RegistrarRutina(RutinaDto rutina, List<EjercicioDto> ejerciciosRutina)
        {
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            rutina.nombre_rutina = sintetizar.Sintetizar(rutina.nombre_rutina);
            rutina.descripcion = sintetizar.Sintetizar(rutina.descripcion);

            int id_rutina = await _rutinaRepository.RegistrarRutina(rutina);
            int resultadoEjerRut = await _rutinaRepository.RegistrarEjerciciosRutina(ejerciciosRutina, id_rutina);
            return resultadoEjerRut;
        }
        
        public async Task<List<RutinaDto>> MostrarRutinas()
        {
            List<RutinaDto> listRutinas = new List<RutinaDto>();
            listRutinas = await _rutinaRepository.MostrarRutinas();
            return listRutinas;
        }

        public async Task<int> EliminarRutina(int id_rutina)
        {
            int resultado = 0;
            resultado = await _rutinaRepository.EliminarRutina(id_rutina);
            return resultado;
        }

        ///------------------------------ PLAN ALIMENTICIO -------------------------------
        
        public async Task<int> RegistrarPlanAlimenticio(PlanAlimenticioDto planAlimenticio, List<int> idAlimentos)
        {
            SintetizarFormulariosUtility sintetizar = new SintetizarFormulariosUtility();
            planAlimenticio.nombre = sintetizar.Sintetizar(planAlimenticio.nombre);
            planAlimenticio.dia = sintetizar.Sintetizar(planAlimenticio.dia);
            planAlimenticio.descripcion = sintetizar.Sintetizar(planAlimenticio.descripcion);

            int id_plan_alimenticio = await _planAlimenticioRepository.RegistrarPlan(planAlimenticio);

            int registroAlimentoPlan = await _planAlimenticioRepository.RegistrarAlimentoPlan(idAlimentos,id_plan_alimenticio);
            return registroAlimentoPlan;
        }

        public async Task<List<PlanAlimenticioDto>> MostrarPlanes()
        {
            List<PlanAlimenticioDto> listPlanes = new List<PlanAlimenticioDto>();
            listPlanes = await _planAlimenticioRepository.MostrarPlanes();
            return listPlanes;
        }

        public async Task<int> EliminarPlan(int id_plan_alimenticio)
        {
            int resultado = 0;
            resultado = await _planAlimenticioRepository.EliminarPlan(id_plan_alimenticio);
            return resultado;
        }
    }
}
