using Microsoft.Data.SqlClient;
using SPARTANFIT.Dto;
using System.Data;
namespace SPARTANFIT.Repository
{
    public class AlimentoRepository
    {
        private readonly string _connectionString;
        public AlimentoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool>BuscarAlimento(string nombre)
        {
            string sql = "SELECT COUNT(*) FROM ALIMENTO WHERE nombre = @nombre";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    
                    cmd.Parameters.Add(new SqlParameter("@nombre", SqlDbType.NVarChar) { Value = nombre });

                    int resultado = (int)await cmd.ExecuteScalarAsync();
                    return resultado > 0; 
                }
                await con.CloseAsync();
            }
        }


        public async Task<int> RegistrarAlimento (AlimentoDto alimento)
        {
            int resultado = 0;
            string sql = "INSERT INTO Alimento (id_categoria_alimento,nombre,calorias_x_gramo,grasa,carbohidrato,proteina,fibra)"
                            + "VALUES (@id_categoria_alimento,@nombre,@calorias_x_gramo,@grasa,@carbohidrato,@proteina,@fibra)";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_categoria_alimento", alimento.id_categoria_alimento);
                        cmd.Parameters.AddWithValue("@nombre", alimento.nombre);
                        cmd.Parameters.AddWithValue("@calorias_x_gramo", alimento.calorias_x_gramo);
                        cmd.Parameters.AddWithValue("@grasa", alimento.grasa);
                        cmd.Parameters.AddWithValue("@carbohidrato", alimento.carbohidrato);
                        cmd.Parameters.AddWithValue("@proteina", alimento.proteina);
                        cmd.Parameters.AddWithValue("@fibra", alimento.fibra);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch(SqlException ex)
            {
                throw new Exception("Error al momento de registrar alimento");
            }
            return resultado;
        }

        public async Task<int>ActualizarAlimento(AlimentoDto alimento)
        {
            int resultado = 0;
            string sql = "UPDATE ALIMENTO SET nombre= @nombre,calorias_x_gramo = @calorias_x_gramo, grasa=@grasa, carbohidrato=@carbohidrato, proteina=@proteina,fibra=@fibra  " + "WHERE id_alimento = @id_alimento";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_categoria_alimento", alimento.id_categoria_alimento);
                        cmd.Parameters.AddWithValue("@nombre", alimento.nombre);
                        cmd.Parameters.AddWithValue("@calorias_x_gramo", alimento.calorias_x_gramo);
                        cmd.Parameters.AddWithValue("@grasa", alimento.grasa);
                        cmd.Parameters.AddWithValue("@carbohidrato", alimento.carbohidrato);
                        cmd.Parameters.AddWithValue("@proteina", alimento.proteina);
                        cmd.Parameters.AddWithValue("@fibra", alimento.fibra);
                        cmd.Parameters.AddWithValue("@id_alimento", alimento.id_alimento);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el alimento");
            }
            return resultado;
        }

        public async Task<int>EliminarAlimento(int id_alimento)
        {
            int resultado = 0;
            string sql = "DELETE FROM ALIMENTO WHERE id_alimento = @id_alimento";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_alimento", id_alimento);
                        cmd.ExecuteNonQuery();
                    }
                    await con.CloseAsync();
                }
                resultado = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el alimento");
            }
            return resultado;
        }

        public async Task<List<AlimentoDto>> MostrarAlimentos()
        {
            List<AlimentoDto> listAlimentos = new List<AlimentoDto>();
            string sql = "SELECT id_alimento,id_categoria_alimento,nombre,calorias_x_gramo,grasa,carbohidrato,proteina,fibra FROM ALIMENTO";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AlimentoDto alimento = new AlimentoDto();
                                {
                                    alimento.id_alimento = Convert.ToInt32(reader["id_alimento"]);
                                    alimento.id_categoria_alimento = Convert.ToInt32(reader["id_categoria_alimento"]);
                                    alimento.nombre = reader["nombre"].ToString();
                                    alimento.calorias_x_gramo = Convert.ToDouble(reader["calorias_x_gramo"]);
                                    alimento.grasa = Convert.ToDouble(reader["grasa"]);
                                    alimento.carbohidrato = Convert.ToDouble(reader["carbohidrato"]);
                                    alimento.proteina = Convert.ToDouble(reader["proteina"]);
                                    alimento.fibra = Convert.ToDouble(reader["fibra"]);
                                }
                                listAlimentos.Add(alimento);
                            }
                        }
                    }
                    await con.CloseAsync();
                    return listAlimentos;
                }
            }catch (Exception ex) { throw new Exception("Error al mostrar los alimentos"); }
        }
    }
}
