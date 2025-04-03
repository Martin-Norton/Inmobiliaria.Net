using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioPagos : RepositorioBase
    {
        public RepositorioPagos(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pago (ContratoId, FechaPago, Monto) 
                            VALUES (@contratoId, @fechaPago, @monto);
                            SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
                    command.Parameters.AddWithValue("@monto", p.Monto);

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
                string sql = @"DELETE FROM Pago WHERE Id = @id";
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

        public int Modificacion(Pago p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Pago 
                            SET ContratoId=@contratoId, FechaPago=@fechaPago, Monto=@monto
                            WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@contratoId", p.ContratoId);
                    command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
                    command.Parameters.AddWithValue("@monto", p.Monto);
                    command.Parameters.AddWithValue("@id", p.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public Pago ObtenerPorId(int id)
        {
            Pago? p = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, ContratoId, FechaPago, Monto 
                            FROM Pago WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        p = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto))
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Pago> ObtenerTodos()
        {
            IList<Pago> lista = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, ContratoId, FechaPago, Monto FROM Pago";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto))
                        };
                        lista.Add(p);
                    }
                    connection.Close();
                }
            }
            return lista;
        }

        public IList<Pago> ObtenerPorContrato(int contratoId)
        {
            IList<Pago> lista = new List<Pago>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, ContratoId, FechaPago, Monto FROM Pago 
                            WHERE ContratoId = @contratoId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@contratoId", MySqlDbType.Int32).Value = contratoId;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto))
                        };
                        lista.Add(p);
                    }
                    connection.Close();
                }
            }
            return lista;
        }
    }
}
