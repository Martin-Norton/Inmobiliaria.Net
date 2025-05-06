// using System;
// using System.Collections.Generic;
// using MySql.Data.MySqlClient;
// using System.Data;

// namespace inmobiliariaNortonNoe.Models
// {
//     public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
//     {
//         public RepositorioInmueble(IConfiguration configuration) : base(configuration) {}
        
//         public int Alta(Inmueble p)
//         {
//             int res = -1;
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"INSERT INTO Inmueble 
//                     (Direccion, Uso, Tipo, Cantidad_Ambientes, Coordenadas, Precio, Estado, ID_Propietario, Portada) 
//                     VALUES (@direccion, @uso, @tipo, @cantidadAmbientes, @coordenadas, @precio, @estado, @idPropietario, @portada);
//                     SELECT LAST_INSERT_ID();";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@direccion", p.Direccion);
//                     command.Parameters.AddWithValue("@uso", p.Uso);
//                     command.Parameters.AddWithValue("@tipo", p.Tipo);
//                     command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
//                     command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
//                     command.Parameters.AddWithValue("@precio", p.Precio);
//                     command.Parameters.AddWithValue("@estado", p.Estado);
//                     command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);

//                     command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(p.Portada) ? (object)DBNull.Value : p.Portada);

//                     connection.Open();
//                     res = Convert.ToInt32(command.ExecuteScalar());
//                     p.Id = res;
//                     connection.Close();
//                 }
//             }
//             return res;
//         }


//         public int Baja(int id)
//         {
//             int res = -1;
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"DELETE FROM Inmueble WHERE Id = @id";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@id", id);
//                     connection.Open();
//                     res = command.ExecuteNonQuery();
//                     connection.Close();
//                 }
//             }
//             return res;
//         }

//         public int Modificacion(Inmueble p)
//         {
//             int res = -1;
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"UPDATE Inmueble SET
//                     Direccion=@direccion, Uso=@uso, Tipo=@tipo, Cantidad_Ambientes=@cantidadAmbientes,
//                     Coordenadas=@coordenadas, Precio=@precio, Estado=@estado, ID_Propietario=@idPropietario, Portada = @portada
//                     WHERE Id = @id";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@direccion", p.Direccion);
//                     command.Parameters.AddWithValue("@uso", p.Uso);
//                     command.Parameters.AddWithValue("@tipo", p.Tipo);
//                     command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
//                     command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
//                     command.Parameters.AddWithValue("@precio", p.Precio);
//                     command.Parameters.AddWithValue("@estado", p.Estado);
//                     command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);
//                     command.Parameters.AddWithValue("@id", p.Id);
//                     command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(p.Portada) ? (object)DBNull.Value : p.Portada);
                    
//                     connection.Open();
//                     res = command.ExecuteNonQuery();
//                     connection.Close();
//                 }
//             }
//             return res;
//         }
      
//         public int ModificarPortada(int id, string url)
//         {
//             int res = -1;
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"
//                     UPDATE Inmuebles SET
//                     Portada = @portada
//                     WHERE Id = @id";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(url) ? (object)DBNull.Value : url);
//                     command.Parameters.AddWithValue("@id", id);
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     res = command.ExecuteNonQuery();
//                 }
//             }
//             return res;
//         }

//         public IList<Inmueble> ObtenerPorPropietario(int idPropietario)
//         {
//             IList<Inmueble> lista = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = "SELECT * FROM Inmueble WHERE ID_Propietario = @id_Propietario";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@id_Propietario", idPropietario);
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         lista.Add(MapearInmueble(reader));
//                     }
//                     connection.Close();
//                 }
//             }
//             return lista;
//         }

//         public IList<Inmueble> ObtenerTodos()
//         {
//             IList<Inmueble> res = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"SELECT * FROM Inmueble";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         res.Add(new Inmueble
//                         {
//                             Id = reader.GetInt32("Id"),
//                             Direccion = reader.GetString("Direccion"),
//                             Uso = reader.GetString("Uso"),
//                             Tipo = reader.GetString("Tipo"),
//                             Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
//                             Coordenadas = reader.GetString("Coordenadas"),
//                             Precio = reader.GetDecimal("Precio"),
//                             Estado = reader.GetString("Estado"),
//                             Id_Propietario = reader.GetInt32("ID_Propietario"),
//                             Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada")
//                         });
//                     }
//                     connection.Close();
//                 }
//             }
//             return res;
//         }

//         public Inmueble ObtenerPorId(int id)
//         {
//             Inmueble p = null;
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"SELECT * FROM Inmueble WHERE Id = @id";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@id", id);
//                     connection.Open();
//                     var reader = command.ExecuteReader();

//                     if (reader.Read())
//                     {
//                         p = new Inmueble
//                         {
//                             Id = reader.GetInt32("Id"),
//                             Direccion = reader.GetString("Direccion"),
//                             Uso = reader.GetString("Uso"),
//                             Tipo = reader.GetString("Tipo"),
//                             Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
//                             Coordenadas = reader.GetString("Coordenadas"),
//                             Precio = reader.GetDecimal("Precio"),
//                             Estado = reader.GetString("Estado"),
//                             Id_Propietario = reader.GetInt32("ID_Propietario"),
//                             Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada"),

//                         };
//                     }
//                     connection.Close();
//                 }
//             }
//             return p;
//         }

//         public IList<Inmueble> BuscarPorTipo(string tipo)
//         {
//             IList<Inmueble> lista = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = "SELECT * FROM Inmueble WHERE Tipo = @tipo";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@tipo", tipo);
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         lista.Add(MapearInmueble(reader));
//                     }
//                     connection.Close();
//                 }
//             }
//             return lista;
//         }

//         public IList<Inmueble> BuscarPorUso(string uso)
//         {
//             IList<Inmueble> lista = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = "SELECT * FROM Inmueble WHERE Uso = @uso";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@uso", uso);
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         lista.Add(MapearInmueble(reader));
//                     }
//                     connection.Close();
//                 }
//             }
//             return lista;
//         }

//         public IList<Inmueble> ObtenerPorEstado(string estado)
//         {
//             IList<Inmueble> lista = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"
//                     SELECT 
//                         *
//                     FROM 
//                         Inmueble
//                     WHERE 
//                         Estado = @estado";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@estado", estado);
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         lista.Add(MapearInmueble(reader));
//                     }
//                     connection.Close();
//                 }
//             }
//             return lista;
//         }

//         public IList<Inmueble> ObtenerLista(int paginaNro, int tamPagina)
//         {
//             IList<Inmueble> lista = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = "SELECT * FROM Inmueble LIMIT @tamPagina OFFSET @offset";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@tamPagina", tamPagina);
//                     command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         lista.Add(MapearInmueble(reader));
//                     }
//                     connection.Close();
//                 }
//             }
//             return lista;
//         }

//         public int ObtenerCantidad()
//         {
//             int cantidad = 0;
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = "SELECT COUNT(*) FROM Inmueble";
//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.CommandType = CommandType.Text;
//                     connection.Open();
//                     cantidad = Convert.ToInt32(command.ExecuteScalar());
//                     connection.Close();
//                 }
//             }
//             return cantidad;
//         }

//         private Inmueble MapearInmueble(MySqlDataReader reader)
//         {
//             return new Inmueble
//             {
//                 Id = reader.GetInt32("Id"),
//                 Direccion = reader.GetString("Direccion"),
//                 Uso = reader.GetString("Uso"),
//                 Tipo = reader.GetString("Tipo"),
//                 Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
//                 Coordenadas = reader.GetString("Coordenadas"),
//                 Precio = reader.GetDecimal("Precio"),
//                 Estado = reader.GetString("Estado"),
//                 Id_Propietario = reader.GetInt32("Id_Propietario"),
//                 Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada"),

//             };
//         }

//         //zona busquedas
//        public IList<Inmueble> ObtenerInmueblesDisponiblesPorFechas(DateTime fechaInicio, DateTime fechaFin)
//         {
//             IList<Inmueble> res = new List<Inmueble>();
//             using (var connection = new MySqlConnection(connectionString))
//             {
//                 string sql = @"
//                     SELECT i.id AS InmuebleId, i.direccion
//                     FROM Inmueble i
//                     WHERE i.Estado = 'Disponible'
//                     AND i.id NOT IN (
//                         SELECT c.ID_Inmueble
//                         FROM Contrato c
//                         WHERE c.EstadoLogico = 1
//                             AND c.Estado = 'Vigente'
//                             AND (
//                                 (@fechaInicio BETWEEN c.Fecha_Inicio AND c.Fecha_Fin)
//                                 OR (@fechaFin BETWEEN c.Fecha_Inicio AND c.Fecha_Fin)
//                                 OR (c.Fecha_Inicio BETWEEN @fechaInicio AND @fechaFin)
//                                 OR (c.Fecha_Fin BETWEEN @fechaInicio AND @fechaFin)
//                             )
//                     )";

//                 using (var command = new MySqlCommand(sql, connection))
//                 {
//                     command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
//                     command.Parameters.AddWithValue("@fechaFin", fechaFin);
//                     connection.Open();
//                     var reader = command.ExecuteReader();
//                     while (reader.Read())
//                     {
//                         Inmueble inmueble = new Inmueble
//                         {
//                             Id = reader.GetInt32("InmuebleId"),
//                             Direccion = reader.GetString("direccion")
//                         };
//                         res.Add(inmueble);
//                     }
//                     connection.Close();
//                 }
//             }
//             return res;
//         }
//         //Fin zona busquedas
//     }
// }
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration) { }

        public int Alta(Inmueble p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble 
                    (Direccion, Uso, Cantidad_Ambientes, Coordenadas, Precio, Estado, ID_Propietario, Portada, id_tipoinmueble) 
                    VALUES (@direccion, @uso, @cantidadAmbientes, @coordenadas, @precio, @estado, @idPropietario, @portada, @idTipoInmueble);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
                    command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);
                    command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(p.Portada) ? (object)DBNull.Value : p.Portada);
                    command.Parameters.AddWithValue("@idTipoInmueble", p.Id_TipoInmueble);
                    Console.WriteLine("ID_TipoInmueble: " + p.Id_TipoInmueble);

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
                    Direccion=@direccion, Uso=@uso, Cantidad_Ambientes=@cantidadAmbientes,
                    Coordenadas=@coordenadas, Precio=@precio, Estado=@estado, ID_Propietario=@idPropietario, Portada = @portada, ID_TipoInmueble = @idTipoInmueble
                    WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", p.Direccion);
                    command.Parameters.AddWithValue("@uso", p.Uso);
                    command.Parameters.AddWithValue("@cantidadAmbientes", p.Cantidad_Ambientes);
                    command.Parameters.AddWithValue("@coordenadas", p.Coordenadas);
                    command.Parameters.AddWithValue("@precio", p.Precio);
                    command.Parameters.AddWithValue("@estado", p.Estado);
                    command.Parameters.AddWithValue("@idPropietario", p.Id_Propietario);
                    command.Parameters.AddWithValue("@id", p.Id);
                    command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(p.Portada) ? (object)DBNull.Value : p.Portada);
                    command.Parameters.AddWithValue("@idTipoInmueble", p.Id_TipoInmueble);

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
                    UPDATE Inmueble SET
                    Portada = @portada
                    WHERE Id = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@portada", string.IsNullOrEmpty(url) ? (object)DBNull.Value : url);
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> ObtenerPorPropietario(int idPropietario)
        {
            IList<Inmueble> lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql =@"SELECT i.Id, i.Direccion, i.Uso, i.Id_TipoInmueble, i.Cantidad_Ambientes,
                                    i.Coordenadas, i.Precio, i.Estado, i.Id_Propietario, i.Portada,
                                    ti.Nombre AS TipoNombre
                                FROM Inmueble i
                                JOIN TipoInmueble ti ON i.Id_TipoInmueble = ti.Id WHERE i.ID_Propietario = @id_Propietario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_Propietario", idPropietario);
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

        public IList<Inmueble> ObtenerTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.*, t.Nombre AS TipoNombre
                            FROM Inmueble i
                            INNER JOIN TipoInmueble t ON i.ID_TipoInmueble = t.Id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(MapearInmueble(reader));
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
                string sql = @"SELECT i.Id, i.Direccion, i.Uso, i.Id_TipoInmueble, i.Cantidad_Ambientes,
                                    i.Coordenadas, i.Precio, i.Estado, i.Id_Propietario, i.Portada,
                                    ti.Nombre AS TipoNombre
                                FROM Inmueble i
                                JOIN TipoInmueble ti ON i.Id_TipoInmueble = ti.Id
                                WHERE i.Id = @id;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        p = MapearInmueble(reader);
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Inmueble> BuscarPorTipo(int idTipoInmueble)
        {
            IList<Inmueble> lista = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Inmueble WHERE ID_TipoInmueble = @idTipoInmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idTipoInmueble", idTipoInmueble);
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
                string sql = @"SELECT i.Id, i.Direccion, i.Uso, i.Id_TipoInmueble, i.Cantidad_Ambientes,
                                    i.Coordenadas, i.Precio, i.Estado, i.Id_Propietario, i.Portada,
                                    ti.Nombre AS TipoNombre
                                FROM Inmueble i
                                JOIN TipoInmueble ti ON i.Id_TipoInmueble = ti.Id
                                WHERE i.Estado = @estado;";
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
                Cantidad_Ambientes = reader.GetInt32("Cantidad_Ambientes"),
                Coordenadas = reader.GetString("Coordenadas"),
                Precio = reader.GetDecimal("Precio"),
                Estado = reader.GetString("Estado"),
                Id_Propietario = reader.GetInt32("Id_Propietario"),
                Portada = reader["Portada"] == DBNull.Value ? null : reader.GetString("Portada"),
                Id_TipoInmueble = reader.GetInt32("ID_TipoInmueble"),
                Tipo = new TipoInmueble
                {
                    Id = reader.GetInt32("ID_TipoInmueble"),
                    Nombre = reader.GetString("TipoNombre")
                }
            };
        }

        //zona busquedas
        public IList<Inmueble> ObtenerInmueblesDisponiblesPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
                    SELECT i.id AS InmuebleId, i.direccion
                    FROM Inmueble i
                    WHERE i.Estado = 'Disponible'
                    AND i.id NOT IN (
                        SELECT c.ID_Inmueble
                        FROM Contrato c
                        WHERE c.EstadoLogico = 1
                            AND c.Estado = 'Vigente'
                            AND (
                                (@fechaInicio BETWEEN c.Fecha_Inicio AND c.Fecha_Fin)
                                OR (@fechaFin BETWEEN c.Fecha_Inicio AND c.Fecha_Fin)
                                OR (c.Fecha_Inicio BETWEEN @fechaInicio AND @fechaFin)
                                OR (c.Fecha_Fin BETWEEN @fechaInicio AND @fechaFin)
                            )
                    )";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", fechaFin);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32("InmuebleId"),
                            Direccion = reader.GetString("direccion")
                        };
                        res.Add(inmueble);
                    }
                    connection.Close();
                }
            }
            return res;
        }
        //Fin zona busquedas
    }
}