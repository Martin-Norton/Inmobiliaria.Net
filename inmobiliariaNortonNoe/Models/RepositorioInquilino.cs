using System;
using System.Collections.Generic;
using  MySql.Data.MySqlClient;
using System.Data;

namespace inmobiliariaNortonNoe.Models
{
	public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
	{
		public RepositorioInquilino(IConfiguration configuration) : base(configuration)
		{
		}

		public int Alta(Inquilino p)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Inquilino 
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
				string sql = @$"DELETE FROM Inquilino WHERE {nameof(Inquilino.Id)} = @id";
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
		public int Modificacion(Inquilino p)
		{
			int res = -1;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"UPDATE Inquilino 
					SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email
					WHERE {nameof(Inquilino.Id)} = @id";
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

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					Id, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilino";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino p = new Inquilino
						{
							Id = reader.GetInt32(nameof(Inquilino.Id)),
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

		public IList<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT Id, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilino
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino p = new Inquilino
						{
							Id = reader.GetInt32(nameof(Inquilino.Id)),
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
					FROM Inquilino
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

		public Inquilino ObtenerPorId(int id)
		{
			Console.WriteLine($"Obteniendo inquilino con ID: {id}");
			Inquilino ? p= null;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					Id, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilino
					WHERE Id=@id";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Inquilino
						{
							Id = reader.GetInt32(nameof(Inquilino.Id)),
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

		public Inquilino ObtenerPorEmail(string email)
		{
			Inquilino? p = null;
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @$"SELECT 
					{nameof(Inquilino.Id)}, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilino
					WHERE Email=@email";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.String).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Inquilino
						{
							Id = reader.GetInt32(nameof(Inquilino.Id)),//más seguro
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

		public IList<Inquilino> BuscarPorNombre(string nombre)
		{
			List<Inquilino> res = new List<Inquilino>();
			Inquilino? p = null;
			nombre = "%" + nombre + "%";
			using (var connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT
					Id, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilino
					WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", MySqlDbType.String).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Inquilino
						{
							Id = reader.GetInt32(nameof(Inquilino.Id)),
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