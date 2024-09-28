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
                await con.CloseAsync();
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
                await con.CloseAsync();
            }
            return UsuarioEncontrado > 0;
        }

        public async Task<List<UsuarioDto>> Mostrar_Usuarios()
        {
            List<UsuarioDto> listUsuarios = new List<UsuarioDto>();
            string sql = "SELECT  id_usuario,nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE id_rol = 1";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PersonaDto persona;
                            persona = new PersonaDto
                            {
                                id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                nombres = reader["nombres"].ToString(),
                                apellidos = reader["apellidos"].ToString(),
                                correo = reader["correo"].ToString(),
                                contrasena = reader["contrasena"].ToString(),
                                fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                                genero = reader["genero"].ToString()
                            };
                            UsuarioDto usuario = new UsuarioDto
                            {
                                estatura = Convert.ToDouble(reader["estatura"]),
                                peso = Convert.ToDouble(reader["peso"]),
                                id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]),
                                id_objetivo = Convert.ToInt32(reader["id_objetivo"]),
                                rehabilitacion = Convert.ToInt32(reader["rehabilitacion"])
                            };

                            usuario.persona = persona;
                            listUsuarios.Add(usuario);
                        }
                    }
                }
                await con.CloseAsync();
            }
            return listUsuarios;
        }
    }

}
