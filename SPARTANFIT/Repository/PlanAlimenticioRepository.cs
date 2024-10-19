using Microsoft.Data.SqlClient;
using SPARTANFIT.Dto;

namespace SPARTANFIT.Repository
{
    public class PlanAlimenticioRepository
    {
        private readonly string _connectionString;

        public PlanAlimenticioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> RegistrarPlan(PlanAlimenticioDto planAlimenticio)
        {
            int resultado = 0;
            try
            {
                string sql = "INSERT INTO PLAN_ALIMENTICIO (nombre, dia, id_entrenador)"
                            + "VALUES (@nombre, @dia, @id_entrenador)";

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nombre", planAlimenticio.nombre);
                        cmd.Parameters.AddWithValue("@dia", planAlimenticio.dia);
                        cmd.Parameters.AddWithValue("@id_entrenador", planAlimenticio.id_entrenador);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }

                resultado = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            int id_plan = 0;

            id_plan = await ObtenerUltimoPlan();
            return id_plan;
        }


        public async Task<int> ObtenerUltimoPlan()
        {
            int id_plan = 0;
            string sql = "SELECT MAX(id_plan_alimenticio) AS id_max FROM PLAN_ALIMENTICIO";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                id_plan = Convert.ToInt32(reader["id_max"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return id_plan;
        }

        public async Task<int> RegistrarAlimentoPlan(List<int> idAlimentos, int id_plan_alimenticio)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    foreach(int alimento in idAlimentos)
                    {
                        string sql = "INSERT INTO PLAN_ALIMENTO (id_plan_alimenticio, id_alimento)" +
                                "VALUES (@id_plan_alimenticio, @id_alimento)";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                            cmd.Parameters.AddWithValue("@id_alimento", alimento);
                        }
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

        public async Task<List<PlanAlimenticioDto>> MostrarPlanes()
        {
            List<PlanAlimenticioDto> listPlanes = new List<PlanAlimenticioDto>();
            try
            {
                string sql = "SELECT id_plan_alimenticio, nombre, dia FROM PLAN_ALIMENTICIO";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PlanAlimenticioDto plan = new PlanAlimenticioDto()
                                {
                                    id_plan_alimenticio = Convert.ToInt32(reader["id_plan_alimenticio"]),
                                    nombre = reader["nombre"].ToString(),
                                    dia = reader["dia"].ToString()
                                };
                                listPlanes.Add(plan);
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
            return listPlanes;
        }

        public async Task<int> EliminarPlan(int id_plan_alimenticio)
        {
            int resultado = 0;
            try
            {
                resultado = await EliminarUsuarioPlanAlimenticio(id_plan_alimenticio);
                if (resultado != 0)
                {
                    resultado = await EliminarPlanAlimento(id_plan_alimenticio);
                    if (resultado != 0)
                    {
                        string sql = "DELETE FROM PLAN_ALIMENTICIO WHERE id_plan_alimenticio = @id_plan_alimenticio";
                        using (SqlConnection con = new SqlConnection(_connectionString))
                        {
                            await con.OpenAsync();
                            using (SqlCommand cmd = new SqlCommand(sql, con))
                            {
                                cmd.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                                cmd.ExecuteNonQuery();
                                resultado = 1;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }

        public async Task<int> EliminarUsuarioPlanAlimenticio(int id_plan_alimenticio)
        {
            int resultado = 0;
            try
            {
                string sql = "DELETE FROM USUARIO_PLAN_ALIMENTICIO WHERE id_plan_alimenticio = @id_plan_alimenticio";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {

                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
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

        public async Task<int> EliminarPlanAlimento(int id_plan_alimenticio)
        {
            int resultado = 0;
            try
            {
                string sql = "DELETE FROM PLAN_ALIMENTO WHERE id_plan_alimenticio = @id_plan_alimenticio";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                        cmd.ExecuteNonQuery();
                        resultado = 1;
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }
    }
}
