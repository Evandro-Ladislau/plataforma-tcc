using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace client.storage 
{
    class ClientStorage
    {
        string connectionString = "Server=localhost;Database=client_data;Uid=root;Pwd=plataforma_tcc_2024;";

        public List<string> GetClientNames()
        {
            List<string> ListclientName = new List<string>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "SELECT surname FROM client"; // Seleciona apenas a coluna name
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clientName = reader.GetString(0); // Obtém o valor da primeira coluna como um inteiro
                            ListclientName.Add(clientName); // Adiciona o name do cliente à lista
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro:" + ex.Message);
            }

            return ListclientName; // Retorna a lista com os IDs dos clientes
        }
    }
}
