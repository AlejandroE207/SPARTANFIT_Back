namespace SPARTANFIT.Dto
{
    public class AlimentoDto
    {
        public int id_alimento { get; set; }
        public int id_categoria_alimento { get; set; }
        public string nombre { get; set; }
        public double calorias_x_gramo { get; set; }
        public double grasa { get; set; }
        public double carbohidrato { get; set; }
        public double proteina { get; set; }
        public double fibra { get; set; } 
    }
}
