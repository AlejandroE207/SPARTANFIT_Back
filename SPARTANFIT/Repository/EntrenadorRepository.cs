
using Microsoft.Data.SqlClient;
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
            string sql = "SELECT  id_usuario, id_rol,nombres, apellidos, correo, contrasena, fecha_nacimiento, genero FROM USUARIO WHERE id_rol=2";
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
                                id_rol = Convert.ToInt32(reader["id_rol"]),
                                nombres = reader["nombres"].ToString(),
                                apellidos = reader["apellidos"].ToString(),
                                correo = reader["correo"].ToString(),
                                contrasena = reader["contrasena"].ToString(),
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

    }
}
