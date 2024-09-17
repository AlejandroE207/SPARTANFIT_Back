namespace SPARTANFIT.Dto
{
    public class PersonaDto
    {
        public int id_usuario { get; set; }
        public int id_rol { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string fecha_nacimiento { get; set; }
        public string correo { get; set; }
        public string contrasena { get; set; }

        public string genero { get; set; }     
    }
}
