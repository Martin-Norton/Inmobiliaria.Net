using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaNortonNoe.Models 
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public int Alta(Pago pago)
        {
            throw new NotImplementedException("Usar la versión con ID de usuario");
        }

        public RepositorioPago(IConfiguration configuration) : base(configuration) { }

        public int Alta(Pago pago, int idUsuario)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO Pago 
                    (id_contrato, fecha_pago, periodo_pago, monto, pagado, esMulta, descripcion, estado, id_usuarioalta)
                    VALUES 
                    (@idContrato, @fechaPago, @periodoPago, @monto, @pagado, @esMulta, @descripcion, @estado, @idUsuario);
                    SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", pago.Id_Contrato);
                    command.Parameters.AddWithValue("@fechaPago", (object)pago.Fecha_Pago ?? DBNull.Value);
                    command.Parameters.AddWithValue("@periodoPago", pago.Periodo_Pago);
                    command.Parameters.AddWithValue("@monto", pago.Monto);
                    command.Parameters.AddWithValue("@pagado", pago.Pagado);
                    command.Parameters.AddWithValue("@esMulta", pago.EsMulta);
                    command.Parameters.AddWithValue("@descripcion", (object?)pago.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@estado", pago.Estado);
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    pago.Id = res;
                    connection.Close();
                }
            }
            return res;
        }


        public int Modificacion(Pago pago)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @"UPDATE Pago 
                        SET Id_Contrato = @idContrato,
                            Fecha_Pago = @fechaPago,
                            Periodo_Pago = @periodoPago,
                            Monto = @monto,
                            Pagado = @pagado,
                            EsMulta = @esMulta,
                            Descripcion = @descripcion
                        WHERE Id = @id";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", pago.Id);
            command.Parameters.AddWithValue("@idContrato", pago.Id_Contrato);
            command.Parameters.AddWithValue("@fechaPago", (object)pago.Fecha_Pago ?? DBNull.Value);
            command.Parameters.AddWithValue("@periodoPago", pago.Periodo_Pago);
            command.Parameters.AddWithValue("@monto", pago.Monto);
            command.Parameters.AddWithValue("@pagado", pago.Pagado ? 1 : 0);
            command.Parameters.AddWithValue("@esMulta", pago.EsMulta ? 1 : 0);
            command.Parameters.AddWithValue("@descripcion", pago.Descripcion ?? "");

            connection.Open();
            return command.ExecuteNonQuery();
        }
        public int Baja(int pago)
        {
            throw new NotImplementedException("Usar la versión con ID de usuario");
        }

        public int Baja(int id, int idUsuarioBaja)
        {
            int resultado = -1;
            using var connection = new MySqlConnection(connectionString);
            var sql = @"UPDATE Pago 
                        SET Estado = 0, Id_UsuarioBaja = @idUsuarioBaja  
                        WHERE Id = @id";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@idUsuarioBaja", idUsuarioBaja);

            try
            {
                connection.Open();
                resultado = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al dar de baja el pago: " + ex.Message);
            }

            return resultado;
        }

        public IList<Pago> ObtenerPagosPorContrato(int idContrato)
        {
            var pagos = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, id_contrato, fecha_pago, periodo_pago, monto, pagado, esmulta, 
                                    descripcion, estado, ID_UsuarioAlta, ID_UsuarioBaja
                            FROM Pago 
                            WHERE id_contrato = @idContrato AND Estado = 1
                            ORDER BY fecha_pago DESC;";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", idContrato);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32("Id"),
                            Id_Contrato = reader.GetInt32("id_contrato"),
                            Fecha_Pago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) ? null : reader.GetDateTime("fecha_pago"),
                            Periodo_Pago = reader.GetDateTime("periodo_pago"),
                            Monto = reader.GetDecimal("monto"),
                            Pagado = reader.GetBoolean("pagado"),
                            EsMulta = reader.GetBoolean("esmulta"),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString("descripcion"),
                            Estado = reader.GetInt32("estado"),
                            Id_UsuarioAlta = reader.GetInt32("ID_UsuarioAlta"),
                            Id_UsuarioBaja = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioBaja")) ? null : reader.GetInt32("ID_UsuarioBaja")
                        });
                    }
                    connection.Close();
                }
            }
            return pagos;
        }

        public IList<Pago> ObtenerPagosPagadosPorContrato(int idContrato)
        {
            var lista = new List<Pago>();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT Id, id_contrato, fecha_pago, periodo_pago, monto, pagado, esmulta, descripcion, estado, ID_UsuarioAlta, ID_UsuarioBaja
                        FROM Pago 
                        WHERE id_contrato = @idContrato AND Estado = 1 AND pagado = 1
                        ORDER BY fecha_pago DESC;"; 
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idContrato", idContrato);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPago(reader));
            }
            return lista;
        }

        public IList<Pago> ObtenerPagosImpagosPorContrato(int idContrato)
        {
            var lista = new List<Pago>();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT Id, id_contrato, fecha_pago, periodo_pago, monto, pagado, esmulta, descripcion, estado, ID_UsuarioAlta, ID_UsuarioBaja
                        FROM Pago 
                        WHERE id_contrato = @idContrato AND Estado = 1 AND pagado = 0
                        ORDER BY fecha_pago DESC;"; 
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idContrato", idContrato);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPago(reader));
            }
            return lista;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago pago = null;
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT * FROM Pago WHERE Id = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                pago = MapearPago(reader);
            }
            return pago;
        }

        public IList<Pago> ObtenerTodos()
        {
            var lista = new List<Pago>();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT * FROM Pago and Estado = 1";
            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPago(reader));
            }
            return lista;
        }

        public IList<Pago> ObtenerPagosDeBajaPorContrato(int idContrato)
        {
            var lista = new List<Pago>();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT Id, id_contrato, fecha_pago, periodo_pago, monto, pagado, esmulta, descripcion, estado, ID_UsuarioAlta, ID_UsuarioBaja
                        FROM Pago 
                        WHERE id_contrato = @idContrato AND Estado = 0
                        ORDER BY fecha_pago DESC;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idContrato", idContrato);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPago(reader));
            }
            return lista;
        }


        public IList<Pago> ObtenerLista(int paginaNro, int tamPagina)
        {
            var lista = new List<Pago>();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT * FROM Pago LIMIT @tamPagina OFFSET @offset";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@tamPagina", tamPagina);
            command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPago(reader));
            }
            return lista;
        }

        public int ObtenerCantidad()
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT COUNT(*) FROM Pago";
            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar());
        }

        private Pago MapearPago(MySqlDataReader reader)
        {
            return new Pago
            {
                Id = reader.GetInt32("Id"),
                Id_Contrato = reader.GetInt32("id_contrato"),
                Fecha_Pago = reader.IsDBNull("fecha_pago") ? null : reader.GetDateTime("fecha_pago"),
                Periodo_Pago = reader.GetDateTime("periodo_pago"),
                Monto = reader.GetDecimal("monto"),
                Pagado = reader.GetBoolean("pagado"),
                EsMulta = reader.GetBoolean("esmulta"),
                Descripcion = reader.IsDBNull("descripcion") ? null : reader.GetString("descripcion"),
                Estado = reader.GetInt32("estado"),
                Id_UsuarioAlta = reader.GetInt32("ID_UsuarioAlta"),
                Id_UsuarioBaja = reader.IsDBNull("ID_UsuarioBaja") ? null : reader.GetInt32("ID_UsuarioBaja")
            };
        }


    }
}
