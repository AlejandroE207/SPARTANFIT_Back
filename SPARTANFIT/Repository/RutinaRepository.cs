using Microsoft.Data.SqlClient;
using SPARTANFIT.Dto;

namespace SPARTANFIT.Repository
{
    public class RutinaRepository
    {
        private readonly string _connectionString;

        public RutinaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int>RegistrarRutina(RutinaDto rutina)
        {
            int resultado = 0;
            string sql = "INSERT INTO RUTINA (id_nivel_rutina, id_objetivo, nombre_rutina,dia,descripcion, id_entrenador)"
                            + "VALUES (@id_nivel_rutina, @id_objetivo, @nombre_rutina, @dia, @descripcion, @id_entrenador)";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using(SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_nivel_rutina", rutina.id_nivel_rutina);
                        cmd.Parameters.AddWithValue("@id_objetivo", rutina.id_objetivo);
                        cmd.Parameters.AddWithValue("@nombre_rutina", rutina.nombre_rutina);
                        cmd.Parameters.AddWithValue("@dia", rutina.dia);
                        cmd.Parameters.AddWithValue("@descripcion", rutina.descripcion);
                        cmd.Parameters.AddWithValue("@id_entrenador", rutina.id_entrenador);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;

            }
            catch(SqlException ex)
            {
                throw new Exception("Error al momento de registar rutina", ex);
            }
            int resultadoRut = 0;
            if(resultado != 0)
            {
                resultadoRut = await ObtenerUltimaRutina();
            }
            return resultadoRut;
        }

        public async Task<int> ObtenerUltimaRutina()
        {
            int id_rutina = 0;
            string sql = "SELECT MAX(id_rutina) as id_max FROM RUTINA";
            try
            {
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                id_rutina = Convert.ToInt32(reader["id_max"]);
                            }
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error al obtener ultima rutina", ex);
            }
            return id_rutina;
        }

        public async Task<int> RegistrarEjerciciosRutina(List<EjercicioDto> ejerciciosRutina, int id_rutina)
        {
            int resultado = 0;
            try
            {
                string sql = "INSERT INTO RUTINA_EJERCICIO (id_ejercicio, id_rutina, num_series, num_repeticiones)" +
                                "VALUES (@id_ejercicio, @id_rutina, @num_series, @num_repeticiones)";
                foreach (EjercicioDto ejercicio in ejerciciosRutina)
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        await con.OpenAsync();
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@id_ejercicio", ejercicio.id_ejercicio);
                            cmd.Parameters.AddWithValue("@id_rutina", id_rutina);
                            cmd.Parameters.AddWithValue("@num_series", ejercicio.num_series);
                            cmd.Parameters.AddWithValue("@num_repeticiones", ejercicio.repeticiones);
                            cmd.ExecuteNonQuery();
                        }
                        await con.CloseAsync();
                    }
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return resultado;
        }
        
        public async Task<List<RutinaDto>> MostrarRutinas()
        {
            List<RutinaDto> listRutinas = new List<RutinaDto>();
            try
            {
                string sql ="SELECT id_rutina, id_nivel_rutina, id_objetivo, nombre_rutina, dia, descripcion FROM RUTINA";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                RutinaDto rutina = new RutinaDto()
                                {
                                    id_rutina = Convert.ToInt32(reader["id_rutina"]),
                                    id_nivel_rutina = Convert.ToInt32(reader["id_nivel_rutina"]),
                                    id_objetivo = Convert.ToInt32(reader["id_objetivo"]),
                                    nombre_rutina = reader["nombre_rutina"].ToString(),
                                    dia = reader["dia"].ToString(),
                                    descripcion = reader["descripcion"].ToString()
                                };
                                listRutinas.Add(rutina);
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
            return listRutinas;
        }

        public async Task<int> EliminarRutina(int id_rutina)
        {
            int resultado = 0;
            try
            {
                resultado = await EliminarUsuarioRutina(id_rutina);
                if(resultado != 0)
                {
                    resultado = await EliminarRutinaEjercicio(id_rutina);
                    if(resultado != 0)
                    {
                        string sql = "DELETE FROM RUTINA WHERE id_rutina = @id_rutina";
                        using (SqlConnection con = new SqlConnection(_connectionString))
                        {
                            await con.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@id_rutina", id_rutina);
                                cmd.ExecuteNonQuery();
                            }
                            await con.CloseAsync();
                        }
                        resultado = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }


        public async Task<int> EliminarUsuarioRutina(int id_rutina)
        {
            int resultado = 0;
            try
            {
                string sql = "DELETE FROM USUARIO_RUTINA WHERE id_rutina = @id_rutina";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_rutina", id_rutina);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }

        public async Task<int> EliminarRutinaEjercicio(int id_rutina)
        {
            int resultado = 0;
            try
            {
                string sql = "DELETE FROM RUTINA_EJERCICIO WHERE id_rutina = @id_rutina";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using(SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_rutina", id_rutina);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }
    }
}
