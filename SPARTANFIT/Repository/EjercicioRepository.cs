using Microsoft.Data.SqlClient;
using SPARTANFIT.Dto;
using System.Data;

namespace SPARTANFIT.Repository
{
    public class EjercicioRepository
    {
        private readonly string _connectionString;
        public EjercicioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> BuscarEjercicio(string nombreEjercicio)
        {
            string sql = "SELECT COUNT(*) FROM EJERCICIO WHERE nombre_ejercicio = @nombre_ejercicio";

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add(new SqlParameter("@nombre_ejercicio", SqlDbType.NVarChar, 100)
                        {
                            Value = nombreEjercicio
                        });

                        int ejercicioEncontrado = (int)await cmd.ExecuteScalarAsync();
                        return ejercicioEncontrado > 0;
                    }
                    await con.OpenAsync();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al buscar el ejercicio en la base de datos", ex);
            }
        }

        public async Task<int>RegistrarEjercicio(EjercicioDto ejercicio)
        {
            int resultado = 0;
            string sql = "INSERT INTO EJERCICIO (nombre_ejercicio, id_grupo_muscular,apoyo_visual)"
                            + "VALUES (@nombre_ejercicio, @id_grupo_muscular, @apoyo_visual)";
            try
            {
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using(SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre_ejercicio", ejercicio.nombre_ejercicio);
                        cmd.Parameters.AddWithValue("@id_grupo_muscular", ejercicio.id_grupo_muscular);
                        cmd.Parameters.AddWithValue("@apoyo_visual", ejercicio.apoyo_visual);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch(SqlException ex)
            {
                throw new Exception("Error al momento de registrar ejercicio", ex);
            }
            return resultado;
        }

        public async Task<int>EliminarEjercicio(int id_ejercicio)
        {
            int resultado = 0;
            string sql = "DELETE FROM EJERCICIO WHERE id_ejercicio = @id_ejercicio";
            try
            {
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using(SqlCommand cmd = new SqlCommand(sql,con))
                    {
                        cmd.Parameters.AddWithValue("@id_ejercicio", id_ejercicio);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al momento de eliminar ejercicio", ex);
            }
            return resultado;
        }

        public async Task<int>ActualizarEjercicio(EjercicioDto ejercicio)
        {
            int resultado = 0;
            string sql = "UPDATE EJERCICIO SET nombre_ejercicio = @nombre_ejercicio, apoyo_visual = @apoyo_visual  " + "WHERE id_ejercicio = @id_ejercicio";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre_ejercicio",ejercicio.nombre_ejercicio);
                        cmd.Parameters.AddWithValue("@apoyo_visual", ejercicio.apoyo_visual);
                        cmd.Parameters.AddWithValue("@id_ejercicio", ejercicio.id_ejercicio);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch(SqlException ex)
            {
                throw new Exception("Error al actualizar ejercicio", ex);
            }
            return resultado;
        }

        public async Task<List<EjercicioDto>> MostrarEjercicios()
        {
            List<EjercicioDto> listEjercicios = new List<EjercicioDto>();
            string sql = "SELECT id_ejercicio, nombre_ejercicio, id_grupo_muscular, apoyo_visual FROM EJERCICIO";
            try
            {
                using(SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EjercicioDto ejercicio = new EjercicioDto();
                                {
                                    ejercicio.id_ejercicio = Convert.ToInt32(reader["id_ejercicio"]);
                                    ejercicio.nombre_ejercicio = reader["nombre_ejercicio"].ToString();
                                    ejercicio.id_grupo_muscular = Convert.ToInt32(reader["id_grupo_muscular"]);
                                    ejercicio.apoyo_visual = reader["apoyo_visual"].ToString();
                                }
                                listEjercicios.Add(ejercicio);
                            }
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar ejercicio", ex);
            }
            return listEjercicios;
        }

    }
}
