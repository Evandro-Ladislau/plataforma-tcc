using System;
using MySql.Data.MySqlClient;
using client.models;
using NLog;

namespace client.storage 
{
    class ClientStorage
    {
        private string _connectionString = "Server=localhost;Database=client_data;Uid=root;Pwd=plataforma_tcc_2024;";
        private MySqlConnection _connection;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ClientStorage()
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        public void Insert(ClientModel client)
        {
            try
            {
                string sql = "INSERT INTO client (name, surname, email, birthdate, created_at, updated_at, active) VALUES (@Name, @Surname, @Email, @BirthDate, @CreatedAt, @UpdatedAt, @Active)";
                
                using (MySqlCommand command = new MySqlCommand(sql, _connection))
                {
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Surname", client.Surname);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@BirthDate", client.BirthDate);
                    command.Parameters.AddWithValue("@CreatedAt", client.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", client.UpdatedAt);
                    command.Parameters.AddWithValue("@Active", client.Active);
                    
                    command.ExecuteNonQuery();
                    logger.Info("Client inserted successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error inserting client: {ex.Message}");
            }
        }    
    }
}
