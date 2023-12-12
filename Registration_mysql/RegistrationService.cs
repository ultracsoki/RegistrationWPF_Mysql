using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration_mysql
{
    public class RegistrationService
    {
		MySqlConnection connection;

		public RegistrationService()
		{
			MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
			builder.Server = "localhost";
			builder.Port = 3306;
			builder.UserID = "root";
			builder.Password = "";
			builder.Database = "registrations";

			connection = new MySqlConnection(builder.ConnectionString);
		}

		//CRUD

		public bool Create(Registration registration)
		{
			OpenConnection();
			string sql = "INSERT INTO workers(username,password,email,phoneNumber) VALUES(@username,@password,@email,@phoneNumber)";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.AddWithValue("@name", registration.Username);
			command.Parameters.AddWithValue("@gender", registration.Password);
			command.Parameters.AddWithValue("@age", registration.Email);
			command.Parameters.AddWithValue("@salary", registration.PhoneNumber);
			int affectedRows = command.ExecuteNonQuery();
			CloseConnection();
			return affectedRows == 1;
		}

		public List<Registration> GetAll()
		{
			List<Registration> registrations = new List<Registration>();
			OpenConnection();
			string sql = "SELECT * FROM workers";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			using (MySqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					Registration registration = new Registration();
                    registration.Id = reader.GetInt32("id");
                    registration.Username = reader.GetString("username");
                    registration.Password = reader.GetString("password");
                    registration.Email = reader.GetString("email");
                    registration.PhoneNumber = reader.GetInt32("phoneNumber");
                    registrations.Add(registration);
				}
			}
			CloseConnection();
			return registrations;
		}

		public bool Update(int id, Registration newValues)
		{
			OpenConnection();
			string sql = "UPDATE workers SET name = @name,username = @username,password = @password,email = @email,phoneNumber = @phoneNumber WHERE workers.id = @id";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.AddWithValue("@id", id);
			command.Parameters.AddWithValue("@username", newValues.Username);
			command.Parameters.AddWithValue("@password", newValues.Password);
			command.Parameters.AddWithValue("@email", newValues.Email);
			command.Parameters.AddWithValue("@phoneNumber", newValues.PhoneNumber);
			int affectedRows = command.ExecuteNonQuery();
			CloseConnection();
			return affectedRows == 1;
		}

		public bool Delete(int id)
		{
			OpenConnection();
			string sql = $"DELETE FROM workers WHERE id = @id";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.AddWithValue("@id", id);
			int affectedRows = command.ExecuteNonQuery();
			CloseConnection();
			return affectedRows == 1;
		}


		private void CloseConnection()
		{
			if (connection.State == System.Data.ConnectionState.Open)
			{
				connection.Close();
			}
		}

		private void OpenConnection()
		{
			if (connection.State != System.Data.ConnectionState.Open)
			{
				connection.Open();
			}
		}

	}
}
