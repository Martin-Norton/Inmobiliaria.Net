using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioImagen : RepositorioBase, IRepositorioImagen
    {
        public RepositorioImagen(IConfiguration configuration) : base(configuration) {}

        public int Alta(Imagen p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Imagenes 
                    (InmuebleId, Url) 
                    VALUES (@inmuebleId, @url);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inmuebleId", p.InmuebleId);
                    command.Parameters.AddWithValue("@url", p.Url);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Imagenes WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Imagen p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Imagenes SET 
                    Url=@url
                    WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", p.Id);
                    command.Parameters.AddWithValue("@url", p.Url);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Imagen ObtenerPorId(int id)
        {
            Imagen res = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
                                Id, 
                                InmuebleId, 
                                Url 
                              FROM Imagenes 
                              WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Imagen
                        {
                            Id = reader.GetInt32("Id"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Url = reader.GetString("Url")
                        };
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Imagen> ObtenerTodos()
        {
            List<Imagen> res = new List<Imagen>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
                                Id, 
                                InmuebleId, 
                                Url 
                              FROM Imagenes";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Imagen
                        {
                            Id = reader.GetInt32("Id"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Url = reader.GetString("Url")
                        });
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Imagen> BuscarPorInmueble(int inmuebleId)
        {
            List<Imagen> res = new List<Imagen>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT 
                                Id, 
                                InmuebleId, 
                                Url 
                              FROM Imagenes 
                              WHERE InmuebleId=@inmuebleId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inmuebleId", inmuebleId);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Imagen
                        {
                            Id = reader.GetInt32("Id"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            Url = reader.GetString("Url")
                        });
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}
