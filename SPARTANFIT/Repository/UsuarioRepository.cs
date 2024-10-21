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

        public async Task<UsuarioDto> ObtenerUsuario(string correoUsuario)
        {
            UsuarioDto usuario = new UsuarioDto();
            PersonaDto persona = new PersonaDto();

            try
            {
                string sql = "SELECT  id_usuario, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE correo = @correo ";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correoUsuario);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                persona.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                                persona.fecha_nacimiento = reader["fecha_nacimiento"].ToString();
                                persona.genero = reader["genero"].ToString();
                                usuario.estatura = Convert.ToDouble(reader["estatura"]);
                                usuario.peso = Convert.ToDouble(reader["peso"]);
                                usuario.id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]);
                                usuario.id_objetivo = Convert.ToInt32(reader["id_objetivo"]);
                                usuario.rehabilitacion = Convert.ToInt32(reader["rehabilitacion"]);
                                usuario.persona = persona;
                            }
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usuario;
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

        public async Task<UsuarioDto> SeleccionarUsuarioAsync(int id_usuario)
        {
            UsuarioDto usuario = null;
            PersonaDto persona = null;
            UsuarioDto personaResp = new UsuarioDto();
            string query = "SELECT id_usuario, nombres, apellidos, correo, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion FROM USUARIO WHERE id_usuario = @id_usuario";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_usuario);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                usuario = new UsuarioDto
                                {
                                    persona = new PersonaDto
                                    {
                                        id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                        nombres = reader["nombres"].ToString(),
                                        apellidos = reader["apellidos"].ToString(),
                                        fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                                        genero = reader["genero"].ToString()
                                    },
                                    estatura = Convert.ToDouble(reader["estatura"]),
                                    peso = Convert.ToDouble(reader["peso"]),
                                    id_nivel_entrenamiento = Convert.ToInt32(reader["id_nivel_entrenamiento"]),
                                    id_objetivo = Convert.ToInt32(reader["id_objetivo"]),
                                    rehabilitacion = Convert.ToInt32(reader["rehabilitacion"]),
                                };

                                return usuario;
                            }
                            else
                            {
                                return personaResp;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return usuario;
        }

        public async Task<int> ActualizarObjetivo(UsuarioDto usuario)
        {
            int resultado = 0;
            try
            {
                string sql = "UPDATE USUARIO SET id_nivel_entrenamiento = @id_nivel_entrenamiento, id_objetivo = @id_objetivo, rehabilitacion = @rehabilitacion " + "WHERE id_usuario = @id_usuario";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_nivel_entrenamiento", usuario.id_nivel_entrenamiento);
                        cmd.Parameters.AddWithValue("@id_objetivo", usuario.id_objetivo);
                        cmd.Parameters.AddWithValue("@rehabilitacion", usuario.rehabilitacion);
                        cmd.Parameters.AddWithValue("@id_usuario", usuario.persona.id_usuario);
                        cmd.ExecuteNonQuery();
                        resultado = 1;
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultado;
        }


        public async Task<int>EliminarUsuario(int id_usuario)
        {
            int resultado = 0;
            try
            {
                string sql = "UPDATE USUARIO SET correo = ' ' , contrasena = ' ' WHERE id_usuario = @id_usuario  ";
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using(SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                        cmd.ExecuteNonQuery();
                    }
                    resultado = 1;
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultado;
        }

        public async Task<int> ActualizarDatosUsuario(UsuarioDto usuario)
        {
            int resultado = 0;
            try
            {
                string sql = "UPDATE USUARIO SET estatura=@estatura , peso=@peso" + "WHERE id_usuario = @id_usuario";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using( SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@estatura", usuario.estatura);
                        cmd.Parameters.AddWithValue("@peso", usuario.peso);
                        cmd.Parameters.AddWithValue("@id_usuario", usuario.persona.id_usuario);
                        cmd.ExecuteNonQuery();
                    }
                    resultado = 1;
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultado;
        }
        
    }
}
