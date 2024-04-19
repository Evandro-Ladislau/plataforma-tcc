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
        private ClientModel client;
        public ClientStorage()
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        public bool Insert(ClientModel client)
        {
            try
            {
                string sql = "INSERT INTO client (name, surname, email, birthdate, created_at, active) VALUES (@Name, @Surname, @Email, @BirthDate, NOW(), @Active)";
                
                using (MySqlCommand command = new MySqlCommand(sql, _connection))
                {
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Surname", client.Surname);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@BirthDate", client.BirthDate);
                    command.Parameters.AddWithValue("@Active", client.Active);
                    
                    command.ExecuteNonQuery();
                    logger.Info("Client inserted successfully.");

                    return true;
                }
            }
            catch (MySqlException ex)
            {
                logger.Error($"Error inserting client: {ex.Message}");
            }

            return false;
        }  

        public List<ClientModel> Select()
        {

            List<ClientModel> clientList = new List<ClientModel>();

            try
            {
                string sql = "SELECT id, name, surname, birthdate, created_at, updated_at, active FROM client";
                
                using (MySqlCommand command = new MySqlCommand(sql, _connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int clientId = reader.GetInt32("id");
                            string name = reader.GetString("name");
                            string surname = reader.GetString("surname");
                            DateTime birthdateUtc = reader.GetDateTime("birthdate"); // Corrigido para DateTime
                            DateTime birthdate = birthdateUtc.ToLocalTime();
                            DateTime createdAtUtc = reader.GetDateTime("created_at");
                            DateTime createdAt = createdAtUtc.ToLocalTime();
                            DateTime updatedAtUtc = reader.GetDateTime("updated_at");
                            DateTime updatedAt = updatedAtUtc.ToLocalTime();
                            bool active = reader.GetBoolean("active"); // Corrigido para GetBoolean

                            client = new ClientModel
                            {
                                Id = clientId,
                                Name = name,
                                Surname = surname,
                                BirthDate = birthdate,
                                CreatedAt = createdAt,
                                UpdatedAt = updatedAt,
                                Active = active
                            };

                            clientList.Add(client);
                        }
                    }

                    logger.Info($"Listing Client!");

                    return clientList;
                }
            }
            catch (MySqlException ex)
            {
                
                logger.Error($"Error listing client: {ex.Message}");
            }

            return clientList;
        }  

        public ClientModel selectById(int id)
        {
            try
            {
                string sql = "SELECT id, name, surname, birthdate, created_at, updated_at, active FROM client WHERE id=@id AND active=1";
                
                using (MySqlCommand command = new MySqlCommand(sql, _connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                                while (reader.Read())
                            {
                                int clientId = reader.GetInt32("id");
                                string name = reader.GetString("name");
                                string surname = reader.GetString("surname");
                                DateTime birthdateUtc = reader.GetDateTime("birthdate");
                                DateTime birthdate = birthdateUtc.ToLocalTime();
                                DateTime createdAtUtc = reader.GetDateTime("created_at");
                                DateTime createdAt = createdAtUtc.ToLocalTime();
                                DateTime updatedAtUtc = reader.GetDateTime("updated_at");
                                DateTime updatedAt = updatedAtUtc.ToLocalTime();
                                bool active = reader.GetBoolean("active"); 

                                client = new ClientModel
                                {
                                    Id = clientId,
                                    Name = name,
                                    Surname = surname,
                                    BirthDate = birthdate,
                                    CreatedAt = createdAt,
                                    UpdatedAt = updatedAt,
                                    Active = active
                                };
                            }

                            logger.Info($"Listing Client by id!");
                            return client;
                        }
                        else
                        {
                            logger.Info("No client found with the provided ID.");
                            return null;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                logger.Error($"Error listing client by id: {ex.Message}");
            }

            return client;
        }

        public bool Update(int id, ClientModel client)
        {
            try
            {
                string sql = "UPDATE client SET name = @Name, surname = @Surname, birthdate = @Birthdate, updated_at = NOW() WHERE id = @Id AND active = 1";
                
                using (MySqlCommand command = new MySqlCommand(sql, _connection))
                {
                    
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Surname", client.Surname);
                    command.Parameters.AddWithValue("@Birthdate", client.BirthDate);

                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        logger.Info("Client updated successfully.");
                        return true;
                    }   
                    else
                    {
                        logger.Info("No client found with the provided ID.");
                        return false;
                    }                         
                }
            }
            catch (MySqlException ex)
            {
                logger.Error($"Error updating client: {ex.Message}");
            }

            return false;
        }

        public bool Delete(int id)
        {
            try
            {
                string sql = "UPDATE client SET active = 0 WHERE id = @id";
                
                using (MySqlCommand command = new MySqlCommand(sql, _connection))
                {
                    
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if(rowsAffected > 0)
                    {
                        logger.Info("Client deleted successfully.");
                        return true;
                    }   
                    else
                    {
                        logger.Info("No client found with the provided ID.");
                        return false;
                    }                         
                }
            }
            catch (MySqlException ex)
            {
                logger.Error($"Error deleted client: {ex.Message}");
            }

            return false;
        }
    }
}
