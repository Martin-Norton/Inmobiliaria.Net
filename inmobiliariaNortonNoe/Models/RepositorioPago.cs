using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaNortonNoe.Models 
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration) { }

        public int Alta(Pago pago)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pago (id_contrato, fecha_pago, monto)
                            VALUES (@idContrato,  @fechaPago, @Monto);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idContrato", pago.Id_Contrato);
                    command.Parameters.AddWithValue("@fechaPago", pago.Fecha_Pago);
                    command.Parameters.AddWithValue("@Monto", pago.Monto);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    pago.Id = res;
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Pago> ObtenerPagosPorContrato(int idContrato)
        {
            var pagos = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, id_contrato, fecha_pago, monto 
                            FROM Pago 
                            WHERE id_contrato = @idContrato";
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
                            Fecha_Pago = reader.GetDateTime("fecha_pago"),
                            Monto = reader.GetDecimal("monto")
                        });
                    }
                    connection.Close();
                }
            }
            return pagos;
        }

        public int Baja(int id)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @"DELETE FROM Pago WHERE Id = @id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return command.ExecuteNonQuery();
        }

        public int Modificacion(Pago pago)
        {
            using var connection = new MySqlConnection(connectionString);
            var sql = @"UPDATE Pago SET Id_Contrato=@idContrato, Nro_Pago= Fecha_Pago=@fechaPago, Monto=@Monto 
                        WHERE Id=@id";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", pago.Id);
            command.Parameters.AddWithValue("@idContrato", pago.Id_Contrato);
            command.Parameters.AddWithValue("@fechaPago", pago.Fecha_Pago);
            command.Parameters.AddWithValue("@Monto", pago.Monto);

            connection.Open();
            return command.ExecuteNonQuery();
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
            var sql = @"SELECT * FROM Pago";
            using var command = new MySqlCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearPago(reader));
            }
            return lista;
        }

        public IList<Pago> ObtenerPorContrato(int idContrato)
        {
            var lista = new List<Pago>();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"SELECT * FROM Pago WHERE id_contrato = @idContrato";
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
                Id_Contrato = reader.GetInt32("Id_Contrato"),
                Fecha_Pago = reader.GetDateTime("Fecha_Pago"),
                Monto = reader.GetDecimal("Monto")
            };
        }
    }

}
