using Microsoft.Data.SqlClient;
using SPARTANFIT.Dto;

namespace SPARTANFIT.Repository
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<UsuarioDto> RegistroUsuarioAsync(UsuarioDto usuario)
        {
            UsuarioDto usuarioResp = new UsuarioDto(); // Usuario de respuesta



            string query = @"INSERT INTO USUARIO (id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion) 
                       VALUES (@id_rol, @nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento, @estatura, @peso, @genero, @id_nivel_entrenamiento, @id_objetivo, @rehabilitacion)";


            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id_rol", usuario.persona.id_rol);
                    cmd.Parameters.AddWithValue("@nombres", usuario.persona.nombres);
                    cmd.Parameters.AddWithValue("@apellidos", usuario.persona.apellidos);
                    cmd.Parameters.AddWithValue("@correo", usuario.persona.correo);
                    cmd.Parameters.AddWithValue("@contrasena", usuario.persona.contrasena);
                    cmd.Parameters.AddWithValue("@fecha_nacimiento", usuario.persona.fecha_nacimiento);
                    cmd.Parameters.AddWithValue("@estatura", usuario.estatura);
                    cmd.Parameters.AddWithValue("@peso", usuario.peso);
                    cmd.Parameters.AddWithValue("@genero", usuario.persona.genero);
                    cmd.Parameters.AddWithValue("@id_nivel_entrenamiento", usuario.id_nivel_entrenamiento);
                    cmd.Parameters.AddWithValue("@id_objetivo", usuario.id_objetivo);
                    cmd.Parameters.AddWithValue("@rehabilitacion", usuario.rehabilitacion);
                    await cmd.ExecuteNonQueryAsync();

                }
                return usuario;

            }
        }
        public async Task<bool> BuscarUsuario(string correo)
        {
            int UsuarioEncontrado = 0;
            string query = @"SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@correo", correo);
                    UsuarioEncontrado = (int)await cmd.ExecuteScalarAsync();
                }

            }
            return UsuarioEncontrado > 0;
        }
    }
    
}
