using Microsoft.Data.SqlClient;
using System.Data;
using SPARTANFIT.Dto;
namespace SPARTANFIT.Repository
{
    public class Recuperacion_ContrasenaRepository
    {
        private readonly string _connectionString;
        public Recuperacion_ContrasenaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<int> RegistroRecuperacion(int id_usuario, string codigo)
        {
            int comando = 0;
            string query = @"
                INSERT INTO RECUPERACION_CONTRASENA (id_usuario, codigo, fecha) 
                VALUES (@id_usuario, @codigo, GETDATE())";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id_usuario;
                        cmd.Parameters.Add("@codigo", SqlDbType.VarChar).Value = codigo;

                        await cmd.ExecuteNonQueryAsync();
                        comando = 1; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en registroRecuperacion: " + ex.Message);
            }

            return comando;
        }
        public async Task<Recuperacion_ContrasenaDto> SeleccionarCodigo(int id_usuario)
        {
            Recuperacion_ContrasenaDto codigo = null;
            string query = @"
                SELECT id_usuario, codigo 
                FROM RECUPERACION_CONTRASENA 
                WHERE id_usuario = @id_usuario";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id_usuario;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                codigo = new Recuperacion_ContrasenaDto
                                {
                                    id_usuario = Convert.ToInt32(reader["id_usuario"]),
                                    codigo = reader["codigo"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                codigo = new Recuperacion_ContrasenaDto
                {
                    mensaje = "Error al traer la información: " + ex.Message
                };
            }

            return codigo;
        }
        public async Task<int> EliminarCodigo(int id_usuario)
        {
            int filasAfectadas = 0;
            string query = "DELETE FROM RECUPERACION_CONTRASENA WHERE id_usuario = @id_usuario";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id_usuario;

                        await cmd.ExecuteNonQueryAsync();
                        filasAfectadas = 1; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar código: " + ex.Message);
            }

            return filasAfectadas;
        }
        public async Task<int?> BuscarIDPersona(string codigo)
        {
            int? personaID = null;
            string query = "SELECT id_usuario FROM RECUPERACION_CONTRASENA WHERE codigo = @codigo";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@codigo", codigo);

                      
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            personaID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al buscar persona: " + ex.Message);
            }

            return personaID;
        }


    }
}
