namespace SPARTANFIT.Dto
{
    public class Recuperacion_ContrasenaDto
    {
        public int id_recuperacion { get; set; }
        public int id_usuario { get; set; }
        public string codigo { get; set; }
        public string fecha { get; set; }
        public string mensaje { get; set; }
    }
}
