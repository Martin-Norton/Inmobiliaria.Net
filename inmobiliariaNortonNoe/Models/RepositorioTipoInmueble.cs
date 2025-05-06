using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioTipoInmueble : RepositorioBase, IRepositorioTipoInmueble
    {
        public RepositorioTipoInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(TipoInmueble t)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO TipoInmueble (Nombre, Descripcion, Estado) VALUES (@nombre, @descripcion, @estado)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", t.Nombre);
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);
                    command.Parameters.AddWithValue("@estado", t.Estado);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "UPDATE TipoInmueble SET Estado = 0 WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(TipoInmueble t)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE TipoInmueble 
                               SET Nombre = @nombre, Descripcion = @descripcion, Estado = @estado 
                               WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", t.Id);
                    command.Parameters.AddWithValue("@nombre", t.Nombre);
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);
                    command.Parameters.AddWithValue("@estado", t.Estado);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public TipoInmueble ObtenerPorId(int id)
        {
            TipoInmueble res = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, Nombre, Descripcion, Estado FROM TipoInmueble WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new TipoInmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Descripcion = reader.GetString("Descripcion"),
                            Estado = reader.GetInt32("Estado")
                        };
                    }
                }
            }
            return res;
        }

        public IList<TipoInmueble> ObtenerTodos()
        {
            var res = new List<TipoInmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, Nombre, Descripcion, Estado FROM TipoInmueble WHERE Estado = 1"; // solo activos
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new TipoInmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre"),
                            Descripcion = reader.GetString("Descripcion"),
                            Estado = reader.GetInt32("Estado")
                        });
                    }
                }
            }
            return res;
        }
    }
}
