using System;
using System.Text.RegularExpressions;
using client.models;
using client.storage;
using NLog;

namespace client.services
{
    class ClientService 
    {
        private ClientStorage storage = new ClientStorage();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private List<ClientModel> clientList;
        public bool Insert(ClientModel client)
        {
            var isValidateDataClient = IsValidDataClient(client);

            if(isValidateDataClient){
                storage.Insert(client);
            }
            
            return isValidateDataClient;
        }

        public List<ClientModel> Select()
        {
            clientList = storage.Select();
            return clientList;
        }

        public ClientModel selectById(int id)
        {
            ClientModel client = storage.selectById(id);
            return client;
        }
        public bool IsValidDataClient(ClientModel client)
        {
            if (!string.IsNullOrEmpty(client.Name) && !string.IsNullOrWhiteSpace(client.Name) && !string.IsNullOrEmpty(client.Surname) && !string.IsNullOrWhiteSpace(client.Surname) && client.BirthDate != DateTime.MinValue)
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                logger.Info("Validated customer data!");
                return regex.IsMatch(client.Email);
            }
            
            logger.Error("All customer data must be filled in correctly!");
            return false;
        }

        public bool Update(int id, ClientModel client)
        {
            var isValidateDataClient = IsValidDataClient(client);
            var clienteUpdate = storage.Update(id, client);

            if(isValidateDataClient && clienteUpdate){
                
                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            var clienteDelete = storage.Delete(id);

            if(clienteDelete){

                return true;
            }

            return false;
        }
    }
}