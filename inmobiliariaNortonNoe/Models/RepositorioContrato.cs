using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contrato contrato)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contrato 
                    (ID_Inmueble, ID_Inquilino, Fecha_Inicio, Fecha_Fin, Monto_Alquiler, Multa, Estado) 
                    VALUES (@idInmueble, @idInquilino, @fechaInicio, @fechaFin, @montoAlquiler, @multa, @estado);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@idInmueble", contrato.ID_Inmueble);
                    command.Parameters.AddWithValue("@idInquilino", contrato.ID_Inquilino);
                    command.Parameters.AddWithValue("@fechaInicio", contrato.Fecha_Inicio);
                    command.Parameters.AddWithValue("@fechaFin", contrato.Fecha_Fin);
                    command.Parameters.AddWithValue("@montoAlquiler", contrato.Monto_Alquiler);
                    command.Parameters.AddWithValue("@multa", contrato.Multa);
                    command.Parameters.AddWithValue("@estado", contrato.Estado);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    contrato.ID_Contrato = res;
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
                string sql = @"DELETE FROM Contrato WHERE ID_Contrato = @id";
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

        public int Modificacion(Contrato contrato)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato 
                    SET ID_Inmueble=@idInmueble, ID_Inquilino=@idInquilino, Fecha_Inicio=@fechaInicio, 
                        Fecha_Fin=@fechaFin, Monto_Alquiler=@montoAlquiler, Multa=@multa, Estado=@estado
                    WHERE ID_Contrato=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", contrato.ID_Contrato);
                    command.Parameters.AddWithValue("@idInmueble", contrato.ID_Inmueble);
                    command.Parameters.AddWithValue("@idInquilino", contrato.ID_Inquilino);
                    command.Parameters.AddWithValue("@fechaInicio", contrato.Fecha_Inicio);
                    command.Parameters.AddWithValue("@fechaFin", contrato.Fecha_Fin);
                    command.Parameters.AddWithValue("@montoAlquiler", contrato.Monto_Alquiler);
                    command.Parameters.AddWithValue("@multa", contrato.Multa);
                    command.Parameters.AddWithValue("@estado", contrato.Estado);
                    
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Contrato";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            ID_Contrato = reader.GetInt32("ID_Contrato"),
                            ID_Inmueble = reader.GetInt32("ID_Inmueble"),
                            ID_Inquilino = reader.GetInt32("ID_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                            Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                            Multa = reader.GetDecimal("Multa"),
                            Estado = reader.GetString("Estado")
                        };
                        res.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato contrato = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Contrato WHERE ID_Contrato=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            ID_Contrato = reader.GetInt32("ID_Contrato"),
                            ID_Inmueble = reader.GetInt32("ID_Inmueble"),
                            ID_Inquilino = reader.GetInt32("ID_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                            Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                            Multa = reader.GetDecimal("Multa"),
                            Estado = reader.GetString("Estado")
                        };
                    }
                    connection.Close();
                }
            }
            return contrato;
        }
        public Contrato ObtenerPorInmueble(int idInmueble)
        {
                    Contrato contrato = null;
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        string sql = @"SELECT * FROM Contrato WHERE ID_Inmueble = @idInmueble";
                        using (var command = new MySqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@idInmueble", idInmueble);
                            connection.Open();
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    contrato = MapearContrato(reader);
                                }
                            }
                            connection.Close();
                        }
                    }
                    return contrato;
                }

        public IList<Contrato> ObtenerVigentes()
        {
            List<Contrato> contratos = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Contrato WHERE Estado = 'Vigente'";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(MapearContrato(reader));
                        }
                    }
                    connection.Close();
                }
            }
            return contratos;
        }

        public IList<Contrato> ObtenerPorInquilino(int idInquilino)
        {
            List<Contrato> contratos = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Contrato WHERE ID_Inquilino = @idInquilino";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInquilino", idInquilino);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(MapearContrato(reader));
                        }
                    }
                    connection.Close();
                }
            }
            return contratos;
        }

        public IList<Contrato> ObtenerLista(int paginaNro, int tamPagina)
        {
            List<Contrato> contratos = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT * FROM Contrato LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contratos.Add(MapearContrato(reader));
                        }
                    }
                    connection.Close();
                }
            }
            return contratos;
        }

        public int ObtenerCantidad()
        {
            int cantidad = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(ID_Contrato) FROM Contrato";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    cantidad = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return cantidad;
        }
        private Contrato MapearContrato(MySqlDataReader reader)
        {
            return new Contrato
            {
                ID_Contrato = reader.GetInt32("ID_Contrato"),
                ID_Inmueble = reader.GetInt32("ID_Inmueble"),
                ID_Inquilino = reader.GetInt32("ID_Inquilino"),
                Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                Multa = reader.GetDecimal("Multa"),
                Estado = reader.GetString("Estado")
            };
        }

    

        public bool ExisteContratoSuperpuesto(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(*) FROM Contrato 
                            WHERE ID_Inmueble = @idInmueble
                            AND ((@fechaInicio <= Fecha_Fin AND @fechaFin >= Fecha_Inicio))";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", fechaFin);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        public bool ExisteContratoSuperpuestoE(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(*) FROM Contrato 
                            WHERE ID_Inmueble = @idInmueble
                            AND ((@fechaInicio <= Fecha_Fin AND @fechaFin >= Fecha_Inicio)) AND ID_Contrato != @idContrato";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", fechaFin);
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
    }
}
