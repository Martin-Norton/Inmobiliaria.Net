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
            throw new NotImplementedException("Usar la versión con ID de usuario");
        }

        public int Alta(Contrato contrato, int ID_UsuarioAlta)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contrato 
                    (ID_Inmueble, ID_Inquilino, Fecha_Inicio, Fecha_Fin, Monto_Alquiler, Multa, Estado, EstadoLogico, ID_UsuarioAlta) 
                    VALUES (@idInmueble, @idInquilino, @fechaInicio, @fechaFin, @montoAlquiler, @multa, @estado, 1, @idUsuarioAlta);
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
                    command.Parameters.AddWithValue("@estadoLogico", 1);
                    command.Parameters.AddWithValue("@idUsuarioAlta", ID_UsuarioAlta);              
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    contrato.ID_Contrato = res;
                    connection.Close();
                }
            }
            return res;
        }
        public int Baja(int contrato)
        {
            throw new NotImplementedException("Usar la versión con ID de usuario");
        }


        public int Baja(int id, int idUsuarioBaja)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato 
                    SET  Estado='Anulado', EstadoLogico=0, ID_UsuarioBaja=@idUsuarioBaja 
                    WHERE ID_Contrato=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@idUsuarioBaja", idUsuarioBaja);
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
    //zona Fin Anticipado
        public int AnularContrato(Contrato contrato, DateTime fecha_finAnticipada, int idUsuarioAnulacion, int multa)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato 
                    SET ID_Inmueble=@idInmueble, ID_Inquilino=@idInquilino, Fecha_Inicio=@fechaInicio, 
                        Fecha_Fin=@fechaFin, Monto_Alquiler=@montoAlquiler, Multa=@multa, Estado='Anulado', ID_UsuarioAnulacion=@idUsuarioAnulacion, Fecha_FinAnt=@fechaFinAnticipada
                    WHERE ID_Contrato=@id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", contrato.ID_Contrato);
                    command.Parameters.AddWithValue("@idInmueble", contrato.ID_Inmueble);
                    command.Parameters.AddWithValue("@idInquilino", contrato.ID_Inquilino);
                    command.Parameters.AddWithValue("@fechaInicio", contrato.Fecha_Inicio);
                    command.Parameters.AddWithValue("@fechaFin", contrato.Fecha_Fin);
                    command.Parameters.AddWithValue("@montoAlquiler", contrato.Monto_Alquiler);
                    command.Parameters.AddWithValue("@multa", multa);
                    command.Parameters.AddWithValue("@idUsuarioAnulacion", idUsuarioAnulacion);
                    command.Parameters.AddWithValue("@fechaFinAnticipada", fecha_finAnticipada);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
    //Fin zona fin anticipado
        public IList<Contrato> ObtenerTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.ID_Contrato, c.ID_Inmueble, c.ID_Inquilino, c.Fecha_Inicio, c.Fecha_Fin,
                                    c.Monto_Alquiler, c.Multa, c.Estado, c.Fecha_FinAnt,
                                    i.Nombre, i.Apellido,
                                    inm.direccion
                            FROM Contrato c
                            JOIN Inquilino i ON c.ID_Inquilino = i.id
                            JOIN Inmueble inm ON c.ID_Inmueble = inm.id
                            WHERE c.EstadoLogico = 1";

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
                            Estado = reader.GetString("Estado"),
                            Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("Direccion")
                            }
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
                string sql = @"SELECT c.ID_Contrato, c.ID_Inmueble, c.ID_Inquilino, c.Fecha_Inicio, c.Fecha_Fin,
                                    c.Monto_Alquiler, c.Multa, c.Estado, c.ID_UsuarioAlta, c.ID_UsuarioBaja,c.Fecha_FinAnt,c.ID_UsuarioAnulacion,
                                    i.Nombre, i.Apellido,
                                    inm.Direccion
                            FROM Contrato c
                            JOIN Inquilino i ON c.ID_Inquilino = i.id
                            JOIN Inmueble inm ON c.ID_Inmueble = inm.id
                            WHERE c.ID_Contrato = @id";
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
                            Estado = reader.GetString("Estado"),
                            ID_UsuarioAlta = reader.GetInt32("ID_UsuarioAlta"),
                            ID_UsuarioBaja = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioBaja")) ? (int?)null : reader.GetInt32("ID_UsuarioBaja"),
                            Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                            ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("Direccion")
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return contrato;
        }

        //zona contratos de baja
        public IList<Contrato> ObtenerTodosBaja()
        {
            IList<Contrato> res = new List<Contrato>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.ID_Contrato, c.ID_Inmueble, c.ID_Inquilino, c.Fecha_Inicio, c.Fecha_Fin,
                                    c.Monto_Alquiler, c.Multa, c.Estado, c.ID_UsuarioAlta, c.ID_UsuarioBaja, c.Fecha_FinAnt, c.ID_UsuarioAnulacion,
                                    i.Nombre, i.Apellido,
                                    inm.Direccion
                            FROM Contrato c
                            JOIN Inquilino i ON c.ID_Inquilino = i.id
                            JOIN Inmueble inm ON c.ID_Inmueble = inm.id
                            WHERE c.EstadoLogico = 0";
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
                            Estado = reader.GetString("Estado"),
                            ID_UsuarioAlta = reader.GetInt32("ID_UsuarioAlta"),
                            ID_UsuarioBaja = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioBaja")) ? (int?)null : reader.GetInt32("ID_UsuarioBaja"),
                            Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                            ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("Direccion")
                            }
                        };
                        res.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        //fin zona contratos de baja
        public Contrato ObtenerPorInquilino(int idInquilino)
        {
            Contrato contrato = null;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.ID_Contrato, c.ID_Inmueble, c.ID_Inquilino, c.Fecha_Inicio, c.Fecha_Fin,
                                    c.Monto_Alquiler, c.Multa, c.Estado,c.Fecha_FinAnt,c.ID_UsuarioAnulacion,
                                    i.Nombre, i.Apellido,
                                    inm.Direccion
                            FROM Contrato c
                            JOIN Inquilino i ON c.ID_Inquilino = i.id
                            JOIN Inmueble inm ON c.ID_Inmueble = inm.id
                            WHERE c.ID_Inquilino = @idInquilino AND c.EstadoLogico = 1";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInquilino", idInquilino);
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
                            Estado = reader.GetString("Estado"),
                            Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                            ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("Direccion")
                            }
                        };
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
                string sql = @"SELECT c.ID_Contrato, c.ID_Inmueble, c.ID_Inquilino, c.Fecha_Inicio, c.Fecha_Fin,
                                    c.Monto_Alquiler, c.Multa, c.Estado,c.Fecha_FinAnt,c.ID_UsuarioAnulacion,
                                    i.Nombre, i.Apellido,
                                    inm.Direccion
                            FROM Contrato c
                            JOIN Inquilino i ON c.ID_Inquilino = i.id
                            JOIN Inmueble inm ON c.ID_Inmueble = inm.id
                            WHERE c.Estado = 'Vigente' AND c.EstadoLogico = 1";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            ID_Contrato = reader.GetInt32("ID_Contrato"),
                            ID_Inmueble = reader.GetInt32("ID_Inmueble"),
                            ID_Inquilino = reader.GetInt32("ID_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                            Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                            Multa = reader.GetDecimal("Multa"),
                            Estado = reader.GetString("Estado"),
                            Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                            ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("Direccion")
                            }
                        });
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
                string sql = @"SELECT c.ID_Contrato, c.ID_Inmueble, c.ID_Inquilino, c.Fecha_Inicio, c.Fecha_Fin,
                                    c.Monto_Alquiler, c.Multa, c.Estado,c.Fecha_FinAnt,c.ID_UsuarioAnulacion,
                                    i.Nombre, i.Apellido,
                                    inm.Direccion
                            FROM Contrato c
                            JOIN Inquilino i ON c.ID_Inquilino = i.id
                            JOIN Inmueble inm ON c.ID_Inmueble = inm.id
                            WHERE c.EstadoLogico = 1
                            LIMIT @tamPagina OFFSET @offset";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            ID_Contrato = reader.GetInt32("ID_Contrato"),
                            ID_Inmueble = reader.GetInt32("ID_Inmueble"),
                            ID_Inquilino = reader.GetInt32("ID_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                            Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                            Multa = reader.GetDecimal("Multa"),
                            Estado = reader.GetString("Estado"),
                            Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                            ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("Direccion")
                            }
                        });
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
                            AND ((@fechaInicio <= Fecha_Fin AND @fechaFin >= Fecha_Inicio)) AND EstadoLogico=1 AND Estado = 'Vigente'";
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
                            AND ((@fechaInicio <= Fecha_Fin AND @fechaFin >= Fecha_Inicio)) AND ID_Contrato != @idContrato AND EstadoLogico=1  AND Estado = 'Vigente'";
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

        public IList<ContratoViewModel> ObtenerPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            IList<ContratoViewModel> res = new List<ContratoViewModel>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.ID_Contrato, c.id_Inmueble, c.id_Inquilino, c.Fecha_Inicio, c.Fecha_Fin, c.Monto_Alquiler, c.Multa, c.Estado,
                                    c.Fecha_FinAnt,c.ID_UsuarioAnulacion,
                                    i.id AS InmuebleId, i.direccion,
                                    inq.id AS InquilinoId, inq.nombre, inq.apellido, inq.dni
                            FROM Contrato c
                            JOIN Inmueble i ON c.ID_Inmueble = i.id
                            JOIN Inquilino inq ON c.ID_Inquilino = inq.id
                            WHERE c.EstadoLogico = 1 
                                AND c.fecha_inicio >= @fechaInicio 
                                AND c.fecha_fin <= @fechaFin 
                                AND c.Estado = 'Vigente'";
                
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", fechaFin);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ContratoViewModel contratoView = new ContratoViewModel
                        {
                            Contrato = new Contrato
                            {
                                ID_Contrato = reader.GetInt32("ID_Contrato"),
                                ID_Inmueble = reader.GetInt32("id_Inmueble"),
                                ID_Inquilino = reader.GetInt32("id_Inquilino"),
                                Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                                Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                                Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                                Multa = reader.GetDecimal("Multa"),
                                Estado = reader.GetString("Estado"),
                                Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                                ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion")
                            },
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                                Dni = reader.GetString("dni")
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32("InmuebleId"),
                                Direccion = reader.GetString("direccion")
                            }
                        };
                        res.Add(contratoView);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<ContratoViewModel> ObtenerPorInmueble(int idInmueble)
        {
            IList<ContratoViewModel> res = new List<ContratoViewModel>();
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.ID_Contrato, c.id_Inmueble, c.id_Inquilino, c.Fecha_Inicio, c.Fecha_Fin, c.Monto_Alquiler, c.Multa, c.Estado,
                                    c.Fecha_FinAnt,c.ID_UsuarioAnulacion,
                                    i.id AS InmuebleId, i.direccion,
                                    inq.id AS InquilinoId, inq.nombre, inq.apellido, inq.dni
                            FROM Contrato c
                            JOIN Inmueble i ON c.ID_Inmueble = i.id
                            JOIN Inquilino inq ON c.ID_Inquilino = inq.id
                            WHERE c.EstadoLogico = 1 
                                AND c.ID_Inmueble = @idInmueble";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idInmueble", idInmueble);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ContratoViewModel contratoView = new ContratoViewModel
                        {
                            Contrato = new Contrato
                            {
                                ID_Contrato = reader.GetInt32("ID_Contrato"),
                                ID_Inmueble = reader.GetInt32("id_Inmueble"),
                                ID_Inquilino = reader.GetInt32("id_Inquilino"),
                                Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                                Fecha_Fin = reader.GetDateTime("Fecha_Fin"),
                                Monto_Alquiler = reader.GetDecimal("Monto_Alquiler"),
                                Multa = reader.GetDecimal("Multa"),
                                Estado = reader.GetString("Estado"),
                                Fecha_FinAnt = reader.IsDBNull(reader.GetOrdinal("Fecha_FinAnt")) ? (DateTime?)null : reader.GetDateTime("Fecha_FinAnt"),
                                ID_UsuarioAnulacion = reader.IsDBNull(reader.GetOrdinal("ID_UsuarioAnulacion")) ? (int?)null : reader.GetInt32("ID_UsuarioAnulacion")
                            },
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32("InquilinoId"),
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido"),
                                Dni = reader.GetString("dni")
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32("InmuebleId"),
                                Direccion = reader.GetString("direccion")
                            }
                        };
                        res.Add(contratoView);
                    }
                    connection.Close();
                }
            }
            return res;
        }
	
    }
}
