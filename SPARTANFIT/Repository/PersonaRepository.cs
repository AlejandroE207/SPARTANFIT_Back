using Microsoft.Data.SqlClient;
using SPARTANFIT.Dto;
using SPARTANFIT.Utilitys;

using System.Data;
using System.Data.Common;

namespace SPARTANFIT.Repository
{
    public class PersonaRepository
    {
        private readonly string _connectionString;

        public PersonaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PersonaDto> IniciarSesion(string correo, string contrasena)
        {
            HashUtility hash = new HashUtility();
            PersonaDto persona = null;
            string query = @"
               SELECT id_usuario, id_rol, nombres, apellidos, correo, contrasena, fecha_nacimiento, estatura, peso, genero, id_nivel_entrenamiento, id_objetivo, rehabilitacion 
               FROM USUARIO 
               WHERE correo = @correo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string contrasenaAlmacenada = reader["contrasena"].ToString();

                                if (hash.VerifyPassword(contrasena, contrasenaAlmacenada))
                                {
                                    persona = new PersonaDto
                                    {
                                        id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                        id_rol = Convert.ToInt32(reader["id_rol"]),
                                        nombres = reader["nombres"].ToString(),
                                        apellidos = reader["apellidos"].ToString(),
                                        correo = reader["correo"].ToString(),
                                        fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                                        genero = reader["genero"].ToString()
                                    };


                                    return persona;
                                }
                            }
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al iniciar sesión: " + ex.Message);
                return null;
            }
            return null;
        }
        public async Task<int> ActualizarContrasenaAsync(string correo, string contrasena)
        {
            int comando = 0;
            string query = "UPDATE USUARIO SET contrasena = @contrasena WHERE correo = @correo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);
                        cmd.Parameters.AddWithValue("@contrasena", contrasena);

                        await cmd.ExecuteNonQueryAsync();
                        comando = 1;
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar la contraseña: " + ex.Message);
            }

            return comando;
        }
        public async Task<bool> BuscarPersonaAsync(string correo)
        {
            int personaEncontrada = 0;
            string query = "SELECT COUNT(*) FROM USUARIO WHERE correo = @correo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);

                        personaEncontrada = (int)await cmd.ExecuteScalarAsync();
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar persona: " + ex.Message);
            }

            return personaEncontrada > 0;
        }
        public async Task<PersonaDto> SeleccionarPersonaAsync(string correo)
        {
            PersonaDto persona = null;
            PersonaDto personaResp = new PersonaDto();
            string query = "SELECT id_usuario,id_rol, correo FROM USUARIO WHERE correo = @correo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@correo", correo);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                persona = new PersonaDto
                                {
                                    id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                    id_rol = Convert.ToInt32(reader["id_rol"]),
                                    correo = reader["correo"].ToString(),

                                };

                                return persona;
                            }
                            else
                            {

                                return personaResp;
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

            return persona;
        }
        public async Task<PersonaDto> SeleccionarPersona(int id_persona)
        {
            PersonaDto persona = null;
            PersonaDto personaResp = new PersonaDto();
            string query = "SELECT id_usuario, correo FROM USUARIO WHERE id_usuario = @id_usuario";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_persona);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                persona = new PersonaDto
                                {
                                    id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                    correo = reader["correo"].ToString(),

                                };

                                return persona;
                            }
                            else
                            {

                                return personaResp;
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

            return persona;
        }

    }
}