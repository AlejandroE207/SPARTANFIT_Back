
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Data;
using SPARTANFIT.Dto;
using System.Linq.Expressions;
namespace SPARTANFIT.Repository

{
    public class EntrenadorRepository
    {
        private readonly string _connectionString;

        public EntrenadorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<PersonaDto>> Mostrar_Entrenadores()
        {
            List<PersonaDto> listEntrenadores = new List<PersonaDto>();
            string sql = "SELECT  id_usuario,nombres, apellidos, correo, fecha_nacimiento, genero FROM USUARIO WHERE id_rol=2";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PersonaDto entrenador = new PersonaDto
                            {
                                id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                nombres = reader["nombres"].ToString(),
                                apellidos = reader["apellidos"].ToString(),
                                correo = reader["correo"].ToString(),
                                fecha_nacimiento = reader["fecha_nacimiento"].ToString(),
                                genero = reader["genero"].ToString()
                            };
                            listEntrenadores.Add(entrenador);
                        }
                    }
                }
                await con.CloseAsync();
            }
            return listEntrenadores;
        }

        public async Task<int> Registrar_Entrenador(PersonaDto entrenador)
        {
            int respuesta = 0;
            try
            {
                //string sql = "INSERT INTO USUARIO ( id_rol,nombres, apellidos, correo, contrasena, fecha_nacimiento, genero)"
                //            + "VALUES ( @id_rol,@nombres, @apellidos, @correo, @contrasena, @fecha_nacimiento,@genero)";

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("dbo.sp_InsertarEntrenador", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@id_rol", 2);
                        cmd.Parameters.AddWithValue("@nombres", entrenador.nombres);
                        cmd.Parameters.AddWithValue("@apellidos", entrenador.apellidos);
                        cmd.Parameters.AddWithValue("@correo", entrenador.correo);
                        cmd.Parameters.AddWithValue("@contrasena", entrenador.contrasena);
                        cmd.Parameters.AddWithValue("@fecha_nacimiento", entrenador.fecha_nacimiento);
                        cmd.Parameters.AddWithValue("@genero", entrenador.genero);

                        await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
                respuesta = 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return respuesta;
        }

        public async Task<int> Actualizar_Entrenador(PersonaDto entrenador)
        {
            int resultado = 0;
            try
            {
                //string sql = "UPDATE USUARIO SET nombres = @nombres, apellidos = @apellidos, correo = @correo WHERE id_usuario = @id_usuario";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using(SqlCommand cmd = new SqlCommand("dbo.sp_ActualizarEntrenador", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@nombres", entrenador.nombres);
                        cmd.Parameters.AddWithValue("@apellidos", entrenador.apellidos);
                        cmd.Parameters.AddWithValue("@correo", entrenador.correo);
                        cmd.Parameters.AddWithValue("@id_usuario", entrenador.id_usuario);

                        await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultado;
        }

        public async Task<int> Eliminar_Entrenador(int idEntrenador)
        {
            int resultado = 0;
            try
            {
                string sql = "DELETE FROM USUARIO WHERE id_usuario = @id_usuario";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", idEntrenador);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultado;
        }

    }
}
