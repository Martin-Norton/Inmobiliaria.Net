using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble 
                    (Direccion, Ambientes, Superficie, Tipo, Uso, Precio, PropietarioId) 
                    VALUES (@direccion, @ambientes, @superficie, @tipo, @uso, @precio, @propietarioId);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@uso", i.Uso);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
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
                string sql = @"DELETE FROM Inmueble WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble 
                    SET Direccion=@direccion, Ambientes=@ambientes, Superficie=@superficie, Tipo=@tipo, Uso=@uso, Precio=@precio, PropietarioId=@propietarioId
                    WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@uso", i.Uso);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    command.Parameters.AddWithValue("@id", i.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, Direccion, Ambientes, Superficie, Tipo, Uso, Precio, PropietarioId FROM Inmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Superficie = reader.GetInt32("Superficie"),
                            Tipo = reader.GetString("Tipo"),
                            Uso = reader.GetString("Uso"),
                            Precio = reader.GetDecimal("Precio"),
                            PropietarioId = reader.GetInt32("PropietarioId")
                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble? i = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, Direccion, Ambientes, Superficie, Tipo, Uso, Precio, PropietarioId FROM Inmueble WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Superficie = reader.GetInt32("Superficie"),
                            Tipo = reader.GetString("Tipo"),
                            Uso = reader.GetString("Uso"),
                            Precio = reader.GetDecimal("Precio"),
                            PropietarioId = reader.GetInt32("PropietarioId")
                        };
                    }
                    connection.Close();
                }
            }
            return i;
        }
    }
}
