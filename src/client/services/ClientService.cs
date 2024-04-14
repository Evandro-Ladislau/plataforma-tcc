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
        public bool Insert(ClientModel client)
        {
            var isValidateDataClient = IsValidDataClient(client);

            if(isValidateDataClient){
                //Console.WriteLine("Cliente Name: " + client.Name);
                storage.Insert(client);
            }
            
            return isValidateDataClient;
        }

        public bool IsValidDataClient(ClientModel client)
        {
            if (!string.IsNullOrEmpty(client.Name) && !string.IsNullOrWhiteSpace(client.Name) &&!string.IsNullOrEmpty(client.Surname) && !string.IsNullOrWhiteSpace(client.Surname))
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(pattern);
                return regex.IsMatch(client.Email);
            }
            
            logger.Info("All customer data must be filled in correctly!");
            return false;
        }
    }
}