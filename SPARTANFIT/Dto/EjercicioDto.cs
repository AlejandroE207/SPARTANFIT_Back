namespace SPARTANFIT.Dto
{
    public class EjercicioDto
    {
        public int id_ejercicio {  get; set; } 
        public string nombre_ejercicio { get; set; }
        public int id_grupo_muscular { get; set; }

        public string apoyo_visual {  get; set; }
        public int num_series {  get; set; }
        public int repeticiones {  get; set; }

    }
}
