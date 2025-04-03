using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliariaNortonNoe.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contrato (InmuebleId, InquilinoId, FechaInicio, FechaFin, Monto) 
                                VALUES (@inmuebleId, @inquilinoId, @fechaInicio, @fechaFin, @monto);
                                SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);
                    command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", c.FechaFin);
                    command.Parameters.AddWithValue("@monto", c.Monto);
                    
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res;
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
                string sql = "DELETE FROM Contrato WHERE Id = @id";
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

        public int Modificacion(Contrato c)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato SET 
                                InmuebleId=@inmuebleId, InquilinoId=@inquilinoId, 
                                FechaInicio=@fechaInicio, FechaFin=@fechaFin, Monto=@monto 
                                WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);
                    command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", c.FechaFin);
                    command.Parameters.AddWithValue("@monto", c.Monto);
                    command.Parameters.AddWithValue("@id", c.Id);
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
                string sql = "SELECT Id, InmuebleId, InquilinoId, FechaInicio, FechaFin, Monto FROM Contrato";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Monto = reader.GetDecimal("Monto")
                        };
                        res.Add(c);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato c = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT Id, InmuebleId, InquilinoId, FechaInicio, FechaFin, Monto FROM Contrato WHERE Id=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        c = new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Monto = reader.GetDecimal("Monto")
                        };
                    }
                    connection.Close();
                }
            }
            return c;
        }
    }
}