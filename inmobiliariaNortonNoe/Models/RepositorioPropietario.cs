using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
namespace inmobiliariaNortonNoe.Models
{
	public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
	{
		public RepositorioPropietario(IConfiguration configuration) : base(configuration)
		{
		}

		public int Alta(Propietario p)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Propietario 
					(Nombre, Apellido, Dni, Telefono, Email) 
					VALUES (@nombre, @apellido, @dni, @telefono, @email);
					SELECT LAST_INSERT_ID();";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@email", p.Email);

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
				string sql = @$"DELETE FROM Propietario WHERE {nameof(Propietario.Id)} = @id";
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
		public int Modificacion(Propietario p)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"UPDATE Propietario 
					SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email
					WHERE {nameof(Propietario.Id)} = @id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@email", p.Email);
					command.Parameters.AddWithValue("@id", p.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Propietario> ObtenerTodos()
		{
			IList<Propietario> res = new List<Propietario>();
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					Id, Nombre, Apellido, Dni, Telefono, Email
					FROM Propietario";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Console.WriteLine(reader.GetInt32(nameof(Propietario.Id)));
						Propietario p = new Propietario
						{
							Id = reader.GetInt32(nameof(Propietario.Id)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Propietario> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Propietario> res = new List<Propietario>();
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT Id, Nombre, Apellido, Dni, Telefono, Email
					FROM Propietario
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							Id = reader.GetInt32(nameof(Propietario.Id)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}


		public int ObtenerCantidad()
		{
			int res = 0;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT COUNT(Id)
					FROM Propietario
				";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						res = reader.GetInt32(0);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Propietario ObtenerPorId(int id)
		{
			Console.WriteLine($"Obteniendo propietario con ID: {id}");
			Propietario? p = null;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					Id, Nombre, Apellido, Dni, Telefono, Email 
					FROM Propietario
					WHERE Id=@id";

				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						p = new Propietario
						{
							Id = reader.GetInt32(nameof(Propietario.Id)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),

						};
						Console.WriteLine($"Propietario encontrado: {p.Nombre} {p.Apellido}");
					}
					connection.Close();
				}
			}
			return p;
		}

		public Propietario ObtenerPorEmail(string email)
		{
			Propietario? p = null;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"SELECT 
					{nameof(Propietario.Id)}, Nombre, Apellido, Dni, Telefono, Email 
					FROM Propietario
					WHERE Email=@email";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.String).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Propietario
						{
							Id = reader.GetInt32(nameof(Propietario.Id)),//más seguro
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),

						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public IList<Propietario> BuscarPorNombre(string nombre)
		{
			List<Propietario> res = new List<Propietario>();
			Propietario? p = null;
			nombre = "%" + nombre + "%";
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT
					Id, Nombre, Apellido, Dni, Telefono, Email 
					FROM Propietario
					WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", MySqlDbType.String).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Propietario
						{
							Id = reader.GetInt32(nameof(Propietario.Id)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),

						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
    }
}