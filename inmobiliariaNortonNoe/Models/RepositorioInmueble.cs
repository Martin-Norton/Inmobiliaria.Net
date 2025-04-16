using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration) {}

        // public int Alta(Inmueble p)
        // {
        //     int res = -1;
        //     using (var connection = new MySqlConnection(connectionString))
        //     {
        //         string sql = @"INSERT INTO Inmueble 
        //             (Direccion, Uso, Tipo, Cantidad_Ambientes, Coordenadas, Precio, Estado, ID_Propietario) 
        //             VALUES (@direccion, @uso, @tipo, @cantidadAmbientes, @coordenadas, @precio, @estado, @idPropietario);
        //             SELECT LAST_INSERT_ID();";
        //         using (var command = new MySqlCommand(sql, connection))
        //         {
        //             command.Parameters.AddWithValue("@direccion", p.Direccion);
        //             command.Parameters.AddWithValue("@uso", p.Uso);
        //             command.Parameters.AddWithValue("@tipo", p.Tipo);
        //             command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
        //             command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
        //             command.Parameters.AddWithValue("@precio", p.Precio);
        //             command.Parameters.AddWithValue("@estado", p.Estado);
        //             command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);
                    
        //             connection.Open();
        //             res = Convert.ToInt32(command.ExecuteScalar());
        //             p.Id = res;
        //             connection.Close();
        //         }
        //     }
        //     return res;
        // }
        public int Alta(Inmueble p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble 
                    (Direccion, Uso, Tipo, Cantidad_Ambientes, Coordenadas, Precio, Estado, ID_Propietario, Portada) 
                    VALUES (@direccion, @uso, @tipo, @cantidadAmbientes, @coordenadas, @precio, @estado, @idPropietario, @portada);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
                    command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);

                    // Portada opcional
                    command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(p.Portada) ? (object)DBNull.Value : p.Portada);

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
                string sql = @"DELETE FROM Inmueble WHERE Id = @id";
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

        public int Modificacion(Inmueble p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble SET 
                    Direccion=@direccion, Uso=@uso, Tipo=@tipo, Cantidad_Ambientes=@cantidadAmbientes, 
                    Coordenadas=@coordenadas, Precio=@precio, Estado=@estado, ID_Propietario=@idPropietario
                    WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@tipo", p.Tipo);
                    command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
                    command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);
                    command.Parameters.AddWithValue("@id", p.Id);
                    
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public int ModificarPortada(int id, string url)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
                    UPDATE Inmuebles SET
                    Portada = @portada
                    WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(url) ? (object)DBNull.Value : url);
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Inmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Tipo = reader.GetString("Tipo"),
                            Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
                            Coordenadas = reader.GetString("Coordenadas"),
                            Precio = reader.GetDecimal("Precio"),
                            Estado = reader.GetString("Estado"),
                            Id_Propietario = reader.GetInt32("ID_Propietario"),
                            Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada")
                        });
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Inmueble ObtenerPorId(int id)
        {
            Inmueble p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Inmueble WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        p = new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Tipo = reader.GetString("Tipo"),
                            Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
                            Coordenadas = reader.GetString("Coordenadas"),
                            Precio = reader.GetDecimal("Precio"),
                            Estado = reader.GetString("Estado"),
                            Id_Propietario = reader.GetInt32("ID_Propietario"),
                            Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada"),

                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

    public IList<Inmueble> BuscarPorTipo(string tipo)
        {
            IList<Inmueble> lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Inmueble WHERE Tipo = @tipo";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tipo", tipo);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(MapearInmueble(reader));
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public IList<Inmueble> BuscarPorUso(string uso)
        {
            IList<Inmueble> lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Inmueble WHERE Uso = @uso";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@uso", uso);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(MapearInmueble(reader));
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public IList<Inmueble> ObtenerPorEstado(string estado)
        {
            IList<Inmueble> lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Inmueble WHERE Estado = @estado";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@estado", estado);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(MapearInmueble(reader));
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public IList<Inmueble> ObtenerLista(int paginaNro, int tamPagina)
        {
            IList<Inmueble> lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Inmueble LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(MapearInmueble(reader));
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Inmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return cantidad;
        }

        private Inmueble MapearInmueble(MySqlDataReader reader)
        {
            return new Inmueble
            {
                Id = reader.GetInt32("Id"),
                Direccion = reader.GetString("Direccion"),
                Uso = reader.GetString("Uso"),
                Tipo = reader.GetString("Tipo"),
                Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
                Coordenadas = reader.GetString("Coordenadas"),
                Precio = reader.GetDecimal("Precio"),
                Estado = reader.GetString("Estado"),
                Id_Propietario = reader.GetInt32("Id_Propietario"),
                Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada"),

            };
        }
    }
}
