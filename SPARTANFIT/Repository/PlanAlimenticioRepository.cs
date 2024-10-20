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
                    foreach (int alimento in idAlimentos)
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
                        using (SqlDataReader reader = cmd.ExecuteReader())
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

        public async Task<int> AsignarPlanAlimenticio(UsuarioDto usuario)
        {
            int resultado = 0;
            double TMB = 0;
            try
            {
                DateTime fechaActual = DateTime.Now;
                int añoActual = fechaActual.Year;
                DateTime fechaNacimiento = DateTime.Parse(usuario.persona.fecha_nacimiento);
                int añoNacimiento = fechaNacimiento.Year;
                double calorias = 0;
                double edad = añoActual - añoNacimiento;
                if (usuario.rehabilitacion == 1)
                {
                    usuario.id_objetivo = 2;
                }
                if (usuario.persona.genero == "masculino")
                {

                    TMB = 66 + (13.7 * usuario.peso) + (5 * usuario.estatura) - (6.8 * edad);

                    switch (usuario.id_objetivo)
                    {
                        case 1:
                            calorias = TMB + 734;
                            break;
                        case 2:
                            calorias = TMB + 1270;
                            break;
                        case 3:
                            calorias = TMB + 1810;
                            break;
                        case 4:
                            calorias = TMB + 1810;
                            break;
                    }
                }
                else
                {
                    TMB = 665 + (9.6 * usuario.peso) + (1.8 * usuario.estatura) - (4.7 * edad);
                    switch (usuario.id_objetivo)
                    {
                        case 1:
                            calorias = TMB + 606;
                            break;
                        case 2:
                            calorias = TMB + 1050;
                            break;
                        case 3:
                            calorias = TMB + 1270;
                            break;
                        case 4:
                            calorias = TMB + 1270;
                            break;
                    }
                }
                double carbohidratos = calorias * 0.55;
                double proteinas = calorias * 0.25;
                double grasas = calorias * 0.20;

                string descripcion = "Ten encuenta " + usuario.persona.nombres + " que tienes que consumir la cantidad de " + calorias + " diarias, distribuidas en las siguientes cantidades calorias de macronutrientes: \n" +
                                    "Carbohidratos: " + carbohidratos + "\nProteinas: " + proteinas + "\nGrasas: " + grasas;
                List<String> dias = new List<String> { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };

                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    foreach (string dia in dias)
                    {
                        string sql = "SELECT TOP (1) id_plan_alimenticio FROM PLAN_ALIMENTICIO WHERE dia = @dia";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@dia", dia);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int id_plan_alimenticio = Convert.ToInt32(reader["id_plan_alimenticio"]);
                                    resultado = await AsignarPlanAlimenticioDia(usuario.persona.id_usuario, id_plan_alimenticio, descripcion);
                                }
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
            return resultado;
        }

        public async Task<int> AsignarPlanAlimenticioDia(int id_usuario, int id_plan_alimenticio, string descripcion)
        {
            int resultado = 0;
            try
            {
                string sql = "INSERT INTO USUARIO_PLAN_ALIMENTICIO (id_usuario,id_plan_alimenticio,descripcion) VALUES (@id_usuario,@id_plan_alimenticio,@descripcion)";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                        cmd.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);
                        cmd.Parameters.AddWithValue("@descripcion", descripcion);
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

        public async Task<PlanAlimenticioDto> BuscarPlanIdUsuario(int id_usuario, string dia)
        {
            PlanAlimenticioDto planAlimenticio = new PlanAlimenticioDto();
            try
            {
                string sql = "SELECT pa.id_plan_alimenticio, pa.nombre, pa.dia, usp.descripcion " +
                    "FROM PLAN_ALIMENTICIO AS pa " +
                    "INNER JOIN USUARIO_PLAN_ALIMENTICIO AS usp ON pa.id_plan_alimenticio = usp.id_plan_alimenticio " +
                    "WHERE usp.id_usuario = @id_usuario AND pa.dia = @dia";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
                        cmd.Parameters.AddWithValue("@dia", dia);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                planAlimenticio.id_plan_alimenticio = Convert.ToInt32(reader["id_plan_alimenticio"]);
                                planAlimenticio.nombre = reader["nombre"].ToString();
                                planAlimenticio.dia = reader["dia"].ToString();
                                planAlimenticio.descripcion = reader["descripcion"].ToString();
                                return planAlimenticio;
                            }
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return planAlimenticio;
        }

        public async Task<List<AlimentoDto>> ObtenerAlimentosDia(int id_plan_alimenticio)
        {
            List<AlimentoDto> listAlimentosDia = new List<AlimentoDto>();
            try
            {
                string sql = "SELECT a.id_alimento, a.id_categoria_alimento, a.nombre,a.calorias_x_gramo, a.grasa, a.carbohidrato, a.proteina " +
                    "FROM ALIMENTO AS a " +
                    "INNER JOIN PLAN_ALIMENTO AS pa ON pa.id_alimento = a.id_alimento " +
                    "WHERE pa.id_plan_alimenticio = @id_plan_alimenticio";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_plan_alimenticio", id_plan_alimenticio);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AlimentoDto alimento = new AlimentoDto()
                                {
                                    id_alimento = Convert.ToInt32(reader["id_alimento"]),
                                    id_categoria_alimento = Convert.ToInt32(reader["id_categoria_alimento"]),
                                    nombre = reader["nombre"].ToString(),
                                    calorias_x_gramo = Convert.ToDouble(reader["calorias_x_gramo"]),
                                    grasa = Convert.ToDouble(reader["grasa"]),
                                    carbohidrato = Convert.ToDouble(reader["carbohidrato"]),
                                    proteina = Convert.ToDouble(reader["proteina"])
                                };
                                listAlimentosDia.Add(alimento);
                            }
                        }
                        return listAlimentosDia;
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return listAlimentosDia;
        }
    }
}
